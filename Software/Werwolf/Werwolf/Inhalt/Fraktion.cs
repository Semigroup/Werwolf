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
        public enum RuckseitenArt
        {
            /// <summary>
            /// Rückseite wird durch ein Bild bestimmt
            /// </summary>
            Normal,
            /// <summary>
            /// Rückseite = Vorderseite
            /// </summary>
            Identisch,
            /// <summary>
            /// Rückseite = Vorderseite gespiegelt
            /// </summary>
            Deckungsgleich
        }

        public Aufgabe StandardAufgaben { get;  set; }
        public Titel.Art TitelArt { get;  set; }
        public HintergrundBild HintergrundBild { get; set; }
        public HintergrundBild HintergrundBildQuer { get; set; }
        public RuckseitenBild RuckseitenBild { get; set; }
        public TextBild Symbol { get; set; }
        public RuckseitenArt RuckArt { get; set; }

        public Fraktion()
            : base("Fraktion", true)
        {
        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            this.StandardAufgaben = new Aufgabe();
            this.TitelArt = Titel.Art.Rund;
            this.HintergrundBild = Universe.HintergrundBilder.Standard;
            this.HintergrundBildQuer = Universe.HintergrundBilder.Standard;
            this.RuckseitenBild = Universe.RuckseitenBilder.Standard;
            this.Symbol = Universe.TextBilder.Standard;
            this.RuckArt = RuckseitenArt.Normal;
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            StandardAufgaben = Loader.GetAufgabe("StandardAufgaben");
            TitelArt = Loader.XmlReader.GetEnum<Titel.Art>("TitelArt");
            HintergrundBild = Loader.GetHintergrundBild();
            HintergrundBildQuer = Loader.GetHintergrundBildQuer();
            RuckseitenBild = Loader.GetRuckseitenBild();
            Symbol = Loader.GetTextBild();
            RuckArt = Loader.XmlReader.GetEnum<RuckseitenArt>("RuckArt");
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.WriteAttribute("StandardAufgaben", StandardAufgaben.ToString());
            XmlWriter.WriteEnum<Titel.Art>("TitelArt", TitelArt);
            XmlWriter.WriteAttribute("HintergrundBild", HintergrundBild.Name);
            XmlWriter.WriteAttribute("HintergrundBildQuer", HintergrundBildQuer.Name);
            XmlWriter.WriteAttribute("RuckseitenBild", RuckseitenBild.Name);
            XmlWriter.WriteAttribute("TextBild", Symbol.Name);
            XmlWriter.WriteEnum<RuckseitenArt>("RuckArt", RuckArt);
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
            f.Symbol = Symbol;
            f.RuckArt = RuckArt;
            f.HintergrundBildQuer = HintergrundBildQuer;
        }

        public int CompareTo(Fraktion other)
        {
            return Name.CompareTo(other.Name);
        }

        public override void Rescue()
        {
            Universe.HintergrundBilder.Rescue(HintergrundBild);
            Universe.HintergrundBilder.Rescue(HintergrundBildQuer);
            Universe.RuckseitenBilder.Rescue(RuckseitenBild);
            Universe.TextBilder.Rescue(Symbol);
            StandardAufgaben.Rescue();
        }
    }
}
