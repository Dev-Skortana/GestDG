using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;
using Gest.Services.Interfaces;
using Gest.Database_Initialize;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;
using SQLite;

namespace Gest.Services.Classes
{
    class Service_Activite : IService_Activite
    {
        public async Task<bool> delete(Activite activite)
        {
            var connection = await Database_configuration.Database_Initialize();
            var resultat = await connection.ExecuteAsync($"delete from activites where activites.membre_pseudo='{activite.membre_pseudo}' and activites.libelle_activite='{activite.libelle_activite}'");
            return (resultat>=1);
        }

        public async Task<Activite> Get(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            var connection = await Database_configuration.Database_Initialize();
            var activites = await connection.QueryAsync<Activite>($"Select * from activites {new Creation_recherche_sql().creationclause_conditionrequete(parametres_recherches_sql)}");
            return activites.Count!=0 ? activites[0] :null;
        }

        public async Task<IEnumerable<Activite>> GetList(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            var connection = await Database_configuration.Database_Initialize();
            var activites = await connection.QueryAsync<Activite>($"Select * from activites {new Creation_recherche_sql().creationclause_conditionrequete(parametres_recherches_sql)}");
            return activites;
        }

        public async Task<bool> insert(Activite activite)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombre_record = 0;
            try
            {
                 nombre_record = await connection.InsertAsync(activite);
            }
            catch (SQLiteException exception)
            {
                if (exception.Result != SQLite3.Result.Constraint)
                {
                    throw SQLiteException.New(exception.Result, exception.InnerException.ToString());
                }
            }
            return (nombre_record >= 1);
        }

        public async Task<bool> update(Activite activite)
        {
            var connection = await Database_configuration.Database_Initialize();
            var resultat = await connection.ExecuteAsync($"update activites set activites.membre_pseudo='{activite.membre_pseudo}',activites.libelle_activite='{activite.libelle_activite}' where activites.membre_pseudo='{activite.membre_pseudo}' and activites.libelle_activite='{activite.libelle_activite}'");
            return (resultat >= 1);
        }
    }
}
