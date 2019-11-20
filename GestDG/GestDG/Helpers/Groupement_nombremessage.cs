using System;
using System.Collections.Generic;
using System.Text;

namespace GestDG.Helpers
{
    public struct Groupement_nombremessage
    {
        private DateTime _date_connexion;

        public DateTime Date_connexion
        {
            get { return _date_connexion; }
            set { _date_connexion = value; }
        }

        private int _nbmessage;

        public int Nbmessage
        {
            get { return _nbmessage; }
            set { _nbmessage = value; }
        }


    }
}
