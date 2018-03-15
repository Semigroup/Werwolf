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

        public virtual Aufgabe GetAufgabe(string AttributeName)
        {
            string s = XmlReader.GetString(AttributeName);
            return new Aufgabe(s, Universe);
        }

        public virtual Fraktion GetFraktion()
        {
            string s = XmlReader.GetString("Fraktion");
            return Universe.Fraktionen[s];
        }
        public virtual Gesinnung GetGesinnung()
        {
            string s = XmlReader.GetString("Gesinnung");
            return Universe.Gesinnungen[s];
        }
        public virtual HintergrundDarstellung GetHintergrundDarstellung()
        {
            string s = XmlReader.GetString("HintergrundDarstellung");
            return Universe.HintergrundDarstellungen[s];
        }
        public virtual TextDarstellung GetTextDarstellung()
        {
            string s = XmlReader.GetString("TextDarstellung");
            return Universe.TextDarstellungen[s];
        }
        public virtual TitelDarstellung GetTitelDarstellung()
        {
            string s = XmlReader.GetString("TitelDarstellung");
            return Universe.TitelDarstellungen[s];
        }
        public virtual BildDarstellung GetBildDarstellung()
        {
            string s = XmlReader.GetString("BildDarstellung");
            return Universe.BildDarstellungen[s];
        }
        public virtual InfoDarstellung GetInfoDarstellung()
        {
            string s = XmlReader.GetString("InfoDarstellung");
            return Universe.InfoDarstellungen[s];
        }
        public virtual LayoutDarstellung GetLayoutDarstellung()
        {
            string s = XmlReader.GetString("LayoutDarstellung");
            return Universe.LayoutDarstellungen[s];
        }

        public virtual HauptBild GetHauptBild()
        {
            string s = XmlReader.GetString("HauptBild");
            return Universe.HauptBilder[s];
        }
        public virtual HintergrundBild GetHintergrundBild()
        {
            string s = XmlReader.GetString("HintergrundBild");
            return Universe.HintergrundBilder[s];
        }
        public virtual HintergrundBild GetHintergrundBildQuer()
        {
            string s = XmlReader.GetString("HintergrundBildQuer");
            return Universe.HintergrundBilder[s];
        }
        public virtual TextBild GetTextBild()
        {
            string s = XmlReader.GetString("TextBild");
            return Universe.TextBilder[s];
        }
        public virtual RuckseitenBild GetRuckseitenBild()
        {
            string s = XmlReader.GetString("RuckseitenBild");
            return Universe.RuckseitenBilder[s];
        }
        public virtual Deck GetDeck()
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
