using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;
using Gest.Services.Interfaces;
using Gest.Database_Initialize;
using SQLite;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;

namespace Gest.Services.Classes
{
    class Service_Rang : IService_Rang
    {
        public async Task<bool> delete(Rang rang)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = await connection.ExecuteAsync($"delete from Rangs where rangs.nom_rang='{rang.nom_rang}'");
            return (nombres_records>=1);
        }

        public async Task<Rang> Get(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            var connection = await Database_configuration.Database_Initialize();
            var rangs = await connection.QueryAsync<Rang>($"select * from rangs {new Recherche_donnees_GESTDG.Creation_recherche_sql().creationclause_conditionrequete(parametres_recherches_sql)}");
            return rangs.Count!=0 ? rangs[0] :null;
        }

        public async Task<IEnumerable<Rang>> GetList(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            var connection = await Database_configuration.Database_Initialize();
            var liste = await connection.QueryAsync<Rang>($"select * from rangs {new  Recherche_donnees_GESTDG.Creation_recherche_sql().creationclause_conditionrequete(parametres_recherches_sql)}");
            return liste;
        }

        public async Task<bool> insert(Rang rang)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = 0;
            try
            {
                nombres_records=await connection.InsertAsync(rang);
            }
            catch (SQLiteException exception)
            {
                if (exception.Result != SQLite3.Result.Constraint)
                {
                    throw SQLiteException.New(exception.Result, exception.InnerException.ToString());
                }
            }
            return (nombres_records >= 1);
        }

        public async Task<bool> update(Rang rang)
        {
            var connection = await Database_configuration.Database_Initialize();   
            var nombres_records = await connection.ExecuteAsync($"update rangs set url_rang='{rang.url_rang}' where rangs.nom_rang='{rang.nom_rang}'");
            return (nombres_records >= 1);
        }
    }
}
