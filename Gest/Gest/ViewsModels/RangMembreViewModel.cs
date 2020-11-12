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
using Gest.Helpers.Load_donnees;
using Gest.Helpers.Manager_parametre_recherche_sql;
using Gest.Helpers.Initialise_parametres_recherches_for_navigation_with_prism;

namespace Gest.ViewModels
{
    class RangMembreViewModel : BindableBase,INavigationAware
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
                    parametre.Add("names_tables", this.Liste_noms_tables);
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
                    parametre_recherche_sql = new Manager_parametre_recherche_sql().update_parametre_recherche_sql(
                        parametre_recherche_sql, this.nom_table_selected, this.Champ_selected, this.methoderecherche_selected
                        );
                });
            }
        }

        public ICommand Command_search
        {
            get
            {
                return new Command<Object>(async (donnees) => {
                    parametre_recherche_sql.Valeur = donnees;
                    await load(new List<Parametre_recherche_sql>() { parametre_recherche_sql });
                });
            }
        }
        #endregion

        #region Variables
        public string title { get; set; } = "Page rangs des membres";

        private Boolean _isloading = false;
        public Boolean Isloading
        {
            get { return _isloading; }
            set { SetProperty(ref _isloading, value); }
        }

        private Parametre_recherche_sql parametre_recherche_sql = new Parametre_recherche_sql();

        public List<String> Liste_methodesrecherches { get { return Enumerations_methodes_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumeration_type_recherche.get_liste_typesrecherches();} }
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



        private IEnumerable<Rang> _liste_rangs;
        public IEnumerable<Rang> Liste_rangs
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
            this.Isloading = true;
            IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql = new Gest.Helpers.Generate_dictionnaire_parametresrecherche.Generate_parametresrecherche().generate(parametres_recherches_sql);
            Load_donnees<IEnumerable<Rang>> load_donnees = new Load_donnees_of_viewmodel_membrerang<IEnumerable<Rang>>(service_membre, service_rang);
           await Task.Delay(3000);
            this.Liste_rangs = await load_donnees.get_donnees(dictionnaire_parametres_sql);
            this.Isloading = false;
        }

        
        #endregion

        #region Methode_navigation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
           
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            IEnumerable<Parametre_recherche_sql> parametres_recherches_sql = new Initialise_parametres_recherches_for_navigation_with_prism().get_initialise_parametres_recherches_sql_for_navigation_with_prism(parameters, "parametres_recherches_sql");
            await load(parametres_recherches_sql);
        }
        #endregion
    }
}
