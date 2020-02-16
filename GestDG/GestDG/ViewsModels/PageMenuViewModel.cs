﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Commands;
using GestDG.Views;
using Xamarin.Forms;
namespace GestDG.ViewModels
{
    class PageMenuViewModel : BindableBase
    {
        public string title { get; set; } = "Gestion interface";
        private INavigationService service_naviguation;

        public PageMenuViewModel(INavigationService _service_navigation) {
            this.service_naviguation = _service_navigation;
        }
           
       
    }
}