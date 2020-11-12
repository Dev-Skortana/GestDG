using Prism;
using Gest.Services.Classes;
using Gest.Services.Interfaces;
using Prism.Ioc;
using Prism.DryIoc;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using System;
using Prism.Plugin.Popups;
using System.Linq;
using Rg.Plugins.Popup.Contracts;
using Rg.Plugins.Popup.Services;
using Prism.Navigation;
using Xamarin.Forms;
namespace Gest
{
   [AutoRegisterForNavigation]
    public partial class App:PrismApplication
    { 

        public App() : this(null)
        {
            Device.SetFlags(new[] { "SwipeView_Experimental" });
        }
        public App(IPlatformInitializer initializer=null):base(initializer)
        {

        }

        protected async override  void OnInitialized()
        {
            
            InitializeComponent();
            await NavigationService.NavigateAsync("BaseNavigationPage/MasterPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.RegisterPopupNavigationService();

            containerRegistry.RegisterInstance(typeof(RestService),new RestService());
            containerRegistry.Register<IService_Membre, Service_Membre>();
            containerRegistry.Register<IService_Connexion, Service_Connexion>();
            containerRegistry.Register<IService_Activite, Service_Activite>();
            containerRegistry.Register<IService_Membre_Connexion_Message, Service_Membre_Connexion_Message>();
            containerRegistry.Register<IService_Message, Service_Message>();
            containerRegistry.Register<IService_Rang, Service_Rang>();
            containerRegistry.Register<IService_Visite, Service_Visite>();
        }

        
    }
       
}
