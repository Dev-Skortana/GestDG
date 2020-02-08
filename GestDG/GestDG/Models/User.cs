using System;
using System.Collections.Generic;
using System.Text;

namespace GestDG.Models
{
    public class User
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public User(String username,String password)
        {
            this.Username = username;
            this.Password = password;
        }
    }
}
