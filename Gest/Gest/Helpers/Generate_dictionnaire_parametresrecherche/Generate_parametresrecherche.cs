using CarouselView.FormsPlugin.Abstractions;
using Recherche_donnees_GESTDG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gest.Helpers.Generate_dictionnaire_parametresrecherche
{
    class Generate_parametresrecherche
    {
        public IDictionary<String, IEnumerable<Parametre_recherche_sql>> generate(IEnumerable parametres_recherches_sql)
        {
            if (if_parametres_recherches_sql_is_object_oftype_dictionnary_and_has_value(parametres_recherches_sql) == true)
                return (parametres_recherches_sql as IDictionary<String, IEnumerable<Parametre_recherche_sql>>);
            else if (parametres_recherches_sql.GetCount() > 0)
                return create_dictionnary_parametres_recherches_sql_for_one_table(parametres_recherches_sql as IList<Parametre_recherche_sql>);
            else
                return new Dictionary<String, IEnumerable<Parametre_recherche_sql>>();
        }

        private Boolean if_parametres_recherches_sql_is_object_oftype_dictionnary_and_has_value(IEnumerable parametres_sql)
        {
            if ((parametres_sql is IDictionary<String, IEnumerable<Parametre_recherche_sql>>) == true && (parametres_sql as IDictionary<String, IEnumerable<Parametre_recherche_sql>>).Count > 0)
                return true;
            else
                return false;
        }

        private IDictionary<String, IEnumerable<Parametre_recherche_sql>> create_dictionnary_parametres_recherches_sql_for_one_table(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            return new Dictionary<String, IEnumerable<Parametre_recherche_sql>>() { { parametres_recherches_sql.ToList()[0].Nom_table, parametres_recherches_sql } };
        }
    }
}
