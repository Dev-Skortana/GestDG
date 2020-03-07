using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using GestDG.Interface_SQLiteAccess;
using System.Threading.Tasks;
using SQLite;
using GestDG.Models;

namespace GestDG.Database_Initialize
{
    public static class Database_configuration
    {
        public async static Task<SQLiteAsyncConnection> Database_Initialize()
        {
            var file_database= await DependencyService.Get<ISQLiteAccess>().GetAsyncConnection();
            var connection = new SQLite.SQLiteAsyncConnection(file_database);
            //await connection.DropTableAsync<Rang>();
            //await connection.DropTableAsync<Membre>();
            //await connection.DropTableAsync<Connexion>();
            //await connection.DropTableAsync<Message>();
            //await connection.DropTableAsync<Visite>();
            //await connection.DropTableAsync<Membre_Connexion_Message>();
            //await connection.DropTableAsync<Activite>();

            await connection.CreateTableAsync<Rang>();
            await connection.CreateTableAsync<Membre>();
            await connection.CreateTableAsync<Connexion>();
            await connection.CreateTableAsync<Message>();
            await connection.CreateTableAsync<Visite>();
            await connection.CreateTableAsync<Membre_Connexion_Message>();
            await connection.CreateTableAsync<Activite>();
            return connection;
        }
    }
}
 