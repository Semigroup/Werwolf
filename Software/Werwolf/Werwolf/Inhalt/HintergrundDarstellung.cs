using System;
using System.Linq;
using System.Xml;
using System.Drawing;
using Assistment.Xml;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;
using Assistment.Mathematik;
using Assistment.Extensions;

namespace Werwolf.Inhalt
{
    public class HintergrundDarstellung : Darstellung
    {
        public bool RundeEcken { get; set; }
        /// <summary>
        /// Größe in Millimeter
        /// </summary>
        public SizeF Size { get; set; }
        public Color RuckseitenFarbe { get; set; }
        public PointF Anker { get; set; }
        public float MarginLeft { get; set; }
        public float MarginRight { get; set; }
        public float MarginTop { get; set; }
        public float MarginBottom { get; set; }
        public bool Quer { get; set; }

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
            //Modus = KartenModus.Werwolfkarte;
            Size = new SizeF(63, 89.1f);
            Farbe = Color.White;
            Rand = new SizeF(3, 3);
            RuckseitenFarbe = Color.White;
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Size = Loader.XmlReader.GetSizeF("Size");
            RundeEcken = Loader.XmlReader.GetBoolean("RundeEcken");
            RuckseitenFarbe = Loader.XmlReader.GetColorHexARGB("RuckseitenFarbe");
            Anker = Loader.XmlReader.GetPointF("Anker");
            MarginLeft = Loader.XmlReader.GetFloat("MarginLeft");
            MarginRight = Loader.XmlReader.GetFloat("MarginRight");
            MarginTop = Loader.XmlReader.GetFloat("MarginTop");
            MarginBottom = Loader.XmlReader.GetFloat("MarginBottom");
            Quer = Loader.XmlReader.GetBoolean("Quer");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.WriteSize("Size", Size);
            XmlWriter.WriteBoolean("RundeEcken", RundeEcken);
            XmlWriter.WriteColorHexARGB("RuckseitenFarbe", RuckseitenFarbe);
            XmlWriter.WritePoint("Anker", Anker);
            XmlWriter.WriteFloat("MarginLeft", MarginLeft);
            XmlWriter.WriteFloat("MarginRight", MarginRight);
            XmlWriter.WriteFloat("MarginTop", MarginTop);
            XmlWriter.WriteFloat("MarginBottom", MarginBottom);
            XmlWriter.WriteBoolean("Quer", Quer);
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

                //clip.Complement(aussen);
                //g.Clip = clip;
                g.FillPolygon(RandFarbe.ToBrush(), y.GetPolygon((int)(100 * y.L), 0, 1));
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                g.FillRectangle(Color.FromArgb(0).ToBrush(), innen); //Color.FromArgb(0)
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
                t => new PointF((float)(-p*Math.Cos(t*p)), (float)(p * Math.Sin(t * p))),
                (Rand.Width + Rand.Height) * p / 2);
            OrientierbarerWeg Sektor2 = Sektor1.Spiegel(Horizontale) ^ -1;
            OrientierbarerWeg Sektor3 = Sektor2.Spiegel(Vertikale) ^ -1;
            OrientierbarerWeg Sektor4 = Sektor3.Spiegel(Horizontale) ^ -1;

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
            hg.RuckseitenFarbe = RuckseitenFarbe;
            hg.Anker = Anker;
            hg.MarginLeft = MarginLeft;
            hg.MarginRight = MarginRight;
            hg.MarginTop = MarginTop;
            hg.MarginBottom = MarginBottom;
            hg.Quer = Quer;
        }
    }
}
