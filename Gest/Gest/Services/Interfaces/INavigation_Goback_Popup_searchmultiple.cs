using Recherche_donnees_GESTDG;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Services.Interfaces
{
    interface INavigation_Goback_Popup_searchmultiple
    {
        Task navigation_Goback_Popup_searchmultiple(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql);
    }
}
