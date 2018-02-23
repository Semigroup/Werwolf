using System;
using System.Xml;
using System.Drawing;
using System.Drawing.Imaging;
using Assistment.Extensions;
using Assistment.Xml;
using Assistment.Mathematik;

namespace Werwolf.Inhalt
{
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

            KorrekturPosition = Loader.XmlReader.GetPointF("KorrekturPosition");
            KorrekturSkalierung = Loader.XmlReader.GetSizeF("KorrekturSkalierung");

            MyFilter = Loader.XmlReader.GetEnum<Filter>("MyFilter");
            ErsteFilterFarbe = Loader.XmlReader.GetColorHexARGB("ErsteFilterFarbe");
            ZweiteFilterFarbe = Loader.XmlReader.GetColorHexARGB("ZweiteFilterFarbe");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.WriteSize("KorrekturSkalierung", KorrekturSkalierung);
            XmlWriter.WritePoint("KorrekturPosition", KorrekturPosition);
            XmlWriter.WriteEnum<Filter>("MyFilter", MyFilter);
            XmlWriter.WriteColorHexARGB("ErsteFilterFarbe", ErsteFilterFarbe);
            XmlWriter.WriteColorHexARGB("ZweiteFilterFarbe", ZweiteFilterFarbe);
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
}
