using Gest.ViewModels;
using Recherche_donnees_GESTDG.enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gest.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MembreActivite : ContentPage
	{
		public MembreActivite ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (zone_saisi_text.IsVisible)
            {
                zone_saisi_text.IsVisible = false;
                zone_saisi_date.IsVisible = true;
            }else if (zone_saisi_date.IsVisible)
            {
                zone_saisi_date.IsVisible = false;
                zone_saisi_text.IsVisible = true;
            }
        }

        private void Picker_SelectedIndexChanged_methodesrecherches(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as MembreActiviteViewModel).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);
        }
        private void Picker_SelectedIndexChanged_champs(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as MembreActiviteViewModel).Command_methodes_recherches.Execute(null);
            ((sender as BindableObject).BindingContext as MembreActiviteViewModel).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);

        }

        private void Picker_switchsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BindingContext as MembreActiviteViewModel != null)
            {
                ((sender as BindableObject).BindingContext as MembreActiviteViewModel).Command_switch_source.Execute(null);
            }
        }
    }
}