using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recherche_donnees_GESTDG.enumeration
{
    public static class Enumeration_methodes_recherches_string
    {
        public enum methodes_recherches
        {
            Commence_par,
            Fini_par,
            Contient,
        }

        public static List<String> get_liste_methodesrecherches()
        {
            return Enum.GetNames(typeof(Enumeration_methodes_recherches_string.methodes_recherches)).Cast<String>().ToList();
        }
    }
}
