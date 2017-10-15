using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Forms;
using Werwolf.Karten;
using Werwolf.Inhalt;
using Assistment.Extensions;
using Assistment.Texts;
using System.Windows.Forms;
using System.Drawing;

namespace Translation.PDFWorkarounds
{
    class ProduktionSteik : DrawBox, ITool
    {
        public bool ruck;

        public string ToolDescription => "ProduktionStreikBogenWorkaround";

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }

        public override void draw(DrawContext con)
        {
            float max = 991 + 2f / 3;
            PointF[] points = { new PointF(50, 50), new PointF(max - 50, 50), new PointF(max - 50, 1350) };
            if (ruck)
                for (int i = 0; i < points.Length; i++)
                    points[i].X = max - points[i].X;
            Pen pen = new Pen(Color.Red, 1);
            Brush brush = Brushes.Yellow;
            float radius = 20;

            for (int i = 0; i < points.Length; i++)
            {
                PointF p = points[i];
                con.drawLine(pen, p.X - radius, p.Y, p.X + radius, p.Y);
                con.drawLine(pen, p.X, p.Y - radius, p.X, p.Y +radius);
            }
        }

        public DialogResult EditUniverse(Universe universe)
        {
            EditUniverse(universe, true);
          return  EditUniverse(universe, false);
        }
        public DialogResult EditUniverse(Universe universe, bool ruck)
        {
            this.ruck = ruck;
            float ppm = 23.62205f;
            Pen rand = new Pen(Color.White, 1);

            DrawBox[,] Prod = new DrawBox[7, 3];
            DrawBox[,] Agen = new DrawBox[2, 3];
            string[] prods = {
                "Arbeitsmarke",
                "Arbeitsmarke",
                "Stahlmarke",
                "Stahlmarke",
                "SchwerMarke",
                "Forschungsmarke",
                "Luxusmarke",
            };
            float max = 991 + 2f / 3;
            float einzugProd = ruck ? max - 835.1583f :80;
            float einzugAgen = ruck ? max - 898.936f : 80;

            PointF prog = new PointF(0, 70);

            Text all = new Text();
            all.add(this);
            all.addWhitespace(10, prog.Y, true);

            for (int i = 0; i < 7; i++)
            {
                prog.X = einzugProd;
                all.addWhitespace(einzugProd, 1);
                for (int j = 0; j < 3; j++)
                {
                    Prod[i, j] = new FixedFontKarte(universe.Karten[prods[i]], ppm, ruck,false);
                    Prod[i, j].setup(prog);
                    all.add(Prod[i, j]);
                    all.add(new Whitespace(2,2, false));
                    prog.X += Prod[i, j].box.Width;
                }
                all.addAbsatz();
                    all.add(new Whitespace(2,2, false));
                all.addAbsatz();
                prog.Y += Prod[i, 0].box.Height;
            }
            for (int i = 0; i < 2; i++)
            {
                prog.X = einzugAgen;
                all.addWhitespace(einzugAgen, 1);
                for (int j = 0; j < 3; j++)
                {
                    Agen[i, j] = new FixedFontKarte(universe.Karten["Streik"], ppm, ruck, false);
                    Agen[i, j].setup(prog);
                    all.add(Agen[i, j]);
                    all.add(new Whitespace(2,2, false));
                    prog.X += Agen[i, j].box.Width;
                }
                all.addAbsatz();
                all.add(new Whitespace(2, 2, false));
                all.addAbsatz();
                prog.Y += Agen[i, 0].box.Height;
            }
            all.createPDF("ProdStreik"+ruck);

            //MessageBox.Show(Prod[0,2].box.Right+
            //    "\r\n"
            //    +Agen[0,2].box.Right);

            return DialogResult.OK;
        }

        public override float getMax()
        {
            return 1;
        }

        public override float getMin()
        {
            return 0;
        }

        public override float getSpace()
        {
            return 1;
        }

        public override void setup(RectangleF box)
        {
        }

        public override void update()
        {
        }
    }
}
