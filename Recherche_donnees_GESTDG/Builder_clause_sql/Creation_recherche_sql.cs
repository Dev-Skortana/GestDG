using System;
using System.Collections.Generic;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
using Recherche_donnees_GESTDG.Formats.Interface;
using Recherche_donnees_GESTDG.Formats.Classes;
namespace Recherche_donnees_GESTDG
{
    public class Creation_recherche_sql
    {

        private readonly List<ICreate_with_format> liste_format_condition;

        public Creation_recherche_sql()
        {
            liste_format_condition= new List<ICreate_with_format>() { new Create_with_format_string(), new Create_with_format_datetime(), new Create_with_format_int() };
        }

        public String creationclause_conditionrequete(IEnumerable<Parametre_recherche_sql> parametres_recherches_sql)
        {
            List<Parametre_recherche_sql> liste_parametres_recherches_sql = parametres_recherches_sql!=null ? parametres_recherches_sql.ToList():null;
            String resultat = "";                                                                                                                                         
            if (is_valid_parametres_sql(liste_parametres_recherches_sql))
            {
                resultat = "where ";
                for (var i = 0; i < liste_parametres_recherches_sql.Count; i++)
                {
                    String champ = liste_parametres_recherches_sql[i].Champ; ;
                    Object valeur = liste_parametres_recherches_sql[i].Valeur; ;
                    Enumerations_methodes_recherches.methodes_recherches methode_recherche=(Enumerations_methodes_recherches.methodes_recherches)Enum.Parse(typeof(Enumerations_methodes_recherches.methodes_recherches), liste_parametres_recherches_sql[i].Methode_recherche);
                    Boolean format_connu = liste_format_condition.Exists((item) => item.get_format(liste_parametres_recherches_sql[i].Valeur));
                    if ((i != 0) && (format_connu))
                    {
                        resultat += " and ";
                    }
                    resultat += format_connu ? liste_format_condition.Find((item) => item.get_format(valeur)).Create_condition(champ,valeur,methode_recherche) :String.Empty;
                    
                }
            }
            return resultat;
        }
        private Boolean is_valid_parametres_sql(List<Parametre_recherche_sql> parametres_recherches_sql)
        {
            if((parametres_recherches_sql != null) && (parametres_recherches_sql.Count > 0) && !(parametres_recherches_sql.TrueForAll((parametre) => !liste_format_condition.Exists((itemcondition) => itemcondition.get_format(parametre.Valeur))))){
                return true;
            }
            else{
                return false;
            }
        }
    }
}