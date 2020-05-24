using Gest.Services.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Xaml;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Gest.ViewModels
{
    class Popup_search_multipleViewModel : BindableBase, INavigationAware
    {
        #region Interfaces_services
        INavigationService service_navigation;
        INavigation_Goback_Popup_searchmultiple service_navigation_goback_popup;
        #endregion

        #region Constructeur
        public Popup_search_multipleViewModel(INavigationService _service_navigation)
        {
            this.service_navigation = _service_navigation;
        }
        #endregion

        #region Variables

        private List<Parametre_recherche_sql> _liste_parametres_recherche_sql;

        public List<Parametre_recherche_sql> Liste_parametres_recherche_sql { get{ return _liste_parametres_recherche_sql; } set { SetProperty(ref _liste_parametres_recherche_sql,value); } }

        private List<String> _liste_methodesrecherches=Enumerations_recherches.get_liste_methodesrecherches();

       
        public List<String> Liste_methodesrecherches { get { return _liste_methodesrecherches; } }
        #endregion

        #region Commande_MVVM
        public ICommand Navigation_Goback
        {
            get
            {
                return new Command(async () => {
                    this.delete_parametre_recherche_sql_with_value_null(this.Liste_parametres_recherche_sql);
                    await this.service_navigation_goback_popup.navigation_Goback_Popup_searchmultiple(this.Liste_parametres_recherche_sql);
                    await this.service_navigation.GoBackAsync();
                });
            }
        }

        public ICommand Command_update_dateandtime_debut
        {
            get
            {
                return new Command((dictionnaire) =>
                {
                    Parametre_recherche_sql parametre_recherche_sql = (Parametre_recherche_sql)(dictionnaire as Dictionary<String, Object>)["objet_source"];
                    Object nouvelle_donnees = (dictionnaire as Dictionary<String, Object>)["nouvelle_donnees"];
                    parametre_recherche_sql.Valeur = update_dateandtime(nouvelle_donnees, (DateTime)parametre_recherche_sql.Valeur);
                });
            }
        }
        #endregion

        #region Methodes_priver

        private void delete_parametre_recherche_sql_with_value_null(List<Parametre_recherche_sql> liste_parametres_recherches_sql)
        {
            liste_parametres_recherches_sql.RemoveAll((parametre_recherche_sql)=>parametre_recherche_sql.Valeur==null);
        }
        private DateTime update_dateandtime(Object donnees, DateTime date_actuel)
        {
            
            DateTime resultat = new DateTime();
            if (date_actuel == null)
            {
                resultat = new DateTime(donnees is DateTime ? ((DateTime)donnees).Ticks : ((TimeSpan)donnees).Ticks);
            }
            else
            {
                if (donnees is DateTime)
                {
                    resultat = new DateTime(((DateTime)donnees).Year, ((DateTime)donnees).Month, ((DateTime)donnees).Day, date_actuel.TimeOfDay.Hours, date_actuel.TimeOfDay.Minutes, date_actuel.TimeOfDay.Seconds);
                }
                else if (donnees is TimeSpan)
                {
                    resultat = new DateTime(date_actuel.Year, date_actuel.Month, date_actuel.Day, ((TimeSpan)donnees).Hours, ((TimeSpan)donnees).Minutes, ((TimeSpan)donnees).Seconds);
                }
            }
            return resultat;
        }
        #endregion

        #region Methodes_navigations_prism
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            this.service_navigation_goback_popup =(INavigation_Goback_Popup_searchmultiple)parameters["navigation_goback"];
            this.Liste_parametres_recherche_sql =(List<Parametre_recherche_sql>)parameters["liste"];
        }
        #endregion


    }
}
