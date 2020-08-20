using Gest.Services.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Xaml;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Gest.ViewModels
{
    class Popup_search_multipleViewModel : BindableBase, INavigationAware
    {
        #region Interfaces_services
        INavigationService service_navigation;
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
                    Dictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql = (Dictionary<String, IEnumerable<Parametre_recherche_sql>>)return_dictionnary_parametresrecherchessql_trier(this.Liste_parametres_recherche_sql);
                    NavigationParameters parametres = new NavigationParameters() {
                        {"liste_parametres_recherches_sql",Liste_parametres_recherche_sql},
                        { "dictionnaire_parametres_sql",dictionnaire_parametres_sql}
                    }; 
                    await this.service_navigation.GoBackAsync(parametres);
                });
            }
        }

        public ICommand Command_update_dateandtime
        {
            get
            {
                return new Command((dictionnaire) =>
                {
                    Parametre_recherche_sql parametre_recherche_sql = (Parametre_recherche_sql)(dictionnaire as Dictionary<String, Object>)["objet_source"];
                    Object nouvelle_donnees = (dictionnaire as Dictionary<String, Object>)["nouvelle_donnees"];
                    parametre_recherche_sql.Valeur = update_dateandtime(nouvelle_donnees,(DateTime?)parametre_recherche_sql.Valeur);
                });
            }
        }
        #endregion

        #region Methodes_priver
        private void delete_parametre_recherche_sql_with_value_null(List<Parametre_recherche_sql> liste_parametres_recherches_sql)
        {
            liste_parametres_recherches_sql.RemoveAll((parametre_recherche_sql)=>parametre_recherche_sql.Valeur==null);
        }

        private IDictionary<String, IEnumerable<Parametre_recherche_sql>> return_dictionnary_parametresrecherchessql_trier(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql){
           return trie_dictionnary(parametres_recherches_sql);
        }

        private Dictionary<String, IEnumerable<Parametre_recherche_sql>> trie_dictionnary(IEnumerable<Parametre_recherche_sql> parametres_general)
        {
            Dictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_trier = new Dictionary<string, IEnumerable<Parametre_recherche_sql>>();
            get_dictionnary_with_namestables_and_nameschamps_from_liste_parametres_recherches_sql(parametres_general).ForEach((item) =>{
                dictionnaire_trier.Add(item.Key, (parametres_general.Where((parametre) => item.Value.Contains(parametre.Champ) && parametre.Nom_table == item.Key)));
            });
            return dictionnaire_trier;
        }

                                                                /* Cette méthode ci-dessous est à étudier */
        private Dictionary<String, IEnumerable<String>> get_dictionnary_with_namestables_and_nameschamps_from_liste_parametres_recherches_sql(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql){
            List<Parametre_recherche_sql> liste_parametres_recherches_sql = (List<Parametre_recherche_sql>)parametres_recherches_sql;

            IEnumerable<KeyValuePair<String, IEnumerable<String>>> liste_key_value_pair_namestables_with_namechamps = new List<KeyValuePair<string, IEnumerable<string>>>();
            List<String> liste_nomstables_parcouru = new List<string>();
            foreach (var parametre in liste_parametres_recherches_sql){
                if (liste_nomstables_parcouru.Contains(parametre.Nom_table) == false){
                    liste_key_value_pair_namestables_with_namechamps = liste_key_value_pair_namestables_with_namechamps.Concat(new List<KeyValuePair<String, IEnumerable<String>>> { new KeyValuePair<string, IEnumerable<string>>(parametre.Nom_table, parametres_recherches_sql.Where((item) => item.Nom_table == parametre.Nom_table).Select((items) => items.Champ)) });
                    liste_nomstables_parcouru.Add(parametre.Nom_table);
                }
            }
            return liste_key_value_pair_namestables_with_namechamps.ToDictionary((key) => key.Key, (value) => value.Value);
        }

        private DateTime update_dateandtime(Object donnees, DateTime? _date_actuel)
        {
            
            DateTime resultat = new DateTime();
            if (_date_actuel == null)
            {
                resultat = new DateTime(donnees is DateTime ? ((DateTime)donnees).Ticks : ((TimeSpan)donnees).Ticks);
            }
            else
            {
                if (donnees is DateTime)
                {
                    DateTime nouvelle_date = (DateTime)donnees;
                    TimeSpan temps_actuel = _date_actuel.Value.TimeOfDay;
                    resultat = new DateTime(nouvelle_date.Year, nouvelle_date.Month, nouvelle_date.Day, temps_actuel.Hours, temps_actuel.Minutes, temps_actuel.Seconds);
                }
                else if (donnees is TimeSpan)
                {
                    DateTime date_actuel = _date_actuel.Value;
                    TimeSpan nouveau_temps = (TimeSpan)donnees;
                    resultat = new DateTime(date_actuel.Year, date_actuel.Month, date_actuel.Day, nouveau_temps.Hours, nouveau_temps.Minutes, nouveau_temps.Seconds);
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
            this.Liste_parametres_recherche_sql =(List<Parametre_recherche_sql>)parameters["liste_parametre_recherche_sql"];
        }
        #endregion


    }
}
