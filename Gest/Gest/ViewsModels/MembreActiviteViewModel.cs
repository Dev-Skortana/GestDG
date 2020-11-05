using Gest.Models;
using Gest.Services.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using Gest.Models;
using Gest.Services.Classes;
using Gest.Services.Interfaces;
using Gest.Helpers.Load_donnees;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;
using Recherche_donnees_GESTDG;
using System.IO;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Internals;
using System.Collections;
using System.Collections.ObjectModel;
using CarouselView.FormsPlugin.Abstractions;
using Gest.Helpers.Initialise_parametres_recherches_for_navigation_with_prism;
using System.ComponentModel.Design.Serialization;
using System.Runtime.CompilerServices;
using DryIoc;
using Gest.Helpers.Manager_parametre_recherche_sql;

namespace Gest.ViewModels
{
    class MembreActiviteViewModel : BindableBase, INavigationAware
    {

        #region Constructeure
        public MembreActiviteViewModel(INavigationService _service_navigation, IService_Membre _service_membre, IService_Activite _service_activite)
        {
            this.service_membre = _service_membre;
            this.service_activite = _service_activite;
            this.service_navigation = _service_navigation;

            this.Liste_champs = this.Liste_champs_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
            this.Liste_methodesrecherches = Manager_enumeration_methodes_recherches.getmethodes_recherches<Membre>(Database_Initialize.Database_configuration.Database_Initialize().Result.GetConnection(), Champ_selected);
            this.Methoderecherche_selected = Liste_methodesrecherches[0];
            this.Type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Activite service_activite;
        #endregion

        #region Variables
        public string title { get; set; } = "Page des activité des membres";

        private Boolean _isloading = false;
        public Boolean Isloading
        {
            get { return _isloading; }
            set { SetProperty(ref _isloading, value); }
        }

        private Parametre_recherche_sql parametre_recherche_sql = new Parametre_recherche_sql();
        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Activite" }; } }
        public String nom_table_selected { get; set; }

        private List<String> _liste_methodesrecherches;
        public List<String> Liste_methodesrecherches { set { SetProperty(ref _liste_methodesrecherches, value); } get { return _liste_methodesrecherches; } }

        private String _methoderecherche_selected;

        public String Methoderecherche_selected
        {
            get { return _methoderecherche_selected; }
            set { SetProperty(ref _methoderecherche_selected, value); }
        }



        public List<string> Liste_typesrecherches { get { return Enumeration_type_recherche.get_liste_typesrecherches(); } }

        private String _type_selected;

        public String Type_selected
        {
            get { return _type_selected; }
            set { SetProperty(ref _type_selected, value); }
        }

        public List<String> Liste_champs_activites { get { return new List<string>() { "libelle_activite" }; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected, value); }
        }

        private IEnumerable<Membre> _membres;
        public IEnumerable<Membre> membres {
            get
            {
                return _membres;
            }
            set
            {
                SetProperty(ref _membres, value);
            }
        }
        #endregion

        #region Commandes_MVVM_and_methodes

        public ICommand Command_navigation_to_popup_search_multiple
        {
            get
            {
                return new Command(() => {            
                    List<Parametre_recherche_sql> liste_parametre_recherche_with_contains_of_all_namestables_and_nameschamps = get_list_all_parametres_recherches_sql().ToList();
                    NavigationParameters parametre = new NavigationParameters(){
                        {"liste_parametre_recherche_sql",liste_parametre_recherche_with_contains_of_all_namestables_and_nameschamps }
                    };
                    service_navigation.NavigateAsync("Popup_search_multiple", parametre);
                });
            }
        }
        
        private IEnumerable<Parametre_recherche_sql> get_list_all_parametres_recherches_sql()
        {
            IEnumerable<Parametre_recherche_sql> resultat = new List<Parametre_recherche_sql>();
            Dictionary<String, IEnumerable<String>> dictionnary_parametres_recherches_sql = (Dictionary<String, IEnumerable<String>>)get_dictionnary_namestables_with_nameschamps();
            for (var i = 0; i < dictionnary_parametres_recherches_sql.Count; i++)
            {
                resultat = resultat.Concat(dictionnary_parametres_recherches_sql.Values.ToList()[i].Select((champ) => new Parametre_recherche_sql(dictionnary_parametres_recherches_sql.Keys.ToList()[i], champ, null, null))).ToList();
            }
            return resultat;
        }

        private IDictionary<String, IEnumerable<String>> get_dictionnary_namestables_with_nameschamps()
        {
            return new Dictionary<String, IEnumerable<String>>() { { "Membre", Liste_champs_membres }, { "Activite", Liste_champs_activites } };
        }

        public ICommand Command_navigation_to_popup_searchbetweendates
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("name_table", nom_table_selected);
                    service_navigation.NavigateAsync("Popup_search_betweendates", parametre);
                });
            }
        }
        public ICommand Command_switch_source
        {
            get
            {
                return new Command(() => {
                    if (nom_table_selected == "Membre")
                    {
                        Champ_selected = Liste_champs_membres[0];
                        Liste_champs = Liste_champs_membres;
                    }
                    else if (nom_table_selected == "Activite")
                    {
                        Champ_selected = Liste_champs_activites[0];
                        Liste_champs = Liste_champs_activites;
                    }
                });
            }
        }

        public ICommand Command_methodes_recherches
        {
            get
            {
                return new Command(async () =>
                {
                    this.Liste_methodesrecherches = Manager_enumeration_methodes_recherches.getmethodes_recherches<Membre>((await Database_Initialize.Database_configuration.Database_Initialize()).GetConnection(), Champ_selected);
                    this.Methoderecherche_selected = Liste_methodesrecherches[0];
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
                       parametre_recherche_sql, this.nom_table_selected, this.Champ_selected, this.Methoderecherche_selected
                       );
                });
            }
        }

        public ICommand Command_search
        {
            get { 
                return new Command<Object>(async (donnees) => {
                    parametre_recherche_sql.Valeur = donnees;
                    await  launch_load(new List<Parametre_recherche_sql>() {parametre_recherche_sql});
                });
            }
        }
        #endregion

        #region Methodes priver
        private async Task launch_load(IEnumerable parametres_recherches_sql){
            this.Isloading = true;
            IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql = new Gest.Helpers.Generate_dictionnaire_parametresrecherche.Generate_parametresrecherche().generate(parametres_recherches_sql);
            Load_donnees<IEnumerable<Membre>> load_donnees = new Load_donnees__of_viewmodel_membreactivite<IEnumerable<Membre>>(service_membre, service_activite);
            this.membres = await load_donnees.get_donnees(dictionnaire_parametres_sql);
            this.Isloading = false;
        }
       
        #endregion

        #region Methode_navigation_PRISM_and_methodes
        public void OnNavigatedFrom(INavigationParameters parameters){
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters){
            IEnumerable<Parametre_recherche_sql> parametres_recherches_sql = new Initialise_parametres_recherches_for_navigation_with_prism().get_initialise_parametres_recherches_sql_for_navigation_with_prism(parameters, "parametres_recherches_sql");
            await launch_load(parametres_recherches_sql);
        }    
        #endregion
    }
}
