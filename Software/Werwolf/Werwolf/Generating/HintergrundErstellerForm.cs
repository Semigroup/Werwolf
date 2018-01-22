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

namespace Werwolf.Generating
{
    public partial class HintergrundErstellerForm : Form, IDrawer
    {

        private PDFDialog pdf;

        public HintergrundErstellerForm()
        {
            InitializeComponent();

            pdf = new PDFDialog(this);

            this.BurstBox.UserValue = 0.02f;

            this.BoxLO.Color = //Color.Red.flat(100);
            this.BoxRO.Color = Color.FromArgb(150, 50, 200, 255);//Color.Blue.flat(100);
            this.BoxRU.Color = //Color.Green.flat(100);
            this.BoxLU.Color = Color.FromArgb(150, 255, 255, 0);//Color.Yellow.flat(100);
            this.BackColorBox.Color = Color.Black;

            this.BoxenBox.UserPoint = new Point(1, 121);// new Point(10, 10);
            this.ThumbBox.UserPoint = new Point(1, 2);//new Point(2, 2);
            this.SamplesBox.UserPoint = new Point(40, 50);//new Point(50, 1);//new Point(100, 100);

            this.enumBox1.UserValue = HintergrundSchema.Art.ChaosRechteck;

            this.BurstBox.UserValueChanged += Make;

            this.BoxLO.ColorChanged += Make;
            this.BoxRO.ColorChanged += Make;
            this.BoxRU.ColorChanged += Make;
            this.BoxLU.ColorChanged += Make;
            this.BackColorBox.ColorChanged += Make;

            this.BoxenBox.PointChanged += Make;
            this.SamplesBox.PointChanged += Make;
            this.ThumbBox.PointChanged += Make;

            this.enumBox1.UserValueChanged += Make;
            this.GroseBox.PointChanged += Make;

            Make(this, new EventArgs());
        }

        public void Make(object sender, EventArgs e)
        {
            this.pictureBox1.Image = Draw(true, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pdf.ShowDialog();
        }

        public Image Draw(bool Hoch, float ppm)
        {
            Random d = new Random();
            float burst = BurstBox.UserValue;

            HintergrundSchema hs = new HintergrundSchema();
            hs.MeineArt = (HintergrundSchema.Art)this.enumBox1.UserValue;
            hs.Size = GroseBox.UserSize.mul(5).ToSize();
            hs.Schema = new FlachenSchema();
            hs.Schema.BackColor = BackColorBox.Color;
            hs.Schema.Flache = (u, v) => hs.Size.mul(u + burst * d.NextCenterd(), v + burst * d.NextCenterd()).ToPointF();

            Color lo = BoxLO.Color;
            Color lu = BoxLU.Color;
            Color ro = BoxRO.Color;
            Color ru = BoxRU.Color;

            hs.Schema.Pinsel = (u, v) =>
            {
                Color oben = lo.tween(ro, u);
                Color unten = lu.tween(ru, u);
                Color mitte = oben.tween(unten, v);
                return new SolidBrush(mitte);
            };

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
                        .mul(sqr * (v + burst * d.NextFloat())/2).add(0.5f, 0.5f)).ToPointF();
                    Shadex.ChaosFlache(g, hs.Schema);
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
