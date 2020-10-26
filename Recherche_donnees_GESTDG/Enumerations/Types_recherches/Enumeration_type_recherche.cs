using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recherche_donnees_GESTDG
{
    public static class Enumeration_type_recherche
    {
        public enum types_recherches
        {
            Simple,
            Multiples
        }

        public static List<String> get_liste_typesrecherches()
        {
            return Enum.GetNames(typeof(Enumeration_type_recherche.types_recherches)).Cast<String>().ToList();
        }
    }
}
