using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Database_Initialize;
using SQLite;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;

namespace GestDG.Services.Classes
{
    class Service_Rang : IService_Rang
    {
        public async Task<bool> delete(Rang rang)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = await connection.ExecuteAsync($"delete from Rang where rangs.nom_rang='{rang.nom_rang}'");
            return (nombres_records>=1);
        }

        public async Task<Rang> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connection = await Database_configuration.Database_Initialize();
            var rangs = await connection.QueryAsync<Rang>($"select * from rang where {new Recherche_donnees_GESTDG.Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees, methodes_recherches, recherche_type)}");
            return rangs.Count!=0 ? rangs[0] :null;
        }

        public async Task<IEnumerable<Rang>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connection = await Database_configuration.Database_Initialize();
            var liste = await connection.QueryAsync<Rang>($"select * from rang {new  Recherche_donnees_GESTDG.Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees,methodes_recherches,recherche_type)}");
            return liste;
        }

        public async Task<bool> insert(Rang rang)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = await connection.InsertAsync(rang);
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
