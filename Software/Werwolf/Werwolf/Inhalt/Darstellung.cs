using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using Assistment.Texts;
using Assistment.Extensions;
using Assistment.Xml;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;

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
                //FontMeasurer = value != null ? value.GetMeasurer() : null;
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

        private Font font;
        public xFont FontMeasurer { get; private set; }

        public Darstellung(string XmlName)
            : base(XmlName)
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
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            Existiert = Loader.XmlReader.getBoolean("Existiert");
            Font = Loader.GetFont("Font");
            Rand = Loader.XmlReader.getSizeF("Rand");
            Farbe = Loader.XmlReader.getColorHexARGB("Farbe");
            RandFarbe = Loader.XmlReader.getColorHexARGB("RandFarbe");
            TextFarbe = Loader.XmlReader.getColorHexARGB("TextFarbe");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeBoolean("Existiert", Existiert);
            XmlWriter.writeFont("Font", Font);
            XmlWriter.writeSize("Rand", Rand);
            XmlWriter.writeColorHexARGB("Farbe", Farbe);
            XmlWriter.writeColorHexARGB("RandFarbe", RandFarbe);
            XmlWriter.writeColorHexARGB("TextFarbe", TextFarbe);
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
    public class HintergrundDarstellung : Darstellung
    {
        public bool RundeEcken { get; set; }
        /// <summary>
        /// Größe in Millimeter
        /// </summary>
        public SizeF Size { get; set; }

        public Image RandBild { get; private set; }
        private Size LastSize = new Size();
        private SizeF LastRand = new SizeF();
        private Color LastRandFarbe = Color.Black;
        private bool LastRundeEcken = true;

        public HintergrundDarstellung()
            : base("HintergrundDarstellung")
        {

        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            RundeEcken = true;
            Size = new SizeF(63, 89.1f);
            Farbe = Color.White;
            Rand = new SizeF(3, 3);
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Size = Loader.XmlReader.getSizeF("Size");
            RundeEcken = Loader.XmlReader.getBoolean("RundeEcken");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeSize("Size", Size);
            XmlWriter.writeBoolean("RundeEcken", RundeEcken);
        }

        public void MakeRandBild(float ppm)
        {
            Size s = Size.mul(ppm).Max(1, 1).ToSize();
            if (LastSize.Equals(s)
                && LastRand.sub(Rand).norm() < 1
                && LastRundeEcken == RundeEcken
                && LastRandFarbe == RandFarbe)
                return;
            LastSize = s;
            LastRand = Rand;
            LastRundeEcken = RundeEcken;
            LastRandFarbe = RandFarbe;

            RandBild = new Bitmap(s.Width, s.Height);
            using (Graphics g = RandBild.GetHighGraphics())
            {
                g.ScaleTransform(ppm, ppm);
                OrientierbarerWeg y;

                if (RundeEcken)
                    y = RunderRand(Size);
                else
                    y = HarterRand(Size);

                RectangleF aussen = new RectangleF(new PointF(), Size);
                RectangleF innen = aussen.Inner(Rand);

                //Region clip = new Region(innen);
                //clip.Complement(aussen);
                //g.Clip = clip;
                g.FillPolygon(RandFarbe.ToBrush(), y.getPolygon((int)(100 * y.L), 0, 1));
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.FillRectangle(Color.FromArgb(1, 255, 255, 255).ToBrush(), innen); //Color.FromArgb(0)
                //g.FillRectangle(Color.FromArgb(0, 0, 0, 0).ToBrush(), innen); //Color.FromArgb(0)
            }
        }
        private OrientierbarerWeg RunderRand(SizeF Size)
        {
            Gerade Horizontale = new Gerade(0, Size.Height / 2, 1, 0);
            Gerade Vertikale = new Gerade(Size.Width / 2, 0, 0, 1);

            float p = (float)(Math.PI / 2);
            OrientierbarerWeg Sektor1 = new OrientierbarerWeg(
                t => new PointF(0, -1).rot(t * p).add(1, 1).mul(Rand.ToPointF()),
                t => new PointF(0, -p).rot(t * p + p).mul(Rand.ToPointF()).linksOrtho(),
                (Rand.Width + Rand.Height) * p / 2);
            OrientierbarerWeg Sektor2 = Sektor1.Spiegel(Horizontale) ^ -1;
            OrientierbarerWeg Sektor3 = Sektor2.Spiegel(Vertikale) ^ -1;
            OrientierbarerWeg Sektor4 = Sektor3.Spiegel(Horizontale) ^ -1;

            //(Sektor1 * 100f + new PointF(500, 500)).print(1000, 1000, 10);
            Sektor1 = Sektor1.ConcatLinear(Sektor2).ConcatLinear(Sektor3).ConcatLinear(Sektor4).AbschlussLinear();
            return Sektor1;
        }
        private OrientierbarerWeg HarterRand(SizeF Size)
        {
            return OrientierbarerWeg.HartPolygon(new PointF(),
                new PointF(0, Size.Height),
                Size.ToPointF(),
                new PointF(Size.Width, 0),
                new PointF());
        }

        public override void AdaptToCard(Karte Karte)
        {
            Karte.HintergrundDarstellung = this;
        }
        public override object Clone()
        {
            HintergrundDarstellung hg = new HintergrundDarstellung();
            Assimilate(hg);
            return hg;
        }
        public override void Assimilate(XmlElement Darstellung)
        {
            base.Assimilate(Darstellung);
            HintergrundDarstellung hg = Darstellung as HintergrundDarstellung;
            hg.RundeEcken = RundeEcken;
            hg.Size = Size;
            hg.RandBild = RandBild;
            hg.LastRand = LastRand;
            hg.LastSize = LastSize;
        }
    }
    public class TitelDarstellung : Darstellung
    {
        public TitelDarstellung()
            : base("TitelDarstellung")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Farbe = Color.White;
            Font = new Font("Exocet", 14);
        }
        public override void AdaptToCard(Karte Karte)
        {
            Karte.TitelDarstellung = this;
        }
        public override object Clone()
        {
            TitelDarstellung hg = new TitelDarstellung();
            Assimilate(hg);
            return hg;
        }
    }
    public class BildDarstellung : Darstellung
    {
        public PointF KorrekturPosition { get; private set; }
        public SizeF KorrekturSkalierung { get; private set; }

        public BildDarstellung()
            : base("BildDarstellung")
        {

        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            KorrekturPosition = Loader.XmlReader.getPointF("KorrekturPosition");
            KorrekturSkalierung = Loader.XmlReader.getSizeF("KorrekturSkalierung");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeSize("KorrekturSkalierung", KorrekturSkalierung);
            XmlWriter.writePoint("KorrekturPosition", KorrekturPosition);
        }
        public override void AdaptToCard(Karte Karte)
        {
            Karte.BildDarstellung = this;
        }
        public override object Clone()
        {
            BildDarstellung hg = new BildDarstellung();
            Assimilate(hg);
            return hg;
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            BildDarstellung hg = Element as BildDarstellung;
            hg.KorrekturPosition = KorrekturPosition;
            hg.KorrekturSkalierung = KorrekturSkalierung;
        }
    }
    public class TextDarstellung : Darstellung
    {
        public float BalkenDicke { get; set; }
        public float InnenRadius { get; set; }

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
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            BalkenDicke = Loader.XmlReader.getFloat("BalkenDicke");
            InnenRadius = Loader.XmlReader.getFloat("InnenRadius");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeFloat("BalkenDicke", BalkenDicke);
            XmlWriter.writeFloat("InnenRadius", InnenRadius);
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
        }
    }
    public class InfoDarstellung : Darstellung
    {
        public InfoDarstellung()
            : base("InfoDarstellung")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
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
    }
}
