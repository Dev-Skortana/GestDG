using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using GestDG.Models;
using GestDG.Services.Classes;
using GestDG.Services.Interfaces;
using GestDG.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using System.Windows.Input;

namespace GestDG.ViewModels
{
    class SuiviMembreViewModel : BindableBase,INavigationAware
    {
        

        #region Interfaces_services
        private INavigationService service_navigation;
        private IService_Membre service_membre;
        private IService_Connexion service_connexion;
        private IService_Visite service_visite;
        private IService_Activite service_activite;
        private IService_Message service_message;
        private IService_Rang service_rang;
        private IService_Membre_Connexion_Message service_membre_connexion_message;
        #endregion

        #region Variables
        public string title { get; set; } = "Page suivi du membre";
        private Dictionary<String, String> dictionnaire_champs_methodesrecherche = new Dictionary<string, string>();
        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }
        public List<String> Liste_champs { get { return new List<string>() { "libelle_activite","connexion_date","message_nb"}; } }
        public String champ_selected { get; set; }
        private Membre _membre;

        public Membre Membre
        {
            get { return _membre; }
            set { SetProperty(ref _membre,value); }
        }

        private List<Connexion> _connexions;

        public List<Connexion> Connexions
        {
            get { return _connexions; }
            set { SetProperty(ref _connexions, value); }
        }

        private List<Visite> _visites;

        public List<Visite> Visites
        {
            get { return _visites; }
            set { SetProperty(ref _visites,value); }
        }

        private List<Activite> _activites;

        public List<Activite> Activites
        {
            get { return _activites; }
            set { SetProperty(ref _activites,value); }
        }

        private Rang _rang;

        public Rang Rang
        {
            get { return _rang; }
            set { SetProperty(ref _rang,value); }
        }

        private List<Message> _messages;

        public List<Message> Messages
        {
            get { return _messages; }
            set { SetProperty(ref _messages,value); }
        }

        private List<Membre_Connexion_Message> _membre_connexion_messages;

        public List<Membre_Connexion_Message> Membre_Connexion_Messages
        {
            get { return _membre_connexion_messages; }
            set { SetProperty(ref _membre_connexion_messages,value); }
        }

        private List<Groupement_nombremessage> _groupement_nombremessage;

        public List<Groupement_nombremessage> Groupement_nombremessage
        {
            get { return _groupement_nombremessage; }
            set { SetProperty(ref _groupement_nombremessage,value); }
        }
        #endregion

        #region Command_MVVM
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

        #region Constructeure
        public SuiviMembreViewModel(IService_Membre _service_membre, IService_Connexion _service_connexion, IService_Visite _service_visite, IService_Activite _service_activite, IService_Message _service_message,IService_Rang _service_rang, IService_Membre_Connexion_Message _service_membre_connexion_message,INavigationService _service_navigation)
        {
            this.service_membre = _service_membre;
            this.service_connexion =_service_connexion;
            this.service_visite = _service_visite;
            this.service_activite = _service_activite;
            this.service_message = _service_message;
            this.service_rang = _service_rang;
            this.service_membre_connexion_message = _service_membre_connexion_message;
            this.service_navigation = _service_navigation;
            this.champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        #endregion

        #region Methode_priver
        private async Task load(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, String recherche_type)
        {
            Enumerations_recherches.types_recherches type = (Enumerations_recherches.types_recherches)Enum.Parse(typeof(Enumerations_recherches.types_recherches), recherche_type);
            /* Retrouver les informations des differents ensembles(entité) en liaison avec ce membre. */
            Rang = (from item in await service_rang.GetList(null, null, Enumerations_recherches.types_recherches.Simple) where item.nom_rang.ToUpper() == Membre.rang_nom.ToUpper() select item).Single();
            Activites = (from item in (List<Activite>)await service_activite.GetList(null,null,Enumerations_recherches.types_recherches.Simple) where item.membre_pseudo.ToUpper()==Membre.pseudo.ToUpper() select item).ToList();
            Visites =(from item in (List<Visite>)await service_visite.GetList(null,null,Enumerations_recherches.types_recherches.Simple) where item.membre_pseudo.ToUpper()==Membre.pseudo.ToUpper() select item).ToList();

            Groupement_nombremessage = (from item in (List<Membre_Connexion_Message>)await service_membre_connexion_message.GetList(null, null, Enumerations_recherches.types_recherches.Simple) where item.membre_pseudo.ToUpper() == Membre.pseudo.ToUpper() orderby item.connexion_date descending select new Groupement_nombremessage() {Date_connexion=item.connexion_date,Nbmessage=item.message_nb}).ToList();
        }
        #endregion

        #region Methode_naviguation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            /* Récuperer le membre */
            Membre = parameters["Membre"] as Membre;
            await load(null,null,"Simple");
        }
        #endregion
    }

}

