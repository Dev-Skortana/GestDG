using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using Recherche_donnees_GESTDG.enumeration;

namespace GestDG.Services.Interfaces
{
    interface IService_Rang
    {
        Task<IEnumerable<Rang>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Rang> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Boolean> insert(Rang rang);
        Task<Boolean> update(Rang rang);
        Task<Boolean> delete(Rang rang);
    }
}
