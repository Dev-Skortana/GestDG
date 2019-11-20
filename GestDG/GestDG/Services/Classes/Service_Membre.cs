using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Database_Initialize;

namespace GestDG.Services.Classes
{
    class Service_Membre : IService_Membre
    {
        public async Task<bool> delete(Membre membre)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = await connection.ExecuteAsync($"delete from membres");
            return(nombres_records>=1);
        }

        public async Task<Membre> Get(String pseudo)
        {
            var connection=await Database_configuration.Database_Initialize();
            var membres = await connection.QueryAsync<Membre>($"Select * from membres where {(await connection.GetMappingAsync<Membre>()).TableName}.{(await connection.GetMappingAsync<Membre>()).Columns[0].Name} like '%{pseudo}%'");
            return membres.Count!=0 ? membres[0] : null;
        }

        public async Task<IEnumerable<Membre>> GetList(String pseudo)
        {
            var connection = await Database_configuration.Database_Initialize();
            var liste_membres = await connection.QueryAsync<Membre>($"select * from  membre where pseudo like '%{pseudo}%'");
            return liste_membres;
        }

        public async Task<bool> insert(Membre membre)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = await connection.InsertAsync(membre);
            return (nombres_records>=1);
        }

        public async Task<bool> update(Membre membre,Boolean only_change_statut)
        {
            var connection = await Database_configuration.Database_Initialize();
            var query = "";
            var nombres_records =0;
            if (only_change_statut)
            {
                query = $"update Membres set Membres.Statut='{membre.statut}' where Membres.Pseudo='{membre.pseudo}'";
            }
            else
            {
                query = $"update Membre set Membres.date_naissance='{DateTime.ParseExact(membre.date_naissance.ToString(), "yyyy-MM-dd",null).ToString("yyyy-MM-dd")}',Membres.Age={membre.age},Membres.Date_inscription={DateTime.ParseExact(membre.date_inscription.ToString(), "yyyy-MM-dd", null).ToString("yyyy-MM-dd")},Membres.Url_site={membre.url_site},Membres.Url_avatar={membre.url_avatar},Membres.Sexe={membre.sexe},Membres.Localisation={membre.localisation},Membres.Statut={membre.statut},Membres.Rang_nom={membre.rang_nom} where Membres.Pseudo={membre.pseudo}";
            }
            nombres_records= await connection.ExecuteAsync(query);
            return (nombres_records>=1);
        }
    }
}
