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
	public partial class ShowPosteNBmessage : ContentPage
	{
		public ShowPosteNBmessage ()
		{
			InitializeComponent ();
		}

        private void CarouselViewControl_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            var carrousel = sender as CarouselView.FormsPlugin.Abstractions.CarouselViewControl;
            var viewmodel = carrousel.BindingContext as ViewsModels.ViewModel_PosteNBmessage;
            viewmodel.test.Execute(e.NewValue);
        }
    }
}