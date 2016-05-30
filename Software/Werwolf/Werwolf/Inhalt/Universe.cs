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

        /// <summary>
        /// Pixel pro Millimeter
        /// </summary>
        public float ppm { get; private set; }

        public Darstellung StandardDarstellung { get;private set; }

        public Universe()
            : base("Universe", false)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            BilderPfad = Loader.XmlReader.getString("BilderPfad");
            ppm = Loader.XmlReader.getFloat("ppm");
            Loader.XmlReader.Next();
            StandardDarstellung = new Darstellung();
            StandardDarstellung.Read(Loader);
        }

        public Loader CreateLoader(XmlReader XmlReader)
        {
            return new Loader(this, XmlReader);
        }
    }
}
