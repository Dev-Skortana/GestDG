using GestDG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestDG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SuiviMembre : ContentPage
	{
		public SuiviMembre ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            zone_saisi_text.IsVisible = !zone_saisi_text.IsVisible;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as SuiviMembreViewModel).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);
        }

        private void Picker_switchsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BindingContext as SuiviMembreViewModel != null)
            {
                ((sender as BindableObject).BindingContext as SuiviMembreViewModel).Command_switch_source.Execute(null);
            }
        }
    }
}