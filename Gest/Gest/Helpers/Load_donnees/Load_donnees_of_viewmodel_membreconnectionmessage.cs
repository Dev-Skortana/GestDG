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
    class Load_donnees_of_viewmodel_membreconnectionmessage<type_retour> :Load_donnees_base, Load_donnees<type_retour>
    {
        IService_Membre service_membre;
        IService_Membre_Connexion_Message service_membreconnectionmessage;

        public Load_donnees_of_viewmodel_membreconnectionmessage(IService_Membre service_membre, IService_Membre_Connexion_Message service_membre_connection_message)
        {
            this.service_membre = service_membre;
            this.service_membreconnectionmessage = service_membre_connection_message;
        }

        public async Task<type_retour> get_donnees(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionaire_parametres_recherche)
        {
            fill_dictionnaire_parametres_recherche(dictionaire_parametres_recherche);
            type_retour connectionsmessages_of_membres = (await build_full_infos_connectionsmessages_of_membres());
            return connectionsmessages_of_membres;

        }

        private async Task<type_retour> build_full_infos_connectionsmessages_of_membres()
        {
            IEnumerable<Membre> membres = await get_informations_membres(get_parametres_recherches_of_entity_if_exist(this.dictionnaire_parametres_recherche, "Membre"));
            IEnumerable<Membre_Connexion_Message> membresconnectionsmessages = await get_informations_membresconnectionsmessages(get_parametres_recherches_of_entity_if_exist(this.dictionnaire_parametres_recherche, "Message"));
            type_retour connectionsmessages_of_membres = get_connectionsmessages_of_membres(membres, membresconnectionsmessages);
            connectionsmessages_of_membres = remove_membre_have_not_connectionsmessages_if_table_in_search_is_only_membreconnectionmessage(connectionsmessages_of_membres);
            return connectionsmessages_of_membres;
        }

        private IEnumerable<Parametre_recherche_sql> get_parametres_recherches_of_entity_if_exist(IDictionary<String, IEnumerable<Parametre_recherche_sql>> dictionnaire_parametres_recherche, String key)
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

        private async Task<IEnumerable<Membre_Connexion_Message>> get_informations_membresconnectionsmessages(IEnumerable<Parametre_recherche_sql> parametres_recheches_sql_membresconnectionsmessages)
        {
            return (await service_membreconnectionmessage.GetList(parametres_recheches_sql_membresconnectionsmessages));
        }

        private type_retour get_connectionsmessages_of_membres(IEnumerable<Membre> membres, IEnumerable<Membre_Connexion_Message> membresconnexionsmessages)
        {
            IDictionary connectionsmessages_of_membres = new Dictionary<Membre, IEnumerable<Groupement_nombremessage>>();
            foreach (Membre membre in membres)
                connectionsmessages_of_membres.Add(membre, membresconnexionsmessages.Where((membre_connexion_message) => membre_connexion_message.membre_pseudo == membre.pseudo).Select<Membre_Connexion_Message, Groupement_nombremessage>((membre_connexion_message) => new Groupement_nombremessage() { Date_connexion = membre_connexion_message.connexion_date, Nbmessage = membre_connexion_message.message_nb }).ToList());
           
            return (type_retour)connectionsmessages_of_membres;
        }

        private type_retour remove_membre_have_not_connectionsmessages_if_table_in_search_is_only_membreconnectionmessage(type_retour dictionnaire_origine)
        {
            if (check_if_table_in_search_is_only_membreconnectionmessage())
                return remove_membres_have_not_membreconnectionmessage(dictionnaire_origine);
            return dictionnaire_origine;
        }

        private Boolean check_if_table_in_search_is_only_membreconnectionmessage()
        {
            if ((this.dictionnaire_parametres_recherche.Count == 1 && dictionnaire_parametres_recherche.ContainsKey("Message")))
                return true;
            else
                return false;
        }

        private type_retour remove_membres_have_not_membreconnectionmessage(type_retour dictionnaire_origine)
        {
            IDictionary<Membre, IEnumerable<Groupement_nombremessage>> dictionnaire_trie = (IDictionary<Membre, IEnumerable<Groupement_nombremessage>>)dictionnaire_origine;
            foreach (var item in dictionnaire_trie)
            {
                if (item.Value.Count() != 0)
                    dictionnaire_trie.Add(item.Key, item.Value);
            }
            return (type_retour)dictionnaire_trie;
        }
    }
}
