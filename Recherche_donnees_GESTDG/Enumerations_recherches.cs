using System;
using System.Collections.Generic;
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
            inferieure,
        }
        public enum type_recherche
        {
            Simple,
            Multiples
        }
    }
}
