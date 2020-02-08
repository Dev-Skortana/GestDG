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
        #endregion

        #region Constructeure
        public PosteNBmessageViewModel(INavigationService _service_navigation, IService_Membre _service_membre)
        {
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;
            this.champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables
        public string title { get; set; } = "Page statistic des messages des membres";
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }
        public List<String> Liste_champs { get { return new List<string>() { "pseudo", "url_avatar"}; } }
        public String champ_selected { get; set; }

        private List<Membre> _liste_membres;
        public List<Membre> liste_membres
        {
            get
            {
                 return _liste_membres;
            }
            set
            {
                SetProperty(ref _liste_membres,value);
            }
        }

        private List<Groupement_nombremessage> _liste_dateconnexion_message;

        public List<Groupement_nombremessage> liste_dateconnexion_message
        {
            get { return _liste_dateconnexion_message; }
            set { SetProperty(ref _liste_dateconnexion_message, value); }
        }


        private Groupement_nombremessage _dernier_dateconnexion_message;

        public Groupement_nombremessage dernier_dateconnexion_message
        {
            get { return _dernier_dateconnexion_message; }
            set { SetProperty(ref _dernier_dateconnexion_message,value); }
        }
        #endregion

        #region Commande_MVVM

        public ICommand Command_gestion_dictionnaire_champsmethodesrecherches
        {
            get
            {
                return new Command(() =>
                {
                    if (champ_selected != null)
                    {
                        if (dictionnaire_champs_methodesrecherche != null && dictionnaire_champs_methodesrecherche.ContainsKey(champ_selected))
                        {
                            dictionnaire_champs_methodesrecherche[champ_selected] = methoderecherche_selected;
                        }
                        else
                        {
                            dictionnaire_champs_methodesrecherche.Add(champ_selected, methoderecherche_selected);
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
                    await load(new Dictionary<string, Object>() { { champ_selected, donnees } }, dictionnaire_champs_methodesrecherche, type_selected);

                });
            }
        }

        public Command<int> Command_sync
        {
            get
            {
                return new Command<int>(async (position)=> {

                    List<Membre_Connexion_Message> list_membres_connexions_message = (from item in (List<Membre_Connexion_Message>)await new Services.Classes.Service_Membre_Connexion_Message().GetList(null,null,Enumerations_recherches.types_recherches.Simple) where item.membre_pseudo.ToUpper() == liste_membres[position].pseudo.ToUpper() select item).ToList();
                    list_membres_connexions_message.ForEach((item) => {
                        liste_dateconnexion_message.Add(new Groupement_nombremessage() { Date_connexion = item.connexion_date, Nbmessage = item.message_nb });
                    });
                    DateTime dernier_dateconnexion = list_membres_connexions_message.Max<Membre_Connexion_Message, DateTime>((item) => item.connexion_date);
                    int nombre_messages_max = (from item in list_membres_connexions_message where item.connexion_date == dernier_dateconnexion select item.message_nb).ToList().Max();
                    dernier_dateconnexion_message = new Groupement_nombremessage() {Date_connexion=dernier_dateconnexion,Nbmessage=nombre_messages_max};
                });
            }
        }
        #endregion
      
        #region Methode_priver
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);
            liste_membres = (from item in (List<Membre>)await service_membre.GetList(dictionnaire_donnees, methodes_recherches, type) orderby item.pseudo select item).ToList();
        }
        #endregion

        #region Methodes_naviguations_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(new Dictionary<string, Object>() { { "pseudo", "" } }, new Dictionary<string, string>() { { "pseudo", "Contient" } }, "Simple");
        }
        #endregion
    }
}
