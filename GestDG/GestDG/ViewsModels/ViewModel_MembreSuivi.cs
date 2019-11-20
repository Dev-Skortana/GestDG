using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace GestDG.ViewsModels
{
    class ViewModel_MembreSuivi:INotifyPropertyChanged
    {
        public string title { get; set; } = "";

        private void notifypropertychanged([CallerMemberName] String property = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }

        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
