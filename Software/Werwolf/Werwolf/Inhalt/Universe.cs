using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Universe : XmlElement
    {
        /// <summary>
        /// ./ ... /Bilder/
        /// </summary>
        public string BilderPfad { get; private set; }

        public string Name { get; private set; }
        public string Desc { get; private set; }

        public Darstellung StandardDarstellung { get;private set; }

        public ElementMenge<Fraktion> Fraktionen { get; private set; }
        public ElementMenge<Gesinnung> Gesinnungen { get; private set; }
        public ElementMenge<Karte> Karten { get; private set; }

        public Universe()
            : base("Universe", false)
        {
            Fraktionen = new ElementMenge<Fraktion>("Fraktionen", this);
            Gesinnungen = new ElementMenge<Gesinnung>("Gesinnungen", this);
            Karten = new ElementMenge<Karte>("Karten", this);

            StandardDarstellung = new Darstellung();
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Name = Loader.XmlReader.getString("Name");
            Desc = Loader.XmlReader.getString("Desc");

            BilderPfad = Loader.XmlReader.getString("BilderPfad");
            Fraktionen.Read(Loader.XmlReader.getString("FraktionenPfad"));
            Gesinnungen.Read(Loader.XmlReader.getString("GesinnungenPfad"));
            Karten.Read(Loader.XmlReader.getString("KartenPfad"));

            Loader.XmlReader.Next();
            StandardDarstellung.Read(Loader);
        }

        public Loader CreateLoader(XmlReader XmlReader)
        {
            return new Loader(this, XmlReader);
        }
    }
}
