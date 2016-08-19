using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Universe : Element
    {
        public Darstellung StandardDarstellung { get; private set; }

        public ElementMenge<Bild> Bilder { get; private set; }
        public ElementMenge<Fraktion> Fraktionen { get; private set; }
        public ElementMenge<Gesinnung> Gesinnungen { get; private set; }
        public ElementMenge<Karte> Karten { get; private set; }

        public string rootBilder { get; set; }

        public Universe()
            : base("Universe", false)
        {
            Bilder = new ElementMenge<Bild>("Bilder", this);
            Fraktionen = new ElementMenge<Fraktion>("Fraktionen", this);
            Gesinnungen = new ElementMenge<Gesinnung>("Gesinnungen", this);
            Karten = new ElementMenge<Karte>("Karten", this);

            StandardDarstellung = new Darstellung();
        }
        public Universe(string Pfad)
            : this()
        {
            this.rootBilder = Path.GetDirectoryName(Pfad) + "\\Bilder";
            Loader l = CreateLoader(Pfad);
            Read(l);
            l.XmlReader.Close();
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            
            Bilder.Read(Loader.XmlReader.getString("BilderPfad"));
            Fraktionen.Read(Loader.XmlReader.getString("FraktionenPfad"));
            Gesinnungen.Read(Loader.XmlReader.getString("GesinnungenPfad"));
            Karten.Read(Loader.XmlReader.getString("KartenPfad"));

            Loader.XmlReader.Next();
            StandardDarstellung.Read(Loader);
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("BilderPfad", Bilder.Pfad);
            XmlWriter.writeAttribute("FraktionenPfad", Fraktionen.Pfad);
            XmlWriter.writeAttribute("GesinnungenPfad", Gesinnungen.Pfad);
            XmlWriter.writeAttribute("KartenPfad", Karten.Pfad);

            StandardDarstellung.Write(XmlWriter);
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
        public void MakePfade(string root)
        {
            Bilder.Pfad = root + "Bilder.xml";
            Fraktionen.Pfad = root + "Fraktionen.xml";
            Gesinnungen.Pfad = root + "Gesinnungen.xml";
            Karten.Pfad = root + "Karten.xml";

            this.rootBilder = root + "\\Bilder\\";
        }
        public void Save(string FileName)
        {
            XmlWriterSettings s = new XmlWriterSettings();
            s.NewLineOnAttributes = true;
            s.Indent = true;
            s.IndentChars = new string(' ', 4);
            XmlWriter writer = XmlWriter.Create(FileName, s);
            writer.WriteStartDocument();
            this.Write(writer);
            writer.WriteEndDocument();
            writer.Close();

            Bilder.Save();
            Fraktionen.Save();
            Gesinnungen.Save();
            Karten.Save();
        }
    }
}
