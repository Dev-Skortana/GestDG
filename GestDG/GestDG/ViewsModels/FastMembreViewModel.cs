using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Navigation;
using GestDG.Models;
using GestDG.Services.Interfaces;
using System.Threading.Tasks;
using Recherche_donnees_GESTDG.enumeration;
using System.Linq;

namespace GestDG.ViewModels
{
    class FastMembreViewModel: BindableBase,INavigationAware
    {
        #region Interfaces_services
        private IService_Membre service_membre;
        private INavigationService service_naviguation;
        #endregion

        #region Constructeure
        public FastMembreViewModel(IService_Membre _service_membre,INavigationService _service_navigation)
        {
            this.service_membre = _service_membre;
            this.service_naviguation = _service_navigation;
            this.champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Variables
        public String title { set; get; } = "Vue rapide des membres";
        public Membre membre_selected=new Membre();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }
        public List<String> Liste_champs { get { return new List<string>() { "pseudo","url_avatar" }; } }
        public String champ_selected { get; set; }
        
 
        private List<Membre> _liste_membres;
        public List<Membre> Liste_membres {
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

        #region Commandes_MVVM

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

        public ICommand Command_search
        {
            get
            {
                return new Command<Object>(async (donnees) => {
                    await load(new Dictionary<string, Object>() { { champ_selected, donnees } }, dictionnaire_champs_methodesrecherche, type_selected);
                });
            }
        }


        /* Permettre la naviguation vers la page suivi du membre avec la transmission de l'objet membre en respectant le pattern MVVM(Model View ViewModel) */

        public ICommand save_membre_selected
        {
            get
            {
                return new Command<int>((position) => {
                    if (Liste_membres!=null) {
                        membre_selected = Liste_membres?[position];
                    }
                });
            }
        }
        
        
        public ICommand getpage_membre
        {
            get
            {
                return new Command(async()=> {
                    NavigationParameters parametre = new NavigationParameters();
                    parametre.Add("Membre",membre_selected);
                    await service_naviguation.NavigateAsync("Page_Menu/BaseNaviguationPage/SuiviMembre", parametre);
                });
            }
        } 
        #endregion

        #region Methode_priver
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);
            Liste_membres = (from item in (List<Membre>)await service_membre.GetList(dictionnaire_donnees, methodes_recherches, type) orderby item.pseudo select item).ToList();
        }
        #endregion

        #region Methodes_naviguations_PRISM
        public async void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(new Dictionary<string, Object>() { { "pseudo", "" } }, new Dictionary<string, string>() { { "pseudo", "Contient" } }, "Simple");
        }
        #endregion
    }
}
