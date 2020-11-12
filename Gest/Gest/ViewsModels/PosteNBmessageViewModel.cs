using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using Gest.Models;
using Gest.Services.Interfaces;
using Gest.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;
using Gest.Interface_SQLiteAccess;
using Recherche_donnees_GESTDG;
using Gest.Helpers.Manager_parametre_recherche_sql;
using Gest.Helpers.Load_donnees;
using Gest.Helpers.Initialise_parametres_recherches_for_navigation_with_prism;

namespace Gest.ViewModels
{
    class PosteNBmessageViewModel : BindableBase,INavigationAware
    {
        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Membre_Connexion_Message service_membre_connexion_message;
        #endregion

        #region Constructeure
        public PosteNBmessageViewModel(INavigationService _service_navigation, IService_Membre _service_membre,IService_Membre_Connexion_Message _service_membre_connexion_message)
        {
            this.service_membre = _service_membre;
            this.service_membre_connexion_message = _service_membre_connexion_message;
            this.service_navigation = _service_navigation;

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

        public string title { get; set; } = "Page messages des membres";

        private Boolean _isloading = false;
        public Boolean Isloading
        {
            get { return _isloading; }
            set { SetProperty(ref _isloading, value); }
        }

        private Parametre_recherche_sql parametre_recherche_sql = new Parametre_recherche_sql();

        public List<String> Liste_methodesrecherches { get { return Enumerations_methodes_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }

        public List<string> Liste_typesrecherches { get { return Enumeration_type_recherche.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }

        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Message" }; } }
        public String nom_table_selected { get; set; }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        public List<String> Liste_champs_membre_connexion_message { get { return new List<string>() { "nb_message", "connexion_date" }; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected,value); }
        }


        private IDictionary<Membre, IEnumerable<Groupement_nombremessage>> _dictionnaire_membres_messages = new Dictionary<Membre, IEnumerable<Groupement_nombremessage>>();
        public IDictionary<Membre, IEnumerable<Groupement_nombremessage>> Dictionnaire_membres_messages
        {
            get
            {  
                return _dictionnaire_membres_messages;
            }
            set
            {
                SetProperty(ref _dictionnaire_membres_messages, value);
            }
        }


        private Groupement_nombremessage _dernier_dateconnexion_message=new Groupement_nombremessage();

        public Groupement_nombremessage Dernier_dateconnexion_message
        {
            get { return _dernier_dateconnexion_message; }
            set { SetProperty(ref _dernier_dateconnexion_message,value); }
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
        public ICommand Command_update_dateandtime
        {
            get
            {
                return new Command((donnees) =>
                {

                    if ((donnees is DateTime))
                    {
                        this.Dateandtime = new DateTime(((DateTime)donnees).Year, ((DateTime)donnees).Month, ((DateTime)donnees).Day, this.Dateandtime.TimeOfDay.Hours, this.Dateandtime.TimeOfDay.Minutes, this.Dateandtime.TimeOfDay.Seconds);
                    }
                    else if ((donnees is TimeSpan))
                    {
                        this.Dateandtime = new DateTime(this.Dateandtime.Year, this.Dateandtime.Month, this.Dateandtime.Day, ((TimeSpan)donnees).Hours, ((TimeSpan)donnees).Minutes, ((TimeSpan)donnees).Seconds);
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
                    else if (nom_table_selected == "Message")
                    {
                        Liste_champs = Liste_champs_membre_connexion_message;
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

        public ICommand Command_sync
        {
            get
            {
                return new Command<Membre>((membre) => {
                    if (membre!=null) {
                        DateTime dernier_dateconnexion = Dictionnaire_membres_messages[membre].Max<Groupement_nombremessage, DateTime>((item) => item.Date_connexion);
                        int nombre_messages_max = (from item in Dictionnaire_membres_messages where item.Key.pseudo == membre.pseudo && item.Value.ToList().Exists((groupe_message) => groupe_message.Date_connexion == dernier_dateconnexion) select item.Value.ToList().FindAll((el) => el.Date_connexion == dernier_dateconnexion).Max<Groupement_nombremessage, int>((els) => els.Nbmessage)).First();
                        Dernier_dateconnexion_message = new Groupement_nombremessage() { Date_connexion = dernier_dateconnexion, Nbmessage = nombre_messages_max };
                    }
                });
            }
        }

        #endregion

        #region Methode_priver
        private async Task load(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            this.Isloading = true;
            IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql = new Gest.Helpers.Generate_dictionnaire_parametresrecherche.Generate_parametresrecherche().generate(parametres_recherches_sql);
            Load_donnees<IDictionary<Membre, IEnumerable<Groupement_nombremessage>>> load_donnees = new Load_donnees_of_viewmodel_membreconnectionmessage<IDictionary<Membre, IEnumerable<Groupement_nombremessage>>>(service_membre, service_membre_connexion_message);
            this.Dictionnaire_membres_messages = await load_donnees.get_donnees(dictionnaire_parametres_sql);
            this.Isloading = false;
        }

        public async Task navigation_Goback_Popup_searchbetweendates(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            await load(parametres_recherches_sql);
        }
        #endregion

        #region Methodes_naviguations_PRISM
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
