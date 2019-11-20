using System;
using Prism;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GestDG.Views;
using GestDG.ViewsModels;
using GestDG.Services.Classes;
using GestDG.Services.Interfaces;
using Prism.Ioc;
using Prism.DryIoc;
using Unity;

namespace GestDG
{
    public partial class App:PrismApplication
    { 
        public App(IPlatformInitializer initializer=null):base(initializer)
        {
            
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            await NavigationService.NavigateAsync("Page_Menu/BaseNaviguationPage/MasterPageView");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<BaseNaviguationPage, BaseNavigationPageViewModel>();
            containerRegistry.RegisterForNavigation<Page_Menu,ViewModel_PageMenu>();
            containerRegistry.RegisterForNavigation<MasterPageView, MasterPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterMembre,ViewModel_RegisterMembre>();
            containerRegistry.RegisterForNavigation<ShowMembre,ViewModel_Membre>();
            containerRegistry.RegisterForNavigation<ShowMembreActivite,ViewModel_MembreActivite>();
            containerRegistry.RegisterForNavigation<ShowMembreRang,ViewModel_RangMembre>();
            containerRegistry.RegisterForNavigation<ShowViewSuiviMembre,ViewModel_SuiviMembre>();
            containerRegistry.RegisterForNavigation<ShowMembreVisite,ViewModel_Visite>();
            containerRegistry.RegisterForNavigation<ShowPosteNBmessage,ViewModel_PosteNBmessage>();
            containerRegistry.RegisterForNavigation<ShowViewFastMembre,ViewModel_ViewFastMembre>();

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
