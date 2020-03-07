using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;

namespace GestDG.ViewModels
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
            this.dictionnaire_champs_methodesrecherche = this.dictionnaire_champs_methodesrecherche_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables
        public string title { get; set; } = "Page messages des membres";
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche_membres = new Dictionary<string, string>();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche_messages = new Dictionary<string, string>();

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }

        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }

        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Membre_connexion_message" }; } }
        public String nom_table_selected { get; set; }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        public List<String> Liste_champs_message { get { return new List<string>() { "message_nb" }; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected,value); }
        }


        private Dictionary<Membre, List<Groupement_nombremessage>> _dictionnaire_membres_messages = new Dictionary<Membre, List<Groupement_nombremessage>>();
        public Dictionary<Membre, List<Groupement_nombremessage>> Dictionnaire_membres_messages
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

        public ICommand Command_switch_source
        {
            get
            {
                return new Command(() => {
                    if (nom_table_selected == "Membre")
                    {
                        Liste_champs = Liste_champs_membres;
                        dictionnaire_champs_methodesrecherche = dictionnaire_champs_methodesrecherche_membres;
                    }
                    else if (nom_table_selected == "Membre_connexion_message")
                    {
                        Liste_champs = Liste_champs_message;
                        dictionnaire_champs_methodesrecherche = dictionnaire_champs_methodesrecherche_messages;
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
                    if (this.Champ_selected != null)
                    {
                        if (dictionnaire_champs_methodesrecherche != null && dictionnaire_champs_methodesrecherche.ContainsKey(this.Champ_selected))
                        {
                            dictionnaire_champs_methodesrecherche[this.Champ_selected] = methoderecherche_selected;
                        }
                        else
                        {
                            dictionnaire_champs_methodesrecherche.Add(this.Champ_selected, methoderecherche_selected);
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
                    await load(new Dictionary<string, Object>() { { this.Champ_selected, donnees } }, dictionnaire_champs_methodesrecherche, type_selected);
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
                        int nombre_messages_max = (from item in Dictionnaire_membres_messages where item.Key.pseudo == membre.pseudo && item.Value.Exists((groupe_message) => groupe_message.Date_connexion == dernier_dateconnexion) select item.Value.FindAll((el) => el.Date_connexion == dernier_dateconnexion).Max<Groupement_nombremessage, int>((els) => els.Nbmessage)).First();
                        Dernier_dateconnexion_message = new Groupement_nombremessage() { Date_connexion = dernier_dateconnexion, Nbmessage = nombre_messages_max };
                    }
                });
            }
        }

        #endregion

        #region Methode_priver
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);
            Dictionary<Membre, List<Groupement_nombremessage>> dictionnaire_intermediaire = new Dictionary<Membre, List<Groupement_nombremessage>>();

            var liste_membres = new List<Membre>();
            liste_membres = (from item in (List<Membre>)await service_membre.GetList(nom_table_selected == "Membre" ? dictionnaire_donnees : new Dictionary<string, Object>() { { "pseudo", "" } }, nom_table_selected == "Membre" ? methodes_recherches : new Dictionary<string, string>() { { "pseudo", "Contient" } }, type) orderby item.pseudo select item).ToList();

            var liste_membre_connexion_messages = new List<Membre_Connexion_Message>();
            liste_membre_connexion_messages = (from item in (List<Membre_Connexion_Message>)await service_membre_connexion_message.GetList(nom_table_selected == "Membre_connexion_message" ? dictionnaire_donnees : null, nom_table_selected == "Membre_connexion_message" ? methodes_recherches : null, type) select item).ToList();

            if (nom_table_selected == "Membre_connexion_message")
            {
                liste_membres.RemoveAll((membre) => liste_membre_connexion_messages.Exists((membre_connexion_message) => membre_connexion_message.membre_pseudo == membre.pseudo) == false);
            }

            liste_membres.ForEach((membre) => { dictionnaire_intermediaire.Add(membre, liste_membre_connexion_messages.Where((membre_connexion_message) => membre_connexion_message.membre_pseudo == membre.pseudo).Select<Membre_Connexion_Message, Groupement_nombremessage>((membre_connexion_message) => new Groupement_nombremessage() { Date_connexion = membre_connexion_message.connexion_date, Nbmessage = membre_connexion_message.message_nb }).ToList()); });

            Dictionnaire_membres_messages = dictionnaire_intermediaire;
        }
        #endregion

        #region Methodes_naviguations_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(null,null,"Simple");
        }
        #endregion
    }
}
