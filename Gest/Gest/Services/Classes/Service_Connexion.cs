using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;
using Gest.Services.Interfaces;
using Gest.Database_Initialize;

namespace Gest.Services.Classes
{
    class Service_Connexion : IService_Connexion
    {
        public async Task<bool> delete(Connexion connexion)
        {
            var connexion_database = await Database_configuration.Database_Initialize();
            var resultat = await connexion_database.ExecuteAsync($"delete from Connexions where strftime('%d-%m-%Y %H:%M',datetime(date_connexion/10000000 - 62135596800, 'unixepoch'))='{connexion.date_connexion.ToString("dd-MM-yyyy HH:mm")}'");
            return (resultat>=1);
        }

        public async Task<Connexion> Get(DateTime date_connexion)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.QueryAsync<Connexion>($"select * from Connexions where strftime('%d-%m-%Y %H:%M',datetime(date_connexion/10000000 - 62135596800, 'unixepoch'))='{date_connexion.ToString("dd-MM-yyyy HH:mm")}'");
            return resultat.Count!=0 ? resultat[0]: null;
        }

        public async Task<IEnumerable<Connexion>> GetList()
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.QueryAsync<Connexion>($"select * from Connexions");
            return resultat;
        }

        public async Task<bool> insert(Connexion connexion)
        {
            var connexion_database = await Database_configuration.Database_Initialize();
            var resultat = await connexion_database.InsertAsync(connexion); //connexion_database.ExecuteAsync($"insert into Connexions(date_connexion) values('{connexion.date_connexion.ToString("yyyy-MM-dd HH:mm")}')");
            return (resultat >= 1);
        }

                                                                             /* Méthode à revoir */
        public async Task<bool> update(Connexion connexion)
        {
            var connexion_database = await Database_configuration.Database_Initialize();
            var resultat = await connexion_database.ExecuteAsync($"update Connexions set Date_connexion='{connexion.date_connexion.ToString("yyyy-MM-dd hh:mm")}' where Date_connexion='{connexion.date_connexion.ToString("yyyy-MM-dd hh:mm")}");
            return (resultat >= 1);
        }
    }
}