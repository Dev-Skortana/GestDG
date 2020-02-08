using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using Recherche_donnees_GESTDG.enumeration;

namespace GestDG.Services.Interfaces
{
    interface IService_Activite
    {
        Task<IEnumerable<Activite>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Activite> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Boolean> insert(Activite activite);
        Task<Boolean> update(Activite activite);
        Task<Boolean> delete(Activite activite);
    }
}
