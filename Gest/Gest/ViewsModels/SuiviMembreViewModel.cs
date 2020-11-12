using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using Gest.Models;
using Gest.Services.Classes;
using Gest.Services.Interfaces;
using Gest.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;
using Gest.Interface_SQLiteAccess;
using Recherche_donnees_GESTDG;
using Xamarin.Forms.Internals;

namespace Gest.ViewModels
{
    class SuiviMembreViewModel : BindableBase,INavigationAware
    {

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Connexion service_connexion;
        private IService_Visite service_visite;
        private IService_Activite service_activite;
        private IService_Message service_message;
        private IService_Rang service_rang;
        private IService_Membre_Connexion_Message service_membre_connexion_message;
        #endregion

        #region Variables

        private DateTime _dateandtime;

        public DateTime Dateandtime
        {
            get { return _dateandtime; }
            set { SetProperty(ref _dateandtime, value); }
        }

        public string title { get; set; } = "Page suivi du membre";
        private Parametre_recherche_sql parametre_recherche_sql = new Parametre_recherche_sql();

        public List<String> Liste_methodesrecherches { get { return Enumerations_methodes_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }

        public List<string> Liste_typesrecherches { get { return Enumeration_type_recherche.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }

        public List<String> Liste_noms_tables_recherche { get { return new List<string>() { "Activite", "Visite", "Message" }; } }
        public String nom_table_selected { get; set; }



        public List<String> Liste_champs_activites { get { return new List<string>() { "libelle_activite" }; } }
        public List<String> Liste_champs_visite { get { return new List<string>() { "connexion_date", "date_enregistrement" }; } }
        public List<String> Liste_champs_membre_connexion_message { get { return new List<string>() { "nb_message", "connexion_date" }; } }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected, value); }
        }

        private Membre _membre;

        public Membre Membre
        {
            get { return _membre; }
            set { SetProperty(ref _membre,value); }
        }

        private List<Connexion> _connexions;

        public List<Connexion> Connexions
        {
            get { return _connexions; }
            set { SetProperty(ref _connexions, value); }
        }

        private List<Visite> _visites;

        public List<Visite> Visites
        {
            get { return _visites; }
            set { SetProperty(ref _visites,value); }
        }

        private List<Activite> _activites;

        public List<Activite> Activites
        {
            get { return _activites; }
            set { SetProperty(ref _activites,value); }
        }

        private Rang _rang;

        public Rang Rang
        {
            get { return _rang; }
            set { SetProperty(ref _rang,value); }
        }

        private List<Message> _messages;

