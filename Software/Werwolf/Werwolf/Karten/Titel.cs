using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using Assistment.Drawing;
using Assistment.Texts;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Style;
using Assistment.Extensions;
using Assistment.Mathematik;

namespace Werwolf.Karten
{
    public abstract class Titel : DrawBox
    {
        public Brush HintergrundFarbe { get; private set; }
        public Pen RandFarbe { get; private set; }
        //public Weg RandVerlauf { get; private set; }
        public float RandHohe { get; private set; }
        public DrawBox Inhalt { get; private set; }

        public Titel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
        {
            this.Inhalt = Inhalt.Geometry(RandHohe, 0, RandHohe, 0);
            this.RandHohe = RandHohe;
            ////this.RandVerlauf = RandVerlauf;
            this.RandFarbe = RandFarbe;
            this.HintergrundFarbe = HintergrundFarbe;
        } //Weg RandVerlauf,

        public override float getSpace()
        {
            float ab = RandHohe * 2;
            float breite = (Inhalt.getMax() + Inhalt.getMin()) / 2;
            float hohe = Inhalt.getSpace() / breite;

            return (breite + ab) * (hohe + ab);
        }

        public override float getMin()
        {
            return Inhalt.getMin() + 2 * RandHohe;
        }

        public override float getMax()
        {
            return Inhalt.getMax() + 2 * RandHohe;
        }

        public override void update()
        {
            Inhalt.update();
        }

        public override void setup(RectangleF box)
        {
            float ab = RandHohe * 2;
            this.box = box;
            box.X += RandHohe;
            box.Y += RandHohe;
            box.Width -= ab;
            box.Height -= ab;
            this.Inhalt.setup(box);
            this.box.Height = Inhalt.box.Height + ab;
            this.box.Width = this.Inhalt.box.Width + ab;
        }

        public abstract Weg GetVerlauf(float units);

        public override void draw(DrawContext con)
        {
            Bitmap b = new Bitmap(box.Width.Ceil(), box.Height.Ceil());
            Graphics g = b.GetHighGraphics();
            RectangleF pseudoBox = new RectangleF(RandHohe, RandHohe, Inhalt.box.Width, Inhalt.box.Height);
            OrientierbarerWeg ow = OrientierbarerWeg.RundesRechteck(pseudoBox);
            int samples = 10000;
            g.FillDrawWegAufOrientierbarerWeg(HintergrundFarbe, RandFarbe, GetVerlauf(ow.L / RandHohe), ow, samples);
            g.Dispose();
            con.drawImage(b, box.Location);
            Inhalt.draw(con);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }
    }

