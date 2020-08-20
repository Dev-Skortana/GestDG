using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using HtmlAgilityPack;
using Xamarin.Forms;
namespace Gest.Helpers
{
    public static class Datefull
    {
        static private Boolean premier = true;
        static public List<int> index_relation = new List<int>();
        static public Dictionary<string, int> dico_mois_visite = new Dictionary<string, int>() { { "Jan", 1 }, { "Fév", 2 }, { "Mar", 3 }, { "Avr", 4 }, { "Mai", 5 }, { "Juin", 6 }, { "Juil", 7 }, { "Aoû", 8 }, { "Sep", 9 }, { "Oct", 10 }, { "Nov", 11 }, { "Déc", 12 } };
        static public List<Date_Part> liste_partie_dates = new List<Date_Part>();
                                /* Le code ci-dessous (aprés ce bloc de commentaire) est expression regulier qui permet de recuperer la date de connexion ,qui pourrait étre de l'un ces exemples ci-dessous
                                                    [EX 1 > Aujourd'hui à 23:20 , EX 2 > Hier à 11:10 , EX 3 > Ven 3 Nov - 7:59]
                                 */
        static public Regex patter = new Regex("((?<aujour>Aujourd'hui)[^à]+à|(?<hier>Hier)[^à]+à|[a-zA-Z]+[^0-9]+(?<jours>[0-9]+)[^a-zA-Z]+(?<mois>[a-zA-Zéû]+))[^0-9]+(?<heures>[0-9]+):(?<minutes>[0-9]+)");
        
       static public int _index = 0;
       static public int index {
            get
            { 
                return _index;
            }
        }


        static public void initialise_index()
        {
            _index = 0;
        }
        static public void incremente_index()
        {
            _index += 1;
        }
        static public List<Date_Part> construct_list(HtmlNode liste)
        {
            List<Date_Part> resultats = new List<Date_Part>();
            liste_partie_dates.ForEach((el) => resultats.Add(el));
            foreach (var element_iteration in liste.Descendants("tr"))
            {              
                var donnees = element_iteration.Descendants("td").ToList()[4].InnerText;
                Match result = patter.Match(donnees);
                if (result.Success)
                {
                    Date_Part date_part;
                    if (result.Groups["aujour"].Value == "Aujourd'hui" || result.Groups["hier"].Value == "Hier")
                    {
                        DateTime date;
                        if (result.Groups["aujour"].Value == "Aujourd'hui") {
                            date = DateTime.Today.Date;
                        }
                        else{
                            date = DateTime.Today.Date.AddDays(-1);
                        }
                        date_part = new Date_Part(mois: date.Month, jour: date.Day, heure: int.Parse(result.Groups["heures"].Value), minute: int.Parse(result.Groups["minutes"].Value), index: index);
                    }
                    else
                    {  
                        date_part=new Date_Part(mois: dico_mois_visite[result.Groups["mois"].Value], jour: int.Parse(result.Groups["jours"].Value), heure: int.Parse(result.Groups["heures"].Value), minute: int.Parse(result.Groups["minutes"].Value), index: index);  
                    }
                    resultats.Add(date_part);
                    index_relation.Add(index);
                }
                incremente_index();
           }
            return resultats;
                
        }
       static public DateTime getdate_visite_full(List<Date_Part> liste_donnees, Date_Part date_part_cible)
       {
              List<int> liste_mois = new List<int>();
              KeyValuePair<int, int> mois_cible = new KeyValuePair<int, int>(date_part_cible.Indexe, Convert.ToInt32(date_part_cible.Mois));
              int annees = DateTime.Today.Year, nbmois_total_after = 0;
              liste_donnees.ForEach((el) => { liste_mois.Add(Convert.ToInt32(el.Mois)); });
              if (mois_cible.Key + 1 < liste_donnees.Count)
              {
                liste_mois.RemoveRange(mois_cible.Key + 1, liste_mois.Count - mois_cible.Key - 1);
              }
          
              for (int iteration = liste_mois.Count - 1; iteration > 0; iteration--)
              {
                  nbmois_total_after = liste_mois[iteration] <= liste_mois[iteration - 1] ? nbmois_total_after + Math.Abs(liste_mois[iteration - 1] - liste_mois[iteration]) : nbmois_total_after + Math.Abs((12 - liste_mois[iteration]) + liste_mois[iteration - 1]);
              }
                if (nbmois_total_after >= 12)
                {
                    annees = (int)annees - (nbmois_total_after / 12);
                }
                else
                {
                    int mois_from_date_max = liste_mois[0];
                    if (mois_from_date_max - nbmois_total_after > 0)
                    { 
                        annees = annees;
                    }
                    else
                    {
                        annees = DateTime.Today.Year - 1;
                    }
                }
              return new DateTime(month: Convert.ToInt32(date_part_cible.Mois), day: date_part_cible.Jour, year: annees, hour: date_part_cible.Heure, minute: date_part_cible.Minute, second:0);
            
        }

    }
}
