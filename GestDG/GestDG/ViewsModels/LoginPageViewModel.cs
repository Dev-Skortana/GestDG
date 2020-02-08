using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Navigation;
using GestDG.Models;
using GestDG.Services.Classes;
using System.Threading.Tasks;
using Recherche_donnees_GESTDG.enumeration;
using System.Linq;

namespace GestDG.ViewModels
{
   public class LoginPageViewModel : BindableBase, INavigationAware
    {
        #region Interfaces_services
        INavigationService service_navigation;
        RestService service_login;
        #endregion

        #region Variables
        private String _username;

        public String Username
        {
            get { return _username; }
            set{ _username = value; }
        }

        private String _password;

        public String Password
        {
            get { return _password; }
            set {_password=value;}
        }

        #endregion

        #region Constructeurs
        public LoginPageViewModel(INavigationService _service_navigation, RestService service_login)
        {
            this.service_navigation = _service_navigation;
            this.service_login = service_login;
        }
        #endregion

        #region Commandes_MVVM
        public ICommand Login
        {
            get
            {
                return new Command(()=>{ Log();});
            }
        }
        #endregion

        #region Methode_priver
        private async void Log()
        {
            if (Check_user_infos())
            {
                var user = new User(Username, Password);
                service_login.cookie_connexion = await service_login.Login(user,$"https://dynamixgaming.forumgaming.fr/login?login=Connexion&password={user.Password}&query&redirect&username={user.Username}");
                if (service_login.cookie_connexion!=null)
                {
                    await service_navigation.NavigateAsync("Page_Menu/BaseNavigationPage/RegisterMembre");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Connexion", "Veuillez saisir une seconde fois vos informations", null, "Ok");
                }
            }
        }

        private Boolean Check_user_infos()
        {
            return (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password));
        }
        #endregion

        #region Methodes_navigations_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
        #endregion
    }
}
