using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                                                                    /*[Début] Code pour la création des éléments graphiques en c# au lieu d'utiliser XAML */
            //    //(this.BindingContext as Popup_search_multipleViewModel).Liste_parametres_recherche_sql = new List<Parametre_recherche_sql>() { new Parametre_recherche_sql("pseudo", "", null), new Parametre_recherche_sql("age", 0, null) };
            //    //ListView listview = new ListView();
            //    //listview.HasUnevenRows = true;
            //    //listview.HorizontalOptions = LayoutOptions.CenterAndExpand;
            //    //listview.VerticalOptions = LayoutOptions.EndAndExpand;
            //    //listview.SetBinding(ListView.ItemsSourceProperty, "liste_parametres_recherche_sql");

            //    //listview.ItemTemplate = new DataTemplate(() => {
            //    //    Label label = new Label();
            //    //    label.SetBinding(Label.TextProperty,"Champ");

            //    //    /* Voir comment generer  un objet visuel selon le type de la valeur du champ */


            //    //    Picker picker_methodes_recherches = new Picker();
            //    //    picker_methodes_recherches.Title = "Méthodes de recherches";
            //    //    picker_methodes_recherches.SetBinding(Picker.ItemsSourceProperty, );
            //    //    picker_methodes_recherches.SetBinding(Picker.SelectedItemProperty, );

            //    //    Picker picker_controles_saisies = new Picker();
            //    //    picker_controles_saisies.Title = "Controles de saisies";
            //    //    picker_controles_saisies.ItemsSource = new List<String>() { "Texte", "Date", "Heure", "Date et heure" };
            //    //    picker_controles_saisies.SelectedIndexChanged += picker_controlessaisies_selectedindex_changed;
            //    //    return new ViewCell() { View = new ScrollView() {Orientation=ScrollOrientation.Both, Content = new StackLayout() {Orientation=StackOrientation.Horizontal,HorizontalOptions=LayoutOptions.CenterAndExpand,VerticalOptions=LayoutOptions.CenterAndExpand, Children = { label, picker_methodes_recherches, picker_controles_saisies } } } };
            //    //});
            //    //var button = new Button() { Text="Soumettre",TextColor=Color.Green};
            //    //button.Clicked += Button_Clicked;
            //    //contenaire.Children.Add(listview);
            //    //contenaire.Children.Add(button);
            //    //Content = contenaire;
                                                                                 /* [FIN] */
        }

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

        private void supprime_ancien_controle_dans_zonesaisie_grid(Grid controle_grid)
        {
            for (var i = 0; i < controle_grid.Children.Count; i++)
            {
                int numero_colonne = Grid.GetColumn(controle_grid.Children[i]);
                if ((numero_colonne == 1) || (numero_colonne==2))
                {
                    controle_grid.Children.RemoveAt(i);
                }
            }
        }

        private void ajout_nouveaux_controle_dans_zonesaisie_grid(Grid controle_grid,List<View> liste_nouveaux_controles_graphique)
        {
            int numero_colonne = 1;
            foreach (var controle_graphique_saisie in liste_nouveaux_controles_graphique)
            {
                controle_grid.Children.Add(controle_graphique_saisie);
                Grid.SetRow(controle_graphique_saisie, 0);
                Grid.SetColumn(controle_graphique_saisie, numero_colonne);
                numero_colonne += 1;
            }
        }
       
        private void picker_controlessaisies_selectedindex_changed(Object sender,EventArgs e)
        {
            Picker controle_declencheur = (sender as Picker);
            String element_selectionner = (String)controle_declencheur.SelectedItem;
            List<View> liste_controles_graphique_saisie = generer_liste_controle_graphique(element_selectionner);

            if (liste_controles_graphique_saisie.Count > 0)
            {
                Grid controle_grid = controle_declencheur.Parent as Grid;
                supprime_ancien_controle_dans_zonesaisie_grid(controle_grid);
                ajout_nouveaux_controle_dans_zonesaisie_grid(controle_grid,liste_controles_graphique_saisie);
            }
        }

        private Dictionary<String, Object> generer_dictionnaire_valeursource_valeurcible(Parametre_recherche_sql parametre_recherche_sql, Object nouvelle_donnees)
        {
            Dictionary<String, Object> dictionnaire_valeursource_valeurcible = new Dictionary<string, object>();
            dictionnaire_valeursource_valeurcible.Add("objet_source", parametre_recherche_sql);
            dictionnaire_valeursource_valeurcible.Add("nouvelle_donnees", nouvelle_donnees);
            return dictionnaire_valeursource_valeurcible;
        }

        private void date_selected(object sender, DateChangedEventArgs args)
        {
            Dictionary<String, Object> dictionnaire_valeursource_valeurcible = generer_dictionnaire_valeursource_valeurcible((sender as BindableObject).BindingContext as Parametre_recherche_sql, args.NewDate);
            ((sender as BindableObject).BindingContext as Popup_search_multipleViewModel).Command_update_dateandtime_debut.Execute(dictionnaire_valeursource_valeurcible);
        }

       private void time_selected(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Time")
            {
                Dictionary<String, Object> dictionnaire_valeursource_valeurcible = generer_dictionnaire_valeursource_valeurcible((sender as BindableObject).BindingContext as Parametre_recherche_sql,(sender as TimePicker).Time);
                ((sender as BindableObject).BindingContext as Popup_search_multipleViewModel).Command_update_dateandtime_debut.Execute(dictionnaire_valeursource_valeurcible);
            }
        }

    }
}