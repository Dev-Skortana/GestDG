using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite_origin;
using SQLiteNetExtensions.Attributes;
namespace Gest.Models
{
    [SQLite.Table("Visites")]
    public class Visite
    {
        public Visite()
        {

        }
        [SQLite.PrimaryKey,SQLite.AutoIncrement]
        public int visite_id { get; set; }

        [ManyToOne]
        public Membre membre { get; set; }
        [SQLite.NotNull,ForeignKey(typeof(Membre))]
        public String membre_pseudo { get; set; }
        [ManyToOne]
        public Connexion connexion { get; set; }
        [SQLite.NotNull, ForeignKey(typeof(Connexion))]
        public DateTime connexion_date { get; set; }
        public DateTime date_enregistrement { get;}
    }
}
