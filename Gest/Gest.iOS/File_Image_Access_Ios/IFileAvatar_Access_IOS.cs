using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using UIKit;
using Gest.iOS.File_ImagePicture_Access_Ios;
using Gest.Interface_File_Image_Access;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency(typeof(IFilePicture_Access_IOS))]
namespace Gest.iOS.File_ImagePicture_Access_Ios
{
    class IFilePicture_Access_IOS:IFilePicture_Access
    {
        public async Task<String> save_picture(element_commun.type_image_membre type)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("fichier", @System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) + "\\Image_Avatar", "retour");
            var sub_folder = element_commun.Getsubfolder(type);
            return sub_folder;
        }
    }
}