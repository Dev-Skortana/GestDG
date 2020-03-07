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

            this.Liste_champs = this.Liste_champs_membres;
            this.dictionnaire_champs_methodesrecherche = this.dictionnaire_champs_methodesrecherche_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
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

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected, value); }
        }


        private Dictionary<Membre, List<visite_custom>> _dictionnaire_membre_visite = new Dictionary<Membre,List<visite_custom>>();
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
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);                                                         
            Dictionary<Membre, List<visite_custom>> dictionnaire_intermediaire = new Dictionary<Membre, List<visite_custom>>();

            var liste_membres = new List<Membre>();
            liste_membres=(from item in (List<Membre>)await service_membre.GetList(nom_table_selected == "Membre" ? dictionnaire_donnees : new Dictionary<string, Object>() { { "pseudo", "" } }, nom_table_selected == "Membre" ? methodes_recherches : new Dictionary<string, string>() { { "pseudo", "Contient" } }, type) orderby item.pseudo select item).ToList();

            var liste_visites = new List<Visite>();
            liste_visites=(from item in (List<Visite>)await service_visite.GetList(nom_table_selected == "Visite" ? dictionnaire_donnees : null, nom_table_selected == "Visite" ? methodes_recherches : null, type)  select item).ToList();

            if (nom_table_selected == "Visite")
            {
                liste_membres.RemoveAll((membre) =>liste_visites.Exists((visite)=>visite.membre_pseudo==membre.pseudo)==false);
            }

            liste_membres.ForEach((membre) => { dictionnaire_intermediaire.Add(membre,liste_visites.Where((visite)=>visite.membre_pseudo==membre.pseudo).Select<Visite,visite_custom>((visite)=>new visite_custom() { connexion_date = visite.connexion_date, date_enregistrement = visite.date_enregistrement }).ToList()); });

            Dictionnaire_membre_visite = dictionnaire_intermediaire;

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
                    if (this.Champ_selected != null)
                    {
                        if (dictionnaire_champs_methodesrecherche != null && dictionnaire_champs_methodesrecherche.ContainsKey(this.Champ_selected))
                        {
                            dictionnaire_champs_methodesrecherche[this.Champ_selected] = methoderecherche_selected;
                        }
                        else
                        {
                            dictionnaire_champs_methodesrecherche.Add(this.Champ_selected, methoderecherche_selected);
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
                    await load(new Dictionary<string, Object>() { { this.Champ_selected, donnees } }, dictionnaire_champs_methodesrecherche, type_selected);

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
            await load(null,null, "Simple");
        }
        #endregion
    }
}
