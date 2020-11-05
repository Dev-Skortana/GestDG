using Gest.Interface_SQLiteAccess;
using Gest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Xamarin.Forms;

namespace Gest.Services.Classes
{
    public class Service_database
    {
        public async void clear_all_members_and_them_infos()
        {
            var file_database = await DependencyService.Get<ISQLiteAccess>().GetAsyncConnection();
            var connection = new SQLite.SQLiteAsyncConnection(file_database);
            await connection.DeleteAllAsync<Visite>();
            await connection.DeleteAllAsync<Membre_Connexion_Message>();
            await connection.DeleteAllAsync<Activite>();
            await connection.DeleteAllAsync<Rang>();
            await connection.DeleteAllAsync<Membre>();
            await connection.DeleteAllAsync<Connexion>();
            await connection.DeleteAllAsync<Message>();
        }

        public async Task<IEnumerable<String>>get_name_champ_of_type_datetime_intable(String name_table)
        {
            var file_database = await DependencyService.Get<ISQLiteAccess>().GetAsyncConnection();
            var connection = new SQLite.SQLiteAsyncConnection(file_database);

            foreach (var tablemapping in connection.GetConnection().TableMappings)
            {
                if (tablemapping.MappedType.Name.ToString().ToUpper()==name_table.ToUpper())
                {
                    return (from column in tablemapping.Columns where column.ColumnType==typeof(DateTime) select column.Name);
                }
            }
            return null;
            
        }
    }
}
