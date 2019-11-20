using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;

namespace GestDG.Services.Interfaces
{
    interface IService_Rang
    {
        Task<IEnumerable<Rang>> GetList(String nom_rang);
        Task<Rang> Get(String nom_rang);
        Task<Boolean> insert(Rang rang);
        Task<Boolean> update(Rang rang);
        Task<Boolean> delete(Rang rang);
    }
}
