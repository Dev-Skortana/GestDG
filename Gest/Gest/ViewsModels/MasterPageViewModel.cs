using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;
using Prism.Commands;
using Gest.Views;
using Gest.Services.Classes;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Gest.ViewModels
{
    class MasterPageViewModel:INavigationAware
    {
        #region Services
        private INavigationService service_navigation;
        private RestService service_login;
        #endregion

        #region Variables
        public string title { get; set; } = "Menu de naviguation [ Application Beta ]";
        #endregion

        #region Constructeure
        public MasterPageViewModel(INavigationService nav, RestService service_login)
        { 
            this.service_navigation = nav;
            this.service_login = service_login;
        }
        #endregion

        #region Commandes_MVVM
        public DelegateCommand getpage_registermembre
        {
            get
            {
                Action methode;
                if (service_login.cookie_connexion!=null)
                {
                    methode = async () => await service_navigation.NavigateAsync("RegisterMembre");
                }else
                {
                    methode = async () => await service_navigation.NavigateAsync("LoginPage");
                }

                return new DelegateCommand(methode);
            }
        }

        public DelegateCommand getpage_showmembre
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Membre"); });
            }
        }

        
        public DelegateCommand getpage_showmembrevisite
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("MembreVisite"); });
            }
        }

        public DelegateCommand getpage_showmembreactivite
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("MembreActivite"); });
            }
        }
        public DelegateCommand getpage_showmembrerang
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("RangMembre"); });
            }
        }
        public DelegateCommand getpage_postenbmessage
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("PosteNBmessage"); });
            }
        }
        public DelegateCommand getpage_showviewfastmembre
        {
            get
            {
                return new DelegateCommand(async () => { await service_navigation.NavigateAsync("FastMembre"); });
            }
        }

        public DelegateCommand command_clear_all_members_and_them_infos
        {
            get
            {
                return new DelegateCommand(async ()=> {
                    if (await Application.Current.MainPage.DisplayAlert("Confirmation","en appuyant sur \"OK\" tous les membres et leurs informations seront supprimer.\n\t Cette action est ireverssible", "OK", "Annuler")){
                        this.clear_all_members_and_them_infos();
                    }
                });
            }
        }

        private void clear_all_members_and_them_infos()
        {
            Service_database service_database = new Service_database();
            service_database.clear_all_members_and_them_infos();
        }
        #endregion

        #region Methodes_naviguation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {       
                
        }
        #endregion
    }
}
