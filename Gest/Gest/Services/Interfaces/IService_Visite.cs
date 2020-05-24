using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;

namespace Gest.Services.Interfaces
{
    interface IService_Visite
    {
        Task<IEnumerable<Visite>> GetList(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Visite> Get(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Boolean> insert(Visite visite);
        Task<Boolean> update(Visite visite);
        Task<Boolean> delete(Visite visite);
    }
}
