using Recherche_donnees_GESTDG.enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Recherche_donnees_GESTDG
{
    public class Etat_recherche_sql
    {
        public List<String> Liste_methodesrecherches { get { return Enumerations_recherches.get_liste_methodesrecherches(); } }
        public String methoderecherche_selected { get; set; }
        public List<string> Liste_typesrecherches { get { return Enumerations_recherches.get_liste_typesrecherches(); } }
        public String type_selected { get; set; }
        private List<String> _liste_champs;
        public List<String> Liste_champs { get { return _liste_champs; } private set { _liste_champs = value; } }
        public String champ_selected { get; set; }

        private void initialisation()
        {
            this.champ_selected = Liste_champs[0];
            this.methoderecherche_selected = Liste_methodesrecherches[0];
            this.type_selected = Liste_typesrecherches[0];
        }
        public Etat_recherche_sql()
        {
                
        }

        public Etat_recherche_sql(IEnumerable<String> _liste_champs)
        {
            this.Liste_champs = _liste_champs.ToList();
            this.initialisation();
        }
    }
}
