using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Universe : XmlElement
    {
        public ElementMenge<HintergrundDarstellung> HintergrundDarstellungen { get; private set; }
        public ElementMenge<TitelDarstellung> TitelDarstellungen { get; private set; }
        public ElementMenge<BildDarstellung> BildDarstellungen { get; private set; }
        public ElementMenge<TextDarstellung> TextDarstellungen { get; private set; }
        public ElementMenge<InfoDarstellung> InfoDarstellungen { get; private set; }

        public ElementMenge<HauptBild> HauptBilder { get; private set; }
        public ElementMenge<HintergrundBild> HintergrundBilder { get; private set; }
        public ElementMenge<RuckseitenBild> RuckseitenBilder { get; private set; }
        public ElementMenge<TextBild> TextBilder { get; private set; }

        public ElementMenge<Fraktion> Fraktionen { get; private set; }
        public ElementMenge<Gesinnung> Gesinnungen { get; private set; }
        public ElementMenge<Karte> Karten { get; private set; }

        public ElementMenge<Deck> Decks { get; private set; }

        public string RootBilder { get; set; }
        public string Pfad { get; set; }

        public Menge[] ElementMengen
        {
            get
            {
                return new Menge[] { HintergrundDarstellungen, TitelDarstellungen,
                    BildDarstellungen, TextDarstellungen, InfoDarstellungen,
                    HauptBilder, HintergrundBilder, RuckseitenBilder, TextBilder,
                    Fraktionen, Gesinnungen, Karten,
                    Decks };
            }
        }

        public Universe()
            : base("Universe")
        {
            HintergrundDarstellungen = new ElementMenge<HintergrundDarstellung>("HintergrundDarstellungen", this);
            TitelDarstellungen = new ElementMenge<TitelDarstellung>("TitelDarstellungen", this);
            BildDarstellungen = new ElementMenge<BildDarstellung>("BildDarstellungen", this);
            TextDarstellungen = new ElementMenge<TextDarstellung>("TextDarstellungen", this);
            InfoDarstellungen = new ElementMenge<InfoDarstellung>("InfoDarstellungen", this);

            HauptBilder = new ElementMenge<HauptBild>("HauptBilder", this);
            HintergrundBilder = new ElementMenge<HintergrundBild>("HintergrundBilder", this);
            RuckseitenBilder = new ElementMenge<RuckseitenBild>("RuckseitenBilder", this);
            TextBilder = new ElementMenge<TextBild>("TextBilder", this);

            Fraktionen = new ElementMenge<Fraktion>("Fraktionen", this);
            Gesinnungen = new ElementMenge<Gesinnung>("Gesinnungen", this);
            Karten = new ElementMenge<Karte>("Karten", this);

            Decks = new ElementMenge<Deck>("Decks", this);
        }
        public Universe(string Pfad)
            : this()
        {
            Open(Pfad);
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            RootBilder = Loader.XmlReader.getString("RootBilder");

            Loader.XmlReader.Next();
            foreach (var item in ElementMengen)
                item.Read(Loader);
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeAttribute("RootBilder", RootBilder);

            foreach (var item in ElementMengen)
                item.Write(XmlWriter);
        }
        public void Open(string Pfad)
        {
            this.Pfad = Pfad;
            Loader l = CreateLoader(Pfad);
            Read(l);
            l.XmlReader.Close();
        }
        public Loader CreateLoader(string Pfad)
        {
            XmlReader reader = XmlReader.Create(Pfad);
            reader.Next();
            return CreateLoader(reader);
        }
        public Loader CreateLoader(XmlReader XmlReader)
        {
            return new Loader(this, XmlReader);
        }
        public void Root(string root)
        {
            this.Pfad = Path.Combine(root, "Universe.xml");
            this.RootBilder = Path.Combine(root, "\\Bilder\\");
        }
        public void Save()
        {
            XmlWriterSettings s = new XmlWriterSettings();
            s.NewLineOnAttributes = true;
            s.Indent = true;
            s.IndentChars = new string(' ', 4);
            XmlWriter writer = XmlWriter.Create(Pfad, s);
            writer.WriteStartDocument();
            this.Write(writer);
            writer.WriteEndDocument();
            writer.Close();
        }

        public override void AdaptToCard(Karte Karte)
        {
            throw new NotImplementedException();
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public ElementMenge<T> GetElementMenge<T>() where T : XmlElement, new()
        {
            foreach (var item in ElementMengen)
                if (item is ElementMenge<T>)
                    return item as ElementMenge<T>;
            throw new NotImplementedException();
        }
    }
}
