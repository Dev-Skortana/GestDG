using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG.Formats.Interface;

namespace Recherche_donnees_GESTDG.Formats.Classes
{
    public class Create_with_format_string : ICreate_with_format
    {
        public string Create_condition(string champ, Object valeur, Enumerations_methodes_recherches.methodes_recherches methode_recherche)
        {
            String resultat = "";
            String donnees_string = valeur.ToString();
            resultat += $"CAST({champ} as varchar(255))";
            resultat += methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Commence_par ? $" like '{donnees_string}%'" : (methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Fini_par ? $" like '%{donnees_string}'" : (methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Contient ? $" like '%{donnees_string}%'" : $"=='{donnees_string}'"));
            return resultat;
        }

        public Boolean get_format(Object element)
        {
            return ((element is String) && ((Regex.IsMatch(element.ToString(), "[a-zA-Z]+")) || (String.IsNullOrWhiteSpace(element.ToString()))));
        }
    }
}
