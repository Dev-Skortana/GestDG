using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GestDG.Interface_File_Image_Access
{
    public static class element_commun
    {
        public enum type_image_membre
        {
           Avatar,
           Rang
        }
        public static String Getsubfolder(element_commun.type_image_membre type)
        {    
            var resultat = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Image_Membre", type == element_commun.type_image_membre.Avatar ? "Image_Avatar" : "Image_Rang");
            if (!Directory.Exists(resultat))
            {   
                Directory.CreateDirectory(resultat);
            }
            return resultat;
        }
    }
    
   public interface IFilePicture_Access
    {        
        Task<String> save_picture(element_commun.type_image_membre type);
    }
}
