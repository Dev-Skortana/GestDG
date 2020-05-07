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
            
            string path =Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            string dbPath = Path.Combine(path, "Gest.db");
            if (!File.Exists(dbPath))
            {
                using (var br = new BinaryReader(Android.App.Application.Context.Assets.Open("Gest.db")))
                {
                    using (var bw = new BinaryWriter(new FileStream(dbPath, FileMode.OpenOrCreate)))
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
            return dbPath;
        }
    }
}