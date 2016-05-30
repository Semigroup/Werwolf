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

        public Bild GetBild(string AttributeName)
        {
            string s = XmlReader.getString(AttributeName);
            if (s.Length > 0)
            {
                Image img = Image.FromFile(Universe.BilderPfad + s);
                return new Bild(img, s.Ordner(), XmlReader.getSizeF(AttributeName + "Size"));
            }
            else
                return Bild.Leer;
        }

        public FontMeasurer GetFont(string AttributeName)
        {
            return new FontMeasurer(XmlReader.getString(AttributeName), Universe.ppm * XmlReader.getFloat(AttributeName + "_Size"));
        }

        public Aufgabe GetAufgabe(string AttributeName)
        {
            string s = XmlReader.getString(AttributeName);
            string[] ss = s.Split(new string[] { "\r", "\n", "\\r", "\\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < ss.Length; i++)
                ss[i] = ss[i].Trim();
            return new Aufgabe(ss);
        }
    }
}
