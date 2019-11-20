using System;
using System.Collections.Generic;
using System.Text;
using GestDG.Models;
using System.Threading.Tasks;

namespace GestDG.Services.Interfaces
{
    interface IService_Membre_Connexion_Message
    {
        Task<IEnumerable<Membre_Connexion_Message>> GetList();
        Task<Membre_Connexion_Message> Get(String membre_pseudo,DateTime connexion_date,int message_state);
        Task<Boolean> insert(Membre_Connexion_Message membre_Connexion_Message);
        Task<Boolean> update(Membre_Connexion_Message membre_Connexion_Message);
        Task<Boolean> delete(Membre_Connexion_Message membre_Connexion_Message);
    }
}
