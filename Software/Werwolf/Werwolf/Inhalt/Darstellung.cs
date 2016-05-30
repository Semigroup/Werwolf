using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using Assistment.Texts;
using Assistment.Extensions;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Darstellung : XmlElement
    {
        public bool KeinBild { get; private set; }

        public bool KeinTitel { get; private set; }
        public bool KeinTitelRand { get; private set; }
        
        public bool KeinText { get; private set; }
        public bool KeinTextRand { get; set; }
        public bool KeinBalkenProAbsatz { get; set; }

        public bool KeinHintergrund { get; private set; }
        
        public bool KeineGesinnung { get; private set; }
        
        public bool KeinRand { get; private set; }

        public xFont TitelFont { get; private set; }
        public xFont TextFont { get; private set; }
        public xFont InfoFont { get; private set; }

        public Darstellung() : base("Darstellung", true)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            KeinBild = Loader.XmlReader.getBoolean("KeinBild");
            KeinTitel = Loader.XmlReader.getBoolean("KeinTitel");
            KeinText = Loader.XmlReader.getBoolean("KeinText");
            KeinHintergrund = Loader.XmlReader.getBoolean("KeinHintergrund");
            KeineGesinnung = Loader.XmlReader.getBoolean("KeineGesinnung");
            KeinRand = Loader.XmlReader.getBoolean("KeinRand");
            KeinTitelRand = Loader.XmlReader.getBoolean("KeinTitelRand");
            KeinTextRand = Loader.XmlReader.getBoolean("KeinTextRand");
            KeinBalkenProAbsatz = Loader.XmlReader.getBoolean("KeinBalkenProAbsatz");

            TitelFont = Loader.GetFont("TitelFont");
            TextFont = Loader.GetFont("TextFont");
            InfoFont = Loader.GetFont("InfoFont");
        }

        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            XmlWriter.writeBoolean("KeinBild", KeinBild);
            XmlWriter.writeBoolean("KeinTitel", KeinTitel);
            XmlWriter.writeBoolean("KeinText", KeinText);
            XmlWriter.writeBoolean("KeinHintergrund", KeinHintergrund);
            XmlWriter.writeBoolean("KeineGesinnung", KeineGesinnung);
            XmlWriter.writeBoolean("KeinRand", KeinRand);
        }
    }
}
