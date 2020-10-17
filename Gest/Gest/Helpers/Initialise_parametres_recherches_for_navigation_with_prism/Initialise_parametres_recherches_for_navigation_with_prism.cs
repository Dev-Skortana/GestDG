using Prism.Navigation;
using Recherche_donnees_GESTDG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Gest.Helpers.Initialise_parametres_recherches_for_navigation_with_prism
{
    class Initialise_parametres_recherches_for_navigation_with_prism
    {

        public IEnumerable<Parametre_recherche_sql> get_initialise_parametres_recherches_sql_for_navigation_with_prism(INavigationParameters parameters,String key)
        {
            if (check_navigationparameter_has_key_parametres_recherches_sql(parameters,key))
                return (IEnumerable<Parametre_recherche_sql>)parameters[key];
            else
                return new List<Parametre_recherche_sql>();
        }

        private Boolean check_navigationparameter_has_key_parametres_recherches_sql(INavigationParameters parameters,String key)
        {
            return parameters.ContainsKey(key);
        }
    }
}
