using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Xml;
using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Printing
{
    public class Job : XmlElement
    {
        public enum RuckBildMode
        {
            Keine,
            Einzeln,
            Nur,
            Gemeinsam
        }

        public Deck Deck { get; set; }
        public Color HintergrundFarbe { get; set; }
        public Color TrennlinienFarbe { get; set; }
        public float Ppm { get; set; }
        public bool Zwischenplatz { get; set; }
        public RuckBildMode MyMode { get; set; }
        public bool FixedFont { get; set; }

        public Job(Universe Universe, string Pfad) :this()
        {
            this.Init(Universe);
            using (Loader l = new Loader(Universe,Pfad))
            {
                l.XmlReader.Next();
                this.Read(l);
                l.Dispose();
            }
        }
        public Job( )
            : base("Job")
        {
        }
        public void Init(Deck Deck, Color HintergrundFarbe, Color TrennlinienFarbe, float Ppm, bool Zwischenplatz, RuckBildMode MyMode, bool FixedFont)
        {
            this.Deck = Deck;
            this.HintergrundFarbe = HintergrundFarbe;
            this.TrennlinienFarbe = TrennlinienFarbe;
            this.Ppm = Ppm;
            this.Zwischenplatz = Zwischenplatz;
            this.MyMode = MyMode;
            this.FixedFont = FixedFont;

            this.Init(Deck.Universe);
            this.Name = this.Schreibname = Deck.Name + "-Job";
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            this.Deck = Loader.GetDeck();
            this.HintergrundFarbe = Loader.XmlReader.getColorHexARGB("HintergrundFarbe");
            this.TrennlinienFarbe = Loader.XmlReader.getColorHexARGB("TrennlinienFarbe");
            this.Ppm = Loader.XmlReader.getFloat("Ppm");
            this.Zwischenplatz = Loader.XmlReader.getBoolean("Zwischenplatz");
            this.MyMode = Loader.XmlReader.getEnum<RuckBildMode>("MyMode");
            this.FixedFont = Loader.XmlReader.getBoolean("FixedFont");
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("Deck", Deck.Name);
            XmlWriter.writeColorHexARGB("HintergrundFarbe", HintergrundFarbe);
            XmlWriter.writeColorHexARGB("TrennlinienFarbe", TrennlinienFarbe);
            XmlWriter.writeFloat("Ppm", Ppm);
            XmlWriter.writeBoolean("Zwischenplatz", Zwischenplatz);
            XmlWriter.writeBoolean("FixedFont", FixedFont);
            XmlWriter.writeEnum<RuckBildMode>("MyMode", MyMode);
        }

        public override void AdaptToCard(Karte Karte)
        {
            throw new NotImplementedException();
        }
        public override void Rescue()
        {
            throw new NotImplementedException();
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
