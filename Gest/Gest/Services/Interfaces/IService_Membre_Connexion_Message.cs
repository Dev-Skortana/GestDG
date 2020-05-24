using System;
using System.Collections.Generic;
using System.Text;
using Gest.Models;
using System.Threading.Tasks;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG;

namespace Gest.Services.Interfaces
{
    interface IService_Membre_Connexion_Message
    {
        Task<IEnumerable<Membre_Connexion_Message>> GetList(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Membre_Connexion_Message> Get(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
        Task<Boolean> insert(Membre_Connexion_Message membre_Connexion_Message);
        Task<Boolean> update(Membre_Connexion_Message membre_Connexion_Message);
        Task<Boolean> delete(Membre_Connexion_Message membre_Connexion_Message);
    }
}
