using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Xml;
using Assistment.Texts;

namespace Werwolf.Inhalt
{
    public class Fraktion : Element
    {
        public Bild Hintergrundbild { get; private set; }
        public Aufgabe StandardAufgaben { get; private set; }
        public Titel.Art TitelArt { get; private set; }

        public Fraktion()
            : base("Fraktion", true)
        {
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Hintergrundbild = Loader.GetBild("Hintergrundbild");
            StandardAufgaben = Loader.GetAufgabe("StandardAufgaben");
            TitelArt = Loader.XmlReader.getEnum<Titel.Art>("TitelArt");
        }
    }
}
