using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ImageCircle.Forms.Plugin.Droid;
using CarouselView.FormsPlugin.Android;

namespace Gest.Droid
{
    [Activity(Label = "Gest", Icon = "@drawable/AppIcon", Theme = "@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            /*                              Propriters inexistant voir si le nom de ces proprietes à changer.  */
                                                            //TabLayoutResource = Resource.Layout.Tabbar;
                                                            //ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(savedInstanceState);
            
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState); 
            global::Rg.Plugins.Popup.Popup.Init(this,savedInstanceState);
            CarouselViewRenderer.Init();
            ImageCircleRenderer.Init();
            LoadApplication(new App());
        }
    }
}