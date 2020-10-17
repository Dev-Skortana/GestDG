using Gest.Models;
using Gest.Services.Classes;
using Gest.Services.Interfaces;
using ImTools;
using Recherche_donnees_GESTDG;
using Recherche_donnees_GESTDG.enumeration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Helpers.Load_donnees
{
    class Load_donnees__of_viewmodel_membreactivite<type_retour>:Load_donnees_base,Load_donnees<type_retour>
    {
        IService_Membre service_membre;
        IService_Activite service_activite;

        public Load_donnees__of_viewmodel_membreactivite(IService_Membre service_membre,IService_Activite service_activite)
        {
            this.service_membre = service_membre;
            this.service_activite = service_activite;
        }

        public async Task<type_retour> get_donnees(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionaire_parametres_recherche)
        {
            fill_dictionnaire_parametres_recherche(dictionaire_parametres_recherche);
            type_retour membres = (await build_full_infos_membres());
            return membres;
        }

        private async Task<type_retour> build_full_infos_membres()
        {
            type_retour membres;
            membres = (await get_informations_membres(get_liste_parametres_recherches_of_entity_if_exist(this.dictionnaire_parametres_recherche,"Membre")));
            membres = (await get_membres_with_liste_activite_for_each_membre((IEnumerable<Membre>)membres, get_liste_parametres_recherches_of_entity_if_exist(this.dictionnaire_parametres_recherche,"Activite")));
            membres = remove_membre_have_not_activite_if_tableactive_is_only_activite_with_searche_simple((IEnumerable<Membre>)membres);
            return membres;
        }

        private IEnumerable<Parametre_recherche_sql> get_liste_parametres_recherches_of_entity_if_exist(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_recherche,String key)
        {
            if (dictionnaire_parametres_recherche.ContainsKey(key))
                return dictionnaire_parametres_recherche[key];
            else
                return Enumerable.Empty<Parametre_recherche_sql>();

        }

        private async Task<type_retour> get_informations_membres(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_membre)
        {
            return (type_retour)(await service_membre.GetList(parametres_recheches_sql_membre));
        }

        private async Task<type_retour> get_membres_with_liste_activite_for_each_membre(IEnumerable<Membre> membres, IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_activite)
        {
            IEnumerable<Activite> activites = (await get_information_activite(parametres_recheches_sql_activite));
            foreach (var membre in membres)
                membre.liste_activites = activites.Where((activite)=>activite.membre_pseudo==membre.pseudo).ToList(); 
            return (type_retour)membres;
        }

        private async Task<IEnumerable<Activite>> get_information_activite(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_activite)
        {
            return (await service_activite.GetList(parametres_recheches_sql_activite));
        }

        private type_retour remove_membre_have_not_activite_if_tableactive_is_only_activite_with_searche_simple(IEnumerable<Membre> membres_source)
        {    
            if (check_if_tableactive_is_only_activite())
                return remove_membre_havenot_activite(membres_source);
            return (type_retour)membres_source;
        }

        private Boolean check_if_tableactive_is_only_activite()
        {
            if ((this.dictionnaire_parametres_recherche.Count==1 && dictionnaire_parametres_recherche.ContainsKey("Activite")))
                return true;
            else
                return false;
        }

        private type_retour remove_membre_havenot_activite(IEnumerable<Membre> membres_source)
        {
            return (type_retour)membres_source.Where((membre)=> membre.liste_activites.Count>0);     
        }
    }
}
