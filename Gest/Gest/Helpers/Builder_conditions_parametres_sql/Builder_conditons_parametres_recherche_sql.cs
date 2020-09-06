using Recherche_donnees_GESTDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gest.Helpers.Builder_conditions_parametres_sql
{
    class Builder_conditons_parametres_recherche_sql
    {
        public IDictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> get_dictionnary_conditions_parametres_recherches_sql(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_recherche)
        {
            if (dictionnaire_parametres_recherche.Count == 1)
                return get_configuration_on_search_in_only_onetable(dictionnaire_parametres_recherche);
            else if (dictionnaire_parametres_recherche.Count > 1)
                return get_configuration_on_search_in_manytable(dictionnaire_parametres_recherche);
            else
                return new Dictionary<String, Func<IEnumerable<Parametre_recherche_sql>>>();
        }
        private IDictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> get_configuration_on_search_in_only_onetable(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres)
        {
            var liste_parametres = (dictionnaire_parametres as Dictionary<String, IEnumerable<Parametre_recherche_sql>>).ToList()[0].Value;
            return get_dictionnary_conditions_parametres_recherches_sql_for_one_listeparametres(liste_parametres);
        }

        private IDictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> get_dictionnary_conditions_parametres_recherches_sql_for_one_listeparametres(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql)
        {
            return new Dictionary<string, Func<IEnumerable<Parametre_recherche_sql>>>(){
                { "get_condition_parametres_recherches_sql_membre", ()=> "" == "Membre" ? parametres_recheches_sql : null},
                { "get_condition_parametres_recherches_sql_activite", ()=> "" == "Activite" ? parametres_recheches_sql : null}
             };
        }

        private IDictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> get_configuration_on_search_in_manytable(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres)
        {
            var liste_parametres_membre = get_parametres_recherches_sql_from_nametable_if_exist(dictionnaire_parametres, "Membre").ToList();
            var liste_parametres_activite = get_parametres_recherches_sql_from_nametable_if_exist(dictionnaire_parametres, "Activite").ToList();
            return get_dictionnary_conditions_parametres_recherches_sql_for_two_listesparametres(liste_parametres_membre, liste_parametres_activite);
        }

        private IEnumerable<Parametre_recherche_sql> get_parametres_recherches_sql_from_nametable_if_exist(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres, String key_name_table)
        {
            return dictionnaire_parametres.ContainsKey(key_name_table) ? dictionnaire_parametres[key_name_table] : Enumerable.Empty<Parametre_recherche_sql>();
        }

        private IDictionary<String, Func<IEnumerable<Parametre_recherche_sql>>> get_dictionnary_conditions_parametres_recherches_sql_for_two_listesparametres(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_membre, IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_activite)
        {
            return new Dictionary<string, Func<IEnumerable<Parametre_recherche_sql>>>(){
                { "get_condition_parametres_recherches_sql_membre", ()=> parametres_recheches_sql_membre.Count() > 0 ? parametres_recheches_sql_membre :  Enumerable.Empty<Parametre_recherche_sql>()},
                { "get_condition_parametres_recherches_sql_activite", ()=> parametres_recheches_sql_activite.Count() > 0 ? parametres_recheches_sql_activite : Enumerable.Empty<Parametre_recherche_sql>()}
            };
        }
    }
}
