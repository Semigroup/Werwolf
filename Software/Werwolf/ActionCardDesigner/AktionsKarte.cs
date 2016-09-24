using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Werwolf.Inhalt;
using Werwolf.Karten;
using Assistment.Extensions;
using Assistment.Xml;

namespace ActionCardDesigner
{
    public class AktionsKarte : Karte
    {
        public float Initiative { get; set; }
        public int ReichweiteMin { get; set; }
        public int ReichweiteMax { get; set; }
        public int Felder { get; set; }
        public int Storung { get; set; }

        public AktionsKarte()
        {
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Initiative = Loader.XmlReader.getFloat("Initiative");
            ReichweiteMin = Loader.XmlReader.getInt("ReichweiteMin");
            ReichweiteMax = Loader.XmlReader.getInt("ReichweiteMax");
            Felder = Loader.XmlReader.getInt("Felder");
            Storung = Loader.XmlReader.getInt("Storung");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeFloat("Initiative", Initiative);
            XmlWriter.writeInt("ReichweiteMin", ReichweiteMin);
            XmlWriter.writeInt("ReichweiteMax", ReichweiteMax);
            XmlWriter.writeInt("Felder", Felder);
            XmlWriter.writeInt("Storung", Storung);
        }

        public override void Assimilate(Werwolf.Inhalt.XmlElement Element)
        {
            base.Assimilate(Element);
            AktionsKarte ak = Element as AktionsKarte;
            ak.Initiative = Initiative;
            ak.Storung = Storung;
            ak.ReichweiteMax = ReichweiteMax;
            ak.ReichweiteMin = ReichweiteMin;
            ak.Felder = Felder;
        }

        public override object Clone()
        {
            AktionsKarte ak = new AktionsKarte();
            Assimilate(ak);
            return ak;
        }
        public override WolfBox GetVorderSeite(float Ppm)
        {
            return new StandardAktionsKarte(this, Ppm);
        }
    }
}
