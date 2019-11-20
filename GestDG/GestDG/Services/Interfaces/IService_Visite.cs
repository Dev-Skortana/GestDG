using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
namespace GestDG.Services.Interfaces
{
    interface IService_Visite
    {
        Task<IEnumerable<Visite>> GetList();
        Task<Visite> Get(String pseudo,DateTime date_connexion);
        Task<Boolean> insert(Visite visite);
        Task<Boolean> update(Visite visite);
        Task<Boolean> delete(Visite visite);
    }
}
