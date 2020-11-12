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

        public async Task<Visite> Get(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var visites = await connexion.QueryAsync<Visite>($"select * from visites {new Creation_recherche_sql().creationclause_conditionrequete(parametres_recherches_sql)}");
            return visites.Count!=0 ? visites[0]: null;
        }

        public async Task<IEnumerable<Visite>> GetList(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var visites = await connexion.QueryAsync<Visite>($"select * from visites {new Creation_recherche_sql().creationclause_conditionrequete(parametres_recherches_sql)}");
            return visites;
        }

        public async Task<bool> insert(Visite visite)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.InsertAsync(visite);
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
