using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite_origin;
using SQLiteNetExtensions.Attributes;

namespace Gest.Models
{
    [SQLite.Table("Activites")]
    public class Activite
    {
        public Activite()
        {

        }
        [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int activites_id { get; set; }

        [ManyToOne]
        public Membre membre { get; set; }
        [SQLite.NotNull, ForeignKey(typeof(Membre))]
        public String membre_pseudo { get; set; }
        [SQLite.NotNull]
        public String libelle_activite { get; set; }
    }
}
