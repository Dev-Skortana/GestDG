using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gest.Interface_File_Image_Access;
using Gest.UWP.File_ImageAvatar_Access_Uwp;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;

[assembly: Dependency(typeof(IFilePicture_Access_UWP))]
namespace Gest.UWP.File_ImageAvatar_Access_Uwp
{
    class IFilePicture_Access_UWP : IFilePicture_Access
    {
        public async Task<String> save_picture(element_commun.type_image_membre type)
        {
            await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("fichier", @Windows.Storage.ApplicationData.Current.LocalFolder + "\\Image_Avatar", "retour");
            var sub_folder = element_commun.Getsubfolder(type);
            return sub_folder;
        } 
    }
}