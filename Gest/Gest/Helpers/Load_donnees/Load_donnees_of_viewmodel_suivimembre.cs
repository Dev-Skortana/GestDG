using Recherche_donnees_GESTDG;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Helpers.Load_donnees
{
    class Load_donnees_of_viewmodel_suivimembre<type_retour> : Load_donnees_base, Load_donnees<type_retour>
    {
        public Task<type_retour> get_donnees(IDictionary<string, IEnumerable<Parametre_recherche_sql>> dictionaire_parametres_recherche)
        {
            throw new NotImplementedException();
        }
    }
}
