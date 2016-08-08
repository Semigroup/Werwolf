using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Xml;
using System.Xml;

namespace Werwolf.Inhalt
{
    public abstract class XmlElement
    {
        public string XmlName { get; private set; }
        /// <summary>
        /// Ist dieses XML-Element ein Oneliner oder besitzt es eine eigene Hierarchie?
        /// </summary>
        public bool Klein { get; private set; }

        public XmlElement(string XmlName, bool Klein)
        {
            this.XmlName = XmlName;
            this.Klein = Klein;
        }

        protected virtual void ReadIntern(Loader Loader)
        {
        }
        public void Read(Loader Loader)
        {
            int d = Loader.XmlReader.Depth;
            if (!Loader.XmlReader.Name.Equals(XmlName))
                throw new NotImplementedException();

            ReadIntern(Loader);

            while (Loader.XmlReader.Depth > d)
                Loader.XmlReader.Next();
        }
        protected virtual void WriteIntern(XmlWriter XmlWriter)
        {
        }
        public void Write(XmlWriter XmlWriter)
        {
            XmlWriter.WriteStartElement(XmlName);
            WriteIntern(XmlWriter);
            XmlWriter.WriteEndElement();
        }
    }
}
