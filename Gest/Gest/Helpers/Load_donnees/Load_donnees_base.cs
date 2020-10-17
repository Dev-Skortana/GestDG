using Recherche_donnees_GESTDG;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gest.Helpers.Load_donnees
{
    class Load_donnees_base
    {
        protected IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_recherche;
        public void fill_dictionnaire_parametres_recherche(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_recherche)
        {
            this.dictionnaire_parametres_recherche = dictionnaire_parametres_recherche;
        }
    }
}
