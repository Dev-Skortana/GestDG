using Recherche_donnees_GESTDG;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gest.Helpers.Manager_parametre_recherche_sql{
    public class Manager_parametre_recherche_sql{
        public Parametre_recherche_sql update_parametre_recherche_sql(Parametre_recherche_sql parametre_recherche_sql,String nom_table,String champ,string methoderecherche){
            parametre_recherche_sql.Nom_table = nom_table;
            parametre_recherche_sql.Champ = champ;
            parametre_recherche_sql.Methode_recherche = methoderecherche;
            return parametre_recherche_sql;
        }
    }
}
