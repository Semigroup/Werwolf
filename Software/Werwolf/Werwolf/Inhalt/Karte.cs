using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Karte : Element
    {
        public Darstellung Darstellung { get; set; }
        public Aufgabe Aufgaben { get; set; }
        public Fraktion Fraktion { get; set; }
        public Gesinnung Gesinnung { get; set; }

        public Karte()
            : base("Karte", false)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            
            Aufgaben = Loader.GetAufgabe("Aufgaben");
            Fraktion = Loader.GetFraktion();
            Gesinnung = Loader.GetGesinnung();
            Loader.XmlReader.Next();
            if (Loader.XmlReader.Name.Equals("Darstellung"))
            {
                Darstellung = new Darstellung();
                Darstellung.Read(Loader);
            }
            else
                Darstellung = Loader.StandardDarstellung;
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("Fraktion", Fraktion.Name);
            XmlWriter.writeAttribute("Gesinnung", Gesinnung.Name);
            XmlWriter.writeAttribute("Aufgaben", Aufgaben.ToString());
            if (!Darstellung.IstStandard())
                Darstellung.Write(XmlWriter);
        }
    }
}
