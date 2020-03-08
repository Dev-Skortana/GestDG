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
        public String title { get; set; } = "Page de login";
        private String _username;

        public String Username
        {
            get { return this._username; }
            set{ SetProperty(ref this._username, value); }
        }

        private String _password;

        public String Password
        {
            get { return this._password; }
            set { SetProperty(ref this._password, value); }
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
                return new Command(()=>{ this.Loging_account();});
            }
        }
        #endregion

        #region Methode_priver

        private void reset_user_infos()
        {
            this.Username = "";
            this.Password = "";
        }

        private Boolean Check_user_infos()
        {
            return (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password));
        }

        private async void Loging_account()
        {
            if (this.Check_user_infos())
            {
                var user = new User(this.Username, this.Password);
                this.service_login.cookie_connexion = await this.service_login.Login(user,$"https://dynamixgaming.forumgaming.fr/login?login=Connexion&password={user.Password}&query&redirect&username={user.Username}");
                if (this.service_login.cookie_connexion!=null)
                {
                    await this.service_navigation.NavigateAsync("RegisterMembre");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erreur identifiants", "Les informations saisie ne sont pas valide","OK");
                    this.reset_user_infos();
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Erreur saisie", "Le nom d'utilisateur ou le mot de passe na pas été saisie", "OK");
            }
        } 
        #endregion

        #region Methodes_navigations_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            
        }
        #endregion
    }
}
