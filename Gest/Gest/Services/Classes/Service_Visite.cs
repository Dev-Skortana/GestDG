using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;
using Gest.Services.Interfaces;
using Gest.Database_Initialize;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;

namespace Gest.Services.Classes
{
    class Service_Visite : IService_Visite
    {
        public async Task<bool> delete(Visite visite)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.ExecuteAsync($"delete from visites where visites.membre_pseudo='{visite.membre_pseudo}' and visites.connexion_date='{visite.connexion_date.ToString("yyyy - MM - dd hh: mm")}'");
            return (resultat >= 1);
        }

        public async Task<Visite> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var visites = await connexion.QueryAsync<Visite>($"select * from visites {new Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees,methodes_recherches,recherche_type)}");
            return visites.Count!=0 ? visites[0]: null;
        }

        public async Task<IEnumerable<Visite>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var visites = await connexion.QueryAsync<Visite>($"select * from visites {new Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees, methodes_recherches, recherche_type)}");
            return visites;
        }

        public async Task<bool> insert(Visite visite)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.InsertAsync(visite);//connexion.ExecuteAsync($"insert into Visites(membre_pseudo,connexion_date) values('{visite.membre_pseudo}','{visite.connexion_date.ToString("yyyy-MM-dd HH:mm")}')");
            return (resultat >= 1);
        }

        public async Task<bool> update(Visite visite)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.ExecuteAsync($"update visites set visites.membre_pseudo='{visite.membre_pseudo}',visites.connexion_date='{visite.connexion_date.ToString("yyyy - MM - dd hh: mm")}' where visites.membre_pseudo='{visite.membre_pseudo}' and visites.connexion_date='{visite.connexion_date.ToString("yyyy - MM - dd hh: mm")}'");
            return (resultat >= 1);
        }
    }
}
