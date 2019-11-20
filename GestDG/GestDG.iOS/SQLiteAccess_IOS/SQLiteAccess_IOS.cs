using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using GestDG;
using GestDG.Interface_SQLiteAccess;
using GestDG.iOS.SQLiteAccess_IOS;
using Foundation;
[assembly:Dependency(typeof(SQLiteAccess_IOS))]
namespace GestDG.iOS.SQLiteAccess_IOS
{
            ////var sqliteFilename = "GestDG.db3";
            ////string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
            ////string libraryPath = Path.Combine(documentsPath, "..", "Library"); // Library folder
            ////var path = Path.Combine(libraryPath, sqliteFilename);
            ////if (!File.Exists(path))
            ////{
            ////    File.Create(path);
            ////}

    public class SQLiteAccess_IOS :ISQLiteAccess
    {
        public  async Task<String> GetAsyncConnection()
        {
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            string dbPath = Path.Combine(docFolder, "GestDG.db");
            if (!File.Exists(dbPath))
            {
                var existingDb = NSBundle.MainBundle.PathForResource("GestDG", "db");
                File.Copy(existingDb, dbPath);
            }
            return dbPath;
        }
    }
}
