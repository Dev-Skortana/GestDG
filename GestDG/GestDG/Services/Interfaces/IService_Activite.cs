using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;

namespace GestDG.Services.Interfaces
{
    interface IService_Activite
    {
        Task<IEnumerable<Activite>> GetList(String libelle_activite);
        Task<Activite> Get(String membre_pseudo,String libelle_activite);
        Task<Boolean> insert(Activite activite);
        Task<Boolean> update(Activite activite);
        Task<Boolean> delete(Activite activite);
    }
}
