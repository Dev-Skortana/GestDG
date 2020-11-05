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
        readonly String[] permission = {
            Android.Manifest.Permission.Internet,
            Android.Manifest.Permission.AccessNetworkState,
            Android.Manifest.Permission.WriteExternalStorage,
            Android.Manifest.Permission.ReadExternalStorage
        };
        const int requestid = 0;

        protected override void OnCreate(Bundle bundle)
        {
            /*                              Propriters inexistant voir si le nom de ces proprietes à changer.  */
                                                            //TabLayoutResource = Resource.Layout.Tabbar;
                                                            //ToolbarResource = Resource.Layout.Toolbar;
            base.OnCreate(bundle);
            
            global::Xamarin.Forms.Forms.Init(this, bundle);
            //RequestPermissions(permission,requestid);
            global::Rg.Plugins.Popup.Popup.Init(this,bundle);
            CarouselViewRenderer.Init();
            ImageCircleRenderer.Init();
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }


        public override void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {


            }
        }
    }
}