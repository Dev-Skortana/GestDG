using Gest.Models;
using Gest.Services.Interfaces;
using Recherche_donnees_GESTDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Helpers.Load_donnees
{
    class Load_donnees_of_viewmodel_membrerang<type_retour> :Load_donnees_base, Load_donnees<type_retour>
    {
        IService_Membre service_membre;
        IService_Rang service_rang;

        public Load_donnees_of_viewmodel_membrerang(IService_Membre service_membre, IService_Rang service_rang)
        {
            this.service_membre = service_membre;
            this.service_rang = service_rang;
        }

        public async Task<type_retour> get_donnees(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionaire_parametres_recherche)
        {
            fill_dictionnaire_parametres_recherche(dictionaire_parametres_recherche);
            type_retour rangs = (await build_full_infos_rangs());
            return rangs;
        }

        private async Task<type_retour> build_full_infos_rangs()
        {
            type_retour rangs;
            rangs = (await get_informations_rang(get_liste_parametres_recherches_of_entity_if_exist(this.dictionnaire_parametres_recherche, "Rang")));
            rangs = (await get_rangs_with_liste_membres_for_each_rang((IEnumerable<Rang>)rangs, get_liste_parametres_recherches_of_entity_if_exist(this.dictionnaire_parametres_recherche, "Membre")));
            rangs = remove_rang_have_not_membre_if_tableactive_is_only_membre_with_searche_simple((IEnumerable<Rang>)rangs);
            return rangs;
        }

        private IEnumerable<Parametre_recherche_sql> get_liste_parametres_recherches_of_entity_if_exist(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_recherche, String key)
        {
            if (dictionnaire_parametres_recherche.ContainsKey(key))
                return dictionnaire_parametres_recherche[key];
            else
                return Enumerable.Empty<Parametre_recherche_sql>();

        }

        private async Task<IEnumerable<Membre>> get_informations_membres(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_membre)
        {
            return (await service_membre.GetList(parametres_recheches_sql_membre));
        }

        private async Task<type_retour> get_rangs_with_liste_membres_for_each_rang(IEnumerable<Rang> rangs, IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_membre)
        {
            IEnumerable<Membre> membres = (await get_informations_membres(parametres_recheches_sql_membre));
            foreach (var rang in rangs)
                rang.liste_membres = membres.Where((membre) => membre.rang_nom == rang.nom_rang).ToList();
            return (type_retour)rangs;
        }

        private async Task<type_retour> get_informations_rang(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_rang)
        {
            return (type_retour)(await service_rang.GetList(parametres_recheches_sql_rang));
        }

        private type_retour remove_rang_have_not_membre_if_tableactive_is_only_membre_with_searche_simple(IEnumerable<Rang> rangs_source)
        {
            if (check_if_tableactive_is_only_membre())
                return remove_rang_havenot_membre(rangs_source);
            return (type_retour)rangs_source;
        }

        private Boolean check_if_tableactive_is_only_membre()
        {
            if ((this.dictionnaire_parametres_recherche.Count == 1 && dictionnaire_parametres_recherche.ContainsKey("Rang")))
                return true;
            else
                return false;
        }

        private type_retour remove_rang_havenot_membre(IEnumerable<Rang> rangs_source)
        {
            return (type_retour)rangs_source.Where((item) => item.liste_membres.Count > 0);
        }
    }
}
