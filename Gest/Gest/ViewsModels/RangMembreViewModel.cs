using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Commands;
using Gest.Models;
using Gest.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;
using Gest.Interface_SQLiteAccess;

namespace Gest.ViewModels
{
    class RangMembreViewModel : BindableBase,INavigationAware
    {
        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Rang service_rang;
        private IService_Membre service_membre;
        #endregion

        #region constructeure
        public RangMembreViewModel(INavigationService _service_navigation, IService_Rang _service_rang, IService_Membre _service_membre)
        {
            this.service_rang = _service_rang;
            this.service_membre = _service_membre;
            this.service_navigation = _service_navigation;

            this.Liste_champs = this.Liste_champs_rangs;
            this.dictionnaire_champs_methodesrecherche = this.dictionnaire_champs_methodesrecherche_rangs;
            this.nom_table_selected = "Rang";

            this.Champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region 

        public ICommand Command_switch_source {
            get
            {
                return new Command(()=> {
                    if (nom_table_selected=="Membre")
                    {
                        Liste_champs = Liste_champs_membres;
                        dictionnaire_champs_methodesrecherche = dictionnaire_champs_methodesrecherche_membres;
                    }
                    else if(nom_table_selected=="Rang")
                    {
                        Liste_champs = Liste_champs_rangs;
                        dictionnaire_champs_methodesrecherche = dictionnaire_champs_methodesrecherche_rangs;
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

        #region Variables
        public string title { get; set; } = "Page rangs des membres";

        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche_membres = new Dictionary<string, string>();
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche_rangs = new Dictionary<string, string>();

        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches();} }
        public String type_selected { get; set; }

        public List<String> Liste_noms_tables { get { return new List<string>() { "Membre", "Rang" }; } }
        public String nom_table_selected { get; set; }

        public List<String> liste_champs;
        public List<String> Liste_champs { get { return liste_champs; } set { SetProperty(ref liste_champs,value); } }

        public List<String> Liste_champs_rangs { get {return new List<string>() {"nom_rang","url_rang"}; } }
        public List<String> Liste_champs_membres { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut" }; } }

        private String _champ_selected;

        public String Champ_selected
        {
            get { return _champ_selected; }
            set { SetProperty(ref _champ_selected,value); }
        }



        private List<Rang> _liste_rangs;
        public List<Rang> Liste_rangs
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
            var membres = (List<Membre>)await new Services.Classes.Service_Membre().GetList(nom_table_selected == "Membre" ? dictionnaire_donnees : new Dictionary<string, Object>() { { "pseudo", "" } }, nom_table_selected == "Membre" ? methodes_recherches : new Dictionary<string, string>() { { "pseudo", "Contient" } }, type);
            var rangs= (List<Rang>)await new Services.Classes.Service_Rang().GetList(nom_table_selected=="Rang" ? dictionnaire_donnees: new Dictionary<string, Object>() { { "nom_rang", "" } }, nom_table_selected == "Rang" ? methodes_recherches: new Dictionary<string, string>() { { "nom_rang", "Contient" } }, type);
            rangs.ForEach((item) =>item.liste_membres= (from el in membres where el.rang_nom==item.nom_rang orderby el.pseudo select el).ToList());
            if (nom_table_selected=="Membre") {
                rangs.RemoveAll((item) =>item.liste_membres.Count == 0);
            }
            Liste_rangs = rangs;
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
