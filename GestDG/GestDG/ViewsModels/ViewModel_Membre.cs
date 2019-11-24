using GestDG.Models;
using GestDG.Services.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;

namespace GestDG.ViewsModels
{
    class ViewModel_Membre : BindableBase, INavigationAware
    {
        // Code a voir !!!

        private Dictionary<String, String> dictionnaire=new Dictionary<string, string>();
        
        public ICommand testcmd
        {
            get
            {
                return new Command<String>((item) =>
                {
                    if (dictionnaire!=null && dictionnaire.ContainsKey(champ_selected))
                    {
                        dictionnaire[champ_selected] = item;
                    }
                    else {
                        dictionnaire.Add(champ_selected,item);
                    }
                });
            }
        }

        public List<String> Liste_methodesrecherches { get { return Enum.GetNames(typeof(Enumerations_recherches.methodes_recherches)).Cast<String>().ToList(); }}
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enum.GetNames(typeof(Enumerations_recherches.type_recherche)).Cast<String>().ToList(); } }
        public String type_selected { get; set; }

        //private List<String> get_champs_membre() { return Database_Initialize.Database_configuration.Database_Initialize().Result.GetMappingAsync<Membre>().Result.Columns.Cast<String>().ToList(); }
        public List<String> Liste_champs { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut", "rang_nom" }; } }
        public String champ_selected { get; set; }

        public String title { get; set; } = "Page des membres";
        private INavigationService service_navigation;
        private IService_Membre service_membre;

        public ViewModel_Membre(INavigationService _service_navigation, IService_Membre _service_membre)
        {
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;
        }

        public ICommand Cmd
        {
            get {
                return new Command<Object>(async (donnees) => {
                    await load(new Dictionary<string, Object>() { {champ_selected,donnees}},dictionnaire,type_selected);
                });
            }
        }

        private List<Membre> _liste_membres;
        public List<Membre> Liste_membres
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

        
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String  recherche_type)
        {
            Enumerations_recherches.type_recherche type= (Enumerations_recherches.type_recherche)Enum.Parse(typeof(Enumerations_recherches.type_recherche), recherche_type);
            Liste_membres = (from item in (List<Membre>)await service_membre.GetList(dictionnaire_donnees,methodes_recherches,type) orderby item.pseudo select item).ToList();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(new Dictionary<string, Object>() { {"pseudo",""} },new Dictionary<string, string>() { {"pseudo", "Contient" } },"Simple");
        }
    }
}
