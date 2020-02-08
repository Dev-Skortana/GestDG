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
	public partial class PosteNBmessage : ContentPage
	{
		public PosteNBmessage ()
		{
			InitializeComponent ();
		}

        private void CarouselViewControl_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            var carrousel = sender as CarouselView.FormsPlugin.Abstractions.CarouselViewControl;
            var viewmodel = carrousel.BindingContext as PosteNBmessageViewModel;
            if (viewmodel!=null) {           
                viewmodel.Command_sync.Execute(e.NewValue);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            zone_saisi_text.IsVisible = !zone_saisi_text.IsVisible;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as PosteNBmessageViewModel).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);
        }
    }
}