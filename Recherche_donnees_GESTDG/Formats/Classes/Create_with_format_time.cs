using System;
using System.Collections.Generic;
using System.Text;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG.Formats.Interface;

namespace Recherche_donnees_GESTDG.Formats.Classes
{
    class Create_with_format_time:ICreate_with_format
    {
        public string Create_condition(string champ, Object valeur, Enumerations_methodes_recherches.methodes_recherches methode_recherche)
        {
            String resultat = "";
            TimeSpan donnees_time = TimeSpan.Parse(valeur.ToString());
            resultat += $"time(strftime('%H:%M' ,datetime({champ}/10000000 - 62135596800, 'unixepoch')))";
            resultat += methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Egale_a ? $"='{donnees_time.ToString("hh:mm")}'" : (methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Superieure ? $">='{donnees_time.ToString("hh:mm")}'" : $"<='{donnees_time.ToString("hh:mm")}'");
            return resultat;
        }

        public Boolean get_format(Object element)
        {
            DateTime date;
            return (DateTime.TryParse(element.ToString(), out date));
        }
    }
}
