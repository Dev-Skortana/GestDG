using System;
using System.Collections.Generic;
using System.Text;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG.Formats.Interface;

namespace Recherche_donnees_GESTDG.Formats.Classes
{
    class Create_with_format_int : ICreate_with_format
    {
        public string Create_condition(string champ, Object valeur, Enumerations_methodes_recherches.methodes_recherches methode_recherche)
        {
            String resultat="";
            int donnees_int = int.Parse(valeur.ToString());
            resultat += $"CAST({champ} as int)";
            resultat += methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Egale_a ? $"={donnees_int}" : (methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Superieure ? $">={donnees_int}" : $"<={donnees_int}");
            return resultat;
        }

        public Boolean get_format(Object element)
        {
            int nombre;
            return (int.TryParse(element.ToString(),out nombre));
        }
    }
}
