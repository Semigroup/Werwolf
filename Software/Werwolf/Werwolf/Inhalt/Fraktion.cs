using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Xml;
using Assistment.Texts;

namespace Werwolf.Inhalt
{
    public class Fraktion : XmlElement
    {
        public Aufgabe StandardAufgaben { get; private set; }
        public Titel.Art TitelArt { get; private set; }
        public Bild Bild { get; set; }

        public Fraktion()
            : base("Fraktion", true)
        {
        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            this.StandardAufgaben = new Aufgabe("");
            this.TitelArt = Titel.Art.Rund;
            this.Bild = Universe.Bilder.Standard;
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            StandardAufgaben = Loader.GetAufgabe("StandardAufgaben");
            TitelArt = Loader.XmlReader.getEnum<Titel.Art>("TitelArt");
            Bild = Loader.GetBild("Bild");

            Loader.XmlReader.Next();
        }

        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("StandardAufgaben", StandardAufgaben.ToString());
            XmlWriter.writeEnum<Titel.Art>("TitelArt", TitelArt);
            XmlWriter.writeAttribute("Bild", Bild.Name);
        }
    }
}
