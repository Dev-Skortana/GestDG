using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Commands;
using GestDG.Models;
using GestDG.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace GestDG.ViewsModels
{
    class ViewModel_RangMembre:BindableBase,INavigationAware
    {
        public string title { get; set; } = "Page rangs des membres";
        private INavigationService service_navigation;
        private IService_Rang service_rang;
        private IService_Membre service_membre;

        public ViewModel_RangMembre(INavigationService _service_navigation, IService_Rang _service_rang,IService_Membre _service_membre)
        {
            this.service_rang = _service_rang;
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;
        }

        public Command cmd
        {

            get
            {
                return new Command<String>(async(nom_rang)=>
                {
                    await load(nom_rang);
                });
            } 
        }

        private List<Rang> _liste_rangs;
        public List<Rang> liste_rangs
        {
            get
            {
                return _liste_rangs;
            }
            set
            {
                SetProperty(ref _liste_rangs,value);
            }
        }

        private async Task load(String nom_rang="")
        {
            //liste_rangs = new List<Rang>() { new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png", liste_membres = new List<Membre>() { new Membre() { pseudo = "SharQaaL", url_avatar = "https://7img.net/users/2917/59/36/04/avatars/1-72.png" }, new Membre() { pseudo = "DG dada", url_avatar = "https://imgfast.net/users/2917/59/36/04/avatars/112-43.jpg" } } }, new Rang() { nom_rang = "MS", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/manage10.png", liste_membres = new List<Membre>() { new Membre() { pseudo = "kanibal kombai", url_avatar = "https://imgfast.net/users/2917/59/36/04/avatars/4-91.jpg" }, new Membre() { pseudo = "Kratos", url_avatar = "https://imgfast.net/users/2917/59/36/04/avatars/6-76.jpg" } } } } ;
            liste_rangs = (List<Rang>)await new Services.Classes.Service_Rang().GetList(nom_rang);
            //liste_rangs.ForEach(async (item) =>item.liste_membres= (from el in (List<Membre>)await new Services.Classes.Service_Membre().GetList("") where el.rang_nom==item.nom_rang orderby el.pseudo select el).ToList());
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load();
        }
    }
}
