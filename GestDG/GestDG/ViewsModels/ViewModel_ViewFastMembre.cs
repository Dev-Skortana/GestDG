using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Windows.Input;
using System.ComponentModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Runtime.CompilerServices;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Services.Classes;
using System.Threading.Tasks;

namespace GestDG.ViewsModels
{
    class ViewModel_ViewFastMembre:BindableBase,INavigationAware
    {

        public String title { set; get; } = "Vue rapide des membres";

        private IService_Membre service_membre;
        private INavigationService service_naviguation;

        public ViewModel_ViewFastMembre(IService_Membre _service_membre,INavigationService _service_navigation)
        {
            this.service_membre = _service_membre;
            this.service_naviguation = _service_navigation;
        }

        private List<Membre> _liste_membres;
        public List<Membre> Liste_membres {
            get
            {
                return _liste_membres;
            }
            set
            {
                SetProperty(ref _liste_membres,value);
            }
        }

        /* Permettre la naviguation vers la page suivi du membre avec la transmission de l'objet membre en respectant le pattern MVVM(Model View ViewModel) */
        public Command<Membre> getpage_membre
        {
            get
            {
                return new Command<Membre>(async(objet)=> {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("membre",objet);
                    await service_naviguation.NavigateAsync("BaseNaviguationPage/ShowViewSuiviMembre",parametre);
                });
            }
        }

        private async Task load(String pseudo="")
        {
            Liste_membres = (List<Membre>)await service_membre.GetList(pseudo);
        }
 
        public async void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load();
        }
    }
}
