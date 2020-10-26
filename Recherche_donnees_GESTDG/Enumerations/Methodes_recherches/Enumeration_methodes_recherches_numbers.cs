using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recherche_donnees_GESTDG.enumeration
{
    public static class Enumeration_methodes_recherches_numbers
    {
        public enum methodes_recherches
        {
            Egale_a,
            Superieure,
            Inferieure,
        }

        public static List<String> get_liste_methodesrecherches()
        {
            return Enum.GetNames(typeof(Enumeration_methodes_recherches_numbers.methodes_recherches)).Cast<String>().ToList();
        }
    }
}
