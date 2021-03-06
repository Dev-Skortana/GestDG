﻿ using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Gest.Services.Interfaces;
using Prism.Mvvm;
using Prism.Navigation;
using Gest.Services.Classes;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using Gest.Models;
using System.Linq;

namespace Gest.ViewModels
{
    class Popup_search_betweendatesViewModel:BindableBase,INavigationAware
    {
        #region Interfaces_services
        private INavigationService service_navigation;
        #endregion

        #region Variables
        private List<Parametre_recherche_sql> liste_parametres_recherches_sql = new List<Parametre_recherche_sql>();


        private List<String> _names_of_tables;

        public List<String> Names_of_tables
        {
            get { return _names_of_tables; }
            set { SetProperty(ref _names_of_tables, value); }
        }

        private List<String> _names_of_champs_dates;

        public List<String> Names_of_champs_dates
        {
            get { return _names_of_champs_dates; }
            set { SetProperty(ref _names_of_champs_dates,value); }
        }

        public String Name_of_table { get; set; }
        public String Name_of_champ_date { get; set; }
       
       
        
        private DateTime _date_debut;

        public DateTime Date_debut
        {
            get { return _date_debut; }
            set { _date_debut=value; }
        }

        private DateTime _date_fin;

        public DateTime Date_fin
        {
            get { return _date_fin; }
            set { _date_fin = value; }
        }
        #endregion

        #region Constructeur
        public Popup_search_betweendatesViewModel(INavigationService _service_navigation)
        {
            this.service_navigation = _service_navigation;
        }

        #endregion

        #region Methode_priver
        private  DateTime update_dateandtime(Object donnees,DateTime _date_actuel)
        {
            DateTime resultat = new DateTime();
            if ((donnees is DateTime))
            {
                DateTime nouvelle_date = (DateTime)donnees;
                TimeSpan temps_actuel = _date_actuel.TimeOfDay;
                resultat = new DateTime(nouvelle_date.Year, nouvelle_date.Month, nouvelle_date.Day, temps_actuel.Hours, temps_actuel.Minutes, temps_actuel.Seconds);
            }
            else if ((donnees is TimeSpan))
            {
                DateTime date_actuel = _date_actuel;
                TimeSpan nouveau_temps = (TimeSpan)donnees;
                resultat = new DateTime(date_actuel.Year,date_actuel.Month, date_actuel.Day, nouveau_temps.Hours, nouveau_temps.Minutes, nouveau_temps.Seconds);
            }
            return resultat;
        }
        #endregion

        #region Commande_MVVM

        public ICommand Command_get_names_of_champs_on_tables_selected
        {
            get
            {
                return new Command(async ()=> this.Names_of_champs_dates=(await new Service_database().get_names_champs_of_type_datetime_intable(this.Name_of_table)).ToList());
            }
        }
        public ICommand Command_update_dateandtime_debut
        {
            get
            {
                return new Command((donnees) =>
                {
                    this.Date_debut = this.update_dateandtime(donnees, this.Date_debut);
                });
            }
        }
        public ICommand Command_update_dateandtime_fin
         {
            get
            {
                return new Command((donnees) =>
                {
                    this.Date_fin = this.update_dateandtime(donnees, this.Date_fin);
                });
             }
         }

        public ICommand Navigation_Goback
        {
            get
            {
                return new Command(async ()=> {
                    if (String.IsNullOrWhiteSpace(this.Name_of_champ_date)==false) {
                        liste_parametres_recherches_sql.Add(new Parametre_recherche_sql() { Nom_table = this.Name_of_table, Champ = this.Name_of_champ_date, Valeur = this.Date_debut, Methode_recherche = Enumerations_methodes_recherches.methodes_recherches.Superieure.ToString() });
                        liste_parametres_recherches_sql.Add(new Parametre_recherche_sql() { Nom_table = this.Name_of_table, Champ = this.Name_of_champ_date, Valeur = this.Date_fin, Methode_recherche = Enumerations_methodes_recherches.methodes_recherches.Inferieure.ToString() });
                        NavigationParameters parametres = new NavigationParameters();
                        parametres.Add("parametres_recherches_sql", liste_parametres_recherches_sql);
                        await service_navigation.GoBackAsync(parametres);
                    }
                });
            }
        }
        #endregion

        #region Methodes_prism_navigation
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            this.Names_of_tables = (List<String>)parameters["names_tables"];
        }
        #endregion

    }
}
