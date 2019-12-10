using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Commands;
using GestDG.Models;
using GestDG.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;

namespace GestDG.ViewsModels
{
    class ViewModel_RangMembre:BindableBase,INavigationAware
    {
        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Rang service_rang;
        private IService_Membre service_membre;
        #endregion

        #region constructeure
        public ViewModel_RangMembre(INavigationService _service_navigation, IService_Rang _service_rang, IService_Membre _service_membre)
        {
            this.service_rang = _service_rang;
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;
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
        #endregion

        #region Variables
        public string title { get; set; } = "Page rangs des membres";
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches();} }
        public String type_selected { get; set; }
        public List<String> Liste_champs { get { return new List<string>() { "nom_rang", "url_rang" }; } }
        public String champ_selected { get; set; }

        private List<Rang> _liste_rangs;
        public List<Rang> liste_rangs
        {
            get
            {
                return _liste_rangs;
            }
            set
            {
                SetProperty(ref _liste_rangs, value);
            }
        }

        #endregion

        #region Methodes priver ou interne
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);
            liste_rangs = (List<Rang>)await new Services.Classes.Service_Rang().GetList(dictionnaire_donnees,methodes_recherches,type);
            liste_rangs.ForEach(async (item) =>item.liste_membres= (from el in (List<Membre>)await new Services.Classes.Service_Membre().GetList(new Dictionary<string, Object>() { { "pseudo", "" } }, new Dictionary<string, string>() { { "pseudo", "Contient" } },type) where el.rang_nom==item.nom_rang orderby el.pseudo select el).ToList());
        }
        #endregion

        #region Methode_navigation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(new Dictionary<string, Object>() { { "nom_rang", "" } }, new Dictionary<string, string>() { { "nom_rang", "Contient" } }, "Simple");
        }
        #endregion
    }
}
