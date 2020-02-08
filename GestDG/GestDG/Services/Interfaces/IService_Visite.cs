using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using Recherche_donnees_GESTDG.enumeration;

namespace GestDG.Services.Interfaces
{
    interface IService_Visite
    {
        Task<IEnumerable<Visite>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Visite> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Boolean> insert(Visite visite);
        Task<Boolean> update(Visite visite);
        Task<Boolean> delete(Visite visite);
    }
}
