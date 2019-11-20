using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestDG.Models
{
    [Table(name:"Messages")]
    public class Message
    {
        public Message()
        {
                
        }
        [PrimaryKey]
        public int nb_message { get; set; }
    }
}
