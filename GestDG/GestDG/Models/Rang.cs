using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GestDG.Models
{
    [Table("Rangs")]
    public class Rang
    {
        [PrimaryKey]
        public String nom_rang { get; set; }
        [NotNull]
        public String url_rang { get; set; }

        [OneToMany]
        public List<Membre> liste_membres { get; set; }
    }
}
