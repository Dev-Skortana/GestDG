using Gest.Services.Interfaces;
using Recherche_donnees_GESTDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Helpers.Load_donnees
{
    class Load_donnees_of_viewmodel_membre<type_retour> :Load_donnees_base, Load_donnees<type_retour>
    {
        IService_Membre service_membre;
 
        public Load_donnees_of_viewmodel_membre(IService_Membre service_membre)
        {
            this.service_membre = service_membre;
        }

        public async Task<type_retour> get_donnees(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionaire_parametres_recherche)
        {
            fill_dictionnaire_parametres_recherche(dictionaire_parametres_recherche);
            type_retour membres = (await build_infos_membres());
            return membres;
        }

       
        private async Task<type_retour> build_infos_membres()
        {
            type_retour membres;
            membres = (await get_informations_membres(get_liste_parametres_recherches_of_entity_if_exist(dictionnaire_parametres_recherche, "Membre")));  
            return membres;
        }

        private IEnumerable<Parametre_recherche_sql> get_liste_parametres_recherches_of_entity_if_exist(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_recherche, String key)
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

       
    }
}
