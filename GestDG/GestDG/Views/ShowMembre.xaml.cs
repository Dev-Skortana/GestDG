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

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {        
            ((sender as Picker).BindingContext as ViewModel_Membre).testcmd.Execute((sender as Picker).SelectedItem);
        }
    }
}