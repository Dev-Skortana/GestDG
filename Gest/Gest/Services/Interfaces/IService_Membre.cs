using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Gest.Models;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;

namespace Gest.Services.Interfaces
{
    interface IService_Membre
    {
        Task<IEnumerable<Membre>> GetList(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Membre> Get(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Boolean> insert(Membre membre);
        Task<Boolean> update(Membre membre,Boolean only_change_statut);
        Task<Boolean> delete(Membre membre);
    }
}
