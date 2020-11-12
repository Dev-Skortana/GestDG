using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Navigation;
using Gest.Models;
using Gest.Services.Interfaces;
using System.Threading.Tasks;
using Recherche_donnees_GESTDG.enumeration;
using System.Linq;
using Recherche_donnees_GESTDG;
using Gest.Helpers.Manager_parametre_recherche_sql;
using Gest.Helpers.Load_donnees;
using Gest.Helpers.Initialise_parametres_recherches_for_navigation_with_prism;

namespace Gest.ViewModels
{
    class FastMembreViewModel: BindableBase,INavigationAware
    {
        #region Interfaces_services
        private IService_Membre service_membre;
        private INavigationService service_naviguation;
        #endregion

        #region Constructeure
        public FastMembreViewModel(IService_Membre _service_membre,INavigationService _service_navigation)
        {
            this.service_membre = _service_membre;
            this.service_naviguation = _service_navigation;

            this.Liste_champs = this.Liste_champs_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables
        public String title { set; get; } = "Vue rapide des membres";
        private Boolean _isloading = false;
        public Boolean Isloading
        {
            get { return _isloading; }
            set { SetProperty(ref _isloading, value); }
        }
        public Membre membre_selected=new Membre();
        private Parametre_recherche_sql parametre_recherche_sql = new Parametre_recherche_sql();

        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre" }; } }
        public String nom_table_selected { get; set; }

        public List<String> Liste_methodesrecherches { get { return Enumerations_methodes_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumeration_type_recherche.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }

        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "url_avatar" }; } }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        private String _Champ_selected;

        public String Champ_selected
        {
            get { return _Champ_selected; }
            set { SetProperty(ref _Champ_selected, value); }
        }


        private IEnumerable<Membre> _liste_membres=new List<Membre>();
        public IEnumerable<Membre> Liste_membres {
            get
            {
                return _liste_membres;
            }
            set
            {
                SetProperty(ref _liste_membres,value);
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
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("names_tables", this.Liste_noms_tables);
                    service_naviguation.NavigateAsync("Popup_search_betweendates", parametre);

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


        public ICommand save_membre_selected
        {
            get
            {
                return new Command<int>((position) => {

                    if (Liste_membres.ToList().Count>0)
                    {
                        membre_selected = Liste_membres.ToList()?[position];
                    }                 
                });
            }
        }
        
        
        public ICommand getpage_membre
        {
            get
            {
                return new Command(async()=> {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("Membre",membre_selected);
                    await service_naviguation.NavigateAsync("SuiviMembre", parametre);
                });
            }
        } 
        #endregion

        #region Methode_priver
        private async Task load(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql) 
        { 
            this.Isloading = true;
            IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql = new Gest.Helpers.Generate_dictionnaire_parametresrecherche.Generate_parametresrecherche().generate(parametres_recherches_sql);
            Load_donnees<IEnumerable<Membre>> load_donnees = new Load_donnees_of_viewmodel_fastmembre<IEnumerable<Membre>>(service_membre);
            this.Liste_membres = await load_donnees.get_donnees(dictionnaire_parametres_sql);
            this.Isloading = false;
        }

        #endregion

        #region Methodes_naviguations_PRISM
        public async void OnNavigatedFrom(INavigationParameters parameters)
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
