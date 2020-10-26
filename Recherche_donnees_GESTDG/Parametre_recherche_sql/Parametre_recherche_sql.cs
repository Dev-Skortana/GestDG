using System;
using System.Collections.Generic;
using System.Text;

namespace Recherche_donnees_GESTDG
{
    public class Parametre_recherche_sql
    {
        public String Nom_table { get; set; }
        public String Champ { get; set; }
        public Object Valeur { get; set; }
        public String Methode_recherche { get; set; }

        public Parametre_recherche_sql()
        {

        }

        public Parametre_recherche_sql(String _nom_table,String _champ,Object _valeur,String _methode_recherche)
        {
            this.Nom_table = _nom_table;
            this.Champ = _champ;
            this.Valeur = _valeur;
            this.Methode_recherche = _methode_recherche;
        }
    }
}
