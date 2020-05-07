using SQLite_origin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Gest.Models
{
    [SQLite.Table("Messages")]
    public class Message
    {
        public Message()
        {
                
        }
        [SQLite.PrimaryKey]
        public int nb_message { get; set; }
    }
}
