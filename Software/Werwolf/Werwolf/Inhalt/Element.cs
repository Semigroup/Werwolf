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
        public string Desc { get; private set; }
        public Bild Bild { get; set; }

        public Element(string XmlName, bool Klein)
            : base(XmlName, Klein)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Desc = Loader.XmlReader.getString("Desc");
            Bild = Loader.GetBild("Bild");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeAttribute("Desc", Desc);
            if (Bild != null)
                XmlWriter.writeAttribute("Bild", Bild.Name);
        }
    }
}
