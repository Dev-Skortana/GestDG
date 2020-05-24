using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recherche_donnees_GESTDG.enumeration
{
    public static class Enumerations_recherches
    {
        public enum methodes_recherches
        {
            Commence_par,
            Fini_par,
            Contient,
            Egale_a,
            Superieure,
            Inferieure,
        }
        public enum types_recherches
        {
            Simple,
            Multiples
        }

        public static List<String> get_liste_methodesrecherches()
        {
            return Enum.GetNames(typeof(Enumerations_recherches.methodes_recherches)).Cast<String>().ToList();
        }
        public static List<String> get_liste_typesrecherches()
        {
            return Enum.GetNames(typeof(Enumerations_recherches.types_recherches)).Cast<String>().ToList();
        }
    }
}
