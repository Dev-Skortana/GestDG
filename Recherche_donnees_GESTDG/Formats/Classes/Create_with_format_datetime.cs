using System;
using System.Collections.Generic;
using System.Text;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG.Formats.Interface;
namespace Recherche_donnees_GESTDG.Formats.Classes
{
    class Create_with_format_datetime : ICreate_with_format
    {
                                                               
        public string Create_condition(string champ, Object valeur, Enumerations_methodes_recherches.methodes_recherches methode_recherche)
        {
            String resultat = "";
            DateTime donnees_datetime=DateTime.Parse(valeur.ToString());
            String representation_datetime_langage = get_representation_datetime_langage(donnees_datetime);
            String representation_datetime_database =get_representation_datetime_database(representation_datetime_langage);
            resultat += $"{(representation_datetime_database=="%Y-%m-%d" ? "date" :(representation_datetime_database=="%Y-%m-%d %H:%M" ? "datetime":""))}(strftime('{representation_datetime_database}' ,datetime({champ}/10000000 - 62135596800, 'unixepoch')))";
            resultat += methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Egale_a ? $"='{donnees_datetime.ToString(representation_datetime_langage)}'" : (methode_recherche == Enumerations_methodes_recherches.methodes_recherches.Superieure ? $">='{donnees_datetime.ToString(representation_datetime_langage)}'" : $"<='{donnees_datetime.ToString(representation_datetime_langage)}'");
            return resultat;
        }

        public Boolean get_format(Object element)
        {
            DateTime date;
            return (DateTime.TryParse(element.ToString(),out date));
        }

                                                /*Les noms des deux méthodes c-dessous ,utilisent le mot "représentation" au lieu d'utiliser le mot "format".Pour éviter la confusion avec le nom de cette classe qui utilise aussi le mot "format".*/                                        

        private String get_representation_datetime_langage(DateTime valeur)
        {
            String resultat;
            if (valeur.TimeOfDay.Hours==0 && valeur.TimeOfDay.Minutes == 0)
            {
                resultat = "yyyy-MM-dd";
            }
            else
            {
                resultat = "yyyy-MM-dd HH:mm";
            }
            return resultat;
        }
        private String get_representation_datetime_database(String representation_datetime_langage)
        {
            String resultat="";
            if (representation_datetime_langage == "yyyy-MM-dd")
            {
                resultat ="%Y-%m-%d";
            }
            else if (representation_datetime_langage == "yyyy-MM-dd HH:mm")
            {
                resultat = "%Y-%m-%d %H:%M";
            }
            return resultat;
        }
    }
}
