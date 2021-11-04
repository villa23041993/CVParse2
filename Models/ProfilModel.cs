using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CVParse2.Models
{
    public class ProfilModel
    {
        public int ID { get; set; }
        //public string Username { get; set; }
       // public string Password { get; set; }
        public string Name { get; set; }
        public string Ort { get; set; }
        public string FirmaPosition { get; set; }
        public string Zusammenfassung { get; set; }
        public string Erfahrung { get; set; }
        public string Location { get; set; }
        public string Skill { get; set; }
    }
}