        public List<Message> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages,value); }
        }

        private List<Membre_Connexion_Message> _membre_connexion_messages;

        public List<Membre_Connexion_Message> Membre_Connexion_Messages
        {
            get { return _membre_connexion_messages; }
            set { SetProperty(ref _membre_connexion_messages,value); }
        }

        private List<Groupement_nombremessage> _groupement_nombremessage;

        public List<Groupement_nombremessage> Groupement_nombremessage
        {
            get { return _groupement_nombremessage; }
            set { SetProperty(ref _groupement_nombremessage,value); }
        }
        #endregion

        #region Command_MVVM

        public ICommand Command_navigation_to_popup_searchmultiple
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    Dictionary<String, IEnumerable<String>> dictionnaire_champs = (Dictionary<String, IEnumerable<String>>)get_dictionnary_champs();
                    List<Parametre_recherche_sql> liste_all_parametres = (List<Parametre_recherche_sql>)get_list_parametre_recherche_sql(dictionnaire_champs);
                    parametre.Add("liste", liste_all_parametres);
                    service_navigation.NavigateAsync("Popup_search_multiple", parametre);
                });
            }
        }

        public ICommand Command_navigation_to_popup_searchbetweendates
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("names_tables", this.Liste_noms_tables_recherche);
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
                    if (nom_table_selected == "Activite")
                    {
                        Liste_champs = Liste_champs_activites;
                    }
                    else if (nom_table_selected == "Visite")
                    {
                        Liste_champs = Liste_champs_visite;
                    }else if (nom_table_selected == "Message")
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
                    
                });
            }
        }

        public ICommand Command_search
        {
            get
            {
                return new Command<Object>(async (donnees) => {
                   

                });
            }
        }
        #endregion

        #region Constructeure
        public SuiviMembreViewModel(IService_Membre _service_membre, IService_Connexion _service_connexion, IService_Visite _service_visite, IService_Activite _service_activite, IService_Message _service_message,IService_Rang _service_rang, IService_Membre_Connexion_Message _service_membre_connexion_message,INavigationService _service_navigation)
        {
            this.service_membre = _service_membre;
            this.service_connexion =_service_connexion;
            this.service_visite = _service_visite;
            this.service_activite = _service_activite;
            this.service_message = _service_message;
            this.service_rang = _service_rang;
            this.service_membre_connexion_message = _service_membre_connexion_message;
            this.service_navigation = _service_navigation;

            this.Liste_champs = this.Liste_champs_activites;
            this.nom_table_selected = "Activite";

                    
            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Methode_priver

        private IDictionary<String, IEnumerable<String>> get_dictionnary_champs()
        {
            return new Dictionary<String, IEnumerable<String>>() { { "Activite", Liste_champs_activites }, { "Visite", Liste_champs_visite }, { "Message",Liste_champs_membre_connexion_message } };
        }
        private IEnumerable<Parametre_recherche_sql> get_list_parametre_recherche_sql(Dictionary<String, IEnumerable<String>> dictionnaire_liste_champs)
        {
            IEnumerable<Parametre_recherche_sql> resultat = new List<Parametre_recherche_sql>();
            for (var i = 0; i < dictionnaire_liste_champs.Count; i++)
            {
                resultat = resultat.Concat(dictionnaire_liste_champs.Values.ToList()[i].Select((champ) => new Parametre_recherche_sql(dictionnaire_liste_champs.Keys.ToList()[i], champ, null, null))).ToList();
            }
            return resultat;
        }

        private async Task updates_donnees(String type_recherche, IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres)
        {
            Dictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> dictionnaire_get_conditions_parametres_recherches_sql = new Dictionary<string, Func<IEnumerable<Parametre_recherche_sql>>>();
            dictionnaire_get_conditions_parametres_recherches_sql.Add("get_condition_parametres_recherches_sql_activite",()=>null);
            dictionnaire_get_conditions_parametres_recherches_sql.Add("get_condition_parametres_recherches_sql_visite", () =>null);
            dictionnaire_get_conditions_parametres_recherches_sql.Add("get_condition_parametres_recherches_sql_message", () =>null);
            if (type_recherche == "Simple")
            {            
                var liste_parametre = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Table_selected"].ToList();
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_activite"] =() => nom_table_selected == "Activite" ? liste_parametre : null;
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_visite"]=() => nom_table_selected == "Visite" ? liste_parametre : null;
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_message"]=() => nom_table_selected == "Message" ? liste_parametre : null;
            }
            else if (type_recherche == "Multiples")
            {
                var liste_parametre_activite = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Activite"].ToList();
                var liste_parametre_visite = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Visite"].ToList();
                var liste_parametre_message = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Message"].ToList();
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_activite"]=() => liste_parametre_activite.Count > 0 ? liste_parametre_activite : null;
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_visite"]=() => liste_parametre_visite.Count > 0 ? liste_parametre_visite : null;
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_message"]=() => liste_parametre_message.Count > 0 ? liste_parametre_message : null;             
            }

            Activites = new List<Activite>();
            Activites = (from item in (List<Activite>)await service_activite.GetList(dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_activite"].Invoke()) where item.membre_pseudo.ToUpper() == Membre.pseudo.ToUpper() select item).ToList();

            Visites = new List<Visite>();
            Visites = (from item in (List<Visite>)await service_visite.GetList(dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_visite"].Invoke()) where item.membre_pseudo.ToUpper() == Membre.pseudo.ToUpper() select item).ToList();

            Groupement_nombremessage = new List<Groupement_nombremessage>();
            Groupement_nombremessage = (from item in (List<Membre_Connexion_Message>)await service_membre_connexion_message.GetList(dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_message"].Invoke()) where item.membre_pseudo.ToUpper() == Membre.pseudo.ToUpper() orderby item.connexion_date descending select new Groupement_nombremessage() { Date_connexion = item.connexion_date, Nbmessage = item.message_nb }).ToList();
        }

        private Dictionary<String, IEnumerable<Parametre_recherche_sql>> get_dictionnary_parametrerecherchesql_trier(IEnumerable<Parametre_recherche_sql> liste_parametre_general, IDictionary<String, IEnumerable<String>> dictionnaire_nomtable_listeparametres)
        {
            Dictionary<String, IEnumerable<Parametre_recherche_sql>> resultat = new Dictionary<string, IEnumerable<Parametre_recherche_sql>>();
            dictionnaire_nomtable_listeparametres.ForEach((item) => {
                resultat.Add(item.Key, (liste_parametre_general.Where((parametre) => item.Value.Contains(parametre.Champ) && parametre.Nom_table == item.Key)));
            });
            return resultat;
        }





        public async Task navigation_Goback_Popup_searchbetweendates(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            await load(parametres_recherches_sql);
        }

        public async Task navigation_Goback_Popup_searchmultiple(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            Dictionary<String, IEnumerable<String>> dictionnaire_nomtable_listechamps = (Dictionary<String, IEnumerable<String>>)get_dictionnary_champs();
            Dictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres = get_dictionnary_parametrerecherchesql_trier(parametres_recherches_sql, dictionnaire_nomtable_listechamps);
            await updates_donnees(type_selected, dictionnaire_parametres);

        }








        private async Task get_rang()
        {
            Rang = new Rang();
            Rang = (from item in await service_rang.GetList(null) where item.nom_rang.ToUpper() == Membre.rang_nom?.ToUpper() select item).FirstOrDefault();
        }
        private async Task load(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            await updates_donnees(type_selected, new Dictionary<String, IEnumerable<Parametre_recherche_sql>>() { { "Table_selected", parametres_recherches_sql } });
        }

     
        #endregion

        #region Methode_naviguation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            Membre = parameters["Membre"] as Membre;
            await get_rang();
            await load(new List<Parametre_recherche_sql>());
        }
        #endregion
    }

}

