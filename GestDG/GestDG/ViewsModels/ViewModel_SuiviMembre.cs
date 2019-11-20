using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using GestDG.Models;
using GestDG.Services.Classes;
using GestDG.Services.Interfaces;
using GestDG.Helpers;
using System.Threading.Tasks;
using System.Linq;

namespace GestDG.ViewsModels
{
    class ViewModel_SuiviMembre : BindableBase,INavigationAware
    {
        public string title { get; set; } = "Page suivi du membre";

        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Connexion service_connexion;
        private IService_Visite service_visite;
        private IService_Activite service_activite;
        private IService_Message service_message;
        private IService_Rang service_rang;
        private IService_Membre_Connexion_Message service_membre_connexion_message;


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



        public ViewModel_SuiviMembre(IService_Membre _service_membre, IService_Connexion _service_connexion, IService_Visite _service_visite, IService_Activite _service_activite, IService_Message _service_message,IService_Rang _service_rang, IService_Membre_Connexion_Message _service_membre_connexion_message,INavigationService _service_navigation)
        {
            this.service_membre = _service_membre;
            this.service_connexion =_service_connexion;
            this.service_visite = _service_visite;
            this.service_activite = _service_activite;
            this.service_message = _service_message;
            this.service_rang = _service_rang;
            this.service_membre_connexion_message = _service_membre_connexion_message;
            this.service_navigation = _service_navigation; 
        }


        private async Task load(Object donnees=null)
        {
            Membre = (donnees as Membre);

            /* Retrouver les informations des differents ensembles(entité) en liaison avec ce membre. */
            Rang = await service_rang.Get(Membre.rang_nom);
            Activites = new List<Activite>() { new Activite() { membre_pseudo = "Hitman", libelle_activite = "Rangers" }, new Activite() { membre_pseudo = "hitman", libelle_activite = "Manutent" } }; /*(from item in (List<Activite>)await service_activite.GetList("") where item.membre_pseudo.ToUpper()==Membre.pseudo.ToUpper() select item).ToList();*/
            Visites = new List<Visite>() { new Visite() {membre_pseudo="Hitman",connexion_date=new DateTime(2019,05,20) }, new Visite() {membre_pseudo="Hitman",connexion_date=new DateTime(2019,06,14)}}; /*(from item in (List<Visite>)await service_visite.GetList() where item.membre_pseudo.ToUpper()==Membre.pseudo.ToUpper() select item).ToList();*/
            Connexions = new List<Connexion>() { };
            /* A tester ! ! ! */
            Groupement_nombremessage = new List<Groupement_nombremessage>() { new Groupement_nombremessage() { Date_connexion = new DateTime(2019, 05, 20), Nbmessage = 35 }, new Groupement_nombremessage() {Date_connexion=new DateTime(2019, 06, 14),Nbmessage=48 } };/*(from item in (List<Membre_Connexion_Message>)await service_membre_connexion_message.GetList() where item.membre_pseudo.ToUpper() == Membre.pseudo.ToUpper() orderby item.connexion_date descending select new Groupement_nombremessage() {Date_connexion=item.connexion_date,Nbmessage=item.message_nb}).ToList();*/
        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            /* Récuperer le membre */
            await load(parameters["membre"]);
        }
    }
    
}

