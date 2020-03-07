using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Database_Initialize;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;

namespace GestDG.Services.Classes
{
    class Service_Membre_Connexion_Message : IService_Membre_Connexion_Message
    {
        public async Task<bool> delete(Membre_Connexion_Message membre_Connexion_Message)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.ExecuteAsync($"delete from membres_connexions_messages where membres_connexions_messages.membre_pseudo='{membre_Connexion_Message.membre_pseudo}' and membres_connexions_messages.connexion_date='{membre_Connexion_Message.connexion_date.ToString("yyyy - MM - dd hh: mm")}' and membres_connexions_messages.message_nb={membre_Connexion_Message.message_nb}");
            return (resultat >= 1);
        }

        public async Task<Membre_Connexion_Message> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var membre_connexion_message = await connexion.QueryAsync<Membre_Connexion_Message>($"select * from membre_connexion_message {new Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees,methodes_recherches,recherche_type)}");
            return membre_connexion_message.Count!=0 ? membre_connexion_message[0] :null;
        }

        public async Task<IEnumerable<Membre_Connexion_Message>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var membre_connexion_message = await connexion.QueryAsync<Membre_Connexion_Message>($"select * from membre_connexion_message {new Creation_recherche_sql().creationclause_conditionrequete(dictionnaire_donnees, methodes_recherches, recherche_type)}");
            return membre_connexion_message;
        }

        public async Task<bool> insert(Membre_Connexion_Message membre_Connexion_Message)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.InsertAsync(membre_Connexion_Message);
            return (resultat >= 1);
        }

        public async Task<bool> update(Membre_Connexion_Message membre_Connexion_Message)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var resultat = await connexion.ExecuteAsync($"update membres_connexions_messages set membres_connexions_messages.membre_pseudo='{membre_Connexion_Message.membre_pseudo}',membres_connexions_messages.connexion_date='{membre_Connexion_Message.connexion_date.ToString("yyyy - MM - dd hh: mm")}',membres_connexions_messages.message_nb={membre_Connexion_Message.message_nb}  where membres_connexions_messages.membre_pseudo='{membre_Connexion_Message.membre_pseudo}' and membres_connexions_messages.connexion_date='{membre_Connexion_Message.connexion_date.ToString("yyyy - MM - dd hh: mm")}' and membres_connexions_messages.message_nb={membre_Connexion_Message.message_nb}");
            return (resultat >= 1);
        }
    }
}
