using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Recherche_donnees_GESTDG;

namespace Gest.Services.Interfaces
{
    interface INavigation_Goback_Popup_searchbetweendates
    {
       Task navigation_Goback_Popup_searchbetweendates(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
    }
}
