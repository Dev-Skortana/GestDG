using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using GestDG.Services.Interfaces;
using GestDG.Database_Initialize;
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

        public async Task<Membre_Connexion_Message> Get(string membre_pseudo, DateTime connexion_date, int message_state)
        {
            var connexion = await Database_configuration.Database_Initialize();
            var membre_connexion_message = await connexion.QueryAsync<Membre_Connexion_Message>($"select * from membres_connexions_messages where membres_connexions_messages.membre_pseudo='{membre_pseudo}' and membres_connexions_messages.connexion_date='{connexion_date.ToString("   ")}' and membres_connexions_messages.message_state={message_state}");
            return membre_connexion_message[0];
        }

        public async Task<IEnumerable<Membre_Connexion_Message>> GetList()
        {
            var connexion = await Database_configuration.Database_Initialize();
            var membre_connexion_message = await connexion.QueryAsync<Membre_Connexion_Message>($"select * from membres_connexions_messages");
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
