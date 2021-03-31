 using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Gest;
using Gest.Interface_SQLiteAccess;
using Gest.iOS.SQLiteAccess_IOS;
using Foundation;
[assembly:Dependency(typeof(SQLiteAccess_IOS))]
namespace Gest.iOS.SQLiteAccess_IOS
{
    public class SQLiteAccess_IOS :ISQLiteAccess
    {
        public  async Task<String> GetAsyncConnection()
        {
            Manager_path_database manager_path_database = new Manager_path_database(Environment.GetFolderPath(Environment.SpecialFolder.Resources));
            manager_path_database.create_folder_database_ifnotexist();
            String Path_file_database = manager_path_database.get_path_file_database();
            if (!File.Exists(Path_file_database))
            {
                var existingDb = NSBundle.MainBundle.PathForResource(Manager_path_database.NAME_FILE_DATABASE,Manager_path_database.TYPE_FILE_DATABASE);
                File.Copy(existingDb, Path_file_database);
            }
            return Path_file_database;
        }
    }
}
