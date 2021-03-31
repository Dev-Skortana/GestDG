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
    public partial class Smallprofile_membre : ContentView
    {
        public Smallprofile_membre()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty PseudoProperty = BindableProperty.CreateAttached(propertyName: "Pseudo", returnType: typeof(String), declaringType: typeof(Smallprofile_membre), defaultValue: null);

        public String Pseudo
        {
            get
            {
                return (String)base.GetValue(PseudoProperty);
            }
            set
            {
                base.SetValue(PseudoProperty, value);
            }
        }

        public static readonly BindableProperty Url_avatarProperty = BindableProperty.CreateAttached(propertyName: "Url_avatar", returnType: typeof(String), declaringType: typeof(Smallprofile_membre), defaultValue: null);

        public String Url_avatar
        {
            get
            {
                return (String)base.GetValue(Url_avatarProperty);
            }
            set
            {
                base.SetValue(Url_avatarProperty, value);
            }
        }

        public static readonly BindableProperty Height_propProperty = BindableProperty.CreateAttached(propertyName: "Height_prop", returnType: typeof(Double), declaringType: typeof(Smallprofile_membre), defaultValue:Convert.ToDouble(80));

        public Double Height_prop
        {
            get
            {
                return (Double)base.GetValue(Height_propProperty);
            }
            set
            {
                base.SetValue(Height_propProperty, value);
            }

        }

        public static readonly BindableProperty Width_propProperty = BindableProperty.CreateAttached(propertyName: "Width_prop", returnType: typeof(Double), declaringType: typeof(Smallprofile_membre), defaultValue: Convert.ToDouble(80));

        public Double Width_prop
        {
            get
            {
                return (Double)base.GetValue(Width_propProperty);
            }
            set
            {
                base.SetValue(Width_propProperty, value);
            }

        }

    }
}