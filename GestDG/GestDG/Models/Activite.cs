using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GestDG.Models
{
    [SQLite.Table(name:"Activites")]
    public class Activite
    {
        public Activite()
        {

        }
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int activite_id { get; set; }

        [ManyToOne]
        public Membre membre { get; set; }
        [SQLite.NotNull, ForeignKey(typeof(Membre))]
        public String membre_pseudo { get; set; }
        [SQLite.NotNull]
        public String libelle_activite { get; set; }
    }
}
