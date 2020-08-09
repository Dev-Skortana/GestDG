using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using Gest.Models;
using Gest.Services.Classes;
using Gest.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;
using Recherche_donnees_GESTDG;
using System.IO;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Internals;

namespace Gest.ViewModels
{
    class MembreActiviteViewModel : BindableBase, INavigationAware
    {

        #region Constructeure
        public MembreActiviteViewModel(INavigationService _service_navigation, IService_Membre _service_membre, IService_Activite _service_activite)
        {
            this.service_membre = _service_membre;
            this.service_activite = _service_activite;
            this.service_navigation = _service_navigation;

            this.Liste_champs = this.Liste_champs_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.Type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Activite service_activite;
        #endregion

        #region Variables
        public string title { get; set; } = "Page des activité des membres";
        private List<Parametre_recherche_sql> liste_parametres_recherches_sql = new List<Parametre_recherche_sql>();
        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Activite" }; } }
        public String nom_table_selected { get; set; }


        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }

        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }

        private String _type_selected;

        public String Type_selected
        {
            get { return _type_selected; }
            set { SetProperty(ref _type_selected, value); }
        }


        public List<String> Liste_champs_activites { get { return new List<string>() { "libelle_activite" }; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected, value); }
        }

        private List<Membre> _membres;
        public List<Membre> membres {
            get
            {
                return _membres;
            }
            set
            {
                SetProperty(ref _membres, value);
            }
        }
        #endregion

        #region Commandes_MVVM

        public ICommand Command_navigation_to_popup_search_multiple
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    Dictionary<String, IEnumerable<String>> dictionnary_namestables_with_nameschamps = (Dictionary<String, IEnumerable<String>>)get_dictionnary_namestables_with_nameschamps();
                    List<Parametre_recherche_sql> liste_all_champs = (List<Parametre_recherche_sql>)get_list_all_parametres_recherches_sql(dictionnary_namestables_with_nameschamps);
                    parametre.Add("navigation_goback", this);
                    parametre.Add("dictionnary_namestables_with_nameschamps", dictionnary_namestables_with_nameschamps);
                    parametre.Add("liste", liste_all_champs);

                    service_navigation.NavigateAsync("Popup_search_multiple", parametre);
                });
            }
        }

        public ICommand Command_navigation_to_popup_searchbetweendates
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("champ", Champ_selected);
                    parametre.Add("navigation_goback", this);

                    service_navigation.NavigateAsync("Popup_search_betweendates", parametre);
                });
            }
        }
        public ICommand Command_switch_source
        {
            get
            {
                return new Command(() => {
                    if (nom_table_selected == "Membre")
                    {
                        Liste_champs = Liste_champs_membres;
                    }
                    else if (nom_table_selected == "Activite")
                    {
                        Liste_champs = Liste_champs_activites;
                    }
                    Champ_selected = Liste_champs[0];
                });
            }
        }

        public ICommand Command_gestion_dictionnaire_champsmethodesrecherches
        {
            get
            {
                return new Command(() =>
                {
                    if (Champ_selected != null)
                    {
                        if (liste_parametres_recherches_sql.Exists((parametre) => parametre.Nom_table == nom_table_selected && parametre.Champ == Champ_selected && parametre.Methode_recherche == methoderecherche_selected) == false)
                        {
                            liste_parametres_recherches_sql.Add(new Parametre_recherche_sql() { Nom_table = nom_table_selected, Champ = Champ_selected, Methode_recherche = methoderecherche_selected });
                        }

                    }
                });
            }
        }

        public ICommand Command_search
        {
            get
            {
                return new Command<Object>(async (donnees) => {
                    if (this.Type_selected == Enumerations_recherches.types_recherches.Simple.ToString())
                    {
                        liste_parametres_recherches_sql.ForEach((parametre) =>
                        {
                            if ((parametre.Nom_table == nom_table_selected) && (parametre.Champ == Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected))
                            {
                                parametre.Valeur = donnees;
                            }
                        });
                        await Task.Run(() => {
                            int index_parametre = liste_parametres_recherches_sql.FindIndex((parametre) => (parametre.Nom_table == nom_table_selected) && (parametre.Champ == Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected) && (parametre.Valeur == donnees));
                            for (var i = 0; i < liste_parametres_recherches_sql.Count; i++)
                            {
                                if (i != index_parametre)
                                {
                                    liste_parametres_recherches_sql.RemoveAt(i);
                                }
                            }
                        });
                    }
                    await load_general(liste_parametres_recherches_sql);
                });
            }

        }
        #endregion

        #region Methodes priver ou interne

        private IEnumerable<Parametre_recherche_sql> get_list_all_parametres_recherches_sql(Dictionary<String, IEnumerable<String>> dictionnaire_liste_champs)
        {
            IEnumerable<Parametre_recherche_sql> resultat = new List<Parametre_recherche_sql>();
            for (var i = 0; i < dictionnaire_liste_champs.Count; i++)
            {
                resultat = resultat.Concat(dictionnaire_liste_champs.Values.ToList()[i].Select((champ) => new Parametre_recherche_sql(dictionnaire_liste_champs.Keys.ToList()[i], champ, null, null))).ToList();
            }
            return resultat;
        }

        private IDictionary<String, IEnumerable<String>> get_dictionnary_namestables_with_nameschamps()
        {
            return new Dictionary<String, IEnumerable<String>>() { { "Membre", Liste_champs_membres }, { "Activite", Liste_champs_activites } };
        }















        private async Task load_general(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql, IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql) {
            dictionnaire_parametres_sql = (Dictionary<String, IEnumerable<Parametre_recherche_sql>>)check_dictionnary_parametresrecherchessql(parametres_recherches_sql, dictionnaire_parametres_sql);
            var liste_membres = (await update_listemembres(Type_selected, dictionnaire_parametres_sql)).ToList();
        }

        private IDictionary<String, IEnumerable<Parametre_recherche_sql>> check_dictionnary_parametresrecherchessql(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql, IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql)
        {
            if (dictionnary_parametres_sql_has_value(dictionnaire_parametres_sql) == false) {
                dictionnaire_parametres_sql = create_dictionnary_parametres_recherches_sql_for_one_table(parametres_recherches_sql);
            }
            return dictionnaire_parametres_sql;
        }

        private Boolean dictionnary_parametres_sql_has_value(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_sql) {
            if (dictionnaire_parametres_sql.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private IDictionary<String, IEnumerable<Parametre_recherche_sql>> create_dictionnary_parametres_recherches_sql_for_one_table(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            return new Dictionary<String, IEnumerable<Parametre_recherche_sql>>() { { "Table_selected", parametres_recherches_sql } };
        }

        private async Task<IEnumerable<Membre>> update_listemembres(String type_recherche, IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres){          
            Dictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> dictionnaire_get_conditions_parametres_recherches_sql=null;
            if (type_recherche == Enumerations_recherches.types_recherches.Simple.ToString()) {
                var liste_parametre = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Table_selected"].ToList();
                configure_dictionnary_conditions_parametres_recherches_sql_for_one_listeparametres(ref dictionnaire_get_conditions_parametres_recherches_sql,liste_parametre);
            }
            else if (type_recherche == Enumerations_recherches.types_recherches.Multiples.ToString()) {
                var liste_parametre_membre = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Membre"].ToList();
                var liste_parametre_activite = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Activite"].ToList();
                configure_dictionnary_conditions_parametres_recherches_sql_for_two_listesparametres(ref dictionnaire_get_conditions_parametres_recherches_sql,liste_parametre_membre,liste_parametre_activite);
            }
            List<Membre> liste_membres = (await build_full_infos_membres(dictionnaire_get_conditions_parametres_recherches_sql)).ToList();
            return liste_membres;
        }

        private void configure_dictionnary_conditions_parametres_recherches_sql_for_one_listeparametres(ref Dictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> dictionnaire_conditions_parametres_recherches_sql, IEnumerable<Parametre_recherche_sql> parametres_recheches_sql){
                dictionnaire_conditions_parametres_recherches_sql = new Dictionary<string, Func<IEnumerable<Parametre_recherche_sql>>>(){ 
                    { "get_condition_parametres_recherches_sql_membre", ()=> nom_table_selected == "Membre" ? parametres_recheches_sql : null},
                    { "get_condition_parametres_recherches_sql_activite", ()=> nom_table_selected == "Activite" ? parametres_recheches_sql : null} 
                };
        }

        private void configure_dictionnary_conditions_parametres_recherches_sql_for_two_listesparametres(ref Dictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> dictionnaire_conditions_parametres_recherches_sql, IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_membre,IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_activite)
        {
            dictionnaire_conditions_parametres_recherches_sql = new Dictionary<string, Func<IEnumerable<Parametre_recherche_sql>>>(){
                    { "get_condition_parametres_recherches_sql_membre", ()=> nom_table_selected == "Membre" ? parametres_recheches_sql_membre : null},
                    { "get_condition_parametres_recherches_sql_activite", ()=> nom_table_selected == "Activite" ? parametres_recheches_sql_activite : null}
                };
        }

        private async Task<IEnumerable<Membre>> build_full_infos_membres(IDictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> dictionnaire_get_conditions_parametres_recherches_sql){
            List<Membre> liste_membres = new List<Membre>();
            liste_membres = (await get_informations_membres(dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_membre"].Invoke())).ToList();
            liste_membres = (await set_liste_activite_for_each_membre(liste_membres, dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_activite"].Invoke())).ToList();
            liste_membres = (List<Membre>)check_if_tableactive_is_only_activite(liste_membres);
            return liste_membres;
        }
        private async Task<IEnumerable<Membre>> get_informations_membres(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_membre){
            return (await service_membre.GetList(parametres_recheches_sql_membre)).ToList();
        }

        private async Task<IEnumerable<Membre>> set_liste_activite_for_each_membre (IEnumerable<Membre> membres, IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_activite){
            List<Membre> liste_membres=(List<Membre>)membres;
            List<Activite> liste_activites = (await get_information_activite(parametres_recheches_sql_activite)).ToList();
            liste_membres.ForEach((membre) => membre.liste_activites = (from iteration in liste_activites where iteration.membre_pseudo == membre.pseudo select iteration).ToList());
            return liste_membres;
        }


        private async Task<IEnumerable<Activite>> get_information_activite(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_activite){
            return (await service_activite.GetList(parametres_recheches_sql_activite)).ToList();
        }
        
        /* Trouver un nom plus précis pour cette méthode */
        private IEnumerable<Membre> check_if_tableactive_is_only_activite(IEnumerable<Membre> listemembres_source){
            List<Membre> liste_membres =(List<Membre>)listemembres_source;
            if (table_active_only_has_activite())
               liste_membres= (List<Membre>)remove_membre_havenot_activite(liste_membres);
            return liste_membres;
        }

        private Boolean table_active_only_has_activite(){
            if (Type_selected == Enumerations_recherches.types_recherches.Simple.ToString() && nom_table_selected == "Activite")
                return true;
            else
                return false;
        }

        private IEnumerable<Membre> remove_membre_havenot_activite(IEnumerable<Membre> listemembres_source){
            List<Membre> liste_membres = (List<Membre>)listemembres_source;
            liste_membres.RemoveAll((membre) => membre.liste_activites.Count == 0);
            return liste_membres;
        }  
        #endregion






        #region Methode_navigation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
                                                        /* Découper cela en sous méthodes */
            List<Parametre_recherche_sql> parametresrecherchessql = parameters.ContainsKey("liste_parametres_recherches_sql") ? (List<Parametre_recherche_sql>)parameters["liste_parametres_recherches_sql"] : new List<Parametre_recherche_sql>();
            Dictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnary_parametres_sql = parameters.ContainsKey("dictionnaire_parametres_sql") ? (Dictionary<String, IEnumerable<Parametre_recherche_sql>>)parameters["dictionnaire_parametres_sql"] : new Dictionary<String, IEnumerable<Parametre_recherche_sql>>();
            await load_general(parametresrecherchessql,dictionnary_parametres_sql);
        }
        #endregion
    }
}
