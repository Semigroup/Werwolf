using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assistment.Drawing.Geometries.Typing.Digital;
using Assistment.Drawing.Geometries.Typing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing;
using Assistment.Drawing.Geometries;
using Assistment.Extensions;

namespace Designer
{
    public partial class TypingTest : Form
    {
        string text = "";
        Alphabet alphabet;
        Image Image;

        public TypingTest()
        {
            InitializeComponent();
            alphabet = new Alphabet();
            alphabet.MakeDigital();

            enumBox1.SetValue(LetterBox.Style.Hard);
        }

        private void TypingTest_Paint(object sender, PaintEventArgs e)
        {
            LetterBox box = LetterBox.GetGoldenCut(SizeBox.UserValue);
            box.CornerStyle = (LetterBox.Style)enumBox1.GetValue();
            float burst = BurstBox.UserValue;
            int linesLinks = pointBox1.UserX;
            int linesRechts = pointBox1.UserY;
            float breite = BreiteBox.UserValue;
            float radius = floatBox1.UserValue;
            int samplesPerLetter = SamplesBox.UserValue;

            e.Graphics.Clear(Color.White);
            e.Graphics.Raise();

            DrawString(e.Graphics, text, new PointF(), Pens.Black,
                box, alphabet, burst, linesLinks, linesRechts, breite, radius, samplesPerLetter);
        }

        public void DrawString(Graphics g, string s, PointF offset, Pen pen,
             LetterBox letterBox, Alphabet alphabet,
             float burst, int linesLinks, int linesRechts, float breite, float radius,
             int samplesPerLetter)
        {
            letterBox.Offset = offset;
            foreach (var line in s.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] != ' ')
                    {
                        OrientierbarerWeg[] arcs = letterBox.GetCurves(alphabet[line[i]]);
                        float[] lengths = arcs.Map(arc => arc.L).ToArray();
                        float totalLength = lengths.Sum();
                        for (int k = 0; k < arcs.Length; k++)
                        {
                            int samples = Math.Max((int)(samplesPerLetter * lengths[k] / totalLength), 2);
                            PointF[] poly = arcs[k].GetPolygon(samples, 0, 1);
                            CyberShadex.getCyberPunkDraht(poly,
                                burst, linesLinks, linesRechts, breite, radius, pen)
                                .drawKaskade(g);
                        }

                        letterBox.Offset = new PointF(letterBox.Offset.X + letterBox.InterimWidth, letterBox.Offset.Y);
                    }
                    letterBox.Offset = new PointF(letterBox.Offset.X + letterBox.Width, letterBox.Offset.Y);
                }
                letterBox.Offset = new PointF(offset.X, letterBox.Offset.Y + letterBox.TotalHeight);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            text = textBox1.Text;
            Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LetterBox box = LetterBox.GetGoldenCut(SizeBox.UserValue);
            box.CornerStyle = (LetterBox.Style)enumBox1.GetValue();
            float burst = BurstBox.UserValue;
            int linesLinks = pointBox1.UserX;
            int linesRechts = pointBox1.UserY;
            float breite = BreiteBox.UserValue;
            float radius = floatBox1.UserValue;
            int samplesPerLetter = SamplesBox.UserValue;

            int ppm = 23;
            int dina = 4;
            Size s = dina.DinA(false);
            using (Bitmap bmp = new Bitmap(s.Width * ppm, s.Height * ppm))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.Raise();
                g.ScaleTransform(6, 6);

                DrawString(g, text, new PointF(), Pens.Black,
                    box, alphabet, burst, linesLinks, linesRechts, breite, radius, samplesPerLetter);

                bmp.savePdf("CyberFontTest", 4, false);
            }
        }
    }
}
