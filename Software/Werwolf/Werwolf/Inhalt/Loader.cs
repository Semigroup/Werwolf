using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using Assistment.Extensions;
using Assistment.Xml;
using Assistment.Texts;

namespace Werwolf.Inhalt
{
    public class Loader
    {
        public Universe Universe { get; private set; }
        public XmlReader XmlReader { get; private set; }

        public Loader(Universe Universe, XmlReader XmlReader)
        {
            this.Universe = Universe;
            this.XmlReader = XmlReader;
        }
        public Loader(Universe Universe, string Pfad)
            : this(Universe, XmlReader.Create(Pfad))
        {
        }

        public Font GetFont(string AttributeName)
        {
            //  return new FontMeasurer(XmlReader.getString(AttributeName), XmlReader.getFloat(AttributeName + "_Size"));
            return XmlReader.getFont(AttributeName);
        }

        public Aufgabe GetAufgabe(string AttributeName)
        {
            string s = XmlReader.getString(AttributeName);
            return new Aufgabe(s);
        }

        public Fraktion GetFraktion()
        {
            string s = XmlReader.GetAttribute("Fraktion");
            return Universe.Fraktionen[s];
        }
        public Gesinnung GetGesinnung()
        {
            string s = XmlReader.GetAttribute("Gesinnung");
            return Universe.Gesinnungen[s];
        }
        public HintergrundDarstellung GetHintergrundDarstellung()
        {
            string s = XmlReader.GetAttribute("HintergrundDarstellung");
            return Universe.HintergrundDarstellungen[s];
        }
        public TextDarstellung GetTextDarstellung()
        {
            string s = XmlReader.GetAttribute("TextDarstellung");
            return Universe.TextDarstellungen[s];
        }
        public TitelDarstellung GetTitelDarstellung()
        {
            string s = XmlReader.GetAttribute("TitelDarstellung");
            return Universe.TitelDarstellungen[s];
        }
        public BildDarstellung GetBildDarstellung()
        {
            string s = XmlReader.GetAttribute("BildDarstellung");
            return Universe.BildDarstellungen[s];
        }
        public InfoDarstellung GetInfoDarstellung()
        {
            string s = XmlReader.GetAttribute("InfoDarstellung");
            return Universe.InfoDarstellungen[s];
        }

        public HauptBild GetHauptBild()
        {
            string s = XmlReader.GetAttribute("HauptBild");
            return Universe.HauptBilder[s];
        }
        public HintergrundBild GetHintergrundBild()
        {
            string s = XmlReader.GetAttribute("HintergrundBild");
            return Universe.HintergrundBilder[s];
        }
        public TextBild GetTextBild()
        {
            string s = XmlReader.GetAttribute("TextBild");
            return Universe.TextBilder[s];
        }
        public RuckseitenBild GetRuckseitenBild()
        {
            string s = XmlReader.GetAttribute("RuckseitenBild");
            return Universe.RuckseitenBilder[s];
        }
    }
}
