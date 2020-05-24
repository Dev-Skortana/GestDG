using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Commands;
using Gest.Models;
using Gest.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;
using Gest.Interface_SQLiteAccess;
using Recherche_donnees_GESTDG;

namespace Gest.ViewModels
{
    class RangMembreViewModel : BindableBase,INavigationAware,INavigation_Goback_Popup_searchbetweendates
    {
        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Rang service_rang;
        private IService_Membre service_membre;
        #endregion

        #region constructeure
        public RangMembreViewModel(INavigationService _service_navigation, IService_Rang _service_rang, IService_Membre _service_membre)
        {
            this.service_rang = _service_rang;
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;

            this.Liste_champs = this.Liste_champs_rangs;
            this.nom_table_selected = "Rang";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion


        #region Commande_MVVM
        public ICommand Command_navigation_to_popup_searchbetweendates
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("champ", Champ_selected);
                    parametre.Add("navigation_goback", this);
                    service_navigation.NavigateAsync("Popup_search_betweendates", parametre);
                });
            }
        }
        public ICommand Command_switch_source {
            get
            {
                return new Command(()=> {
                    if (nom_table_selected=="Membre")
                    {
                        Liste_champs = Liste_champs_membres;           
                    }
                    else if(nom_table_selected=="Rang")
                    {
                        Liste_champs = Liste_champs_rangs;        
                    }
                    this.Champ_selected = Liste_champs[0];
                });
            }
        }

        public ICommand Command_gestion_dictionnaire_champsmethodesrecherches
        {
            get
            {
                return new Command(() =>
                {
                    if (Champ_selected != null)
                    {
                        if (liste_parametres_recherches_sql.Exists((parametre) => parametre.Nom_table==nom_table_selected && parametre.Champ == Champ_selected && parametre.Methode_recherche == methoderecherche_selected) == false)
                        {
                            liste_parametres_recherches_sql.Add(new Parametre_recherche_sql() {Nom_table=nom_table_selected, Champ = Champ_selected, Methode_recherche = methoderecherche_selected });
                        }
                    }
                });
            }
        }

        public ICommand Command_search
        {
            get
            {
                return new Command<Object>(async (donnees) => {
                    if (type_selected == Enumerations_recherches.types_recherches.Simple.ToString())
                    {
                        liste_parametres_recherches_sql.ForEach((parametre) =>
                        {
                            if ((parametre.Nom_table == nom_table_selected) && (parametre.Champ == Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected))
                            {
                                parametre.Valeur = donnees;
                            }
                        });
                        await Task.Run(() => {
                            int index_parametre = liste_parametres_recherches_sql.FindIndex((parametre) => (parametre.Nom_table == nom_table_selected) && (parametre.Champ == Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected) && (parametre.Valeur == donnees));
                            for (var i = 0; i < liste_parametres_recherches_sql.Count; i++)
                            {
                                if (i != index_parametre)
                                {
                                    liste_parametres_recherches_sql.RemoveAt(i);
                                }
                            }
                        });
                    }
                    await load(liste_parametres_recherches_sql);
                });
            }
        }
        #endregion

        #region Variables
        public string title { get; set; } = "Page rangs des membres";

        private List<Parametre_recherche_sql> liste_parametres_recherches_sql = new List<Parametre_recherche_sql>();

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches();} }
        public String type_selected { get; set; }

        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Rang" }; } }
        public String nom_table_selected { get; set; }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs,value); } }

        public List<String> Liste_champs_rangs { get {return new List<string>() {"nom_rang","url_rang"}; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected,value); }
        }



        private List<Rang> _liste_rangs;
        public List<Rang> Liste_rangs
        {
            get
            {
                return _liste_rangs;
            }
            set
            {
                SetProperty(ref _liste_rangs, value);
            }
        }

        #endregion

        #region Methodes priver ou interne
        private async Task load(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            var membres = (List<Membre>)await new Services.Classes.Service_Membre().GetList(nom_table_selected == "Membre" ? parametres_recherches_sql :null);
            var rangs= (List<Rang>)await new Services.Classes.Service_Rang().GetList(nom_table_selected=="Rang" ? parametres_recherches_sql :null);
            rangs.ForEach((item) =>item.liste_membres= (from el in membres where el.rang_nom==item.nom_rang orderby el.pseudo select el).ToList());
            if (nom_table_selected=="Membre") {
                rangs.RemoveAll((item) =>item.liste_membres.Count == 0);
            }
            Liste_rangs = rangs;
        }

        public async Task navigation_Goback_Popup_searchbetweendates(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            await load(parametres_recherches_sql);
        }
        #endregion

        #region Methode_navigation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
           
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(null);
        }
        #endregion
    }
}
