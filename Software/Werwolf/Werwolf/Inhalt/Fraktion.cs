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
        public Aufgabe StandardAufgaben { get;  set; }
        public Titel.Art TitelArt { get;  set; }
        public Bild Bild { get; set; }
        public Bild RuckseitenBild { get; set; }

        public Fraktion()
            : base("Fraktion")
        {
        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            this.StandardAufgaben = new Aufgabe("");
            this.TitelArt = Titel.Art.Rund;
            this.Bild = Universe.Bilder.Standard;
            this.RuckseitenBild = Universe.Bilder.Standard;
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            StandardAufgaben = Loader.GetAufgabe("StandardAufgaben");
            TitelArt = Loader.XmlReader.getEnum<Titel.Art>("TitelArt");
            Bild = Loader.GetBild("Bild");
            RuckseitenBild = Loader.GetBild("RuckseitenBild");
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("StandardAufgaben", StandardAufgaben.ToString());
            XmlWriter.writeEnum<Titel.Art>("TitelArt", TitelArt);
            XmlWriter.writeAttribute("Bild", Bild.Name);
            XmlWriter.writeAttribute("RuckseitenBild", RuckseitenBild.Name);
        }

        public override void AdaptToCard(Karte Karte)
        {
            Karte.Fraktion = this;
        }
        public override object Clone()
        {
            Fraktion f = new Fraktion();
            Assimilate(f);
            return f;
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            Fraktion f = Element as Fraktion;
            f.Bild = Bild;
            f.TitelArt = TitelArt;
            f.StandardAufgaben = StandardAufgaben;
            f.RuckseitenBild = RuckseitenBild;
        }
    }
}
