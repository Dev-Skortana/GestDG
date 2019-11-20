using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Database_Initialize;
namespace GestDG.Services.Classes
{
    class Service_Activite : IService_Activite
    {
        public async Task<bool> delete(Activite activite)
        {
            var connection = await Database_configuration.Database_Initialize();
            var resultat = await connection.ExecuteAsync($"delete from activites where activites.membre_pseudo='{activite.membre_pseudo}' and activites.libelle_activite='{activite.libelle_activite}'");
            return (resultat>=1);
        }

        public async Task<Activite> Get(String membre_pseudo, String libelle_activite)
        {
            var connection = await Database_configuration.Database_Initialize();
            var activites = await connection.QueryAsync<Activite>($"Select * from activites where activites.membre_pseudo='{membre_pseudo}' and activites.libelle_activite='{libelle_activite}'");
            return activites[0];
        }

        public async Task<IEnumerable<Activite>> GetList(String libelle_activite)
        {
            var connection = await Database_configuration.Database_Initialize();
            var activites = await connection.QueryAsync<Activite>($"Select * from activites where activites.libelle_activite like '%{libelle_activite}%'");
            return activites;
        }

        public async Task<bool> insert(Activite activite)
        {
            var connection = await Database_configuration.Database_Initialize();
            var resultat = await connection.InsertAsync(activite);
            return (resultat >= 1);
        }

        public async Task<bool> update(Activite activite)
        {
            var connection = await Database_configuration.Database_Initialize();
            var resultat = await connection.ExecuteAsync($"update activites set activites.membre_pseudo='{activite.membre_pseudo}',activites.libelle_activite='{activite.libelle_activite}' where activites.membre_pseudo='{activite.membre_pseudo}' and activites.libelle_activite='{activite.libelle_activite}'");
            return (resultat >= 1);
        }
    }
}
