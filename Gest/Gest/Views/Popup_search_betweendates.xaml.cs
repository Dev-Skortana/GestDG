using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Pages;
using Gest.ViewModels;

namespace Gest.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Popup_search_betweendates :PopupPage
    {
        public Popup_search_betweendates()
        {
            InitializeComponent();
        }

        /* méthodes d'evenement temporaire voir une solution à long terme  */
        private void CheckBox_heuredebut_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            saisie_heuredebut.IsVisible = (sender as CheckBox).IsChecked ? true : false; 
        }

        private void CheckBox_heurefin_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            saisie_heurefin.IsVisible = (sender as CheckBox).IsChecked ? true : false;
        }

        private void Picker_Names_of_tables_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as Popup_search_betweendatesViewModel).Command_get_names_of_champs_on_tables_selected.Execute(null);
        }
    }
}