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

namespace Gest.ViewModels
{
    class RegisterMembreViewModel : BindableBase,INavigationAware
    {
        #region Constante
        private const String url_racine= @"https://dynamixgaming.forumgaming.fr/";
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
        public RegisterMembreViewModel(INavigationService _service_navigation,IService_Membre _service_membre, IService_Connexion _service_connexion, IService_Visite _service_visite, IService_Rang _service_rang, IService_Activite _service_activite, IService_Message _service_message, IService_Membre_Connexion_Message _service_membre_connexion_message,RestService rest)
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
                SetProperty(ref _pseudo,value);
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
                SetProperty(ref _image,value);
            }
        }

        private String _message_redirect;
        public String message_redirect
        {
            get { return _message_redirect; }
            set { SetProperty(ref _message_redirect,value); }
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
                SetProperty(ref _membre,value);
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
                SetProperty(ref _connexion,value);
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
                SetProperty(ref _message,value);
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
                SetProperty(ref _activite,value);
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
                SetProperty(ref _rang,value);
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
                SetProperty(ref _visite,value);
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
                SetProperty(ref _membreconnexionmessage,value);
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
                SetProperty(ref _isfinish_load,value);
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
                SetProperty(ref _isbusy,value);
            }
        }
        #endregion

        #region Methode_priver
        /* Méthode temporaire elle remplace les triggers d'interdiction de doublon*/
        private async Task<Boolean> check_doublon_insert<service_table>(service_table service, Object donnees)
        {
            Boolean reponse = false;
            if (service is IService_Membre)
            {
                Models.Membre membre = await (service as IService_Membre).Get(new List<Parametre_recherche_sql>() { new Parametre_recherche_sql() { Champ= "pseudo", Valeur= (donnees as Models.Membre).pseudo, Methode_recherche= "Egale_a" } });
                if (membre != null)
                {
                    reponse = true;
                }
            }
            else if (service is IService_Activite)
            {
                Activite activite = await (service as IService_Activite).Get(new List<Parametre_recherche_sql>() { new Parametre_recherche_sql() { Champ = "membre_pseudo", Valeur = (donnees as Models.Activite).membre_pseudo, Methode_recherche = "Egale_a" }, new Parametre_recherche_sql() { Champ = "libelle_activite", Valeur = (donnees as Activite).libelle_activite, Methode_recherche = "Egale_a" } }); ;
                if (activite != null)
                {
                    reponse = true;
                }
            }
            else if (service is IService_Connexion)
            {
                Connexion connexion = await (service as IService_Connexion).Get(  (donnees as Connexion).date_connexion);
                if (connexion != null)
                {
                    reponse = true;
                }
            }
            else if (service is IService_Message)
            {
                Message message = await (service as IService_Message).Get((donnees as Message).nb_message);
                if (message != null)
                {
                    reponse = true;
                }
            }
            else if (service is IService_Membre_Connexion_Message)
            {
                Membre_Connexion_Message membre_connexion_message = await (service as IService_Membre_Connexion_Message).Get(new List<Parametre_recherche_sql>() { new Parametre_recherche_sql() { Champ = "membre_pseudo", Valeur = (donnees as Models.Membre_Connexion_Message).membre_pseudo, Methode_recherche = "Egale_a" },new Parametre_recherche_sql() { Champ= "connexion_date", Valeur= (donnees as Membre_Connexion_Message).connexion_date, Methode_recherche="Egale_a" },new Parametre_recherche_sql() {Champ= "message_nb", Valeur= (donnees as Membre_Connexion_Message).message_nb, Methode_recherche= "Egale_a" } });
                if (membre_connexion_message != null)
                {
                    reponse = true;
                }
            }
            else if (service is IService_Visite)
            {
                Visite visite = await (service as IService_Visite).Get(new List<Parametre_recherche_sql>() { new Parametre_recherche_sql() { Champ= "membre_pseudo" ,Valeur= (donnees as Models.Visite).membre_pseudo ,Methode_recherche= "Egale_a" },new Parametre_recherche_sql() { Champ= "connexion_date", Valeur= (donnees as Models.Visite).connexion_date, Methode_recherche= "Egale_a" } });
                if (visite != null)
                {
                    reponse = true;
                }
            }
            else if (service is IService_Rang)
            {
                Rang rang = await (service as IService_Rang).Get(new List<Parametre_recherche_sql>() { new Parametre_recherche_sql() { Champ="nom_rang",Valeur= (donnees as Models.Rang).nom_rang,Methode_recherche= "Egale_a" } });
                if (rang != null)
                {
                    reponse = true;
                }
            }
            return await Task.FromResult(reponse);
        }

        private async Task register()
        {
            /* Risque d'obtention d'informations non Syncroniser */
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
            foreach (String lien in liste_pages)
            {
                document_dynamixgaming_page_memberlist.Load(await rest.getresponse(@lien),Encoding.UTF8);
                table_members = document_dynamixgaming_page_memberlist.GetElementbyId("memberlist").SelectSingleNode("tbody");
                Datefull.liste_partie_dates = Datefull.construct_list(table_members);
            }
            Datefull.initialise_index();
            foreach (String lien in liste_pages)
            {
                document_dynamixgaming_page_memberlist.Load(await rest.getresponse(@lien), Encoding.UTF8);
                table_members = document_dynamixgaming_page_memberlist.GetElementbyId("memberlist").SelectSingleNode("tbody");
                foreach (var member_iteration in table_members.Descendants("tr"))
                {           
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


                    if (reponse_insert_rang && await check_doublon_insert<IService_Rang>(new Service_Rang(), this.Rang) == false)
                    {
                        service_rang = new Service_Rang();
                        await service_rang.insert(this.Rang);
                    }

                    if (await check_doublon_insert<IService_Membre>(service_membre, this.Membre) == false)
                    {
                        service_membre = new Service_Membre();
                        await service_membre.insert(this.Membre);
                    }

                    if (reponse_insert_activite && await check_doublon_insert<IService_Activite>(new Service_Activite(), this.Activite) == false)
                    {
                        service_activite = new Service_Activite();
                        await service_activite.insert(this.Activite);
                    }
                    if (reponse_insert_dateconnexion)
                    {
                        if (await check_doublon_insert<IService_Connexion>(new Service_Connexion(), this.Connexion) == false) {
                            service_connexion = new Service_Connexion();
                            await service_connexion.insert(this.Connexion);
                        }
                        if (await check_doublon_insert<IService_Visite>(new Service_Visite(), this.Visite) == false){
                            service_visite = new Service_Visite();
                            await service_visite.insert(this.Visite);
                        }
                        if (await check_doublon_insert<IService_Message>(new Service_Message(),this.Message)==false) {
                            service_message = new Service_Message();
                            await service_message.insert(this.Message);
                        }
                    if (await check_doublon_insert<IService_Membre_Connexion_Message>(new Service_Membre_Connexion_Message(),this.Membreconnexionmessage)==false) {
                        service_membre_connexion_message = new Service_Membre_Connexion_Message();
                        await service_membre_connexion_message.insert(this.Membreconnexionmessage);
                    }
                }

                    this.Membre = null;
                    this.Connexion = null;
                    this.Rang = null;
                    this.Visite = null;
                    this.Activite = null;
                    this.Message = null;
                    this.Membreconnexionmessage = null;       
                    await Task.Delay(1000);
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
        public Command chargement
        {
            get
            {
                return new Command(async () =>
                {

                    this.isbusy = true;
                    await register();
                    this.isbusy = false;
                    this.isfinish_load = true;
                    await Application.Current.MainPage.DisplayAlert("Fin de chargement !","En appuyant sur OK vous allez étre rediriger dans quelques secondes","OK");
                    await this.redirection_compte_rebour(3);
                    await this.service_navigation.NavigateAsync("/BaseNavigationPage/MasterPage");
                });
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
