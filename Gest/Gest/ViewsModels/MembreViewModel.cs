using Gest.Models;
using Gest.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using Gest.Interface_SQLiteAccess;
using Xamarin.Forms;
using Recherche_donnees_GESTDG;
using System.Linq.Expressions;

namespace Gest.ViewModels
{
    class MembreViewModel : BindableBase, INavigationAware,INavigation_Goback_Popup_searchbetweendates,INavigation_Goback_Popup_searchmultiple
    {

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        #endregion

        #region Constructeure
        public MembreViewModel(INavigationService _service_navigation, IService_Membre _service_membre)
        {
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;

            this.Liste_champs = this.Liste_champs_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables

        private List<Parametre_recherche_sql> liste_parametres_recherches_sql = new List<Parametre_recherche_sql>();
        public String title { get; set; } = "Page des membres";
        private Boolean _isloading=false;
        public Boolean Isloading
        {
            get { return _isloading; }
            set { SetProperty(ref _isloading,value); }
        }


        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre"}; } }
        public String nom_table_selected { get; set; }

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches();} }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        private String _Champ_selected;

        public String Champ_selected
        {
            get { return _Champ_selected; }
            set { SetProperty(ref _Champ_selected, value); }
        }

        private List<Membre> _liste_membres;
        public List<Membre> Liste_membres
        {
            get
            {
                return _liste_membres;
            }
            set
            {
                SetProperty(ref _liste_membres, value);
            }
        }

        #endregion

        #region Commandes_MVVM

        public ICommand Command_switch_source
        {
            get
            {
                return new Command(() => {
                    if (nom_table_selected == "Membre")
                    {
                        Liste_champs = Liste_champs_membres;
                    }
                    Champ_selected = Liste_champs[0];
                });
            }
        }

        public ICommand Command_navigation_to_popup_searchbetweendates
        {
            get
            {
                return new Command(()=> {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("champ", Champ_selected);
                    parametre.Add("navigation_goback", this);

                    service_navigation.NavigateAsync("Popup_search_betweendates", parametre);
                });
            }
        }
        public ICommand Command_gestion_dictionnaire_champsmethodesrecherches
        {
            get
            {
                return new Command(() =>
                {
                    if (Champ_selected!=null) {
                        if ((liste_parametres_recherches_sql.Exists((parametre)=>parametre.Nom_table==nom_table_selected && parametre.Champ==Champ_selected && parametre.Methode_recherche==methoderecherche_selected)==false))
                        {
                            liste_parametres_recherches_sql.Add(new Parametre_recherche_sql() {Nom_table=nom_table_selected, Champ=Champ_selected,Methode_recherche=methoderecherche_selected});
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
                            if ((parametre.Nom_table==nom_table_selected) && (parametre.Champ == Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected))
                            {
                                parametre.Valeur = donnees;
                            }
                        });
                        await Task.Run(() => {
                            int index_parametre = liste_parametres_recherches_sql.FindIndex((parametre) => (parametre.Nom_table==nom_table_selected) && (parametre.Champ == Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected) && (parametre.Valeur == donnees));
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

        #region Methodes priver ou interne
        private async Task load(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            this.Isloading = true;
            Liste_membres = (from membre in (List<Membre>)await service_membre.GetList(parametres_recherches_sql) orderby membre.pseudo select membre).ToList();
            this.Isloading = false;
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

        public async Task navigation_Goback_Popup_searchbetweendates(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            await load(parametres_recherches_sql);
        }
        public async Task navigation_Goback_Popup_searchmultiple(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            await load(parametres_recherches_sql);
        }

        #endregion

    }
}
