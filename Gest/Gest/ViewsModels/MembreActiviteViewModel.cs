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
            this.methoderecherche_selected = Liste_methodesrecherches[0];
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
        private Parametre_recherche_sql parametre_recherche_sql = new Parametre_recherche_sql();
        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Activite" }; } }
        public String nom_table_selected { get; set; }

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }

        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }

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
                    Dictionary<String, IEnumerable<String>> dictionnary_namestables_with_nameschamps = (Dictionary<String, IEnumerable<String>>)get_dictionnary_namestables_with_nameschamps();
                    List<Parametre_recherche_sql> liste_parametre_recherche_with_contains_of_all_namestables_and_nameschamps = get_list_all_parametres_recherches_sql(dictionnary_namestables_with_nameschamps).ToList();
                    NavigationParameters parametre = new NavigationParameters(){
                        {"liste_parametre_recherche_sql",liste_parametre_recherche_with_contains_of_all_namestables_and_nameschamps }
                    };
                    service_navigation.NavigateAsync("Popup_search_multiple", parametre);
                });
            }
        }
        private IDictionary<String, IEnumerable<String>> get_dictionnary_namestables_with_nameschamps()
        {
            return new Dictionary<String, IEnumerable<String>>() { { "Membre", Liste_champs_membres }, { "Activite", Liste_champs_activites } };
        }

        private IEnumerable<Parametre_recherche_sql> get_list_all_parametres_recherches_sql(Dictionary<String, IEnumerable<String>> dictionnaire_liste_champs)
        {
            IEnumerable<Parametre_recherche_sql> resultat = new List<Parametre_recherche_sql>();
            for (var i = 0; i < dictionnaire_liste_champs.Count; i++)
            {
                resultat = resultat.Concat(dictionnaire_liste_champs.Values.ToList()[i].Select((champ) => new Parametre_recherche_sql(dictionnaire_liste_champs.Keys.ToList()[i], champ, null, null))).ToList();
            }
            return resultat;
        }

        public ICommand Command_navigation_to_popup_searchbetweendates
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters(){
                        {"champ",Champ_selected }
                    };
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
                        Liste_champs = Liste_champs_membres;
                    }
                    else if (nom_table_selected == "Activite")
                    {
                        Liste_champs = Liste_champs_activites;
                    }
                    Champ_selected = Liste_champs[0];
                });
            }
        }

        private void change_parametre_recherche_sql() {
            parametre_recherche_sql.Nom_table = this.nom_table_selected;
            parametre_recherche_sql.Champ = this.Champ_selected;
            parametre_recherche_sql.Methode_recherche = this.methoderecherche_selected;          
        }
        public ICommand Command_gestion_dictionnaire_champsmethodesrecherches
        {
            get
            {
                return new Command(() =>
                {
                    this.change_parametre_recherche_sql();
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
            IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql = new Gest.Helpers.Generate_dictionnaire_parametresrecherche.Generate_parametresrecherche().generate(parametres_recherches_sql);
            Load_donnees<IEnumerable<Membre>> load_donnees = new Load_donnees__of_viewmodel_membreactivite<IEnumerable<Membre>>(service_membre, service_activite);
            this.membres = await load_donnees.get_donnees(dictionnaire_parametres_sql);
        }
        
        #endregion

        #region Methode_navigation_PRISM_and_methodes
        public void OnNavigatedFrom(INavigationParameters parameters){
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters){
            IEnumerable<Parametre_recherche_sql> parametres_recherches_sql = new Initialise_parametres_recherches().get_initialise_parametres_recherches_sql(parameters, "parametres_recherches_sql");
            await launch_load(parametres_recherches_sql);
        }    
        #endregion
    }
}
