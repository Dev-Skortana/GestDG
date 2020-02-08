using System;
using System.Collections.Generic;
using System.Text;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG.Formats.Interface;
namespace Recherche_donnees_GESTDG.Formats.Classes
{
    class Create_with_format_datetime : ICreate_with_format
    {
                                                                /* Problème avec ce format */
        public string Create_condition(string champ, Object valeur, Enumerations_recherches.methodes_recherches methode_recherche)
        {
            String resultat = "";
            DateTime donnees_datetime=DateTime.Parse(valeur.ToString());
            resultat += $"julianday({champ})";
            resultat += methode_recherche == Enumerations_recherches.methodes_recherches.Egale_a ? $"=julianday('1996-10-31')" : (methode_recherche == Enumerations_recherches.methodes_recherches.Superieure ? $">=CAST(strftime('%s', '{donnees_datetime.Date}')  AS  integer)" : $"<=CAST(strftime('%s', '{donnees_datetime.Date}')  AS  integer)");
            return resultat;
        }

        public Boolean get_format(Object element)
        {
            DateTime date;
            return (DateTime.TryParse(element.ToString(),out date));
        }
    }
}
