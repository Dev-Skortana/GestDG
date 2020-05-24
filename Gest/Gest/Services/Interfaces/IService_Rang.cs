using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;

namespace Gest.Services.Interfaces
{
    interface IService_Rang
    {
        Task<IEnumerable<Rang>> GetList(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Rang> Get(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Boolean> insert(Rang rang);
        Task<Boolean> update(Rang rang);
        Task<Boolean> delete(Rang rang);
    }
}
