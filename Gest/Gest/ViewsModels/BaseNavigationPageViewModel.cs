using System;
using System.Collections.Generic;
using System.Text;
using Prism.Navigation;

namespace Gest.ViewModels
{
    class BaseNavigationPageViewModel
    {
        #region Interfaces_services
        INavigationService service_navigation;
        #endregion

        #region Constructeure
        public BaseNavigationPageViewModel(INavigationService _navigation)
        {
            this.service_navigation = _navigation;
        }
        #endregion
    }
}
