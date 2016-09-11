using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Xml;
using Assistment.Texts;

namespace Werwolf.Inhalt
{
    public class Fraktion : XmlElement, IComparable<Fraktion>
    {
        public Aufgabe StandardAufgaben { get;  set; }
        public Titel.Art TitelArt { get;  set; }
        public HintergrundBild HintergrundBild { get; set; }
        public RuckseitenBild RuckseitenBild { get; set; }

        public Fraktion()
            : base("Fraktion")
        {
        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            this.StandardAufgaben = new Aufgabe();
            this.TitelArt = Titel.Art.Rund;
            this.HintergrundBild = Universe.HintergrundBilder.Standard;
            this.RuckseitenBild = Universe.RuckseitenBilder.Standard;
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            StandardAufgaben = Loader.GetAufgabe("StandardAufgaben");
            TitelArt = Loader.XmlReader.getEnum<Titel.Art>("TitelArt");
            HintergrundBild = Loader.GetHintergrundBild();
            RuckseitenBild = Loader.GetRuckseitenBild();
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("StandardAufgaben", StandardAufgaben.ToString());
            XmlWriter.writeEnum<Titel.Art>("TitelArt", TitelArt);
            XmlWriter.writeAttribute("HintergrundBild", HintergrundBild.Name);
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
            f.HintergrundBild = HintergrundBild;
            f.TitelArt = TitelArt;
            f.StandardAufgaben = StandardAufgaben;
            f.RuckseitenBild = RuckseitenBild;
        }

        public int CompareTo(Fraktion other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void Rescue()
        {
            Universe.HintergrundBilder.Rescue(HintergrundBild);
            Universe.RuckseitenBilder.Rescue(RuckseitenBild);
            StandardAufgaben.Rescue();
        }
    }
}
