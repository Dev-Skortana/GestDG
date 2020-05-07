using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
namespace Gest.ViewsModels
{
    class Popup_search_betweendatesViewModel:BindableBase
    {
        private INavigationService service_navigation;
        public Popup_search_betweendatesViewModel(INavigationService _service_navigation)
        {
            service_navigation = _service_navigation;
        }

        public ICommand Navigation_Goback
        {
            get
            {
                return new Command(async ()=> {
                    await service_navigation.GoBackAsync(new NavigationParameters()) ;
                });
            }
        }


    }
}
