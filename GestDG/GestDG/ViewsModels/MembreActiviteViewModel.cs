using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using GestDG.Models;
using GestDG.Services.Classes;
using GestDG.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;

namespace GestDG.ViewModels
{
    class MembreActiviteViewModel : BindableBase,INavigationAware
    {


        #region Constructeure
        public MembreActiviteViewModel(INavigationService _service_navigation, IService_Membre _service_membre,IService_Activite _service_activite)
        {
            this. service_membre = _service_membre;
            this.service_activite = _service_activite;
            this.service_navigation = _service_navigation;
            this.champ_selected = Liste_champs[0];
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
        public string title { get; set; } = "Pages regroupent les activité des membres.";
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }
        public List<String> Liste_champs { get { return new List<string>() {"libelle_activite","membre_pseudo"}; } }
        public String champ_selected { get; set; }
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
                return new Command<Object>(async(donnees)=> {
                    await load(new Dictionary<string, object>() { {champ_selected,donnees} },dictionnaire_champs_methodesrecherche,type_selected);
                });
            }
            
        }
        #endregion

        #region Methodes priver ou interne
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);
            membres =(List<Membre>)await service_membre.GetList(new Dictionary<string, Object>() { { "pseudo", "" } }, new Dictionary<string, string>() { { "pseudo", "Contient" } },type);
            foreach (var item in membres)
            {
                item.liste_activites =(from el in  (List<Activite>)await service_activite.GetList(dictionnaire_donnees,methodes_recherches,type) where el.membre_pseudo==item.pseudo select el).ToList();
            }
        }
        #endregion

        #region Methode_navigation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(new Dictionary<string, Object>() { { "libelle_activite", "" } }, new Dictionary<string, string>() { { "libelle_activite", "Contient" } }, "Simple");
        }
        #endregion
    }
}
