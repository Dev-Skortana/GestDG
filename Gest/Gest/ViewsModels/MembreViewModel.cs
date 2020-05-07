using Gest.Models;
using Gest.Services.Interfaces;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using Gest.Interface_SQLiteAccess;
using Xamarin.Forms;

namespace Gest.ViewModels
{
    class MembreViewModel : BindableBase, INavigationAware
    {

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        #endregion

        #region Constructeure
        public MembreViewModel(INavigationService _service_navigation, IService_Membre _service_membre)
        {
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;
            this.champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        public String title { get; set; } = "Page des membres";
        private Boolean _isloading=false;
        public Boolean Isloading
        {
            get { return _isloading; }
            set { SetProperty(ref _isloading,value); }
        }

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches();} }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }
        public List<String> Liste_champs { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut","rang_nom" }; } }
        public String champ_selected { get; set; }

        private List<Membre> _liste_membres;
        public List<Membre> Liste_membres
        {
            get
            {
                return _liste_membres;
            }
            set
            {
                SetProperty(ref _liste_membres, value);
            }
        }

        #endregion

        #region Commandes_MVVM

        public ICommand Command_navigation_to_popup_searchbetweendates
        {
            get
            {
                return new Command(()=> {
                    service_navigation.NavigateAsync("Popup_search_betweendates");
                });
            }
        }
        public ICommand Command_gestion_dictionnaire_champsmethodesrecherches
        {
            get
            {
                return new Command(() =>
                {
                    if (champ_selected!=null) {
                        if (dictionnaire_champs_methodesrecherche != null && dictionnaire_champs_methodesrecherche.ContainsKey(champ_selected))
                        {
                            dictionnaire_champs_methodesrecherche[champ_selected] = methoderecherche_selected;
                        }
                        else {
                            dictionnaire_champs_methodesrecherche.Add(champ_selected, methoderecherche_selected);
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
                    await load(new Dictionary<string, Object>() { { champ_selected, donnees } }, dictionnaire_champs_methodesrecherche, type_selected);
                });
            }
        }
        #endregion

        #region Methodes priver ou interne
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String  recherche_type)
        {
            this.Isloading = true;
            Enumerations_recherches.types_recherches type= (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);
            Liste_membres = (from membre in (List<Membre>)await service_membre.GetList(dictionnaire_donnees,methodes_recherches,type) orderby membre.pseudo select membre).ToList();
            this.Isloading = false;
        }
        #endregion

        #region Methode_navigation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(null,null,"Simple");
        }
        #endregion

    }
}
