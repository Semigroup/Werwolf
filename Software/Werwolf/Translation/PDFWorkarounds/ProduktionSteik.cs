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

        public override DrawBox Clone()
        {
            throw new NotImplementedException();
        }

        public override void Draw(DrawContext con)
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
            all.Add(this);
            all.AddWhitespace(10, prog.Y, true);

            for (int i = 0; i < 7; i++)
            {
                prog.X = einzugProd;
                all.AddWhitespace(einzugProd, 1);
                for (int j = 0; j < 3; j++)
                {
                    Prod[i, j] = new FixedFontKarte(universe.Karten[prods[i]], ppm, ruck,false);
                    Prod[i, j].Setup(prog);
                    all.Add(Prod[i, j]);
                    all.Add(new Whitespace(2,2, false));
                    prog.X += Prod[i, j].Box.Width;
                }
                all.AddAbsatz();
                    all.Add(new Whitespace(2,2, false));
                all.AddAbsatz();
                prog.Y += Prod[i, 0].Box.Height;
            }
            for (int i = 0; i < 2; i++)
            {
                prog.X = einzugAgen;
                all.AddWhitespace(einzugAgen, 1);
                for (int j = 0; j < 3; j++)
                {
                    Agen[i, j] = new FixedFontKarte(universe.Karten["Streik"], ppm, ruck, false);
                    Agen[i, j].Setup(prog);
                    all.Add(Agen[i, j]);
                    all.Add(new Whitespace(2,2, false));
                    prog.X += Agen[i, j].Box.Width;
                }
                all.AddAbsatz();
                all.Add(new Whitespace(2, 2, false));
                all.AddAbsatz();
                prog.Y += Agen[i, 0].Box.Height;
            }
            all.CreatePDF("ProdStreik"+ruck);

            //MessageBox.Show(Prod[0,2].box.Right+
            //    "\r\n"
            //    +Agen[0,2].box.Right);

            return DialogResult.OK;
        }

        public override float Max => 1;

        public override float Min => 0;

        public override float Space => 1;

        public override void Setup(RectangleF box)
        {
        }

        public override void Update()
        {
        }
    }
}
