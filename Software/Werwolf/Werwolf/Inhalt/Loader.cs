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
    public class Loader : IDisposable
    {
        public Universe Universe { get;  set; }
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
        public Loader(string Pfad)
            : this(null, XmlReader.Create(Pfad))
        {
        }

        public Font GetFont(string AttributeName)
        {
            return XmlReader.GetFont(AttributeName);
        }

        public Aufgabe GetAufgabe(string AttributeName)
        {
            string s = XmlReader.GetString(AttributeName);
            return new Aufgabe(s, Universe);
        }

        public Fraktion GetFraktion()
        {
            string s = XmlReader.GetString("Fraktion");
            return Universe.Fraktionen[s];
        }
        public Gesinnung GetGesinnung()
        {
            string s = XmlReader.GetString("Gesinnung");
            return Universe.Gesinnungen[s];
        }
        public HintergrundDarstellung GetHintergrundDarstellung()
        {
            string s = XmlReader.GetString("HintergrundDarstellung");
            return Universe.HintergrundDarstellungen[s];
        }
        public TextDarstellung GetTextDarstellung()
        {
            string s = XmlReader.GetString("TextDarstellung");
            return Universe.TextDarstellungen[s];
        }
        public TitelDarstellung GetTitelDarstellung()
        {
            string s = XmlReader.GetString("TitelDarstellung");
            return Universe.TitelDarstellungen[s];
        }
        public BildDarstellung GetBildDarstellung()
        {
            string s = XmlReader.GetString("BildDarstellung");
            return Universe.BildDarstellungen[s];
        }
        public InfoDarstellung GetInfoDarstellung()
        {
            string s = XmlReader.GetString("InfoDarstellung");
            return Universe.InfoDarstellungen[s];
        }
        public LayoutDarstellung GetLayoutDarstellung()
        {
            string s = XmlReader.GetString("LayoutDarstellung");
            return Universe.LayoutDarstellungen[s];
        }

        public HauptBild GetHauptBild()
        {
            string s = XmlReader.GetString("HauptBild");
            return Universe.HauptBilder[s];
        }
        public HintergrundBild GetHintergrundBild()
        {
            string s = XmlReader.GetString("HintergrundBild");
            return Universe.HintergrundBilder[s];
        }
        public HintergrundBild GetHintergrundBildQuer()
        {
            string s = XmlReader.GetString("HintergrundBildQuer");
            return Universe.HintergrundBilder[s];
        }
        public TextBild GetTextBild()
        {
            string s = XmlReader.GetString("TextBild");
            return Universe.TextBilder[s];
        }
        public RuckseitenBild GetRuckseitenBild()
        {
            string s = XmlReader.GetString("RuckseitenBild");
            return Universe.RuckseitenBilder[s];
        }
        public Deck GetDeck()
        {
            string s = XmlReader.GetString("Deck");
            return Universe.Decks[s];
        }

        public void Dispose()
        {
            XmlReader.Close();
        }
    }
}
