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
    public class Darstellung : XmlElement
    {
        public Darstellung Standard { get; private set; }

        public HintergrundDarstellung Hintergrund { get; private set; }
        public TitelDarstellung Titel { get; private set; }
        public BildDarstellung Bild { get; private set; }
        public TextDarstellung Text { get; private set; }
        public InfoDarstellung Info { get; private set; }

        public UnterDarstellung[] UnterDarstellungen { get { return new UnterDarstellung[] { Hintergrund, Titel, Bild, Text, Info }; } }

        /// <summary>
        /// Größe in Millimeter
        /// </summary>
        public SizeF Size { get; set; }

        public Darstellung()
            : base("Darstellung", false)
        {
            this.Hintergrund = new HintergrundDarstellung();
            this.Titel = new TitelDarstellung();
            this.Bild = new BildDarstellung();
            this.Text = new TextDarstellung();
            this.Info = new InfoDarstellung();
        }

        private void setStandard(Darstellung Standard)
        {
            if (Standard != null)
            {
                this.Standard = Standard;
                Hintergrund = Standard.Hintergrund;
                Titel = Standard.Titel;
                Bild = Standard.Bild;
                Text = Standard.Text;
                Info = Standard.Info;
            }
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            Size = Loader.XmlReader.getSizeF("Size");
            setStandard(Loader.StandardDarstellung);
            while (Loader.XmlReader.Next())
            {
                bool found = false;
                foreach (var item in UnterDarstellungen)
                    if (item.XmlName.Equals(Loader.XmlReader.Name))
                    {
                        item.Read(Loader);
                        found = true;
                        break;
                    }
                if (!found)
                    return;
            }
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            if (Standard != null && Hintergrund != Standard.Hintergrund)
                Hintergrund.Write(XmlWriter);
            if (Standard != null && Titel != Standard.Titel)
                Titel.Write(XmlWriter);
            if (Standard != null && Bild != Standard.Bild)
                Bild.Write(XmlWriter);
            if (Standard != null && Text != Standard.Text)
                Text.Write(XmlWriter);
            if (Standard != null && Info != Standard.Info)
                Info.Write(XmlWriter);
        }

        public bool IstStandard()
        {
            return Standard == null;
        }
    }

    public abstract class UnterDarstellung : XmlElement
    {
        /// <summary>
        /// in Millimeter
        /// </summary>
        public SizeF Rand { get; set; }
        private Font font;
        public Font Font { get { return font; } set { font = value; FontMeasurer = value != null ? value.GetMeasurer() : null; } }
        public FontMeasurer FontMeasurer { get; private set; }
        public bool Existiert { get; set; }
        public bool HatRand { get; set; }
        public Color Farbe { get; set; }
        public Color RandFarbe { get; set; }

        public UnterDarstellung(string XmlName)
            : base(XmlName, true)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            Existiert = Loader.XmlReader.getBoolean("Existiert");
            HatRand = Loader.XmlReader.getBoolean("HatRand");
            Font = Loader.GetFont("Font");
            Rand = Loader.XmlReader.getSizeF("Rand");
            Farbe = Loader.XmlReader.getColorHexARGB("Farbe");
            RandFarbe = Loader.XmlReader.getColorHexARGB("RandFarbe");
        }

        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeBoolean("Existiert", Existiert);
            XmlWriter.writeBoolean("HatRand", HatRand);
            XmlWriter.writeFont("Font", Font);
            XmlWriter.writeSize("Rand", Rand);
        }
    }
    public class HintergrundDarstellung : UnterDarstellung
    {
        public bool RundeEcken { get; set; }
        public Image RandBild { get; private set; }

        public HintergrundDarstellung()
            : base("Hintergrund")
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            RundeEcken = Loader.XmlReader.getBoolean("RundeEcken");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeBoolean("RundeEcken", RundeEcken);
        }

        public void MakeRandBild(float ppm, Darstellung Darstellung, float Faktor)
        {
            Size s = Darstellung.Size.mul(Faktor * ppm).ToSize();
            RandBild = new Bitmap(s.Width, s.Height);
            Graphics g = RandBild.GetHighGraphics();
            g.ScaleTransform(Faktor * ppm, Faktor * ppm);
            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            OrientierbarerWeg y;

            if (RundeEcken)
                y = RunderRand(Darstellung.Size);
            else
                y = HarterRand(Darstellung.Size);

            RectangleF innen = new RectangleF(Rand.ToPointF(), Darstellung.Size.sub(Rand.mul(2)));

            g.FillPolygon(Brushes.Black, y.getPolygon((int)(100 * y.L), 0, 1));
            g.FillRectangle(new SolidBrush(Color.FromArgb(0, 0, 0, 0)), innen);
        }
        private OrientierbarerWeg RunderRand(SizeF Size)
        {
            Gerade Horizontale = new Gerade(0, Size.Height / 2, 1, 0);
            Gerade Vertikale = new Gerade(Size.Width / 2, 0, 0, 1);

            float p = (float)(Math.PI / 2);
            OrientierbarerWeg Sektor1 = new OrientierbarerWeg(t => new PointF(0, 1).rot(t * p).add(1, 1).mul(Rand.ToPointF()), null, (Rand.Width + Rand.Height) * p / 2);
            OrientierbarerWeg Sektor2 = Sektor1.Spiegel(Horizontale) ^ -1;
            OrientierbarerWeg Sektor3 = Sektor2.Spiegel(Vertikale) ^ -1;
            OrientierbarerWeg Sektor4 = Sektor3.Spiegel(Horizontale) ^ -1;

            return Sektor1.ConcatGlatt(Sektor2).ConcatGlatt(Sektor3).ConcatGlatt(Sektor4).Abschluss();
        }
        private OrientierbarerWeg HarterRand(SizeF Size)
        {
            return OrientierbarerWeg.HartPolygon(new PointF(),
                new PointF(0, Size.Height),
                Size.ToPointF(),
                new PointF(Size.Width, 0),
                new PointF());
        }
    }
    public class TitelDarstellung : UnterDarstellung
    {
        public TitelDarstellung()
            : base("Titel")
        {
        }
    }
    public class BildDarstellung : UnterDarstellung
    {
        //public PointF Point { get; private set; }
        //public SizeF Size { get; private set; }

        public BildDarstellung()
            : base("Bild")
        {

        }

        //protected override void ReadIntern(Loader Loader)
        //{
        //    base.ReadIntern(Loader);

        //    Point = Loader.XmlReader.getPointF("Point");
        //    Size = Loader.XmlReader.getSizeF("Size");
        //}
        //protected override void WriteIntern(XmlWriter XmlWriter)
        //{
        //    base.WriteIntern(XmlWriter);

        //    XmlWriter.writeSize("Size", Size);
        //    XmlWriter.writePoint("Point", Point);
        //}
    }
    public class TextDarstellung : UnterDarstellung
    {
        public float BalkenDicke { get; set; }
        public float InnenRadius { get; set; }

        public TextDarstellung()
            : base("Text")
        {
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
    }
    public class InfoDarstellung : UnterDarstellung
    {
        public InfoDarstellung()
            : base("Info")
        {
        }
    }
}
