using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Database_Initialize;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;

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

        public async Task<Activite> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connection = await Database_configuration.Database_Initialize();
            var activites = await connection.QueryAsync<Activite>($"Select * from activite {new Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees,methodes_recherches,recherche_type)}");
            return activites.Count!=0 ? activites[0] :null;
        }

        public async Task<IEnumerable<Activite>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connection = await Database_configuration.Database_Initialize();
            var activites = await connection.QueryAsync<Activite>($"Select * from activite {new Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees, methodes_recherches, recherche_type)}");
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
