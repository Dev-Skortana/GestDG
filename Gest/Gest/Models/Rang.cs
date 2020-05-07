using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite_origin;
using SQLiteNetExtensions.Attributes;

namespace Gest.Models
{
    [SQLite.Table("Rangs")]
    public class Rang
    {
        [SQLite.PrimaryKey]
        public String nom_rang { get; set; }
        [SQLite.NotNull]
        public String url_rang { get; set; }

        [OneToMany]
        public List<Membre> liste_membres { get; set; }
    }
}
