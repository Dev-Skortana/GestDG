using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Gest.Models;
using Gest.Services.Interfaces;
using Gest.Services.Classes;
using Gest.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Prism.Mvvm;
using Prism.Navigation;
using Gest.Views;
using Gest.Interface_File_Image_Access;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;
using DryIoc;

namespace Gest.ViewModels
{
    class RegisterMembreViewModel : BindableBase, INavigationAware
    {
        #region Constante
        private const String url_racine = @"https://dynamixgaming.forumgaming.fr/";
        #endregion

        #region Services
        private RestService rest;
        private INavigationService service_navigation;
        private IFilePicture_Access service_access_picture;
        private IService_Membre service_membre;
        private IService_Connexion service_connexion;
        private IService_Visite service_visite;
        private IService_Rang service_rang;
        private IService_Activite service_activite;
        private IService_Message service_message;
        private IService_Membre_Connexion_Message service_membre_connexion_message;
        #endregion

        #region Constructeure
        public RegisterMembreViewModel(INavigationService _service_navigation, IService_Membre _service_membre, IService_Connexion _service_connexion, IService_Visite _service_visite, IService_Rang _service_rang, IService_Activite _service_activite, IService_Message _service_message, IService_Membre_Connexion_Message _service_membre_connexion_message, RestService rest)
        {
            this.service_navigation = _service_navigation;
            this.service_access_picture = DependencyService.Get<IFilePicture_Access>();
            this.service_membre = _service_membre;
            this.service_connexion = _service_connexion;
            this.service_visite = _service_visite;
            this.service_rang = _service_rang;
            this.service_activite = _service_activite;
            this.service_message = _service_message;
            this.service_membre_connexion_message = _service_membre_connexion_message;
            this.rest = rest;

            this.isfinish_load = false;
            this.isbusy = false;
        }
        #endregion

        #region Variables
        public String title { get; set; } = "Page enregistrement";

        private String _pseudo;
        public String Pseudo
        {
            get
            {
                return _pseudo;
            }
            set
            {
                SetProperty(ref _pseudo, value);
            }
        }


        private String _image;
        public String Image
        {
            get
            {
                return _image;
            }
            set
            {
                SetProperty(ref _image, value);
            }
        }

        private String _message_redirect;
        public String message_redirect
        {
            get { return _message_redirect; }
            set { SetProperty(ref _message_redirect, value); }
        }


        private Models.Membre _membre;
        public Models.Membre Membre
        {
            get
            {
                return _membre;
            }
            set
            {
                SetProperty(ref _membre, value);
            }
        }

        private Connexion _connexion;
        public Connexion Connexion
        {
            get
            {
                return _connexion;
            }
            set
            {
                SetProperty(ref _connexion, value);
            }

        }

        private Message _message;
        public Message Message
        {
            get
            {
                return _message;
            }
            set
            {
                SetProperty(ref _message, value);
            }
        }

        private Activite _activite;
        public Activite Activite
        {
            get
            {
                return _activite;
            }
            set
            {
                SetProperty(ref _activite, value);
            }
        }

        private Rang _rang;
        public Rang Rang
        {
            get
            {
                return _rang;
            }
            set
            {
                SetProperty(ref _rang, value);
            }
        }

        private Visite _visite;
        public Visite Visite
        {
            get
            {
                return _visite;
            }
            set
            {
                SetProperty(ref _visite, value);
            }
        }

        private Membre_Connexion_Message _membreconnexionmessage;
        public Membre_Connexion_Message Membreconnexionmessage
        {
            get
            {
                return _membreconnexionmessage;
            }
            set
            {
                SetProperty(ref _membreconnexionmessage, value);
            }
        }

        private int _pourcentage_avancement_progression;
        public int pourcentage_avancement_progression {
            get {
                return _pourcentage_avancement_progression;
            }
            set {
                SetProperty(ref _pourcentage_avancement_progression, value);
            }
        }

        private Boolean _isfinish_load;
        public Boolean isfinish_load
        {
            get
            {
                return _isfinish_load;
            }
            set
            {
                SetProperty(ref _isfinish_load, value);
            }
        }

        private Boolean _isbusy;
        public Boolean isbusy
        {
            get
            {
                return _isbusy;
            }
            set
            {
                SetProperty(ref _isbusy, value);
            }
        }
        #endregion

        #region Methode_priver

        private void reinitialisations_of_models() {
            this.Membre = null;
            this.Connexion = null;
            this.Rang = null;
            this.Visite = null;
            this.Activite = null;
            this.Message = null;
            this.Membreconnexionmessage = null;
        }

