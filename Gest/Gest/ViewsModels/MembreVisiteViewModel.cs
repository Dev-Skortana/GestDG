using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Navigation;
using Gest.Models;
using Gest.Services.Interfaces;
using Gest.Helpers;
using Xamarin.Forms;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;
using Xamarin.Forms.Internals;
using ImTools;

namespace Gest.ViewModels
{
    class MembreVisiteViewModel : BindableBase, INavigationAware, INavigation_Goback_Popup_searchbetweendates, INavigation_Goback_Popup_searchmultiple
    {

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Visite service_visite;
        #endregion

        #region Constructeure
        public MembreVisiteViewModel(INavigationService _service_navigation, IService_Membre _service_membre, IService_Visite _service_visite)
        {
            this.service_navigation = _service_navigation;
            this.service_membre = _service_membre;
            this.service_visite = _service_visite;

            this.Liste_champs = this.Liste_champs_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables

        private DateTime _dateandtime;

        public DateTime Dateandtime
        {
            get { return _dateandtime; }
            set { SetProperty(ref _dateandtime, value); }
        }


        public string title { get; set; } = "Page des visites des membres";
        private List<Parametre_recherche_sql> liste_parametres_recherches_sql = new List<Parametre_recherche_sql>();

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }

        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Visite" }; } }
        public String nom_table_selected { get; set; }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        public List<String> Liste_champs_visite { get { return new List<string>() { "connexion_date", "date_enregistrement" }; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected, value); }
        }


        private Dictionary<Membre, List<visite_custom>> _dictionnaire_membre_visite = new Dictionary<Membre, List<visite_custom>>();
        public Dictionary<Membre, List<visite_custom>> Dictionnaire_membre_visite
        {
            get
            {
                return _dictionnaire_membre_visite;
            }
            set
            {
                SetProperty(ref _dictionnaire_membre_visite, value);
            }
        }

        #endregion

        #region Methode_priver

        private IDictionary<String, IEnumerable<String>> get_dictionnary_champs()
        {
            return new Dictionary<String, IEnumerable<String>>() { { "Membre", Liste_champs_membres }, { "Visite", Liste_champs_visite } };
        }
        private IEnumerable<Parametre_recherche_sql> get_list_parametre_recherche_sql(Dictionary<String, IEnumerable<String>> dictionnaire_liste_champs)
        {
            IEnumerable<Parametre_recherche_sql> resultat = new List<Parametre_recherche_sql>();
            for (var i = 0; i < dictionnaire_liste_champs.Count; i++)
            {
                resultat = resultat.Concat(dictionnaire_liste_champs.Values.ToList()[i].Select((champ) => new Parametre_recherche_sql(dictionnaire_liste_champs.Keys.ToList()[i], champ, null, null))).ToList();
            }
            return resultat;
        }
        private Dictionary<String, IEnumerable<Parametre_recherche_sql>> get_dictionnary_parametrerecherchesql_trier(IEnumerable<Parametre_recherche_sql> liste_parametre_general, IDictionary<String, IEnumerable<String>> dictionnaire_nomtable_listeparametres)
        {
            Dictionary<String, IEnumerable<Parametre_recherche_sql>> resultat = new Dictionary<string, IEnumerable<Parametre_recherche_sql>>();
            dictionnaire_nomtable_listeparametres.ForEach((item) => {
                resultat.Add(item.Key, (liste_parametre_general.Where((parametre) => item.Value.Contains(parametre.Champ) && parametre.Nom_table == item.Key)));
            });
            return resultat;
        }


