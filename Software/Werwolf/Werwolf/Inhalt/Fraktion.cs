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
        public Aufgabe StandardAufgaben { get; private set; }
        public Titel.Art TitelArt { get; private set; }

        public Fraktion()
            : base("Fraktion", true)
        {
        }
        public Fraktion(string Name, string StandardAufgaben, Bild Bild, Titel.Art TitelArt)
            : this()
        {
            this.Name = Name;

            this.StandardAufgaben = new Aufgabe(StandardAufgaben);
            this.Bild = Bild;
            this.TitelArt = TitelArt;
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            StandardAufgaben = Loader.GetAufgabe("StandardAufgaben");
            TitelArt = Loader.XmlReader.getEnum<Titel.Art>("TitelArt");
        }

        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("StandardAufgaben", StandardAufgaben.ToString());
            XmlWriter.writeEnum<Titel.Art>("TitelArt", TitelArt);
        }
    }
}
