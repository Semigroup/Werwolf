using System;
using System.Linq;
using System.Xml;
using System.Drawing;
using Assistment.Xml;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;
using Assistment.Mathematik;
using Assistment.Extensions;

using Assistment.Texts;

namespace Werwolf.Inhalt
{
    public class InfoDarstellung : Darstellung
    {
        public SizeF Grosse { get; set; }

        public PointF Position2 { get; set; }
        public SizeF Rand2 { get; set; }
        public Color Farbe2 { get; set; }
        public SizeF Grosse2 { get; set; }
        public Color RandFarbe2 { get; set; }
        public Color TextFarbe2 { get; set; }
        public Font Font2
        {
            get { return font2; }
            set
            {
                font2 = value;
                if (value == null)
                    FontMeasurer2 = null;
                else
                    FontMeasurer2 = value.GetMeasurer();
            }
        }
        private Font font2;
        public IFontMeasurer FontMeasurer2 { get; private set; }

        public InfoDarstellung()
            : base("InfoDarstellung")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            this.Grosse = new SizeF();
            this.Position2 = new PointF();
            this.Rand2 = new SizeF();
            this.Grosse2 = new SizeF();
            this.Farbe2 = Color.FromArgb(0);
            this.RandFarbe2 = Color.Black;
            this.TextFarbe2 = Color.Black;
            this.Font2 = new Font("Calibri", 8);
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Grosse = Loader.XmlReader.GetSizeF("Grosse");

            Position2 = Loader.XmlReader.GetPointF("Position2");
            Rand2 = Loader.XmlReader.GetSizeF("Rand2");
            Font2 = Loader.GetFont("Font2");
            if (Font2 == null)
                Font2 = new Font("Calibri", 8);
            Farbe2 = Loader.XmlReader.GetColorHexARGB("Farbe2");
            RandFarbe2 = Loader.XmlReader.GetColorHexARGB("RandFarbe2");
            TextFarbe2 = Loader.XmlReader.GetColorHexARGB("TextFarbe2");
            Grosse2 = Loader.XmlReader.GetSizeF("Grosse2");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.WriteSize("Grosse", Grosse);

            XmlWriter.WritePoint("Position2", Position2);
            XmlWriter.WriteSize("Rand2", Rand2);
            XmlWriter.WriteColorHexARGB("Farbe2", Farbe2);
            XmlWriter.WriteColorHexARGB("RandFarbe2", RandFarbe2);
            XmlWriter.WriteColorHexARGB("TextFarbe2", TextFarbe2);
            XmlWriter.WriteFont("Font2", Font2);
            XmlWriter.WriteSize("Grosse2", Grosse2);
        }

        public override void AdaptToCard(Karte Karte)
        {
            Karte.InfoDarstellung = this;
        }
        public override object Clone()
        {
            InfoDarstellung hg = new InfoDarstellung();
            Assimilate(hg);
            return hg;
        }
        public override void Assimilate(XmlElement Darstellung)
        {
            base.Assimilate(Darstellung);
            InfoDarstellung hg = Darstellung as InfoDarstellung;
            hg.Grosse = this.Grosse;
            hg.Position2 = this.Position2;
            hg.Rand2 = this.Rand2;
            hg.RandFarbe2 = this.RandFarbe2;
            hg.Farbe2 = this.Farbe2;
            hg.Font2 = this.Font2.Clone() as Font;
            hg.TextFarbe2 = this.TextFarbe2;
            hg.Grosse2 = this.Grosse2;
        }
    }
}
