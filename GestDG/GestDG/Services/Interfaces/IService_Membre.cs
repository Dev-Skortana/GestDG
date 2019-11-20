using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;

namespace GestDG.Services.Interfaces
{
    interface IService_Membre
    {
        Task<IEnumerable<Membre>> GetList(String pseudo);
        Task<Membre> Get(String pseudo);
        Task<Boolean> insert(Membre membre);
        Task<Boolean> update(Membre membre,Boolean only_change_statut);
        Task<Boolean> delete(Membre membre);
    }
}
