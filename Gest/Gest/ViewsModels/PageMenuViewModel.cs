using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Commands;
using Gest.Views;
using Xamarin.Forms;
namespace Gest.ViewModels
{
    class PageMenuViewModel : BindableBase
    {
        #region Interfaces_services
        private INavigationService service_naviguation;
        #endregion

        #region Variables
        public string title { get; set; } = "Gestion interface";
        #endregion

        #region Constructeure
        public PageMenuViewModel(INavigationService _service_navigation) {
            this.service_naviguation = _service_navigation;
        }
        #endregion       
       
    }
}
