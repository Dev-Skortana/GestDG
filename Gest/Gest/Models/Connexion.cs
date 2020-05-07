using SQLite_origin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gest.Models
{
    [SQLite.Table("Connexions")]
    public class Connexion
    {
        public Connexion()
        {

        }
        [SQLite.PrimaryKey]
        public DateTime date_connexion { get; set; }
    }
}
