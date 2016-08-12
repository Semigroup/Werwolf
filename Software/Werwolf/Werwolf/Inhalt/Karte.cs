using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Werwolf.Inhalt
{
    public class Karte : Element
    {
        public Darstellung Darstellung { get; set; }
        public Aufgabe Aufgaben { get; set; }
        public Fraktion Fraktion { get; set; }
        public Gesinnung Gesinnung { get; set; }

        public Karte()
            : base("Karte", false)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            Darstellung = new Darstellung();
            Darstellung.Read(Loader);
            Aufgaben = Loader.GetAufgabe("Aufgaben");
            Fraktion = Loader.GetFraktion();
            Gesinnung = Loader.GetGesinnung();
        }
    }
}
