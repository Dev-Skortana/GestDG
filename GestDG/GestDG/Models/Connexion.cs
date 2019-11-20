using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestDG.Models
{
    [Table(name:"Connexions")]
    public class Connexion
    {
        public Connexion()
        {

        }
        [PrimaryKey]
        public DateTime date_connexion { get; set; }
    }
}
