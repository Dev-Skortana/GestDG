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
    class Service_Membre : IService_Membre
    {
        public async Task<bool> delete(Membre membre)
        {
            var connection = await Database_configuration.Database_Initialize();
            var nombres_records = await connection.ExecuteAsync($"delete from membres");
            return(nombres_records>=1);
        }

        public async Task<Membre> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connection=await Database_configuration.Database_Initialize();
            var membres = await connection.QueryAsync<Membre>($"Select * from membres {new Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees, methodes_recherches, recherche_type)}");
            return membres.Count!=0 ? membres[0] : null;
        }

        public async Task<IEnumerable<Membre>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connection = await Database_configuration.Database_Initialize();
            var liste_membres = await connection.QueryAsync<Membre>($"select pseudo,date_naissance, age, date_inscription, url_site, url_avatar,sexe, localisation, statut, rang_nom from  membres {new Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees,methodes_recherches,recherche_type)}");
            return liste_membres;
        }

        public async Task<bool> insert(Membre membre)
        {
            var connection = await Database_configuration.Database_Initialize();
            //var date_naissance = membre.date_naissance.HasValue ? membre.date_naissance.Value.ToString("yyyy-MM-dd"):"  ";
            //var age = membre.age.HasValue ? membre.age.Value.ToString() : " ";
            int nombres_records=0;
            try
            {
                nombres_records = await connection.InsertAsync(membre);  /*connection.ExecuteAsync($"insert into Membres(pseudo,date_naissance,age,date_inscription,url_site,url_avatar,sexe,localisation,statut,rang_nom) values('{membre.pseudo}','{date_naissance}',{age},'{membre.date_inscription.Value.ToString("yyyy-MM-dd")}','{membre.url_site}','{membre.url_avatar}','{membre.sexe}','{membre.localisation}','{membre.statut}','{membre.rang_nom}')");*/
            }
            catch (InvalidOperationException exception_object_nullable)
            {

            }

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
