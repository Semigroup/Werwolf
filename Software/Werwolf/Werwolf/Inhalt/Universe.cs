﻿using System;
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

        public ElementMenge<Bild> Bilder { get; private set; }
        public ElementMenge<Fraktion> Fraktionen { get; private set; }
        public ElementMenge<Gesinnung> Gesinnungen { get; private set; }
        public ElementMenge<Karte> Karten { get; private set; }

        public string RootBilder { get; set; }
        public string Pfad { get; set; }

        public Menge[] ElementMengen
        {
            get
            {
                return new Menge[] { HintergrundDarstellungen, TitelDarstellungen, 
                    BildDarstellungen, TextDarstellungen, InfoDarstellungen,
                    Bilder, Fraktionen, Gesinnungen, Karten };
            }
        }

        public Universe()
            : base("Universe", false)
        {
            HintergrundDarstellungen = new ElementMenge<HintergrundDarstellung>("HintergrundDarstellungen", this);
            TitelDarstellungen = new ElementMenge<TitelDarstellung>("TitelDarstellungen", this);
            BildDarstellungen = new ElementMenge<BildDarstellung>("BildDarstellungen", this);
            TextDarstellungen = new ElementMenge<TextDarstellung>("TextDarstellungen", this);
            InfoDarstellungen = new ElementMenge<InfoDarstellung>("InfoDarstellungen", this);

            Bilder = new ElementMenge<Bild>("Bilder", this);
            Fraktionen = new ElementMenge<Fraktion>("Fraktionen", this);
            Gesinnungen = new ElementMenge<Gesinnung>("Gesinnungen", this);
            Karten = new ElementMenge<Karte>("Karten", this);
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

            foreach (var item in ElementMengen)
                item.Open(Loader.XmlReader.getString(item.XmlName + "Pfad"));
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeAttribute("RootBilder", RootBilder);

            foreach (var item in ElementMengen)
                XmlWriter.writeAttribute(item.XmlName + "itemPfad", item.Pfad);
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
            foreach (var item in ElementMengen)
                item.Pfad = Path.Combine(root, item.XmlName + ".xml");

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

            foreach (var item in ElementMengen)
                item.Save();
        }
    }
}
