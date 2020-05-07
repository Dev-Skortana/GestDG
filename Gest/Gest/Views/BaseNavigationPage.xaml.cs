using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gest.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BaseNavigationPage : NavigationPage,INavigationPageOptions
	{
        public bool ClearNavigationStackOnNavigation { get { return true; }}

         
		public BaseNavigationPage()
		{
			InitializeComponent();
		}
	}
}