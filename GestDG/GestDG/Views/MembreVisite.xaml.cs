using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GestDG.ViewModels;
namespace GestDG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MembreVisite : ContentPage
	{
		public MembreVisite ()
		{
			InitializeComponent ();
		}

     
        private void Button_Clicked(object sender, EventArgs e)
        {
            zone_saisi_text.IsVisible = !zone_saisi_text.IsVisible;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((BindingContext as MembreVisiteViewModel != null)) {
                ((sender as BindableObject).BindingContext as MembreVisiteViewModel).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);
            }
        }

        private void Picker_switchsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (BindingContext as MembreVisiteViewModel != null) {
                ((sender as BindableObject).BindingContext as MembreVisiteViewModel).Command_switch_source.Execute(null);
            }
        }

    }
}