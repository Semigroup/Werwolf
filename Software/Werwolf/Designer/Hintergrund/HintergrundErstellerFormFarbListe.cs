using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Assistment.Drawing.Style;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;
using Assistment.Drawing;
using Assistment.Mathematik;
using Assistment.form;
using Werwolf.Generating;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.Style;
using Assistment.Extensions;

namespace Designer.Hintergrund
{
    public partial class HintergrundErstellerFormFarbListe : Form, IDrawer
    {
        private PDFDialog pdf;

        public HintergrundErstellerFormFarbListe()
        {
            InitializeComponent();

            pdf = new PDFDialog(this);

            this.BurstBox.UserValue = 0.02f;

            this.BackColorBox.Color = Color.Black;

            this.BoxenBox.UserPoint = new Point(1, 121);// new Point(10, 10);
            this.ThumbBox.UserPoint = new Point(1, 2);//new Point(2, 2);
            this.SamplesBox.UserPoint = new Point(40, 50);//new Point(50, 1);//new Point(100, 100);

            this.enumBox1.UserValue = HintergrundSchema.Art.ChaosRechteck;

            this.BurstBox.UserValueChanged += Make;

            this.BackColorBox.ColorChanged += Make;

            this.BoxenBox.PointChanged += Make;
            this.SamplesBox.PointChanged += Make;
            this.ThumbBox.PointChanged += Make;

            this.enumBox1.UserValueChanged += Make;
            this.GroseBox.PointChanged += Make;

            this.textBox1.TextChanged += Make;
            this.checkBox1.CheckedChanged += Make;

            Make(this, new EventArgs());
        }

        public void Make(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Draw(true, 5);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pdf.ShowDialog();
        }

        public Color[] GetColors()
        {
            string s = textBox1.Text;
            string[] ss = s.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            List<Color> colors = new List<Color>();
            foreach (var item in ss)
                if (item.Length > 0 && item[0] != '#')
                {
                    string jtem = item;
                    if (item.Length < 8)
                        jtem += new string('0', 8 - item.Length);
                    colors.Add(jtem.ToColor());
                }

            return colors.ToArray();
        }

        public Image Draw(bool Hoch, float ppm)
        {
            Random d = new Random();
            float burst = BurstBox.UserValue;


            HintergrundSchema hs = new HintergrundSchema();
            hs.MeineArt = (HintergrundSchema.Art)this.enumBox1.UserValue;
            hs.Size = GroseBox.UserSize.ToSize(); //.mul(5)
            hs.Schema = new FlachenSchema();
            hs.Schema.BackColor = BackColorBox.Color;
            hs.Schema.Flache = (u, v) => hs.Size.mul(u + burst * d.NextCenterd(), v + burst * d.NextCenterd()).ToPointF();

            hs.Schema.Boxes = BoxenBox.UserPoint;
            hs.Schema.Boxes.X = Math.Max(hs.Schema.Boxes.X, 1);
            hs.Schema.Boxes.Y = Math.Max(hs.Schema.Boxes.Y, 1);
            hs.Schema.Samples = SamplesBox.UserPoint;
            hs.Schema.Samples.X = Math.Max(hs.Schema.Samples.X, 2);
            hs.Schema.Samples.Y = Math.Max(hs.Schema.Samples.Y, 2);
            hs.Schema.Thumb = ThumbBox.UserPoint;
            hs.Schema.Thumb.X = Math.Max(hs.Schema.Thumb.X, 1);
            hs.Schema.Thumb.Y = Math.Max(hs.Schema.Thumb.Y, 1);
            hs.Schema.DrawingRegion = new RectangleF(new PointF(), hs.Size);

            Color[] colors = GetColors();
            if (colors.Length > 0)
            {
                if (!checkBox1.Checked)
                    hs.Schema.Pinsel = (u, v) =>
                    {
                        float t = v.Saturate() * (colors.Length - 1);
                        int n = (int)t;
                        t -= n;
                        return colors[n].tween(colors[Math.Min(colors.Length - 1, n + 1)], t).ToBrush();
                    };
                else
                    hs.Schema.Pinsel = (u, v) => colors[d.Next(colors.Length)].ToBrush();
            }
            else
                hs.Schema.Pinsel = (u, v) => Brushes.Red;


            Bitmap b = new Bitmap((hs.Size.Width * ppm).Ceil(), (hs.Size.Height * ppm).Ceil());
            Graphics g = b.GetHighGraphics();
            g.ScaleTransform(ppm, ppm);

            switch (hs.MeineArt)
            {
                case HintergrundSchema.Art.ChaosRechteck:
                    Shadex.ChaosFlache(g, hs.Schema);
                    break;
                case HintergrundSchema.Art.OldSchool:
                    Shadex.ChaosFlacheBundig(g, hs.Schema);
                    break;
                case HintergrundSchema.Art.Kreis:
                    float sqr = FastMath.Sqrt(2);
                    hs.Schema.Flache = (u, v) => hs.Size.mul(FastMath.Sphere(u * Math.PI * 2)
                        .mul(sqr * (v + burst * d.NextFloat()) / 2).add(0.5f, 0.5f)).ToPointF();
                    Shadex.ChaosFlache(g, hs.Schema);
                    break;
                case HintergrundSchema.Art.Stetig:
                    FlachenFunktion<PointF> feld = (u, v) => d.NextSpherical();
                    feld = feld.KompaktDeterminieren(hs.Schema.Samples.X, hs.Schema.Samples.Y);
                    FlachenFunktion<PointF> pot = feld.Potential(burst, 10);
                    hs.Schema.Flache = (u, v) => pot(u, v).mul(hs.Size);
                    Shadex.ChaosFlacheBase(g, hs.Schema);
                    break;
                default:
                    throw new NotImplementedException();
            }
            return b;
        }

        public int GetDInA()
        {
            return 4;
        }
    }
}
