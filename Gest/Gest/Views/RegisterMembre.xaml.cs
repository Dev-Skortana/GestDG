using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gest.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterMembre : ContentPage
	{
		public RegisterMembre ()
		{
			InitializeComponent();
		}

		private async void Label_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "Text" && !String.IsNullOrEmpty((sender as Label).Text))
			{
					var valeur = new Regex("([0-9]+)").Match((sender as Label).Text).Value;
					var number_progress = decimal.Divide(Decimal.Parse(valeur.ToString()), 100);
					await progress_load_members.ProgressTo(double.Parse(number_progress.ToString()), 500, Easing.Linear);
            }
        }
    }
}