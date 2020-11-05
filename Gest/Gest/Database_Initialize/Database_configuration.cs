using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Gest.Interface_SQLiteAccess;
using System.Threading.Tasks;
using SQLite;

using Gest.Models;

namespace Gest.Database_Initialize
{
    public static class Database_configuration
    {
        public async static Task<SQLiteAsyncConnection> Database_Initialize()
        {
            var file_database= await DependencyService.Get<ISQLiteAccess>().GetAsyncConnection();
            var connection = new SQLite.SQLiteAsyncConnection(file_database);
            return connection;
        }
    }
}
 