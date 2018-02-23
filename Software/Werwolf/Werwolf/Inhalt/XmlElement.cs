using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Xml;
using System.Xml;

namespace Werwolf.Inhalt
{
    public abstract class XmlElement : ICloneable
    {
        public string XmlName { get; private set; }
        public string Name { get; set; }
        public string Schreibname { get; set; }
        public Universe Universe { get; private set; }
        public bool Unzerstorbar { get; set; }
        public bool Deep { get; private set; }

        public XmlElement(string XmlName, bool Deep)
        {
            this.XmlName = XmlName;
            this.Deep = Deep;
        }

        public virtual void Init(Universe Universe)
        {
            this.Universe = Universe;
            this.Name = "Standard";
            this.Schreibname = "Standard";
            this.Unzerstorbar = false;
        }

        protected virtual void ReadIntern(Loader Loader)
        {
            this.Universe = Loader.Universe;

            this.Name = Loader.XmlReader.GetString("Name");
            this.Schreibname = Loader.XmlReader.GetString("Schreibname");
            this.Unzerstorbar = Loader.XmlReader.GetBoolean("Unzerstorbar");
        }
        public void Read(Loader Loader)
        {
            if (!Loader.XmlReader.Name.Equals(XmlName))
                throw new NotImplementedException(Loader.XmlReader.DumpInfo());
            ReadIntern(Loader);
        }
        protected virtual void WriteIntern(XmlWriter XmlWriter)
        {
            XmlWriter.WriteAttribute("Name", Name);
            XmlWriter.WriteAttribute("Schreibname", Schreibname);
            XmlWriter.WriteBoolean("Unzerstorbar", Unzerstorbar);
        }
        public void Write(XmlWriter XmlWriter)
        {
            XmlWriter.WriteStartElement(XmlName);
            WriteIntern(XmlWriter);
            XmlWriter.WriteEndElement();
        }

        public virtual void Assimilate(XmlElement Element)
        {
            Element.Name = Name;
            Element.Schreibname = Schreibname;
            Element.Universe = Universe;
        }
        public abstract void AdaptToCard(Karte Karte);
        public abstract void Rescue();

        public abstract object Clone();

        public override string ToString()
        {
            return Schreibname;
        }
    }
}