    public class RunderTitel : Titel
    {
        public RunderTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe,
            RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            return t => new PointF(t, 0);
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class StachelTitel : Titel
    {
        public StachelTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe,
            RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int n = units.Ceil();
            return t =>
            {
                float t0 = t * n;
                int i = t0.Floor();
                float y = (1 + 2 * (i - t0));
                y *= y * RandHohe;
                return new PointF(t, y);
            };
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class ZahnTitel : Titel
    {
        public ZahnTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = (units / 2).Ceil();

            OrientierbarerWeg ow = OrientierbarerWeg.HartPolygon(new PointF(),
                new PointF(0, RandHohe),
                new PointF(RandHohe, RandHohe),
                new PointF(RandHohe, 0),
                new PointF(2 * RandHohe, 0));

            ow ^= stachel;

            return ow.weg;

            //return
            //    t =>
            //    {
            //        if ((int)(stachel * t) % 2 == 0) return new PointF(t, RandHohe);
            //        else return new PointF(t,0);
            //    };
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class WellenTitel : Titel
    {
        public WellenTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = (units / 2).Ceil();
            return
                t =>
                {
                    double y = RandHohe * (1 + Math.Sin(stachel * t * Math.PI * 2)) / 2;
                    return new PointF(t, (float)y);
                };
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class SagezahnTitel : Titel
    {
        public SagezahnTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = units.Ceil();
            return
                t =>
                {
                    double y = RandHohe * ((stachel * (1 - t)) % 1);
                    return new PointF(t, (float)y);
                };
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class VierStufenTitel : Titel
    {
        public VierStufenTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = (units / 4).Ceil();

            float lx = RandHohe;
            float ly = RandHohe / 2;
            OrientierbarerWeg oy = OrientierbarerWeg.HartPolygon(
                new PointF(),
                new PointF(0, ly),
                new PointF(lx, ly),
                new PointF(lx, 2 * ly),
                new PointF(2 * lx, 2 * ly),
                new PointF(2 * lx, ly),
                new PointF(3 * lx, ly),
                new PointF(3 * lx, 0),
                new PointF(4 * lx, 0));

            oy ^= stachel;

            return oy.weg;
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class KonigTitel : Titel
    {
        public KonigTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            int stachel = (units / 2).Ceil();
            return
                t =>
                {
                    float y;
                    float T = (stachel * t) % 4;

                    if ((int)T == 0) y = 0;
                    else if ((int)T == 1) y = RandHohe * (T - 1) * (T - 1);
                    else if ((int)T == 2) y = 0;
                    else y = RandHohe * (T - 4) * (T - 4);

                    return new PointF(t, y);
                };
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class ChaosTitel : Titel
    {
        public ChaosTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            Random d = new Random();

            float[] werte = new float[(int)(units * 5)];
            for (int i = 0; i < werte.Length; i++)
                werte[i] = RandHohe * d.NextFloat();

            return
                t =>
                {
                    float T = t * werte.Length;
                    int lower = T.Floor() % werte.Length;
                    int higher = (lower + 1) % werte.Length;
                    float y;
                    if (higher > lower)
                        y = werte[lower] * (T - lower) + werte[higher] * (higher - T);
                    else
                        y = werte[lower] * (T - lower) + werte[higher] * (werte.Length - T);
                    return new PointF(t, y);
                };
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class KreuzTitel : Titel
    {
        public KreuzTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            //Weglänge
            float L = units * RandHohe;
            int stachel = (units / 2).Ceil();
            //Kleines Wegstück
            float l = L / stachel;
            //Ganz kleines Wegstück
            float ll = RandHohe / 3;
            OrientierbarerWeg w = OrientierbarerWeg.HartPolygon(
                new PointF(),
                new PointF(ll, 0),
                new PointF(ll, ll),
                new PointF(0, ll),
                new PointF(0, 2 * ll),
                new PointF(ll, 2 * ll),
                new PointF(ll, 3 * ll),
                new PointF(2 * ll, 3 * ll),
                new PointF(2 * ll, 2 * ll),
                new PointF(3 * ll, 2 * ll),
                new PointF(3 * ll, ll),
                new PointF(2 * ll, ll),
                new PointF(2 * ll, 0),
                new PointF(6 * ll, 0));
            w = w ^ stachel;
            //w.invertier();
            //(w + new PointF(0, 500)).print(1000, 1000, 10);
            //System.Windows.Forms.MessageBox.Show("Test");
            return w.weg;
        }
        //public override Weg GetVerlauf(float units)
        //{
        //    //Weglänge
        //    float L = units * RandHohe;
        //    int stachel = (units / 2).Ceil();
        //    //Kleines Wegstück
        //    float l = L / stachel;
        //    //Ganz kleines Wegstück
        //    float ll = RandHohe / 3;

        //    Weg links = t =>
        //    {
        //        float T = 3 * t;
        //        if (T <= 1)
        //            return new PointF(T * ll, 0);
        //        else if (T <= 2)
        //            return new PointF((2 - T) * ll, ll);
        //        else
        //            return new PointF((T - 2) * ll, 2 * ll);
        //    };
        //    Weg mitte = t => new PointF(0, RandHohe);
        //    Weg rechts = t =>
        //    {
        //        float T = 3 * t;
        //        if (T <= 1)
        //            return new PointF(T * ll, 2 * ll);
        //        else if (T <= 2)
        //            return new PointF((2 - T) * ll, ll);
        //        else
        //            return new PointF((T - 2) * ll, 0);
        //    };
        //    Weg rest = t => new PointF();

        //    Weg stuck = links.Concat(mitte.Concat(rechts.Concat(rest.Concat(rest.Concat(rest)))));
        //    Weg a = stuck;
        //    for (int i = 1; i < stachel; i++)
        //        a = a.Concat(stuck);
        //    return a;
        //}
        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class TriskelenTitel : Titel
    {
        public TriskelenTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            //Weglänge
            float L = units * RandHohe;
            int stachel = (units / 2).Ceil();
            //Kleines Wegstück
            float l = L / stachel;
            //Ganz kleines Wegstück
            float ll = RandHohe / 2;

            OrientierbarerWeg T = (OrientierbarerWeg.Triskele(RandHohe / 4, 1, RandHohe / 6).Trim(0.01f, 0.99f) ^ Math.PI) + new PointF(ll, RandHohe - RandHohe / 4 * (float)(1 + 2 / Math.Sqrt(3)));

            OrientierbarerWeg w =
                OrientierbarerWeg.HartPolygon(new PointF(), T.weg(0))
                * T
                * OrientierbarerWeg.HartPolygon(T.weg(1), new PointF(ll * 4, 0));


            w = w ^ stachel;

            return w.weg;
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
    public class PikTitel : Titel
    {
        public PikTitel(DrawBox Inhalt, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(Inhalt, RandHohe, RandFarbe, HintergrundFarbe)
        {

        }

        public override Weg GetVerlauf(float units)
        {
            //Weglänge
            float L = units * RandHohe;
            int stachel = (units / 1.5f).Ceil();
            //Kleines Wegstück
            float l = L / stachel;
            //Ganz kleines Wegstück
            float ll = RandHohe / 2;

            OrientierbarerWeg T = (OrientierbarerWeg.Pike(RandHohe).Trim(0.05f, 0.95f)) + new PointF(ll, 0);

            OrientierbarerWeg w =
                OrientierbarerWeg.HartPolygon(new PointF(), T.weg(0))
                * T
                * OrientierbarerWeg.HartPolygon(T.weg(1), new PointF(ll * 4, 0));


            w = w ^ stachel;

            return w.weg;
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
}
