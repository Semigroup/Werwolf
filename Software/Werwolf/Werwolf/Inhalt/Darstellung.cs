using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;

using Assistment.Texts;
using Assistment.Extensions;
using Assistment.Xml;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;
using Assistment.Mathematik;

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
        public enum KartenModus
        {
            Werwolfkarte,
            AktionsKarte,
            WondersKarte,
            WondersReichKarte,
            WonderGlobalesProjekt
        }

        public KartenModus Modus { get; set; }
        public bool RundeEcken { get; set; }
        /// <summary>
        /// Größe in Millimeter
        /// </summary>
        public SizeF Size { get; set; }
        public Color RuckseitenFarbe { get; set; }
        public PointF Anker { get; set; }

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
            Modus = KartenModus.Werwolfkarte;
            Size = new SizeF(63, 89.1f);
            Farbe = Color.White;
            Rand = new SizeF(3, 3);
            RuckseitenFarbe = Color.White;
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Size = Loader.XmlReader.getSizeF("Size");
            RundeEcken = Loader.XmlReader.getBoolean("RundeEcken");
            Modus = Loader.XmlReader.getEnum<KartenModus>("Modus");
            RuckseitenFarbe = Loader.XmlReader.getColorHexARGB("RuckseitenFarbe");
            Anker = Loader.XmlReader.getPointF("Anker");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeSize("Size", Size);
            XmlWriter.writeBoolean("RundeEcken", RundeEcken);
            XmlWriter.writeEnum<KartenModus>("Modus", Modus);
            XmlWriter.writeColorHexARGB("RuckseitenFarbe", RuckseitenFarbe);
            XmlWriter.writePoint("Anker", Anker);
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
            hg.Modus = Modus;
            hg.RuckseitenFarbe = RuckseitenFarbe;
            hg.Anker = Anker;
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
        public enum Filter
        {
            Keiner,
            Zweidimensional,
            Eindimensional
        }

        public PointF KorrekturPosition { get; set; }
        public SizeF KorrekturSkalierung { get; set; }
        public Filter MyFilter { get; set; }
        public Color ErsteFilterFarbe { get; set; }
        public Color ZweiteFilterFarbe { get; set; }

        public BildDarstellung()
            : base("BildDarstellung")
        {

        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            MyFilter = Filter.Keiner;
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            KorrekturPosition = Loader.XmlReader.getPointF("KorrekturPosition");
            KorrekturSkalierung = Loader.XmlReader.getSizeF("KorrekturSkalierung");

            MyFilter = Loader.XmlReader.getEnum<Filter>("MyFilter");
            ErsteFilterFarbe = Loader.XmlReader.getColorHexARGB("ErsteFilterFarbe");
            ZweiteFilterFarbe = Loader.XmlReader.getColorHexARGB("ZweiteFilterFarbe");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeSize("KorrekturSkalierung", KorrekturSkalierung);
            XmlWriter.writePoint("KorrekturPosition", KorrekturPosition);
            XmlWriter.writeEnum<Filter>("MyFilter", MyFilter);
            XmlWriter.writeColorHexARGB("ErsteFilterFarbe", ErsteFilterFarbe);
            XmlWriter.writeColorHexARGB("ZweiteFilterFarbe", ZweiteFilterFarbe);
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
            hg.MyFilter = MyFilter;
            hg.ErsteFilterFarbe = ErsteFilterFarbe;
            hg.ZweiteFilterFarbe = ZweiteFilterFarbe;
        }

        //public Color FilterFarbe(Color GrundFarbe)
        //{
        //    switch (MyFilter)
        //    {
        //        case Filter.Keiner:
        //            return GrundFarbe;
        //        case Filter.Zweidimensional:
        //            PointF P = Min(GrundFarbe, ErsteFilterFarbe, ZweiteFilterFarbe);
        //            break;
        //        case Filter.Eindimensional:
        //            float x = SKP(GrundFarbe, ErsteFilterFarbe) / SKP(ErsteFilterFarbe, ErsteFilterFarbe);
        //            return MakeColor(x * GrundFarbe.A, x * GrundFarbe.R, x * GrundFarbe.G, x * GrundFarbe.B);
        //        default:
        //            throw new NotImplementedException();
        //    }
        //}
        //private Color MakeColor(float A, float R, float G, float B)
        //{
        //    return Color.FromArgb(Math.Max(0, Math.Min((int)A, 255)),
        //        Math.Max(0, Math.Min((int)R, 255)),
        //        Math.Max(0, Math.Min((int)G, 255)),
        //        Math.Max(0, Math.Min((int)B, 255)));
        //}
        //private float SKP(Color A, Color B)
        //{
        //    return A.R * B.R + A.B * B.B + A.G * B.G + A.A * B.A;
        //}
        ///// <summary>
        ///// Minimiert |b - a1 * x - a2 * y|
        ///// </summary>
        ///// <param name="b"></param>
        ///// <param name="a1"></param>
        ///// <param name="a2"></param>
        ///// <returns></returns>
        //private PointF Min(Color b, Color a1, Color a2)
        //{
        //    PointF P = new PointF();

        //    float a11 = SKP(a1, a1);
        //    float a12 = SKP(a1, a2);
        //    float a22 = SKP(a2, a2);
        //    float a1b = SKP(a1, b);
        //    float a2b = SKP(a2, b);
        //    //Ax - b = min
        //    //A^T A x = A^T b
        //    //a11 x + a12 y = a1b
        //    //a12 x + a22 y = a2b
        //    if (a11.Equal(0))
        //        return P;
        //    P.Y = a11 * a2b - a12 * a1b;
        //    float nom = a22 * a11 - a12 * a12;
        //    if (nom.Equal(0))
        //        P.Y = 0;
        //    else
        //        P.Y /= nom;
        //    P.X = a1b - a12 * P.Y;
        //    P.X /= a11;
        //    return P;
        //}
        public void FilterBytes32RGB(byte[] Data)
        {
            float[] a1 = vector(ErsteFilterFarbe),
                a2 = vector(ZweiteFilterFarbe);

            float n1 = FastMath.Sqrt(skp(a1, a1));
            a1.SelfMap(f => f / n1);

            float a12 = skp(a1, a2);
            for (int i = 0; i < 3; i++)
                a2[i] -= a12 * a1[i];

            float n2 = FastMath.Sqrt(skp(a2, a2));
            a2.SelfMap(f => f / n2);

            switch (MyFilter)
            {
                case Filter.Keiner:
                    return;
                case Filter.Zweidimensional:
                    for (int i = 0; i < Data.Length; i += 4)
                    {
                        float s1 = skp(a1, Data, i);
                        float s2 = skp(a2, Data, i);
                        for (int j = 0; j < 3; j++)
                            Data[i + j] = (byte)Math.Min(255, Math.Max(0, a1[j] * s1 + a2[j] * s2));
                    }
                    break;
                case Filter.Eindimensional:
                    for (int i = 0; i < Data.Length; i += 4)
                    {
                        float s = skp(a1, Data, i);
                        for (int j = 0; j < 3; j++)
                            Data[i + j] = (byte)Math.Min(255, Math.Max(0, a1[j] * s));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void FilterBytes24RGB(byte[] Data)
        {
            float[] a1 = vector(ErsteFilterFarbe),
                a2 = vector(ZweiteFilterFarbe);

            float n1 = FastMath.Sqrt(skp(a1, a1));
            a1.SelfMap(f => f / n1);

            float a12 = skp(a1, a2);
            for (int i = 0; i < 3; i++)
                a2[i] -= a12 * a1[i];

            float n2 = FastMath.Sqrt(skp(a2, a2));
            a2.SelfMap(f => f / n2);

            int n = 3 * (Data.Length / 3);

            switch (MyFilter)
            {
                case Filter.Keiner:
                    return;
                case Filter.Zweidimensional:
                    for (int i = 0; i < n; i += 3)
                    {
                        float s1 = skp(a1, Data, i);
                        float s2 = skp(a2, Data, i);
                        for (int j = 0; j < 3; j++)
                            Data[i + j] = (byte)Math.Min(255, Math.Max(0, a1[j] * s1 + a2[j] * s2));
                    }
                    break;
                case Filter.Eindimensional:
                    for (int i = 0; i < n; i += 3)
                    {
                        float s = skp(a1, Data, i);
                        for (int j = 0; j < 3; j++)
                            Data[i + j] = (byte)Math.Min(255, Math.Max(0, a1[j] * s));
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public void FilterBytes(byte[] Data, PixelFormat Format)
        {
            switch (Format)
            {
                case PixelFormat.Alpha:
                case PixelFormat.Canonical:
                case PixelFormat.Format32bppArgb:
                case PixelFormat.Format32bppPArgb:
                case PixelFormat.Format32bppRgb:
                    FilterBytes32RGB(Data);
                    break;
                case PixelFormat.Format24bppRgb:
                    FilterBytes24RGB(Data);
                    break;
                case PixelFormat.Format8bppIndexed:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private float skp(float[] a, float[] b)
        {
            return skp(a, b, 0);
        }
        private float skp(float[] a, float[] b, int indexOfB)
        {
            float s = 0;
            for (int i = 0; i < a.Length; i++)
                s += a[i] * b[i + indexOfB];
            return s;
        }
        private float skp(float[] a, byte[] b, int indexOfB)
        {
            float s = 0;
            for (int i = 0; i < a.Length; i++)
                s += a[i] * b[i + indexOfB];
            return s;
        }
        private float[] vector(Color Color)
        {
            return new float[] { Color.B, Color.G, Color.R };
        }
    }
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

        private Font effektFont;
        public xFont EffektFontMeasurer { get; private set; }

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
            EffektFont = new Font("Calibri", 12);
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            BalkenDicke = Loader.XmlReader.getFloat("BalkenDicke");
            InnenRadius = Loader.XmlReader.getFloat("InnenRadius");
            EffektFont = Loader.GetFont("EffektFont");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeFloat("BalkenDicke", BalkenDicke);
            XmlWriter.writeFloat("InnenRadius", InnenRadius);
            XmlWriter.writeFont("EffektFont", EffektFont);
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
