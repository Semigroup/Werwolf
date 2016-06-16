using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Fraktion : Element
    {
        public enum RandArt
        {
            Rund,
            Stachel,
            Zahn,
            Wellen,
            Sagezahn,
            VierStufen,
            Konig,
            Chaos,
            Kreuz,
            Triskelen,
            Pik,
            Blitz
        }

        public Bild Hintergrundbild { get; private set; }
        public Aufgabe StandardAufgaben { get; private set; }
        public RandArt TitelRand { get; private set; }

        public Fraktion()
            : base("Fraktion", true)
        {
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Hintergrundbild = Loader.GetBild("Hintergrundbild");
            StandardAufgaben = Loader.GetAufgabe("StandardAufgaben");
            TitelRand = GetRand(Loader.XmlReader.getString("TitelRand"));
        }

        public static RandArt GetRand(string Name)
        {
            return (RandArt)Enum.Parse(typeof(RandArt), Name, true);
        }
    }
}
