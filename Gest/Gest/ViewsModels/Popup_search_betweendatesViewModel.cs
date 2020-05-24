using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Gest.Services.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;
using Xamarin.Forms;
namespace Gest.ViewModels
{
    class Popup_search_betweendatesViewModel:BindableBase,INavigationAware
    {
        #region Interfaces_services
        private INavigationService service_navigation;
        private INavigation_Goback_Popup_searchbetweendates service_navigation_goback_popup;
        #endregion

        #region Variables
        private List<Parametre_recherche_sql> liste_parametres_recherches_sql = new List<Parametre_recherche_sql>();

        private String nom_champ_date;

        private DateTime _date_debut;

        public DateTime Date_debut
        {
            get { return _date_debut; }
            set { _date_debut=value; }
        }

        private DateTime _date_fin;

        public DateTime Date_fin
        {
            get { return _date_fin; }
            set { _date_fin = value; }
        }
        #endregion

        #region Constructeur
        public Popup_search_betweendatesViewModel(INavigationService _service_navigation)
        {
            this.service_navigation = _service_navigation;
        }
        
        #endregion

        #region Methode_priver
        private  DateTime update_dateandtime(Object donnees,DateTime date_actuel)
        {
            DateTime resultat = new DateTime();
            if ((donnees is DateTime))
            {
                resultat= new DateTime(((DateTime)donnees).Year, ((DateTime)donnees).Month, ((DateTime)donnees).Day, date_actuel.TimeOfDay.Hours, date_actuel.TimeOfDay.Minutes, date_actuel.TimeOfDay.Seconds);
            }
            else if ((donnees is TimeSpan))
            {
                resultat= new DateTime(date_actuel.Year,date_actuel.Month, date_actuel.Day, ((TimeSpan)donnees).Hours, ((TimeSpan)donnees).Minutes, ((TimeSpan)donnees).Seconds);
            }
            return resultat;
        }
        #endregion

        #region Commande_MVVM

        public ICommand Command_update_dateandtime_debut
        {
            get
            {
                return new Command((donnees) =>
                {
                    this.Date_debut = this.update_dateandtime(donnees, this.Date_debut);
                });
            }
        }
        public ICommand Command_update_dateandtime_fin
         {
            get
            {
                return new Command((donnees) =>
                {
                    this.Date_fin = this.update_dateandtime(donnees, this.Date_fin);
                });
             }
         }

        public ICommand Navigation_Goback
        {
            get
            {
                return new Command(async ()=> {
                    liste_parametres_recherches_sql.Add(new Parametre_recherche_sql() { Champ = this.nom_champ_date, Valeur = this.Date_debut, Methode_recherche = Enumerations_recherches.methodes_recherches.Superieure.ToString() });
                    liste_parametres_recherches_sql.Add(new Parametre_recherche_sql() { Champ = this.nom_champ_date, Valeur = this.Date_fin, Methode_recherche = Enumerations_recherches.methodes_recherches.Inferieure.ToString() });
                    
                    await this.service_navigation_goback_popup.navigation_Goback_Popup_searchbetweendates(liste_parametres_recherches_sql);
                    await service_navigation.GoBackAsync();
                });
            }
        }
        #endregion

        #region Methodes_prism_navigation
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            this.nom_champ_date = (String)parameters["champ"];
            this.service_navigation_goback_popup = (INavigation_Goback_Popup_searchbetweendates)parameters["navigation_goback"];
        }
        #endregion

    }
}
