using System;
using System.Collections.Generic;
using System.Text;
using Gest.Models;
using System.Threading.Tasks;
using Recherche_donnees_GESTDG.enumeration;

namespace Gest.Services.Interfaces
{
    interface IService_Membre_Connexion_Message
    {
        Task<IEnumerable<Membre_Connexion_Message>> GetList(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Membre_Connexion_Message> Get(Dictionary<String, Object> dictionnaire_donnees, Dictionary<String, String> methodes_recherches, Enumerations_recherches.types_recherches recherche_type);
        Task<Boolean> insert(Membre_Connexion_Message membre_Connexion_Message);
        Task<Boolean> update(Membre_Connexion_Message membre_Connexion_Message);
        Task<Boolean> delete(Membre_Connexion_Message membre_Connexion_Message);
    }
}
