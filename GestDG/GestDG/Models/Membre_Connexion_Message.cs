using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GestDG.Models
{
    [SQLite.Table("Membres_Connexions_Messages")]
   public class Membre_Connexion_Message
    {
        public Membre_Connexion_Message()
        {
                
        }
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int membre_connexion_message_id { get; set; }

        [ManyToOne]
        public Membre membre { get; set; }
        [SQLite.NotNull, ForeignKey(typeof(Membre))]
        public String membre_pseudo { get; set; }
        [ManyToOne]
        public Connexion connexion { get; set; }
        [SQLite.NotNull, ForeignKey(typeof(Connexion))]
        public DateTime connexion_date { get; set; }
        [ManyToOne]
        public Message message_state { get; set; }
        [SQLite.NotNull, ForeignKey(typeof(Message))]
        public int message_nb { get; set; }
    }
}
