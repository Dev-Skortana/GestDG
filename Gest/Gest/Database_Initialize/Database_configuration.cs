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
            //connection.DropTableAsync<Rang>().Wait();
            //connection.DropTableAsync<Membre>().Wait();
            //connection.DropTableAsync<Connexion>().Wait();
            //connection.DropTableAsync<Message>().Wait();
            //connection.DropTableAsync<Visite>().Wait();
            //connection.DropTableAsync<Membre_Connexion_Message>().Wait();
            //connection.DropTableAsync<Activite>().Wait();

            //connection.CreateTableAsync<Rang>().Wait();
            //connection.CreateTableAsync<Membre>().Wait();
            //connection.CreateTableAsync<Connexion>().Wait();
            //connection.CreateTableAsync<Message>().Wait();
            //connection.CreateTableAsync<Visite>().Wait();
            //connection.CreateTableAsync<Membre_Connexion_Message>().Wait();
            //connection.CreateTableAsync<Activite>().Wait();

            return connection;
        }
    }
}
 