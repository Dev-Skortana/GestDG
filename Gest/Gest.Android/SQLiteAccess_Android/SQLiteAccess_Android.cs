using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Gest.Droid.SQLiteAccess_Android;
using Gest.Interface_SQLiteAccess;
using System.Threading.Tasks;
using Xamarin.Forms;
[assembly:Dependency(typeof(SQLiteAccess_Android))]

namespace Gest.Droid.SQLiteAccess_Android
{

    class SQLiteAccess_Android : ISQLiteAccess
    {
        public async Task<String> GetAsyncConnection()
        {
            Manager_path_database manager_path_database = new Manager_path_database(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal));
            manager_path_database.create_folder_database_ifnotexist();
            String Path_file_database = manager_path_database.get_path_file_database();
            if (!File.Exists(Path_file_database))
            {
                using (var br = new BinaryReader(Android.App.Application.Context.Assets.Open(manager_path_database.get_name_file_database_full())))
                {
                    using (var bw = new BinaryWriter(new FileStream(Path_file_database, FileMode.OpenOrCreate)))
                    {
                        byte[] buffer = new byte[2048];
                        int length = 0;
                        while ((length = br.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            bw.Write(buffer, 0, length);
                        }
                    }
                }
            }        
            return Path_file_database;
        }
    }
}