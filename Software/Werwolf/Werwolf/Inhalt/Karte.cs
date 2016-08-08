using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Werwolf.Inhalt
{
    public class Karte : Element
    {
        public Universe Universe { get; private set; }

        public Darstellung Darstellung { get; private set; }
        public Bild Bild { get; private set; }
        public Aufgabe Aufgaben { get; private set; }
        public Fraktion Fraktion { get; private set; }
        public Gesinnung Gesinnung { get; private set; }

        public Karte()
            : base("Karte", false)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            Darstellung = new Darstellung();
            Darstellung.Read(Loader);
            Bild = Loader.GetBild("Bild");
            Aufgaben = Loader.GetAufgabe("Aufgaben");
            Fraktion = Loader.GetFraktion();
            Gesinnung = Loader.GetGesinnung();
        }
    }
}
