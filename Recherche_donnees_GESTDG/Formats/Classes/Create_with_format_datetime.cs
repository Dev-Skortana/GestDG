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
            resultat += $"strftime('%Y-%m-%d',date(CAST({champ} as text)))";
            resultat += methode_recherche == Enumerations_recherches.methodes_recherches.Egale_a ? $"=strftime('%Y-%m-%d','{donnees_datetime.Date.ToString()}')" : (methode_recherche == Enumerations_recherches.methodes_recherches.Superieure ? $">='{donnees_datetime.Date.ToString("yyyy-MM-dd")}'" : $"<='{donnees_datetime.Date.ToString("yyyy-MM-dd")}'");
            return resultat;
        }

        public Boolean get_format(Object element)
        {
            DateTime date;
            return (DateTime.TryParse(element.ToString(),out date));
        }
    }
}
