using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;

using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public abstract class Element : XmlElement
    {
        /// <summary>
        /// Mit Underlines statt Whitespaces
        /// </summary>
        public string Name { get; private set; }
        public string Schreibname
        {
            get
            {
                return Name.Replace('_', ' ');
            }
        }
        public string Desc { get; private set; }
        public Bild Bild { get; private set; }
        public Universe Universe;

        public Element(string XmlName, bool Klein)
            : base(XmlName, Klein)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            Name = Loader.XmlReader.getString("Name");
            Desc = Loader.XmlReader.getString("Desc");
            Bild = Loader.GetBild("Bild");
        }
    }
}