        private async Task<IDictionary<Membre, List<visite_custom>>> update_dictionnaire_visites(String type_recherche, IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres)
        {
            Dictionary<Membre, List<visite_custom>> dictionnaire_intermediaire = new Dictionary<Membre, List<visite_custom>>();
            var liste_membres = new List<Membre>();
            var liste_visites = new List<Visite>();
            Dictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> dictionnaire_get_conditions_parametres_recherches_sql = new Dictionary<string, Func<IEnumerable<Parametre_recherche_sql>>>();
            dictionnaire_get_conditions_parametres_recherches_sql.Add("get_condition_parametres_recherches_sql_membre", () => null);
            dictionnaire_get_conditions_parametres_recherches_sql.Add("get_condition_parametres_recherches_sql_visite", () => null);
            if (type_recherche == "Simple")
            {
                var liste_parametre = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Table_selected"].ToList();
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_membre"] = () => nom_table_selected == "Membre" ? liste_parametre : null;
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_visite"] = () => nom_table_selected == "Visite" ? liste_parametre : null;
            }
            else if (type_recherche == "Multiples")
            {
                var liste_parametre_membre = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Membre"].ToList();
                var liste_parametre_visite = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Visite"].ToList();
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_membre"] = () => liste_parametre_membre.Count > 0 ? liste_parametre_membre : null;
                dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_visite"] = () => liste_parametre_visite.Count > 0 ? liste_parametre_visite : null;     
            }
            liste_membres = (List<Membre>)await service_membre.GetList(dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_membre"].Invoke());
            liste_visites = (List<Visite>)await service_visite.GetList(dictionnaire_get_conditions_parametres_recherches_sql["get_condition_parametres_recherches_sql_visite"].Invoke());
            liste_membres.ForEach((membre) => { dictionnaire_intermediaire.Add(membre, liste_visites.Where((visite) => visite.membre_pseudo == membre.pseudo).Select<Visite, visite_custom>((visite) => new visite_custom() { connexion_date = visite.connexion_date, date_enregistrement = visite.date_enregistrement }).ToList()); });
            return dictionnaire_intermediaire;
        }

        private IDictionary<Membre, List<visite_custom>> get_dictionnary_visite_withonly_membre_have_visite(Dictionary<Membre, List<visite_custom>> dictionnaire_origine)
        {
            Dictionary<Membre, List<visite_custom>> dictionnaire_trie = new Dictionary<Membre, List<visite_custom>>();
            if (dictionnaire_origine.Values.ToList().TrueForAll((visites)=>visites.Count>0))
            {
                dictionnaire_trie = dictionnaire_origine;
            }
            else
            {
                foreach (var item in dictionnaire_origine)
                {
                    if (item.Value.Count != 0)
                    {
                        dictionnaire_trie.Add(item.Key, item.Value);
                    }
                }
            }
            return dictionnaire_trie;
        }
        private async Task load(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {                                                        
            Dictionary<Membre, List<visite_custom>> dictionnaire_intermediaire = new Dictionary<Membre, List<visite_custom>>();
            
            dictionnaire_intermediaire = (Dictionary<Membre, List<visite_custom>>)(await update_dictionnaire_visites(type_selected, new Dictionary<String, IEnumerable<Parametre_recherche_sql>>() { { "Table_selected", parametres_recherches_sql } }));
            
            if (nom_table_selected == "Visite")
            {
                dictionnaire_intermediaire = (Dictionary<Membre, List<visite_custom>>)get_dictionnary_visite_withonly_membre_have_visite(dictionnaire_intermediaire);
            }
            this.Dictionnaire_membre_visite = dictionnaire_intermediaire;
        }

        public async Task navigation_Goback_Popup_searchbetweendates(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            await load(parametres_recherches_sql);
        }

        
        public  async Task navigation_Goback_Popup_searchmultiple(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            Dictionary<String, IEnumerable<String>> dictionnaire_nomtable_listechamps = (Dictionary<String, IEnumerable<String>>)get_dictionnary_champs();
            Dictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres = get_dictionnary_parametrerecherchesql_trier(parametres_recherches_sql, dictionnaire_nomtable_listechamps);
            Dictionary<Membre, List<visite_custom>> dictionnaire_intermediaire = new Dictionary<Membre, List<visite_custom>>();
            dictionnaire_intermediaire = (Dictionary<Membre, List<visite_custom>>)(await update_dictionnaire_visites(type_selected,dictionnaire_parametres));
            if ((dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>)["Visite"].ToList().Count>0)
            {
                dictionnaire_intermediaire= (Dictionary<Membre, List<visite_custom>>)get_dictionnary_visite_withonly_membre_have_visite(dictionnaire_intermediaire);
            }

            this.Dictionnaire_membre_visite =dictionnaire_intermediaire;

        } 
        #endregion

        #region Commandes_MVVM

        public ICommand Command_navigation_to_popup_search_multiple
        {
            get
            {
                return new Command(() => {
                    NavigationParameters parametre = new NavigationParameters();
                    Dictionary<String, IEnumerable<String>> dictionnaire_champs = (Dictionary<String, IEnumerable<String>>)get_dictionnary_champs();
                    List<Parametre_recherche_sql> liste_all_parametres = (List<Parametre_recherche_sql>)get_list_parametre_recherche_sql(dictionnaire_champs);
                    parametre.Add("navigation_goback", this);
                    parametre.Add("liste", liste_all_parametres);

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
        public ICommand Command_update_dateandtime
        {
            get
            {
                return new Command((donnees)=>
                {

                    if ((donnees is DateTime))
                    {
                        this.Dateandtime =new DateTime(((DateTime)donnees).Year, ((DateTime)donnees).Month, ((DateTime)donnees).Day,this.Dateandtime.TimeOfDay.Hours, this.Dateandtime.TimeOfDay.Minutes, this.Dateandtime.TimeOfDay.Seconds);
                    }else if ((donnees is TimeSpan))
                    {
                        this.Dateandtime = new DateTime(this.Dateandtime.Year,this.Dateandtime.Month,this.Dateandtime.Day,((TimeSpan)donnees).Hours,((TimeSpan)donnees).Minutes,((TimeSpan)donnees).Seconds);
                    }

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
                    else if (nom_table_selected == "Visite")
                    {
                        Liste_champs = Liste_champs_visite;
             
                    }
                    this.Champ_selected = Liste_champs[0];
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
                        if (liste_parametres_recherches_sql.Exists((parametre) => parametre.Nom_table==nom_table_selected && parametre.Champ == Champ_selected && parametre.Methode_recherche == methoderecherche_selected) == false)
                        {
                            liste_parametres_recherches_sql.Add(new Parametre_recherche_sql() {Nom_table=nom_table_selected, Champ = Champ_selected, Methode_recherche = methoderecherche_selected });
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
                    if (type_selected == Enumerations_recherches.types_recherches.Simple.ToString())
                    {
                        liste_parametres_recherches_sql.ForEach((parametre) =>
                        {
                            if ((parametre.Nom_table==nom_table_selected) && (parametre.Champ == this.Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected))
                            {
                                parametre.Valeur = donnees;
                            }
                        });
                        await Task.Run(() => {
                            int index_parametre = liste_parametres_recherches_sql.FindIndex((parametre) =>(parametre.Nom_table==nom_table_selected) && (parametre.Champ == this.Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected) && (parametre.Valeur == donnees));
                            int iteration = 0;
                            Boolean is_finish = false;
                            while (iteration<liste_parametres_recherches_sql.Count && !is_finish)
                            {
                                if (iteration != index_parametre)
                                {
                                    liste_parametres_recherches_sql.RemoveAt(iteration);
                                    index_parametre = liste_parametres_recherches_sql.FindIndex((parametre) => (parametre.Nom_table == nom_table_selected) && (parametre.Champ == this.Champ_selected) && (parametre.Methode_recherche == methoderecherche_selected) && (parametre.Valeur == donnees));
                                }
                                if (iteration==liste_parametres_recherches_sql.Count-1 && !is_finish)
                                {
                                    iteration = 0;
                                }
                                else
                                {
                                    iteration++;
                                }
                                if (liste_parametres_recherches_sql.Count==1)
                                {
                                    is_finish = true;
                                }
                                
                            }
                                                 
                        });
                    }
                    await load(liste_parametres_recherches_sql);

                });
            }
        }
        #endregion

        #region Methodes_naviguations_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            //await load(null);
        }

        
        #endregion
    }
}
