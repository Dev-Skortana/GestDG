using Gest.Models;
using Gest.Services.Interfaces;
using Recherche_donnees_GESTDG;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Helpers.Load_donnees
{
    class Load_donnees_of_viewmodel_membrevisite<type_retour> :Load_donnees_base, Load_donnees<type_retour>
    {
        IService_Membre service_membre;
        IService_Visite service_visite;

        public Load_donnees_of_viewmodel_membrevisite(IService_Membre service_membre, IService_Visite service_visite)
        {
            this.service_membre = service_membre;
            this.service_visite = service_visite;
        }

        public async Task<type_retour> get_donnees(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionaire_parametres_recherche)
        {
            fill_dictionnaire_parametres_recherche(dictionaire_parametres_recherche);
            type_retour visites_of_membres= (await build_full_infos_visites_of_membres());
            return visites_of_membres;

        }

        private async Task<type_retour> build_full_infos_visites_of_membres()
        {
            IEnumerable<Membre> membres = await get_informations_membres(get_liste_parametres_recherches_of_entity_if_exist(this.dictionnaire_parametres_recherche, "Membre"));
            IEnumerable<Visite> visites =await  get_informations_visites(get_liste_parametres_recherches_of_entity_if_exist(this.dictionnaire_parametres_recherche, "Visite"));  
            type_retour visites_of_membres = get_visites_of_membres(membres,visites);
            visites_of_membres = remove_membre_have_not_visites_if_table_in_search_is_only_visite(visites_of_membres);
            return visites_of_membres;
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
            return (await service_membre.GetList(parametres_recheches_sql_membre)).ToList();
        }

        private async Task<IEnumerable<Visite>> get_informations_visites(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_visite)
        {
            return (await service_visite.GetList(parametres_recheches_sql_visite));
        }

        private type_retour get_visites_of_membres(IEnumerable<Membre> membres,IEnumerable<Visite> visites)
        {
            IDictionary visites_of_membres=new Dictionary<Membre,IEnumerable<visite_custom>>();
            foreach (Membre membre in membres)
                visites_of_membres.Add(membre, visites.Where((visite) => visite.membre_pseudo == membre.pseudo).Select<Visite, visite_custom>((visite) => new visite_custom() { connexion_date = visite.connexion_date, date_enregistrement = visite.date_enregistrement }).ToList());
            return (type_retour)visites_of_membres;
        }

        private type_retour remove_membre_have_not_visites_if_table_in_search_is_only_visite(type_retour dictionnaire_origine)
        {
            if (check_if_table_in_search_is_only_visite())
               return remove_membres_have_not_visites(dictionnaire_origine);
            return dictionnaire_origine;
        }

        private Boolean check_if_table_in_search_is_only_visite()
        {
            if ((this.dictionnaire_parametres_recherche.Count == 1 && dictionnaire_parametres_recherche.ContainsKey("Visite")))
                return true;
            else
                return false;
        }

        private type_retour remove_membres_have_not_visites(type_retour dictionnaire_origine)
        {
            IDictionary<Membre, IEnumerable<visite_custom>> dictionnaire_trie = (IDictionary<Membre,IEnumerable<visite_custom>>)dictionnaire_origine;
            foreach (var item in dictionnaire_trie)
            {
                if (item.Value.Count()!=0)
                    dictionnaire_trie.Add(item.Key, item.Value);
            }
            return (type_retour)dictionnaire_trie;
        }
    }
}
