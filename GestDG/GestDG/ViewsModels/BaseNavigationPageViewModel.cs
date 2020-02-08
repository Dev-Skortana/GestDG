using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;

namespace GestDG.ViewModels
{
    class BaseNavigationPageViewModel
    {
        INavigationService service_navigation;

        public BaseNavigationPageViewModel(INavigationService navigation)
        {
            this.service_navigation = navigation;
        }
    }
}
