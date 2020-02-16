using GestDG.ViewModels;
using GestDG.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections;

namespace GestDG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PosteNBmessage : ContentPage
	{
		public PosteNBmessage ()
		{
			InitializeComponent ();
            
		}
   
        private void Button_Clicked(object sender, EventArgs e)
        {
            zone_saisi_text.IsVisible = !zone_saisi_text.IsVisible;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);
        }

        private void CarouselViewControl_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            var dictionnaire =(IDictionary) (sender as CarouselView.FormsPlugin.Abstractions.CarouselViewControl)?.ItemsSource;
            var liste_membres = ((IEnumerable)dictionnaire.Keys).Cast<GestDG.Models.Membre>().ToList();
            if ((liste_membres!=null) & (liste_membres.Count != 0))
            {
                ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_sync.Execute(liste_membres[e.NewValue]);
            }
        }

        private void Picker_switchsource_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_switch_source.Execute(null);
        }
    }
}