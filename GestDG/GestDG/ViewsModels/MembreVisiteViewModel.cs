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
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Helpers;
using Xamarin.Forms;
using Recherche_donnees_GESTDG.enumeration;

namespace GestDG.ViewModels
{
    class MembreVisiteViewModel : BindableBase,INavigationAware
    {

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Visite service_visite;
        #endregion

        #region Constructeure
        public MembreVisiteViewModel(INavigationService _service_navigation, IService_Membre _service_membre,IService_Visite _service_visite)
        {
            this.service_navigation = _service_navigation;
            this.service_membre = _service_membre;
            this.service_visite = _service_visite;
            this.champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables
        public string title { get; set; } = "Page des visites des membres";
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche_membres = new Dictionary<string, string>();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche_visites = new Dictionary<string, string>();

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }

        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Visite" }; } }
        public String nom_table_selected { get; set; }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        public List<String> Liste_champs_visite { get { return new List<string>() { "connexion_date" , "date_enregistrement" }; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }


        public String champ_selected { get; set; }
        private List<visite_custom> _liste_visites=new List<visite_custom>();
        public List<visite_custom> Liste_visites
        {
            get
            {
                return _liste_visites;
            }
            set 
            {
                SetProperty(ref _liste_visites,value);
            }
        }

        private List<Membre> _liste_membres;
        public  List<Membre> Liste_membres
        {
            get
            {
                return _liste_membres;
            }
            set
            {
                SetProperty(ref _liste_membres,value);
            }
        }
        #endregion

        #region Methode_priver
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);                                                                               /* Voir comment effectuer la recherche sur les proprieters de la visite  */
            var lm = (from item in (List<Membre>)await service_membre.GetList(nom_table_selected == "Membre" ? dictionnaire_donnees : new Dictionary<string, Object>() { { "pseudo", "" } }, nom_table_selected == "Membre" ? methodes_recherches : new Dictionary<string, string>() { { "pseudo", "Contient" } }, type) orderby item.pseudo select item).DefaultIfEmpty().ToList();
            var lv = (from item in (List<Visite>)await service_visite.GetList(nom_table_selected == "Visite" ? dictionnaire_donnees : null, nom_table_selected == "Visite" ? methodes_recherches : null, type)  select item).DefaultIfEmpty().ToList();
            if (nom_table_selected == "Visite")
            {
                lm.RemoveAll((item_membre) =>lv.Exists((item_visite)=>item_visite.membre_pseudo==item_membre.pseudo)==false);
            }
            Liste_membres = lm;
            var temp = new List<visite_custom>();
            lv.ForEach((item) =>
            {
                temp.Add(new visite_custom() { connexion_date = item.connexion_date, date_enregistrement = item.date_enregistrement });
            });
            Liste_visites = temp;
        }
        #endregion

        #region Commandes_MVVM

        public ICommand Command_switch_source
        {
            get
            {
                return new Command(() => {
                    if (nom_table_selected == "Membre")
                    {
                        Liste_champs = Liste_champs_membres;
                        dictionnaire_champs_methodesrecherche = dictionnaire_champs_methodesrecherche_membres;
                    }
                    else if (nom_table_selected == "Visite")
                    {
                        Liste_champs = Liste_champs_visite;
                        dictionnaire_champs_methodesrecherche = dictionnaire_champs_methodesrecherche_visites;
                    }
                    champ_selected = Liste_champs[0];
                });
            }
        }

        public ICommand Command_gestion_dictionnaire_champsmethodesrecherches
        {
            get
            {
                return new Command(() =>
                {
                    if (champ_selected != null)
                    {
                        if (dictionnaire_champs_methodesrecherche != null && dictionnaire_champs_methodesrecherche.ContainsKey(champ_selected))
                        {
                            dictionnaire_champs_methodesrecherche[champ_selected] = methoderecherche_selected;
                        }
                        else
                        {
                            dictionnaire_champs_methodesrecherche.Add(champ_selected, methoderecherche_selected);
                        }
                    }
                });
            }
        }

        public ICommand Command_sync
        {
            get
            {
                return new Command<int>(async (position)=>
                {
                    Liste_visites = new List<visite_custom>();
                    List<Visite> liste = new List<Visite>();
                    liste=(from item in (List<Visite>)await service_visite.GetList(new Dictionary<string, Object>() { { "membre_pseudo", "" } }, new Dictionary<string, string>() { { "membre_pseudo", "Contient" } }, Enumerations_recherches.types_recherches.Simple) where item.membre_pseudo==Liste_membres[position].pseudo select item).ToList();
                    var temp = new List<visite_custom>();
                    liste.ForEach((item) =>
                    {
                        temp.Add(new visite_custom() { connexion_date = item.connexion_date, date_enregistrement = item.date_enregistrement });
                    });
                    Liste_visites = temp;
                });
            }
        }

        public ICommand Command_search
        {
            get
            {
                return new Command<Object>(async (donnees) => {
                    await load(new Dictionary<string, Object>() { { champ_selected, donnees } }, dictionnaire_champs_methodesrecherche, type_selected);

                });
            }
        }
        #endregion

        #region Methodes_naviguations_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(new Dictionary<string, Object>() { { "pseudo", "" } }, new Dictionary<string, string>() { { "pseudo", "Contient" } }, "Simple");
        }
        #endregion
    }
}
