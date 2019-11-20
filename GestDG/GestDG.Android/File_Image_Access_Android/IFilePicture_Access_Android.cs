using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.Res;
using GestDG.Droid.File_ImagePicture_Access_Android;
using System.Threading.Tasks;
using GestDG.Interface_File_Image_Access;
using Xamarin.Forms;
[assembly: Dependency(typeof(IFileAvatar_Access_Android))]
namespace GestDG.Droid.File_ImagePicture_Access_Android
{
    class IFileAvatar_Access_Android : IFilePicture_Access
    {
        public async Task<String> save_picture(element_commun.type_image_membre type)
        {
            var sub_folder = element_commun.Getsubfolder(type);
            return sub_folder;
        }
    }
}