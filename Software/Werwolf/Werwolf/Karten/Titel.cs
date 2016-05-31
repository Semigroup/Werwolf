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
            return t => new PointF();
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
                return new PointF(0, y);
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
            int stachel = units.Ceil();
            return
                t =>
                {
                    if ((int)(stachel * t) % 2 == 0) return new PointF(0, RandHohe);
                    else return new PointF();
                };
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
                    return new PointF(0, (float)y);
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
                    return new PointF(0, (float)y);
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
            int stachel = units.Ceil();
            return
                t =>
                {
                    float y;
                    if ((int)(stachel * t) % 4 == 0) y = RandHohe / 2;
                    else if ((int)(stachel * t) % 4 == 1) y = RandHohe;
                    else if ((int)(stachel * t) % 4 == 2) y = RandHohe / 2;
                    else y = 0;
                    return new PointF(0, y);
                };
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

                    return new PointF(0, y);
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
                    int lower = T.Floor();
                    int higher = (lower + 1) % werte.Length;
                    float y;
                    if (higher > lower)
                        y = werte[lower] * (T - lower) + werte[higher] * (higher - T);
                    else
                        y = werte[lower] * (T - lower) + werte[higher] * (werte.Length - T);
                    return new PointF(0, y);
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
            (w + new PointF(0, 500)).print(1000, 1000, 10);
            //System.Windows.Forms.MessageBox.Show("Test");
            return w.weg;
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
}
