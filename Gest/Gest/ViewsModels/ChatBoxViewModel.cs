using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gest.ViewsModels
{
    class ChatBoxViewModel : BindableBase, INavigationAware
    {
        #region Interfaces_Services

        #endregion

        #region Variables

        #endregion

        #region Constructeurs
        public ChatBoxViewModel()
        {

        }
        #endregion

        #region Commande_MVVM

        #endregion

        #region Methode_Priver

        #endregion

        #region Naviguation_PRISM
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