        private void reinitialisations_donnees_that_are_show_in_screen() {
            this.Pseudo = default(String);
            this.Image = default(String);
        }

        private IEnumerable<String> get_collection_liens_of_pages_contains_members(HtmlDocument documenthtml) {
            String pattern_regex_getnumberspages = "<.+class=\"page-sep\">.+</span>\n*(<a href=\"(?<page>[^\"]+)\">[0-9]+</a>)+";
            Regex regex_getnumberspages = new Regex(pattern_regex_getnumberspages);
            MatchCollection collection_page = regex_getnumberspages.Matches(documenthtml.ParsedText);
            List<String> numbers_pages = new List<string>();
            String premier_page = collection_page[0].Groups["page"].Value;
            numbers_pages.Add(collection_page.Count != 0 ? $@"{url_racine}" + Regex.Replace(premier_page, "start=[0-9]+", "start=0") : $@"{url_racine}memberlist?mode=lastvisit&order=DESC&start=0&username");
            return (from Match match_numberpage in collection_page select $@"{url_racine}" + match_numberpage.Value);
        }

        private int get_pourcent_load_members(int position_of_member_intotal, int numbers_total_members) {
            decimal souscalcule_pourcentage = Decimal.Divide(position_of_member_intotal, numbers_total_members);
            return Convert.ToInt32(Math.Truncate(souscalcule_pourcentage * 100));
        }

        private async Task<int> get_numbers_total_members_from_html(HtmlDocument documenthtml, RestService service_for_access, IEnumerable<String> collection_liens_numbers_of_pages) {
            int numbers_total_members = 0;
            foreach (String lien in collection_liens_numbers_of_pages) {
                documenthtml.Load(await service_for_access.getresponse(@lien), Encoding.UTF8);
                HtmlNode table_members = documenthtml.GetElementbyId("memberlist").SelectSingleNode("tbody");
                numbers_total_members += table_members.SelectNodes("//tbody/tr").ToList().Count;
            }
            return numbers_total_members;
        }

        private async Task<List<Date_Part>> get_collection_date_partie_of_members_from_html(HtmlDocument documenthtml, RestService service_for_access, IEnumerable<String> collection_liens_numbers_of_pages) {
            List<Date_Part> parties_dates = new List<Date_Part>();
            foreach (String lien in collection_liens_numbers_of_pages) {
                documenthtml.Load(await service_for_access.getresponse(@lien), Encoding.UTF8);
                HtmlNode table_members = documenthtml.GetElementbyId("memberlist").SelectSingleNode("tbody");
                parties_dates = Datefull.construct_list(table_members);
            }
            return parties_dates;
        }

        private Boolean check_member_have_connection_from_html(HtmlDocument documenthtml_of_page_profilemember) {
            if (documenthtml_of_page_profilemember.GetElementbyId("main-content").SelectSingleNode("div[@class='panel bg2']/div[@class='column2']/dl[@class='left-box details']/dd").InnerText.Equals("Jamais"))
            {
                return false;
            }
            else {
                return true;
            }
        }

