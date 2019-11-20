using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;
namespace GestDG.Models
{
    [Table(name:"Visites")]
    public class Visite
    {
        public Visite()
        {

        }
        [PrimaryKey,AutoIncrement]
        public int visite_id { get; set; }

        [ManyToOne]
        public Membre membre { get; set; }
        [NotNull,ForeignKey(typeof(Membre))]
        public String membre_pseudo { get; set; }
        [ManyToOne]
        public Connexion connexion { get; set; }
        [NotNull, ForeignKey(typeof(Connexion))]
        public DateTime connexion_date { get; set; }
        public DateTime date_enregistrement { get;}
    }
}
