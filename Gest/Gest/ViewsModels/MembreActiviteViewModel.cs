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

namespace Gest.ViewModels
{
    class MembreActiviteViewModel : BindableBase,INavigationAware
    {

        #region Constructeure
        public MembreActiviteViewModel(INavigationService _service_navigation, IService_Membre _service_membre,IService_Activite _service_activite)
        {
            this.service_membre = _service_membre;
            this.service_activite = _service_activite;
            this.service_navigation = _service_navigation;

            this.Liste_champs = this.Liste_champs_membres;
            this.dictionnaire_champs_methodesrecherche = this.dictionnaire_champs_methodesrecherche_membres;
            this.nom_table_selected = "Membre";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Activite service_activite;
        #endregion

        #region Variables
        public string title { get; set; } = "Page des activité des membres";
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche_membres = new Dictionary<string, string>();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche_activite = new Dictionary<string, string>();


        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Activite" }; } }
        public String nom_table_selected { get; set; }


        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }

        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }


        public List<String> Liste_champs_activites { get { return new List<string>() { "libelle_activite" }; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs, value); } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected,value); }
        }

        private List<Membre> _membres;
        public List<Membre> membres {
            get
            {
                return _membres;
            }
            set
            {
                SetProperty(ref _membres,value);
            }
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
                    else if (nom_table_selected == "Activite")
                    {
                        Liste_champs = Liste_champs_activites;
                        dictionnaire_champs_methodesrecherche = dictionnaire_champs_methodesrecherche_activite;
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
                return new Command<Object>(async(donnees)=> {
                    await load(new Dictionary<string, object>() { {this.Champ_selected,donnees} },dictionnaire_champs_methodesrecherche,type_selected);
                });
            }
            
        }
        #endregion

        #region Methodes priver ou interne
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);
            var liste_membres =(List<Membre>)await service_membre.GetList(nom_table_selected == "Membre" ? dictionnaire_donnees : null, nom_table_selected == "Membre" ? methodes_recherches : null, type);
            var liste_activite = (List<Activite>)await service_activite.GetList(nom_table_selected == "Activite" ? dictionnaire_donnees : null, nom_table_selected == "Activite" ? methodes_recherches : null, type);
            liste_membres.ForEach((membre)=>membre.liste_activites=(from iteration_membre in liste_activite where iteration_membre.membre_pseudo==membre.pseudo select iteration_membre).ToList());
            if (nom_table_selected=="Activite")
            {
                liste_membres.RemoveAll((membre)=>membre.liste_activites.Count==0);
            }
            this.membres = liste_membres;
        }
        #endregion

        #region Methode_navigation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(null,null,"Simple");
        }
        #endregion
    }
}