        private Boolean check_member_have_activite_from_html(HtmlDocument documenthtml_of_page_profilemember)
        {
            if (documenthtml_of_page_profilemember.GetElementbyId("field_id-9") == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private Boolean check_member_have_rang_from_html(HtmlDocument documenthtml_of_page_profilemember)
        {
            if (documenthtml_of_page_profilemember.GetElementbyId("main-content").SelectSingleNode("//div[@class='column1']")?.SelectNodes("dl[@class='left-box details']")[1].SelectSingleNode("dd/strong").InnerText.StartsWith("Aucun rang", true, System.Globalization.CultureInfo.CurrentCulture) == true)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private async Task insert_membre_in_database(Models.Membre membre)
        {
            IService_Membre service_membre = new Service_Membre();
            await service_membre.insert(membre);
        }

        private async Task insert_rang_in_database(IService_Rang service_rang, Rang rang)
        {
            service_rang = new Service_Rang();
            await service_rang.insert(rang);
        }

        private async Task insert_activite_in_database(IService_Activite service_activite, Activite activite)
        {
            service_activite = new Service_Activite();
            await service_activite.insert(activite);
        }

        private async Task insert_connection_in_database(IService_Connexion service_connexion,Connexion connexion) {

            service_connexion = new Service_Connexion();
            await service_connexion.insert(connexion);
        }

        private async Task insert_visite_in_database(IService_Visite service_visite,Visite visite)
        {
            service_visite = new Service_Visite();
            await service_visite.insert(visite);
        }
        private async Task insert_message_in_database(IService_Message service_message,Message message)
        {
            service_message = new Service_Message();
            await service_message.insert(message);
        }
        private async Task insert_membreconnectionmessage_in_database(IService_Membre_Connexion_Message service_membreconnexionmessage,Membre_Connexion_Message membreconnexionmessage)
        {
            service_membreconnexionmessage  = new Service_Membre_Connexion_Message();
            await service_membreconnexionmessage.insert(membreconnexionmessage);
        }

        private async Task await_few_secondes(int seconde){
            await Task.Delay(seconde*1000);
        }

        private void cancel_load_members(CancellationToken token)
        {
            this.Pseudo = default(String);
            this.Image = default(String);
            token.ThrowIfCancellationRequested();
        }

        private async Task register(CancellationToken token)
        {
            HtmlDocument document_dynamixgaming_page_memberlist = new HtmlDocument();
            document_dynamixgaming_page_memberlist.Load(await rest.getresponse($"{url_racine}memberlist"),Encoding.UTF8);

            HtmlNode table_members;
            Regex pattern_getpages = new Regex("<.+class=\"page-sep\">.+</span>\n*(<a href=\"(?<page>[^\"]+)\">[0-9]+</a>)+");
            MatchCollection collection_page = pattern_getpages.Matches(document_dynamixgaming_page_memberlist.ParsedText);
            List<String> liste_pages = new List<string>();
            String premier_page = collection_page[0].Groups["page"].Value;
            liste_pages.Add(collection_page.Count != 0 ? $@"{url_racine}" + Regex.Replace(premier_page, "start=[0-9]+", "start=0") : $@"{url_racine}memberlist?mode=lastvisit&order=DESC&start=0&username");
            foreach (Match match in collection_page)
            {
                liste_pages.Add($@"{url_racine}" + match.Groups["page"].Value);
            }
            int pourcentage_avancement_progression=0;
            int nb_total_members = 0;
            int position_member_intotal =1;

            foreach (String lien in liste_pages)
            {
                document_dynamixgaming_page_memberlist.Load(await rest.getresponse(@lien),Encoding.UTF8);
                table_members = document_dynamixgaming_page_memberlist.GetElementbyId("memberlist").SelectSingleNode("tbody");
                nb_total_members += table_members.SelectNodes("//tbody/tr").ToList().Count;
                Datefull.liste_partie_dates = Datefull.construct_list(table_members);
            } 
            
            Datefull.initialise_index();
 
            foreach (String lien in liste_pages)
            {   
                document_dynamixgaming_page_memberlist.Load(await rest.getresponse(@lien), Encoding.UTF8);
                table_members = document_dynamixgaming_page_memberlist.GetElementbyId("memberlist").SelectSingleNode("tbody");
                foreach (var member_iteration in table_members.Descendants("tr"))
                {
                    decimal souscalcule_pourcentage = Decimal.Divide(position_member_intotal,nb_total_members);
                    pourcentage_avancement_progression= Convert.ToInt32(Math.Truncate(souscalcule_pourcentage * 100));
                    this.pourcentage_avancement_progression = pourcentage_avancement_progression;
                     
                    Boolean reponse_insert_dateconnexion = true;
                    Boolean reponse_insert_activite = true;
                    Boolean reponse_insert_rang = true;
                    HtmlDocument document_dynamixgaming_page_member_profile = new HtmlDocument();
                    document_dynamixgaming_page_member_profile.Load(await rest.getresponse($@"{url_racine}" + member_iteration.Descendants("td").ToList()[1].Descendants("a").ToList()[0].Attributes[0].Value),Encoding.UTF8);
                    Membre = new Models.Membre() { pseudo = document_dynamixgaming_page_member_profile.GetElementbyId("main-content")?.SelectSingleNode("//div/h1[@class='page-title']/span/strong | //div/h1[@class='page-title']").InnerText.Split(':')[1].Trim(), date_naissance = DateTime.TryParseExact(document_dynamixgaming_page_member_profile.GetElementbyId("field_id-12")?.SelectSingleNode("dd/div[@class='field_uneditable']").InnerText, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime test_date_naissance) ? DateTime.ParseExact(document_dynamixgaming_page_member_profile.GetElementbyId("field_id-12")?.SelectSingleNode("dd/div[@class='field_uneditable']").InnerText, "dd/MM/yyyy", null) : new Nullable<DateTime>(), age = int.TryParse(document_dynamixgaming_page_member_profile.GetElementbyId("field_id-5")?.SelectSingleNode("dd/div[@class='field_uneditable']").InnerText, out int test_age) ? int.Parse(document_dynamixgaming_page_member_profile.GetElementbyId("field_id-5")?.SelectSingleNode("dd/div[@class='field_uneditable']").InnerText) : new Nullable<int>(), date_inscription = DateTime.ParseExact(document_dynamixgaming_page_member_profile.GetElementbyId("field_id-4")?.SelectSingleNode("dd/div[@class='field_uneditable']").InnerText, "dd/MM/yyyy", null), url_site = document_dynamixgaming_page_member_profile.GetElementbyId("field_id-10")?.SelectSingleNode("//div[@class='field_uneditable']/a").Attributes["href"].Value, url_avatar = document_dynamixgaming_page_member_profile.GetElementbyId("main-content")?.SelectSingleNode("//div[@class='column1']").SelectSingleNode("dl/dd").SelectSingleNode("img").Attributes["src"].Value, sexe =document_dynamixgaming_page_member_profile.GetElementbyId("field_id-7")?.SelectSingleNode("dd/div[@class='field_uneditable']/img").Attributes["title"].Value[0].ToString(), localisation = document_dynamixgaming_page_member_profile.GetElementbyId("field_id-11")?.SelectSingleNode("dd/div[@class='field_uneditable']").InnerText, statut = document_dynamixgaming_page_member_profile.GetElementbyId("main-content")?.SelectSingleNode("div[@class='panel bg1']//div[@class='column1']").SelectNodes("dl[@class='left-box details']")[1].SelectNodes("dd")[1].SelectSingleNode("strong").InnerText };
                    Pseudo = Membre.pseudo;        
                    var folder_avatar = await service_access_picture.save_picture(element_commun.type_image_membre.Avatar);
                    var folder_rang= await service_access_picture.save_picture(element_commun.type_image_membre.Rang);
                    Task tache_telecharge_avatar = null;
                    Task tache_telecharge_rang = null; 
                    tache_telecharge_avatar = new WebClient().DownloadFileTaskAsync(Membre.url_avatar, $"{folder_avatar}/{Membre.url_avatar.Substring(Membre.url_avatar.LastIndexOf("/") + 1)}");         
                    Membre.url_avatar = Path.Combine(folder_avatar, Membre.url_avatar.Substring(Membre.url_avatar.LastIndexOf("/") + 1));
                    if (document_dynamixgaming_page_member_profile.GetElementbyId("main-content").SelectSingleNode("div[@class='panel bg2']/div[@class='column2']/dl[@class='left-box details']/dd").InnerText.Equals("Jamais"))
                    {
                        reponse_insert_dateconnexion = false;
                    }
                    else
                    {
                        Connexion = new Connexion() { date_connexion = Datefull.getdate_visite_full(Datefull.liste_partie_dates, Datefull.liste_partie_dates.Find((el) => Datefull.index_relation[Datefull.index] == el.Indexe))};
                        Datefull.incremente_index();
                        Visite = new Visite() { membre = Membre, connexion = Connexion, membre_pseudo = Membre.pseudo, connexion_date = Connexion.date_connexion };
                        Message = new Message() { nb_message = int.Parse(document_dynamixgaming_page_member_profile.GetElementbyId("field_id-6")?.SelectSingleNode("dd/div[@class='field_uneditable']").InnerText) };
                        Membreconnexionmessage = new Membre_Connexion_Message() { membre = Membre, connexion = Connexion, message_state = Message, membre_pseudo = Membre.pseudo, connexion_date = Connexion.date_connexion, message_nb = Message.nb_message };
                    }

                    if (document_dynamixgaming_page_member_profile.GetElementbyId("field_id-9") == null)
                    {
                        reponse_insert_activite = false;
                    }
                    else
                    {
                        Activite = new Activite() { membre = Membre, membre_pseudo = Membre.pseudo, libelle_activite = document_dynamixgaming_page_member_profile.GetElementbyId("field_id-9")?.SelectSingleNode("dd/div[@class='field_uneditable']").InnerText };
                    }
                    try
                    {
                        await tache_telecharge_avatar;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    this.Image = Membre.url_avatar;
                    if (document_dynamixgaming_page_member_profile.GetElementbyId("main-content").SelectSingleNode("//div[@class='column1']")?.SelectNodes("dl[@class='left-box details']")[1].SelectSingleNode("dd/strong").InnerText.StartsWith("Aucun rang", true, System.Globalization.CultureInfo.CurrentCulture) == true)
                    {
                        reponse_insert_rang = false;
                    }
                    else
                    {   
                        Rang = new Rang() { nom_rang = document_dynamixgaming_page_member_profile.GetElementbyId("main-content").SelectSingleNode("//div[@class='column1']")?.SelectNodes("dl[@class='left-box details']")[1].SelectSingleNode("dd/strong").InnerText, url_rang = document_dynamixgaming_page_member_profile.GetElementbyId("main-content").SelectSingleNode("//div[@class='column1']")?.SelectNodes("dl[@class='left-box details']")[1].SelectSingleNode("dd/strong/img").Attributes["src"].Value };
                        Membre.rang_nom = Rang.nom_rang;
                        tache_telecharge_rang = new WebClient().DownloadFileTaskAsync(Rang.url_rang, $"{folder_rang}/{Rang.url_rang.Substring(Rang.url_rang.LastIndexOf("/") + 1)}");
                        Rang.url_rang = Path.Combine(folder_rang,Rang.url_rang.Substring(Rang.url_rang.LastIndexOf("/") + 1));
                       
                    }     
                    if (tache_telecharge_rang!=null)
                    {
                        try
                        {
                            await tache_telecharge_rang;
                        }
                        catch(Exception ex)
                        {

                        }
                    }

                    if (reponse_insert_rang)
                    {
                        service_rang = new Service_Rang();
                        await service_rang.insert(this.Rang);
                    }
                     service_membre = new Service_Membre();
                     await service_membre.insert(this.Membre);
                    if (reponse_insert_activite)
                    {
                        service_activite = new Service_Activite();
                        await service_activite.insert(this.Activite);
                    }
                    if (reponse_insert_dateconnexion)
                    { 
                            service_connexion = new Service_Connexion();
                            await service_connexion.insert(this.Connexion);
                        
                       
                            service_visite = new Service_Visite();
                            await service_visite.insert(this.Visite);
                        
                        
                            service_message = new Service_Message();
                            await service_message.insert(this.Message);
                        
                   
                        service_membre_connexion_message = new Service_Membre_Connexion_Message();
                        await service_membre_connexion_message.insert(this.Membreconnexionmessage);
                    
                }
                    position_member_intotal += 1;

                    this.Membre = null;
                    this.Connexion = null;
                    this.Rang = null;
                    this.Visite = null;
                    this.Activite = null;
                    this.Message = null;
                    this.Membreconnexionmessage = null;       
                    await Task.Delay(1000);
                    if (token.IsCancellationRequested)
                    {
                        this.Pseudo = default(String);
                        this.Image = default(String);
                        token.ThrowIfCancellationRequested();
                    }
                }
            }
          }
            
        private async Task redirection_compte_rebour(int temps_seconde)
        {
            for (var iteration=temps_seconde;iteration>0;iteration--)
            {
                this.message_redirect = $"Redirection dans {iteration}";
                await Task.Delay(1000);
            }
        }
        #endregion

        #region Commande_MVVM
        CancellationTokenSource token_source = null;
        public Command chargement
        {
            get
            {
                return new Command(async () =>
                {
                    token_source = new CancellationTokenSource();
                    var token=token_source.Token;
                    try
                    {
                        this.isbusy = true;
                        await register(token);
                        this.isbusy = false;
                        this.isfinish_load = true;
                        await Application.Current.MainPage.DisplayAlert("Fin de chargement !", "En appuyant sur OK vous allez étre rediriger dans quelques secondes", "OK");
                        await this.redirection_compte_rebour(3);
                        await this.service_navigation.NavigateAsync("/BaseNavigationPage/MasterPage");
                    }
                    catch (OperationCanceledException operation_canceled)
                    {
                        this.isbusy = false;
                        if ((await Application.Current.MainPage.DisplayAlert("Question","Le chargement à été annuler.\nvoulez-vous que les membres enregistrer soit conserver ?","Conserver","Ne pas conserver"))==false)
                        {
                            Service_database service_database = new Service_database();
                            service_database.clear_all_members_and_them_infos();
                            await Application.Current.MainPage.DisplayAlert("Confirmation","Les membres ont été supprimer","Ok");
                        }
                    }
                    finally
                    {
                        token_source.Dispose();
                    }
                });
            }
        }

        private async void remove_members_with_them_infos()
        {
            Service_database service_database = new Service_database();
            service_database.clear_all_members_and_them_infos();
            await Application.Current.MainPage.DisplayAlert("Confirmation", "Les membres ont été supprimer", "Ok");
        }

        public Command canceled_load
        {
            get
            {
                return new Command(() => token_source.Cancel()); ;
            }
        }

        #endregion

        #region Methode_navigation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
        }
        #endregion
    }
}
