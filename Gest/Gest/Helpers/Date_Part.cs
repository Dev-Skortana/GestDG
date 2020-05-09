using System;
using System.Collections.Generic;
using System.Text;

namespace Gest.Helpers
{
    public class Date_Part
    {
        public Date_Part() { }
        public Date_Part(int mois, int jour,int heure,int minute, int index)
        {
            this.Mois = mois;
            this.Jour = jour;
            this.Heure = heure;
            this.Minute = minute;
            this.Indexe = index;
        }
        private Nullable<int> _mois;
        public Nullable<int> Mois
        {
            get { return _mois; }
            set { _mois = value >= 1 && value <= 12 ? value : null; }
        }

        private int _jour;
        public int Jour
        {
            get { return _jour; }
            set { _jour = value; }
        }

        private int _heure;

        public int Heure
        {
            get { return _heure; }
            set { _heure = value; }
        }

        private int _minute;

        public int Minute
        {
            get { return _minute; }
            set { _minute = value; }
        }



        private int _indexe;

        public int Indexe
        {
            get { return _indexe; }
            set { _indexe = value; }
        }
    }
}
