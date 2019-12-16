using System.Xml;
using System.Drawing;

using Assistment.Texts;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class TextDarstellung : Darstellung
    {
        public float BalkenDicke { get; set; }
        public float InnenRadius { get; set; }
        public Font EffektFont
        {
            get { return effektFont; }
            set
            {
                effektFont = value;
                if (value == null)
                    EffektFontMeasurer = null;
                else
                    EffektFontMeasurer = value.GetMeasurer();
            }
        }
        public RectangleF TextRectangle { get; set; }

        private Font effektFont;
        public xFont EffektFontMeasurer { get; private set; }

        public PointF ShadowOffset { get; set; }
        public Color ShadowColor { get; set; }
        public bool ShadowIsActive { get; set; }

        public TextDarstellung()
            : base("TextDarstellung")
        {

        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            BalkenDicke = 1;
            InnenRadius = 1;
            Farbe = Color.FromArgb(128, Color.White);
            EffektFont = new Font("Calibri", 11);
            TextRectangle = new RectangleF(5, 48.1f, 53, 34);
            this.ShadowOffset = new PointF();
            this.ShadowColor = Color.Black;
            this.ShadowIsActive = false;
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            BalkenDicke = Loader.XmlReader.GetFloat("BalkenDicke");
            InnenRadius = Loader.XmlReader.GetFloat("InnenRadius");
            EffektFont = Loader.GetFont("EffektFont");
            if (EffektFont == null)
                EffektFont = new Font("Calibri", 11);
            TextRectangle = Loader.XmlReader.GetRectangle("TextRectangle");

            ShadowOffset = Loader.XmlReader.GetPointF("ShadowOffset");
            ShadowColor = Loader.XmlReader.GetColorHexARGB("ShadowColor");
            ShadowIsActive = Loader.XmlReader.GetBoolean("ShadowIsActive");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.WriteFloat("BalkenDicke", BalkenDicke);
            XmlWriter.WriteFloat("InnenRadius", InnenRadius);
            XmlWriter.WriteFont("EffektFont", EffektFont);
            XmlWriter.WriteRectangle("TextRectangle", TextRectangle);

            XmlWriter.WritePoint("ShadowOffset", ShadowOffset);
            XmlWriter.WriteColorHexARGB("ShadowColor", ShadowColor);
            XmlWriter.WriteBoolean("ShadowIsActive", ShadowIsActive);
        }
        public override void AdaptToCard(Karte Karte)
        {
            Karte.TextDarstellung = this;
        }
        public override object Clone()
        {
            TextDarstellung hg = new TextDarstellung();
            Assimilate(hg);
            return hg;
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            TextDarstellung hg = Element as TextDarstellung;
            hg.BalkenDicke = BalkenDicke;
            hg.InnenRadius = InnenRadius;
            hg.EffektFont = EffektFont.Clone() as Font;
            hg.TextRectangle = TextRectangle;

            hg.ShadowOffset = this.ShadowOffset;
            hg.ShadowColor = this.ShadowColor;
            hg.ShadowIsActive = this.ShadowIsActive;
        }
    }
}
