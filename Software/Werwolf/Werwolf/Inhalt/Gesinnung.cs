using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Gesinnung : XmlElement
    {
        public Aufgabe Aufgabe { get; set; }

        public Gesinnung()
            : base("Gesinnung")
        {
            Aufgabe = new Aufgabe();
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Aufgabe = new Aufgabe("Gesinnung", Universe);
        }
        
        public override void AdaptToCard(Karte Karte)
        {
            Karte.Gesinnung = this;
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Aufgabe = Loader.GetAufgabe("Aufgabe");
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeAttribute("Aufgabe", Aufgabe.ToString());
        }

        public override object Clone()
        {
            Gesinnung g = new Gesinnung();
            Assimilate(g);
            return g;
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            Gesinnung g = Element as Gesinnung;
            g.Aufgabe = Aufgabe;
        }
    }
}
