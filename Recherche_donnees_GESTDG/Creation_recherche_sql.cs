using System;
using System.Collections.Generic;
using System.Linq;
using Recherche_donnees_GESTDG.enumeration;
namespace Recherche_donnees_GESTDG
{
    public static class Creation_recherche_sql
    {
                                                                            /* Trouver une solution  
                                                                            -> Idee : + Description: Mettre en parametre un autre dictionnary ayant un String(qui represente le champ ou la valeur) un autre String(qui representent la methode de recherche).Ensuite dans le corp de cette methode creer dans la boucle FOR une variable d'enumeration methode de recherche dont la valeur dependra de la valeur de la liste de methode de recherche a la position de la boucle 
                                                                                      + problème:Voir comment generer la liste de type String methode de recherche
                                                                            -> Idee : + Description: utiliser la methode Tryparse a la place de la vérification de type
                                                                                      + probleme:la/les valeurs récuperer de la saisie est automatiquement un string ,donc utiliser une autre méthode pour trouver le type de la valeur. 
                                                                            */
        public static String creationclause_conditionrequete(Dictionary<String,Object> dictionnaire_donnees,Dictionary<String,String> methodes_recherches,Enumerations_recherches.type_recherche recherche_type)
        {
            
            List<String> liste_keys = dictionnaire_donnees.Keys.ToList();
            List<Object> liste_values = dictionnaire_donnees.Values.ToList();
            String resultat = "";
            if (dictionnaire_donnees.Count > 0)
            {
                resultat = "where ";
                for (var i = 0; i < dictionnaire_donnees.Count; i++)
                {     
                    Enumerations_recherches.methodes_recherches methode_recherche=(Enumerations_recherches.methodes_recherches)Enum.Parse(typeof(Enumerations_recherches.methodes_recherches), methodes_recherches[liste_keys[i]]);
                    if (i != 0)
                    {
                        resultat += "and ";
                    }
                    if ((liste_values[i] is String))
                    {
                        String valeur = liste_values[i] as String;
                        resultat += $"CAST({liste_keys[i]} as varchar(255))";
                        resultat += methode_recherche == Enumerations_recherches.methodes_recherches.Commence_par ? $" like '{valeur}%'" :(methode_recherche== Enumerations_recherches.methodes_recherches.Fini_par ? $" like '%{valeur}'":(methode_recherche==Enumerations_recherches.methodes_recherches.Contient ? $" like '%{valeur}%'" :$"=={valeur}"));
                    }else if ((liste_values[i] is DateTime) || (liste_values[i] is DateTime?))
                    {
                        resultat += $"CAST({liste_keys[i]} as datetime)=='";
                        resultat += liste_values[i] is DateTime ? $"{(DateTime)liste_values[i]}'" : $"{((DateTime?)liste_values[i]).Value}'";
                    }
                    else if ((liste_values[i] is int) || (liste_values[i] is int?))
                    {
                        resultat += $"CAST({liste_keys[i]} as int)==";
                        resultat += liste_values[i] is int ? $"{(int)liste_values[i]}'" : $"{((int?)liste_values[i]).Value}'";
                    }
                }
            }
            return resultat;
        }
    }
}