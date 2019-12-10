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
	public partial class ShowMembre : ContentPage
	{
		public ShowMembre ()
		{
			InitializeComponent();
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            zone_saisi_text.IsVisible = !zone_saisi_text.IsVisible;
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((sender as BindableObject).BindingContext as ViewModel_Membre).Command_gestion_dictionnaire_champsmethodesrecherches.Execute(null);
        }
    }
}