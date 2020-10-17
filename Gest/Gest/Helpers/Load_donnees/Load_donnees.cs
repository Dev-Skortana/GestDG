using Recherche_donnees_GESTDG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Helpers.Load_donnees
{
    interface Load_donnees<type_retour>
    {
       Task<type_retour> get_donnees(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionaire_parametres_recherche);
    }
}
