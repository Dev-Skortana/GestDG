using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GestDG.Interface_SQLiteAccess;
using Xamarin.Forms;
using GestDG.UWP.SQLiteAccess_UWP;
using System.Reflection;

[assembly:Dependency(typeof(SQLiteAccess_UWP))]

namespace GestDG.UWP.SQLiteAccess_UWP
{
    class SQLiteAccess_UWP:ISQLiteAccess
    {
        public async Task<String> GetAsyncConnection()
        {
            string path = Windows.Storage.ApplicationData.Current.LocalFolder.Path;
            string dbPath = Path.Combine(path, "GestDG.db");
            var storageFile = IsolatedStorageFile.GetUserStoreForApplication();
            if (!storageFile.FileExists(dbPath))
            {
                var assembly = this.GetType().GetTypeInfo().Assembly;
                using (var resourceStream = assembly.GetManifestResourceStream("GestDG.db"))
                {
                    using (var fileStream = storageFile.CreateFile(dbPath))
                    {
                        byte[] readBuffer = new byte[4096];
                        int bytes = -1;

                        while ((bytes = resourceStream.Read(readBuffer, 0, readBuffer.Length)) > 0)
                        {
                            fileStream.Write(readBuffer, 0, bytes);
                        }
                    }
                }
            }
            return dbPath;
        }
    }
}
