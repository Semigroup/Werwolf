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
            TestBundig();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HintergrundErstellerForm());
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
