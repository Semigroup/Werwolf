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
        public Font Font { get { return font; } set { font = value; FontMeasurer = value != null ? value.GetMeasurer() : null; } }
        public bool Existiert { get; set; }
        public Color Farbe { get; set; }
        public Color RandFarbe { get; set; }
        public Color TextFarbe { get; set; }

        private Font font;
        public FontMeasurer FontMeasurer { get; private set; }

        public Darstellung(string XmlName)
            : base(XmlName, true)
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
     

        public HintergrundDarstellung()
            : base("Hintergrund")
        {
     
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            RundeEcken = true;
            Size = new SizeF(63, 89.1f);
            Farbe = Color.Black;
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
            XmlWriter.writeSize("Size",Size);
            XmlWriter.writeBoolean("RundeEcken", RundeEcken);
        }

        public void MakeRandBild(float ppm)
        {
            Size s = Size.mul(ppm).ToSize();
            if (LastSize.Equals(s) && LastRand.sub(Rand).norm() < 1)
                return;
            LastSize = s;
            LastRand = Rand;

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
                g.FillPolygon(Brushes.Black, y.getPolygon((int)(100 * y.L), 0, 1));
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
    }
    public class TitelDarstellung : Darstellung
    {
        public TitelDarstellung()
            : base("Titel")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Farbe = Color.White;
        }
    }
    public class BildDarstellung : Darstellung
    {
        public PointF KorrekturPosition { get; private set; }
        public SizeF KorrekturSkalierung { get; private set; }

        public BildDarstellung()
            : base("Bild")
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
    }
    public class TextDarstellung : Darstellung
    {
        public float BalkenDicke { get; set; }
        public float InnenRadius { get; set; }

        public TextDarstellung()
            : base("Text")
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
    }
    public class InfoDarstellung : Darstellung
    {
        public InfoDarstellung()
            : base("Info")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
        }
    }
}
