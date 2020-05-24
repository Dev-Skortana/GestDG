using System;
using System.Collections.Generic;
using Prism.Mvvm;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gest.ViewModels;
using Gest.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gest.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FastMembre : ContentPage
	{
		public FastMembre ()
		{
			InitializeComponent ();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            zone_saisi_text.IsVisible = !zone_saisi_text.IsVisible;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as FastMembreViewModel).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);
        }

        private void Picker_switchsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BindingContext as FastMembreViewModel != null)
            {
                ((sender as BindableObject).BindingContext as FastMembreViewModel).Command_switch_source.Execute(null);
            }
        }

        private void CarouselViewControl_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            if ((sender as BindableObject).BindingContext != null)
            {
                ((sender as BindableObject).BindingContext as FastMembreViewModel).save_membre_selected.Execute(e.NewValue);
            }
        }
    }
}