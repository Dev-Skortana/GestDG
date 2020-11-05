using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gest.Views.Usercontroles
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Activityindicateure : ContentView 
    {
        public Activityindicateure()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty IsloadingProperty = BindableProperty.CreateAttached(propertyName: "Isloading", returnType: typeof(Boolean), declaringType: typeof(Activityindicateure), defaultValue: null);
        
        public Boolean Isloading 
        {
            get 
            {
                return (Boolean)base.GetValue(IsloadingProperty);
            }
            set 
            {
                base.SetValue(IsloadingProperty,value);
            } 
        
        }
       
        
    }
}