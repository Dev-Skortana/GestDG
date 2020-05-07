using Gest.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Services.Interfaces
{
    interface IService_Connexion
    {
        Task<IEnumerable<Connexion>> GetList();
        Task<Connexion> Get(DateTime date_connexion);
        Task<Boolean> insert(Connexion connexion);
        Task<Boolean> update(Connexion connexion);
        Task<Boolean> delete(Connexion connexion);
    }
}
