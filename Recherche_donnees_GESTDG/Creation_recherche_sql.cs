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

        public String creationclause_conditionrequete(Dictionary<String,Object> dictionnaire_donnees,Dictionary<String,String> methodes_recherches,Enumerations_recherches.types_recherches recherche_type)
        {
            String resultat = "";                                                                          /* Gerer les conditions dont la valeur de la condition est un format inconnu */ 
                                                                                     /* Verifier la deuxieme condition (rendre cette derniere plus compréhensible ) */
            if ((dictionnaire_donnees!=null) && (dictionnaire_donnees.Count > 0) && !(dictionnaire_donnees.Values.ToList().TrueForAll((itemvalue)=>!liste_format_condition.Exists((itemcondition)=>itemcondition.get_format(itemvalue)))))
            {
                List<String> liste_keys = dictionnaire_donnees.Keys.ToList();
                List<Object> liste_values = dictionnaire_donnees.Values.ToList();
                resultat = "where ";
                for (var i = 0; i < dictionnaire_donnees.Count; i++)
                {     
                    Enumerations_recherches.methodes_recherches methode_recherche=(Enumerations_recherches.methodes_recherches)Enum.Parse(typeof(Enumerations_recherches.methodes_recherches), methodes_recherches[liste_keys[i]]);
                    Boolean format_connu = liste_format_condition.Exists((item) => item.get_format(liste_values[i]));
                    if ((i != 0) && (format_connu))
                    {
                        resultat += " and ";
                    }
                    resultat += format_connu ? liste_format_condition.Find((item) => item.get_format(liste_values[i])).Create_condition(liste_keys[i],liste_values[i],methode_recherche) :String.Empty;
                    
                }
            }
            return resultat;/*liste_keys.Exists((item)=>resultat.Contains(item)) ? resultat:String.Empty;*/
        }
    }
}