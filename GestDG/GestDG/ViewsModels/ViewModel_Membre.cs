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

namespace GestDG.ViewsModels
{
    class ViewModel_Membre : BindableBase, INavigationAware
    {

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
                return new Command<String>(async (donnees) => {
                    await load(new Dictionary<string, string>() { {champ_selected,donnees}},methoderecherche_selected);
                });
            }
        }

        //private List<String> get_champs_membre() { return Database_Initialize.Database_configuration.Database_Initialize().Result.GetMappingAsync<Membre>().Result.Columns.Cast<String>().ToList(); }
        public List<String> Liste_champs { get { return new List<string>() { "pseudo", "date_naissance", "age", "date_inscription", "url_site", "url_avatar", "sexe", "localisation", "statut", "rang_nom" }; } }//Database_Initialize.Database_configuration.Database_Initialize().Result.GetMappingAsync<Membre>().Result.Columns; }
        public String champ_selected { get; set; }

        public List<string> Liste_methodesrecherches { get { return new List<string>() { "Egale a", "Commence par", "Fini par" }; } }
        public String methoderecherche_selected{ get; set; }

        public List<string> Liste_typesrecherches { get { return new List<string>() { "Simple", "Multiple"}; } }
        public String type_selected { get; set; }

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

        private Boolean getvalue(Membre membre,Dictionary<String,String> dico,string methode)
        {
            Boolean resultat = false;
            switch (dico.Keys.ToList()[0])
            {
                case "pseudo":
                    resultat = methode.Contains("Commence par") ? membre.pseudo.ToUpper().StartsWith(dico["pseudo"].ToUpper()) : (methode.Contains("Fini par") ? membre.pseudo.ToUpper().EndsWith(dico["pseudo"].ToUpper()) :( methode.Contains("Egale a") ? membre.pseudo.ToUpper() == dico["pseudo"].ToUpper() : membre.pseudo.Contains(dico["pseudo"])));
                break;
                case "age":
                    resultat =(membre.age==Convert.ToInt64(dico["age"]));
                break;
                case "sexe":
                    resultat = (membre.sexe.Contains(dico["sexe"]));
                break;
            }
            return resultat;
        }
        private async Task load(Dictionary<String,String> dictionnaire_donnees,String methode_recherche)
        {
           //await service_membre.delete(new Membre());
            //_liste_membres = new List<Membre> { new Membre() { pseudo = "SharQaaL", age = 27, date_naissance = new DateTime(), date_inscription = new DateTime(), url_site = "", url_avatar =await DependencyService.Get<IFilePicture_Access>().save_picture(element_commun.type_image_membre.Avatar) + "/1-72.png", sexe = 'M', localisation = "Amérique", statut = "En ligne", rang = new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png" }, rang_nom = "DT" }, new Membre() { pseudo = "DG dada", age = 14, date_naissance = new DateTime(), date_inscription = new DateTime(), url_site = "", url_avatar = await DependencyService.Get<IFilePicture_Access>().save_picture(element_commun.type_image_membre.Avatar) + "/112-43.jpg", sexe = 'M', localisation = "Amérique", statut = "En ligne", rang = new Rang() { nom_rang = "DT", url_rang = "https://i.servimg.com/u/f56/11/26/28/25/dirige10.png" }, rang_nom = "DT" } };
            Liste_membres = (from item in (List<Membre>)await service_membre.GetList(dictionnaire_donnees.ContainsKey("pseudo") ? dictionnaire_donnees["pseudo"]:"") where getvalue(item,dictionnaire_donnees,methode_recherche) orderby item.pseudo select item).ToList();//(List<Membre>)await service_membre.GetList("");
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await load(new Dictionary<string, string>() { {"pseudo",""} },"");
        }
    }
}
