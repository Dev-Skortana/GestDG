using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Navigation;
using Gest.Models;
using Gest.Services.Interfaces;
using Gest.Helpers;
using Xamarin.Forms;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;
using Xamarin.Forms.Internals;
using ImTools;
using System.Collections;
using Gest.Helpers.Load_donnees;
using Gest.Helpers.Initialise_parametres_recherches_for_navigation_with_prism;
using Xamarin.Forms.Markup;

namespace Gest.ViewModels
{
    class MembreVisiteViewModel : BindableBase, INavigationAware
    {

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Visite service_visite;
        #endregion

        #region Constructeure
        public MembreVisiteViewModel(INavigationService _service_navigation, IService_Membre _service_membre, IService_Visite _service_visite)
        {
            this.service_navigation = _service_navigation;
            this.service_membre = _service_membre;
            this.service_visite = _service_visite;

            this.Liste_champs = this.Liste_champs_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables

        private DateTime _dateandtime;

        public DateTime Dateandtime
        {
            get { return _dateandtime; }
            set { SetProperty(ref _dateandtime, value); }
        }


        public string title { get; set; } = "Page des visites des membres";
        private Parametre_recherche_sql parametre_recherche_sql = new Parametre_recherche_sql();

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }

        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Visite" }; } }
        public String nom_table_selected { get; set; }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        public List<String> Liste_champs_visite { get { return new List<string>() { "connexion_date", "date_enregistrement" }; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected, value); }
        }


        private IDictionary<Membre, IEnumerable<visite_custom>> _dictionnaire_membre_visite = new Dictionary<Membre,IEnumerable<visite_custom>>();
        public IDictionary<Membre, IEnumerable<visite_custom>> Dictionnaire_membre_visite
        {
            get
            {
                return _dictionnaire_membre_visite;
            }
            set
            {
                SetProperty(ref _dictionnaire_membre_visite, value);
            }
        }

        #endregion

        #region Methode_priver

        private async Task launch_load(IEnumerable parametres_recherches_sql)
        {
            IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql = new Gest.Helpers.Generate_dictionnaire_parametresrecherche.Generate_parametresrecherche().generate(parametres_recherches_sql);
            
            Load_donnees<IDictionary<Membre, IEnumerable<visite_custom>>> load_donnees = new Load_donnees_of_viewmodel_membrevisite<IDictionary<Membre, IEnumerable<visite_custom>>>(service_membre, service_visite);
            this.Dictionnaire_membre_visite = await load_donnees.get_donnees(dictionnaire_parametres_sql);
        }
          
        #endregion

        #region Commandes_MVVM

        public ICommand Command_navigation_to_popup_search_multiple
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    List<Parametre_recherche_sql> liste_all_parametres = (List<Parametre_recherche_sql>)get_list_parametre_recherche_sql();
                    parametre.Add("liste", liste_all_parametres);
                    service_navigation.NavigateAsync("Popup_search_multiple", parametre);
                });
            }
        }
        
        private IEnumerable<Parametre_recherche_sql> get_list_parametre_recherche_sql()
        {
            IEnumerable<Parametre_recherche_sql> resultat = new List<Parametre_recherche_sql>();
            Dictionary<String, IEnumerable<String>> dictionnaire_parametres_recherches_sql= (Dictionary<String, IEnumerable<String>>)get_dictionnary_champs();
            for (var i = 0; i < dictionnaire_parametres_recherches_sql.Count; i++)
            {
                resultat = resultat.Concat(dictionnaire_parametres_recherches_sql.Values.ToList()[i].Select((champ) => new Parametre_recherche_sql(dictionnaire_parametres_recherches_sql.Keys.ToList()[i], champ, null, null))).ToList();
            }
            return resultat;
        }

        private IDictionary<String, IEnumerable<String>> get_dictionnary_champs()
        {
            return new Dictionary<String, IEnumerable<String>>() { { "Membre", Liste_champs_membres }, { "Visite", Liste_champs_visite } };
        }

        public ICommand Command_navigation_to_popup_searchbetweendates
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("champ", Champ_selected);
                    service_navigation.NavigateAsync("Popup_search_betweendates", parametre);
                });
            }
        }
        public ICommand Command_update_dateandtime
        {
            get
            {
                return new Command((donnees)=>
                {

                    if ((donnees is DateTime))
                    {
                        this.Dateandtime =new DateTime(((DateTime)donnees).Year, ((DateTime)donnees).Month, ((DateTime)donnees).Day,this.Dateandtime.TimeOfDay.Hours, this.Dateandtime.TimeOfDay.Minutes, this.Dateandtime.TimeOfDay.Seconds);
                    }else if ((donnees is TimeSpan))
                    {
                        this.Dateandtime = new DateTime(this.Dateandtime.Year,this.Dateandtime.Month,this.Dateandtime.Day,((TimeSpan)donnees).Hours,((TimeSpan)donnees).Minutes,((TimeSpan)donnees).Seconds);
                    }

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
                    else if (nom_table_selected == "Visite")
                    {
                        Liste_champs = Liste_champs_visite;
             
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
                    this.refresh_parametre_recherche_sql();
                });
            }
        }

        private void refresh_parametre_recherche_sql()
        {
            parametre_recherche_sql.Nom_table = this.nom_table_selected;
            parametre_recherche_sql.Champ = this.Champ_selected;
            parametre_recherche_sql.Methode_recherche = this.methoderecherche_selected;
        }
        public ICommand Command_search
        {
            get
            {
                return new Command<Object>(async (donnees) => {
                    parametre_recherche_sql.Valeur = donnees;
                    await launch_load(new List<Parametre_recherche_sql>() {parametre_recherche_sql});
                });
            }
        }
        #endregion

        #region Methodes_naviguations_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            IEnumerable<Parametre_recherche_sql> parametres_recherches_sql = new Initialise_parametres_recherches().get_initialise_parametres_recherches_sql(parameters, "parametres_recherches_sql");
            await launch_load(parametres_recherches_sql);
        }
       #endregion
    }
}
