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
        public Darstellung StandardDarstellung { get { return Universe.StandardDarstellung; } }

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
            if (s != null && s.Length > 0)
                return Universe.Fraktionen[s];
            else return null;
        }
        public Gesinnung GetGesinnung()
        {
            string s = XmlReader.GetAttribute("Gesinnung");
            if (s != null && s.Length > 0)
                return Universe.Gesinnungen[s];
            else return null;
        }
        public Bild GetBild(string AttributeName)
        {
            string s = XmlReader.GetAttribute("Bild");
            if (s != null && s.Length > 0)
                return Universe.Bilder[s];
            else return null;
        }
    }
}
