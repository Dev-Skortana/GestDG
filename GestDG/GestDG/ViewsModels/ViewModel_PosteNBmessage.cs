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

namespace GestDG.ViewsModels
{
    class ViewModel_PosteNBmessage:BindableBase,INavigationAware
    {
        public string title { get; set; } = "Page statistic des messages des membres";
        private INavigationService service_navigation;
        private IService_Membre service_membre;


        public Command<int> test
        {
            get
            {
                return new Command<int>(async (position)=> {

                    List<Membre_Connexion_Message> list_membres_connexions_message = (from item in (List<Membre_Connexion_Message>)await new Services.Classes.Service_Membre_Connexion_Message().GetList() where item.membre_pseudo.ToUpper() == liste_membres[position].pseudo.ToUpper() select item).ToList();
                    DateTime dernier_dateconnexion = list_membres_connexions_message.Max<Membre_Connexion_Message, DateTime>((item) => item.connexion_date);
                    dernier_dateconnexion_message = new Groupement_nombremessage() {Date_connexion=dernier_dateconnexion,Nbmessage=(from item in list_membres_connexions_message where item.connexion_date==dernier_dateconnexion select item.message_nb).ToList().Max()};
                });
            }
        } 
        public ViewModel_PosteNBmessage(INavigationService _service_navigation, IService_Membre _service_membre)
        {
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;
            load();
        }


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

        private Groupement_nombremessage _dernier_dateconnexion_message;

        public Groupement_nombremessage dernier_dateconnexion_message
        {
            get { return _dernier_dateconnexion_message; }
            set { SetProperty(ref _dernier_dateconnexion_message,value); }
        }

        //private List<Membre_Connexion_Message> _liste_membres_connexions_messages;

        //public List<Membre_Connexion_Message> liste_membres_connexions_messages
        //{
        //    get { return _liste_membres_connexions_messages; }
        //    set { _liste_membres_connexions_messages = value; }
        //}


        private async Task load(String pseudo = "")
        {
            liste_membres = new List<Membre> { new Membre() { pseudo = "SharQaaL", url_avatar = "https://7img.net/users/2917/59/36/04/avatars/1-72.png", liste_messagesnb=new List<Message>() { new Message() { nb_message=100},new Message() { nb_message=150} } }, new Membre() { pseudo = "DG dada", url_avatar = "https://imgfast.net/users/2917/59/36/04/avatars/112-43.jpg" ,liste_messagesnb=new List<Message>() { new Message() {nb_message=250},new Message() { nb_message=300} } }};
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
    }
}
