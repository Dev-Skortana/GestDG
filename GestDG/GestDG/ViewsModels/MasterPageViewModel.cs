using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;
using Prism.Commands;
using GestDG.Views;

namespace GestDG.ViewsModels
{
    class MasterPageViewModel:INavigationAware
    {
        private INavigationService service_navigation;
        public MasterPageViewModel(INavigationService nav)
        {
            this.service_navigation = nav;
        }


        public DelegateCommand getpage_registermembre
        {
            get
            {
                return new DelegateCommand(async () => { await service_navigation.NavigateAsync("Page_Menu/BaseNaviguationPage/RegisterMembre"); });
            }
        }

        public DelegateCommand getpage_showmembre
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("ShowMembre"); });
            }
        }

        
        public DelegateCommand getpage_showmembrevisite
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNaviguationPage/ShowMembreActivite"); });
            }
        }

        public DelegateCommand getpage_showmembreactivite
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNaviguationPage/ShowMembreActivite"); });
            }
        }
        public DelegateCommand getpage_showmembrerang
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNaviguationPage/ShowMembreRang"); });
            }
        }
        public DelegateCommand getpage_postenbmessage
        {
            get
            {
                return new DelegateCommand(async() => { await service_navigation.NavigateAsync("Page_Menu/BaseNaviguationPage/ShowPosteNBmessage"); });
            }
        }
        public DelegateCommand getpage_showviewfastmembre
        {
            get
            {
                return new DelegateCommand(async () => { await service_navigation.NavigateAsync("BaseNaviguationPage/ShowViewFastMembre"); });
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
