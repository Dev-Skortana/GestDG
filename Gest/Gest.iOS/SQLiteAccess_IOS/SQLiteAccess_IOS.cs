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
            string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
            string dbPath = Path.Combine(docFolder, "Gest.db");
            if (!File.Exists(dbPath))
            {
                var existingDb = NSBundle.MainBundle.PathForResource("Gest", "db");
                File.Copy(existingDb, dbPath);
            }
            return dbPath;
        }
    }
}
