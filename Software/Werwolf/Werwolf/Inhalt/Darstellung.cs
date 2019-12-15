using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Drawing;

using Assistment.Texts;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public abstract class Darstellung : XmlElement
    {
        /// <summary>
        /// in Millimeter
        /// </summary>
        public SizeF Rand { get; set; }
        public Font Font
        {
            get { return font; }
            set
            {
                font = value;
                if (value == null)
                    FontMeasurer = null;
                else
                    FontMeasurer = value.GetMeasurer();
            }
        }
        public bool Existiert { get; set; }
        public Color Farbe { get; set; }
        public Color RandFarbe { get; set; }
        public Color TextFarbe { get; set; }
        public PointF Position { get; set; }

        private Font font;
        public xFont FontMeasurer { get; private set; }

        public Darstellung(string XmlName)
            : base(XmlName, false)
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Rand = new SizeF(1, 1);
            Font = new Font("Calibri", 8);
            Existiert = true;
            Farbe = Color.FromArgb(0);
            RandFarbe = Color.Black;
            TextFarbe = Color.Black;
            Position = new PointF();
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            Existiert = Loader.XmlReader.GetBoolean("Existiert");
            Font = Loader.GetFont("Font");
            if (Font == null)
                Font = new Font("Calibri", 11);
            Rand = Loader.XmlReader.GetSizeF("Rand");
            Farbe = Loader.XmlReader.GetColorHexARGB("Farbe");
            RandFarbe = Loader.XmlReader.GetColorHexARGB("RandFarbe");
            TextFarbe = Loader.XmlReader.GetColorHexARGB("TextFarbe");
            Position = Loader.XmlReader.GetPointF("Position");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.WriteBoolean("Existiert", Existiert);
            XmlWriter.WriteFont("Font", Font);
            XmlWriter.WriteSize("Rand", Rand);
            XmlWriter.WriteColorHexARGB("Farbe", Farbe);
            XmlWriter.WriteColorHexARGB("RandFarbe", RandFarbe);
            XmlWriter.WriteColorHexARGB("TextFarbe", TextFarbe);
            XmlWriter.WritePoint("Position", Position);
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            Darstellung Darstellung = Element as Darstellung;
            Darstellung.Rand = Rand;
            Darstellung.Font = Font.Clone() as Font;
            Darstellung.Existiert = Existiert;
            Darstellung.Farbe = Farbe;
            Darstellung.RandFarbe = RandFarbe;
            Darstellung.TextFarbe = TextFarbe;
            Darstellung.Position = Position;
        }

        public override void AdaptToCard(Karte Karte)
        {
            throw new NotImplementedException();
        }
        public override void Rescue()
        {
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
