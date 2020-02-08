using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;
using Prism.Commands;
using GestDG.Views;
using GestDG.Services.Classes;

namespace GestDG.ViewModels
{
    class MasterPageViewModel:INavigationAware
    {
        private INavigationService service_navigation;
        RestService service_login;
        public MasterPageViewModel(INavigationService nav, RestService service_login)
        { 
            this.service_navigation = nav;
            this.service_login = service_login;
        }


        public DelegateCommand getpage_registermembre
        {
            get
            {
                Action methode;
                if (service_login.cookie_connexion!=null)
                {
                    methode = async () => await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/RegisterMembre");
                }else
                {
                    methode = async () => await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/LoginPage");
                }

                return new DelegateCommand(methode);
            }
        }

        public DelegateCommand getpage_showmembre
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/Membre"); });
            }
        }

        
        public DelegateCommand getpage_showmembrevisite
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/MembreVisite"); });
            }
        }

        public DelegateCommand getpage_showmembreactivite
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/MembreActivite"); });
            }
        }
        public DelegateCommand getpage_showmembrerang
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/RangMembre"); });
            }
        }
        public DelegateCommand getpage_postenbmessage
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/PosteNBmessage"); });
            }
        }
        public DelegateCommand getpage_showviewfastmembre
        {
            get
            {
                return new DelegateCommand(async () => { await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/FastMembre"); });
            }
        }
        
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
         
        }
    }
}
