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
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Helpers;
using Xamarin.Forms;

namespace GestDG.ViewsModels
{
    class ViewModel_Visite:BindableBase,INavigationAware
    {
        public string title { get; set; } = "Page des visites des membres";
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Visite service_visite;

        public ViewModel_Visite(INavigationService _service_navigation, IService_Membre _service_membre,IService_Visite _service_visite)
        {
            this.service_navigation = _service_navigation;
            this.service_membre = _service_membre;
            this.service_visite = _service_visite;
        }

        private List<visite_custom> _liste_visites;
        public List<visite_custom> liste_visites
        {
            get
            {
                return _liste_visites;
            }
            set
            {
                SetProperty(ref _liste_visites,value);
            }
        }

        private List<Membre> _liste_membres;
        public  List<Membre> liste_membres
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


        private async Task load()
        {

        }
        public ICommand testcommande
        {
            get
            {
                return new Command<int>(async (position)=>
                {
                    var test = new List<Visite>() { new Visite() { membre_pseudo = "SharQaal", connexion_date = new DateTime(2019, 01, 31)}, new Visite() { membre_pseudo = "SharQaal", connexion_date = new DateTime(2019, 04, 01) }, new Visite() { membre_pseudo = "DG dada", connexion_date = new DateTime(2019, 12, 31) } };
                    List<Visite> liste = (from item in (List<Visite>)test where liste_membres[position].pseudo.ToUpper() == item.membre_pseudo.ToUpper() select item).ToList();
                    liste.ForEach((item)=> {
                        liste_visites.Add(new visite_custom() {connexion_date = item.connexion_date,date_enregistrement=item.date_enregistrement});
                    });
                    
                });
            }
        }


        public ViewModel_Visite()
        {
            //liste_visites = new List<Visite>() { new Visite() { membre_pseudo = "SharQaaL", membre = new Membre() { pseudo = "SharQaaL", age = 27, date_naissance = new DateTime(), date_inscription = new DateTime(), url_site = "", url_avatar = "https://imgfast.net/users/2917/59/36/04/avatars/212-66.jpg", sexe = 'M', localisation = "Amérique", statut = "En ligne", rang = new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png" } }, connexion_date = new DateTime(), connexion = new Connexion() { date_connexion = new DateTime() }, date_enregistrement = new DateTime(2019,03,11) }, new Visite() { membre_pseudo = "DG dada", membre = new Membre() { pseudo = "DG dada", age = 14, date_naissance = new DateTime(), date_inscription = new DateTime(), url_site = "", url_avatar = "https://imgfast.net/users/2917/59/36/04/avatars/112-43.jpg", sexe = 'M', localisation = "Amérique", statut = "En ligne", rang = new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png" }, rang_nom = "DT" }, connexion_date = new DateTime(), connexion = new Connexion() { date_connexion = new DateTime() }, date_enregistrement = new DateTime(2019,03,12) } };
            liste_membres= new List<Membre> { new Membre() { pseudo = "SharQaaL", age = 27, date_naissance = new DateTime(), date_inscription = new DateTime(), url_site = "", url_avatar = "https://7img.net/users/2917/59/36/04/avatars/1-72.png", sexe = "M", localisation = "Amérique", statut = "En ligne", rang = new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png" }, rang_nom = "DT",liste_connexions=new List<Connexion>() { new Connexion() { date_connexion=new DateTime(2019,01,31)},new Connexion() {date_connexion=new DateTime(2019,04,01)} } }, new Membre() { pseudo = "DG dada", age = 14, date_naissance = new DateTime(), date_inscription = new DateTime(), url_site = "", url_avatar = "https://imgfast.net/users/2917/59/36/04/avatars/112-43.jpg", sexe = "M", localisation = "Amérique", statut = "En ligne", rang = new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png" }, rang_nom = "DT" ,liste_connexions= new List<Connexion>() { new Connexion() { date_connexion = new DateTime(2019, 12, 31) }, new Connexion() { date_connexion = new DateTime(2020, 01, 01) } } } };
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
