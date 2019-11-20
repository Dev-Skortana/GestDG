using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GestDG.ViewsModels;
namespace GestDG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ShowMembreVisite : ContentPage
	{
		public ShowMembreVisite ()
		{
			InitializeComponent ();
		}

        private void CarouselViewControl_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            var bd = BindingContext as ViewModel_Visite;
            bd.testcommande.Execute(parameter: null);
        }
    }
}