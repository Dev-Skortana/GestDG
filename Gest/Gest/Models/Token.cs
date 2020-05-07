using System;
using System.Collections.Generic;
using System.Text;

namespace Gest.Models
{
    public class Token
    {
        public int Id { get; set; }
        public String Access_token { get; set; }
        public String Error_description { get; set; }
        public DateTime Expire_date { get; set; }
        public int Expire_in { get; set; }
        public Token()
        {

        }
    }
}
