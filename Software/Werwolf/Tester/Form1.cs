using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Drawing.Algorithms;
using Assistment.Drawing;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;

namespace Tester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //Bitmap input = new Bitmap(@"C:\Users\Rüstü\Downloads\images.jpg");
            //PolygonFillingAlgorithm pfa = PolygonFillingAlgorithm.TriangleTesselation(19, 11);
            //pfa.Execute(input, 1920, 1080).Save(@"C:\Users\Rüstü\Downloads\TriResult.png");

            Bitmap b = new Bitmap(2000, 2000);
            using (Graphics g = b.GetHighGraphics())
            {
                g.Clear(Color.Black);
                int a = 300;
                float boost = 30;
                PointF[] strecke = new PointF[a];
                PointF next = new PointF(b.Width / 2f, b.Height / 2f);
                PointF dir = new PointF(1,0);
                float m = 0;
                for (int i = 0; i < a; i++)
                {
                    strecke[i] = next;
                    m += boost;
                    next = next.add(dir.mul(m));
                    dir = new PointF(dir.Y, -dir.X);
                }
                Shadex.getCyberPunkDraht(strecke, boost, 2, 2, boost*2 , new Pen(Color.White, 1)).drawGraph(g);
            }
            b.Save(@"C:\Users\Rüstü\Downloads\Back.png");
        }
    }
}
