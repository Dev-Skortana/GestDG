using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GestDG.Models;
using Recherche_donnees_GESTDG.enumeration;

namespace GestDG.Services.Interfaces
{
    interface IService_Membre
    {
        Task<IEnumerable<Membre>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Membre> Get(String pseudo);
        Task<Boolean> insert(Membre membre);
        Task<Boolean> update(Membre membre,Boolean only_change_statut);
        Task<Boolean> delete(Membre membre);
    }
}
