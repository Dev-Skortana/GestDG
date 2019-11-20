using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;
using Prism.Navigation;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Services.Classes;
using GestDG.Interface_File_Image_Access;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace GestDG.ViewsModels
{
    class ViewModel_Membre : BindableBase,INavigationAware
    {

        public String title { get; set; } = "Page des membres";
        private INavigationService service_navigation;
        private IService_Membre service_membre;

        public ViewModel_Membre(INavigationService _service_navigation, IService_Membre _service_membre)
        {
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;
        }

        public ICommand Cmd
        {
            get {
                return new Command<String>(async (pseudo)=> {
                   await load(pseudo);
                });
            }
        }

        private List<Membre> _liste_membres;
        public List<Membre> Liste_membres
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

        private async Task load(String pseudo="")
        {
           //await service_membre.delete(new Membre());
            //_liste_membres = new List<Membre> { new Membre() { pseudo = "SharQaaL", age = 27, date_naissance = new DateTime(), date_inscription = new DateTime(), url_site = "", url_avatar =await DependencyService.Get<IFilePicture_Access>().save_picture(element_commun.type_image_membre.Avatar) + "/1-72.png", sexe = 'M', localisation = "Amérique", statut = "En ligne", rang = new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png" }, rang_nom = "DT" }, new Membre() { pseudo = "DG dada", age = 14, date_naissance = new DateTime(), date_inscription = new DateTime(), url_site = "", url_avatar = await DependencyService.Get<IFilePicture_Access>().save_picture(element_commun.type_image_membre.Avatar) + "/112-43.jpg", sexe = 'M', localisation = "Amérique", statut = "En ligne", rang = new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png" }, rang_nom = "DT" } };
            Liste_membres = (List<Membre>)await service_membre.GetList(pseudo); ;//(List<Membre>)await service_membre.GetList("");
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load();
        }
    }
}
