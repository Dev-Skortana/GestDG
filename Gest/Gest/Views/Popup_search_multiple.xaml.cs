using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DryIoc;
using Gest.ViewModels;
using Prism.Mvvm;
using Recherche_donnees_GESTDG;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace Gest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Popup_search_multiple : PopupPage
    {
        public Popup_search_multiple()
        {
            InitializeComponent();
        }

        List<String> _liste_methodesrecherches;

        public List<String> Liste_methodesrecherches
        {
            get { return _liste_methodesrecherches; }
            set { _liste_methodesrecherches = value; }
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.Liste_methodesrecherches = (this.BindingContext as Popup_search_multipleViewModel).Liste_methodesrecherches;    
        }

        /* Découper cette méthode en sous méthodes */
        private List<View> generer_liste_controle_graphique(String choix_selectionner)
        {
            List<View> liste_controles = new List<View>();
            if (choix_selectionner == "Text")
            {
                View controle_graphique_saisie = new Entry();
                controle_graphique_saisie.SetBinding(Entry.TextProperty, "Valeur");
                liste_controles.Add(controle_graphique_saisie);
            }
            else if (choix_selectionner == "Date")
            {
                View controle_graphique_saisie = new DatePicker();
                (controle_graphique_saisie as DatePicker).Format = "yyyy/MM/dd";
                controle_graphique_saisie.SetBinding(DatePicker.DateProperty, "Valeur");
                liste_controles.Add(controle_graphique_saisie);
            }
            else if (choix_selectionner == "Heure")
            {
                View controle_graphique_saisie = new TimePicker();
                (controle_graphique_saisie as TimePicker).Format = "HH:mm";
                controle_graphique_saisie.SetBinding(TimePicker.TimeProperty, "Valeur");
                liste_controles.Add(controle_graphique_saisie);
            }
            else if (choix_selectionner == "Date et heure")
            {
                View controle_graphique_saisie_date = new DatePicker();
                (controle_graphique_saisie_date as DatePicker).Format = "yyyy/MM/dd";
                (controle_graphique_saisie_date as DatePicker).DateSelected += date_selected;

                View controle_graphique_saisie_heure = new TimePicker();
                (controle_graphique_saisie_heure as TimePicker).Format = "HH:mm";
                (controle_graphique_saisie_heure as TimePicker).PropertyChanged += time_selected;

                liste_controles.Add(controle_graphique_saisie_date);
                liste_controles.Add(controle_graphique_saisie_heure);
            }
            return liste_controles;
        }

        private void picker_controlessaisies_selectedindex_changed(Object sender,EventArgs e)
        {
            Picker controle_declencheur = (Picker)sender;
            String nom_controlegraphique_saisie_selectionner = retourne_nom_controlegraphique_saisie_selectionner_from_picker(controle_declencheur);
            List<View> liste_controles_graphique_saisie = generer_liste_controle_graphique(nom_controlegraphique_saisie_selectionner);
            if (liste_controles_graphique_saisie.Count > 0)
            {
                Grid controle_parent_grid = (Grid)controle_declencheur.Parent;
                change_controle_saisie_sur_grid(controle_parent_grid,liste_controles_graphique_saisie);
            }
        }

        private String retourne_nom_controlegraphique_saisie_selectionner_from_picker(Picker controle_picker)
        {
            return controle_picker.SelectedItem.ToString();
        }

        private void change_controle_saisie_sur_grid(Grid controle_grid, List<View> controles_graphique_saisie)
        {
            supprime_ancien_controle_dans_zonesaisie_grid(controle_grid);
            ajout_nouveaux_controle_dans_zonesaisie_grid(controle_grid, controles_graphique_saisie);
        }
        
        private void supprime_ancien_controle_dans_zonesaisie_grid(Grid controle_grid)
        {
            int compteure = 0;
            Boolean stop_parcoure = false;
            while ((compteure<controle_grid.Children.Count) && (stop_parcoure==false))
            {
                int numero_colonne = Grid.GetColumn(controle_grid.Children[compteure]);
                int copie_compteure =compteure;
                if ((numero_colonne == 1) || (numero_colonne == 2))
                {                  
                    if (compteure == controle_grid.Children.Count - 1)
                    {
                        stop_parcoure = true;
                    }
                    else
                    {
                        stop_parcoure = false;
                        copie_compteure -= 1;
                    }  
                    controle_grid.Children.RemoveAt(compteure);
                }
                compteure = copie_compteure;
                compteure += 1;
            }         
        }

        private void ajout_nouveaux_controle_dans_zonesaisie_grid(Grid controle_grid,List<View> liste_nouveaux_controles_graphique){
            int numero_colonne = 1;
            foreach (var controle_graphique_saisie in liste_nouveaux_controles_graphique)
            {
                controle_grid.Children.Add(controle_graphique_saisie);
                Grid.SetRow(controle_graphique_saisie, 0);
                Grid.SetColumn(controle_graphique_saisie, numero_colonne);
                numero_colonne += 1;
            }
        }
           
        private void date_selected(object sender, DateChangedEventArgs args)
        {
            Parametre_recherche_sql parametre_recherche_sql = (sender as BindableObject).BindingContext as Parametre_recherche_sql;
            DateTime newdate = args.NewDate;
            launch_command_update_dateandtime(parametre_recherche_sql, newdate);         
        }

       private void time_selected(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                Parametre_recherche_sql parametre_recherche_sql = (sender as BindableObject).BindingContext as Parametre_recherche_sql;
                TimeSpan time = (sender as TimePicker).Time;
                launch_command_update_dateandtime(parametre_recherche_sql,time);
            }
        }
        private void launch_command_update_dateandtime(Parametre_recherche_sql parametre_recherche_sql,Object nouvelle_donnees)
        {
            Dictionary<String, Object> dictionnaire_valeursource_valeurcible = generer_dictionnaire_valeursource_valeurcible(parametre_recherche_sql, nouvelle_donnees);
            (this.BindingContext as Popup_search_multipleViewModel).Command_update_dateandtime.Execute(dictionnaire_valeursource_valeurcible);
        }

        private Dictionary<String, Object> generer_dictionnaire_valeursource_valeurcible(Parametre_recherche_sql parametre_recherche_sql, Object nouvelle_donnees)
        {
            Dictionary<String, Object> dictionnaire_valeursource_valeurcible = new Dictionary<string, object>();
            dictionnaire_valeursource_valeurcible.Add("objet_source", parametre_recherche_sql);
            dictionnaire_valeursource_valeurcible.Add("nouvelle_donnees", nouvelle_donnees);
            return dictionnaire_valeursource_valeurcible;
        }

    }
}