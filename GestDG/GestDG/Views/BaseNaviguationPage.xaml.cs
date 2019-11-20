using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GestDG.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BaseNaviguationPage : NavigationPage,INavigationPageOptions
	{
        public bool ClearNavigationStackOnNavigation { get { return true; }}

         
		public BaseNaviguationPage ()
		{
			InitializeComponent ();
		}
	}
}