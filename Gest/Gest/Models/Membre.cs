﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite_origin;
using SQLiteNetExtensions.Attributes;

namespace Gest.Models
{

    [SQLite.Table("Membres")]
    public class Membre
    {

        public Membre() { }

        public Membre(String _pseudo,DateTime? _date_naissance,int? age,DateTime? _date_inscription,String _url_site,String _url_avatar,String _sexe,String _localisation,String _statut)
        {
            //A voir
        }

        [SQLite.PrimaryKey]
        public String pseudo { get; set; }

        public DateTime? date_naissance { get; set; }
        public int? age { get; set; }
        public DateTime? date_inscription { get; set; }
        public String url_site { get; set; }
        public String url_avatar { get; set; }
        public String sexe { get; set; }
        public String localisation { get; set; }
        public String statut { get; set; }
        [ManyToOne]
        public Rang rang { get; set; }
        [ForeignKey(typeof(Rang))]
        public String rang_nom { get; set; }

        [OneToMany]
        public List<Connexion> liste_connexions { get; set; }
        [OneToMany]
        public List<Activite> liste_activites { get; set; }
        [OneToMany]
        public List<Message> liste_messagesnb { get; set; }
    }
}
