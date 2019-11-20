using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Database_Initialize;
namespace GestDG.Services.Classes
{
    class Service_Visite : IService_Visite
    {
        public async Task<bool> delete(Visite visite)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.ExecuteAsync($"delete from visites where visites.membre_pseudo='{visite.membre_pseudo}' and visites.connexion_date='{visite.connexion_date.ToString("yyyy - MM - dd hh: mm")}'");
            return (resultat >= 1);
        }

        public async Task<Visite> Get(string pseudo, DateTime date_connexion)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var visites = await connexion.QueryAsync<Visite>($"select * from visites where visites.membre_pseudo='{pseudo}' and visites.connexion_date='{date_connexion.ToString("yyyy-MM-dd hh:mm")}'");
            return visites[0];
        }

        public async Task<IEnumerable<Visite>> GetList()
        {
            var connexion = await Database_configuration.Database_Initialize();
            var visites = await connexion.QueryAsync<Visite>($"select * from visites");
            return visites;
        }

        public async Task<bool> insert(Visite visite)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.InsertAsync(connexion);
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
