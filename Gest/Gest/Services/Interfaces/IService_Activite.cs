using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;

namespace Gest.Services.Interfaces
{
    interface IService_Activite
    {
        Task<IEnumerable<Activite>> GetList(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Activite> Get(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Boolean> insert(Activite activite);
        Task<Boolean> update(Activite activite);
        Task<Boolean> delete(Activite activite);
    }
}
