using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Werwolf.Generating;
using System.Drawing;
using Assistment.Drawing;
using System.Drawing.Imaging;
using Assistment.Drawing.Style;
using Assistment.Drawing.Geometries;

using Assistment.Extensions;
using Assistment.Mathematik;

namespace Designer
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            MachRahmen();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HintergrundErstellerForm());
        }

        static Weg GetTriskele(float RandHohe)
        {
            float units = 1;
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

        static void MachRahmen()
        {
            Bitmap b = new Bitmap(1000, 1000);
            Rectangle r = new Rectangle(0, 0, 1000, 1000);
            Graphics g = b.GetHighGraphics();
            g.Clear(Color.Red);
            OrientierbarerWeg ow = OrientierbarerWeg.RundesRechteck(r, 100);
            ow.invertier();
            float[] steps = { 0,1,1,2,3,5,8,13};
            //Weg w = t => new PointF(t, 5 * steps[(int)(steps.Length * t - 0.0001f)]);
            //Weg w = t => new PointF(t, (float)(50 * Math.Sqrt(1 - t * t)));
            Weg w = GetTriskele(50);
            g.FillDrawWegAufOrientierbarerWeg(Brushes.White, Pens.Black,
                w.Frequent(10),
                ow, 10000);
            b.Save("test.png");
        }

        static void TestBundig()
        {

            Random d = new Random();

            Bitmap b = new Bitmap(1000, 1000);
            Graphics g = b.GetHighGraphics();

            FlachenSchema s = new FlachenSchema();
            s.BackColor = Color.Black;
            s.Boxes = new Point(10, 10);
            s.Flache = (u, v) => new PointF(1000 * u, 1000 * v);
            s.Pinsel = (u, v) => d.NextBrush(100);
            s.Samples = new Point(1000, 1000);
            s.Thumb = new Point(2, 2);
            s.DrawingRegion = new RectangleF(0, 0, 1000, 1000);

            Shadex.ChaosFlacheBundig(g, s);
            b.Save("test.png");
        }

        static void TestArrayExtension()
        {
            int n = 100;
            int[] ns = new int[n];
            ns.CountMap(i => i);

            IEnumerable<int> sub = ns.SubSequence(50, 20, -3);
            MessageBox.Show(sub.Print());

        }
        static void TestPointExtension()
        {
            Point p = new Point(5, 5);


            MessageBox.Show(p.Enumerate().Print());

        }
    }
}
