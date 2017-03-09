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

        public ElementMenge<LayoutDarstellung> LayoutDarstellungen { get; private set; }

        public ElementMenge<Fraktion> Fraktionen { get; private set; }
        public ElementMenge<Gesinnung> Gesinnungen { get; private set; }
        public ElementMenge<Karte> Karten { get; private set; }

        public ElementMenge<Deck> Decks { get; private set; }

        public Menge[] ElementMengen
        {
            get
            {
                return GetMengen();
            }
        }

        public string DirectoryName { get; private set; }
        public string Pfad { get; set; }

        public Universe()
            : base("Universe", true)
        {
            HintergrundDarstellungen = new ElementMenge<HintergrundDarstellung>("Hintergrunddarstellungen", this);
            TitelDarstellungen = new ElementMenge<TitelDarstellung>("Titeldarstellungen", this);
            BildDarstellungen = new ElementMenge<BildDarstellung>("Bilddarstellungen", this);
            TextDarstellungen = new ElementMenge<TextDarstellung>("Textdarstellungen", this);
            InfoDarstellungen = new ElementMenge<InfoDarstellung>("Infodarstellungen", this);

            HauptBilder = new ElementMenge<HauptBild>("Hauptbilder", this);
            HintergrundBilder = new ElementMenge<HintergrundBild>("Hintergrundbilder", this);
            RuckseitenBilder = new ElementMenge<RuckseitenBild>("Rückseitenbilder", this);
            TextBilder = new ElementMenge<TextBild>("Textbilder", this);

            LayoutDarstellungen = new ElementMenge<LayoutDarstellung>("Layoutdarstellungen", this);

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

        protected virtual Menge[] GetMengen()
        {
            return new Menge[] { HintergrundDarstellungen, TitelDarstellungen,
                    BildDarstellungen, TextDarstellungen, InfoDarstellungen, 
                    HauptBilder, HintergrundBilder, RuckseitenBilder, TextBilder,
                    LayoutDarstellungen,
                    Fraktionen, Gesinnungen, Karten,
                    Decks };
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            Loader.XmlReader.Next();
            while (!Loader.XmlReader.EOF)
            {
                Menge M = ElementMengen.First(m => m.XmlName.Equals(Loader.XmlReader.Name));
                M.Read(Loader);
            }
            //foreach (var item in ElementMengen)
            //    item.Read(Loader);
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            foreach (var item in ElementMengen)
                item.Write(XmlWriter);
        }
        private Loader CreateLoader(string Pfad)
        {
            XmlReader reader = XmlReader.Create(Pfad);
            reader.Next();
            return CreateLoader(reader);
        }
        private Loader CreateLoader(XmlReader XmlReader)
        {
            return new Loader(this, XmlReader);
        }

        public void Root(string Pfad)
        {
            this.DirectoryName = Path.GetDirectoryName(Pfad);
            this.Pfad = Pfad;
        }
        public void Open(string Pfad)
        {
            Root(Pfad);
            using (Loader l = CreateLoader(Pfad))
                Read(l);
        }
        public void Save(string Pfad)
        {
            Root(Pfad);
            this.DirectoryName = Path.GetDirectoryName(Pfad);

            XmlWriterSettings s = new XmlWriterSettings();
            s.NewLineOnAttributes = true;
            s.Indent = true;
            s.IndentChars = new string(' ', 4);
            using (XmlWriter writer = XmlWriter.Create(Pfad, s))
            {
                writer.WriteStartDocument();
                this.Write(writer);
                writer.WriteEndDocument();
                writer.Close();
            }
        }
        public void Lokalisieren(bool jpg, string destinyDirectory)
        {
            string[] bilder = { "HauptBilder", "HintergrundBilder", "RuckseitenBilder", "TextBilder" };
            foreach (var item in bilder)
            {
                string pfad = Path.Combine(destinyDirectory, "Bilder", item);
                if (!Directory.Exists(pfad))
                    Directory.CreateDirectory(pfad);
            }
            foreach (var item in HauptBilder.Values)
                item.Lokalisieren(false, destinyDirectory);
            foreach (var item in HintergrundBilder.Values)
                item.Lokalisieren(jpg, destinyDirectory);
            foreach (var item in RuckseitenBilder.Values)
                item.Lokalisieren(jpg, destinyDirectory);
            foreach (var item in TextBilder.Values)
                item.Lokalisieren(false, destinyDirectory);
        }
        public override void Rescue()
        {
            foreach (var item in new Menge[] { Decks, Karten, Fraktionen, Gesinnungen })
                item.Rescue();
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
