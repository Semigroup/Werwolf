using System;
using System.Drawing;
using System.Windows.Forms;
using Assistment.Extensions;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.Geometries.Extensions;

namespace Designer
{
    public partial class PotentialTest : Form
    {
        public Bitmap Picture { get; set; }
        public Graphics Graphics { get; set; }
        public FlachenFunktion<PointF> Vektorfeld { get; set; }

        private Random d = new Random();

        public PotentialTest()
        {
            InitializeComponent();

            Picture = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = Picture;
            Graphics = Graphics.FromImage(Picture);
            Graphics.ScaleTransform(Picture.Size.Width, Picture.Size.Height);



            this.pointFBox1.PointChanged += DrawFragment;
            this.enumBox1.UserValueChanged += DrawFragment;
            this.intBox1.UserValueChanged += DrawFragment;
            this.intBox2.UserValueChanged += DrawFragment;
            this.checkBox1.CheckedChanged += DrawFragment;
            this.floatBox1.UserValueChanged += DrawFragment;
            this.ShowTangents.CheckedChanged += DrawFragment;
            this.ShowNormales.CheckedChanged += DrawFragment;
            this.NumberTangents.UserValueChanged += DrawFragment;
            this.LengthTangents.UserValueChanged += DrawFragment;

            this.enumBox1.SetValue(Fragments.Style.Gerade);
        }

        public void DrawFragment(object sender, EventArgs e)
        {
            Graphics.Clear(Color.White);

            int USamples =10, VSamples = 10;

            Vektorfeld = (u, v) => d.NextSpherical();
            Vektorfeld = Vektorfeld.Determinieren(USamples, VSamples);

            int schritte = NumberTangents.UserValue;
            float t = LengthTangents.UserValue ;

            int faktor = 10;

            USamples *= faktor;
            VSamples *= faktor;

            //for (int u = 0; u < USamples; u++)
            //    for (int v = 0; v < VSamples; v++)
            //    {
            //        PointF position = new PointF(u / (USamples - 1f), v / (VSamples - 1f));
            //        for (int i = 0; i < schritte; i++)
            //        {
            //            PointF newPosition = position.saxpy(t, Vektorfeld(position.X, position.Y).normalize()).sat();
            //            Pen p = new Pen(Color.Red.tween(Color.Blue, i * 1f / schritte));
            //            Graphics.DrawLine(p, position.mul(Picture.Size), newPosition.mul(Picture.Size));
            //            position = newPosition;
            //        }
            //    }


            //for (int u = 0; u < USamples; u++)
            //    for (int v = 0; v < VSamples; v++)
            //    {
            //        PointF position = new PointF(u / (USamples - 1f), v / (VSamples - 1f));
            //        PointF nextPosition = new PointF((u+1) / (USamples - 1f), v / (VSamples - 1f));
            //        for (int i = 0; i < schritte; i++)
            //        {
            //            position = position.saxpy(t, Vektorfeld(position.X, position.Y).normalize()).sat();
            //            nextPosition = nextPosition.saxpy(t, Vektorfeld(nextPosition.X, nextPosition.Y).normalize()).sat();
            //        }
            //        Graphics.DrawLine(Pens.Black, position.mul(Picture.Size), nextPosition.mul(Picture.Size));
            //    }
            //for (int u = 0; u < USamples; u++)
            //    for (int v = 0; v < VSamples; v++)
            //    {
            //        PointF position = new PointF(u / (USamples - 1f), v / (VSamples - 1f));
            //        PointF nextPosition = new PointF(u / (USamples - 1f), (v+1) / (VSamples - 1f));
            //        for (int i = 0; i < schritte; i++)
            //        {
            //            position = position.saxpy(t, Vektorfeld(position.X, position.Y).normalize()).sat();
            //            nextPosition = nextPosition.saxpy(t, Vektorfeld(nextPosition.X, nextPosition.Y).normalize()).sat();
            //        }
            //        Graphics.DrawLine(Pens.Black, position.mul(Picture.Size), nextPosition.mul(Picture.Size));
            //    }


            FlachenFunktion<PointF> Potential = Vektorfeld.Potential(t, schritte);

            for (int u = 0; u < USamples; u++)
                for (int v = 0; v < VSamples; v++)
                {
                    Color c = Color.Blue.tween(Color.Yellow, v / (VSamples - 1f)).tween(Color.Red, u / (USamples -1f));
                    c = (u+v) % 2 == 0 ? Color.White : Color.Black;

                    PointF[] positions = { new PointF(u / (USamples - 1f), v / (VSamples - 1f)) ,
                    new PointF((u+1) / (USamples - 1f), v / (VSamples - 1f)),
                    new PointF((u+1) / (USamples - 1f), (v+1) / (VSamples - 1f)),
                    new PointF(u / (USamples - 1f), (v+1) / (VSamples - 1f))};

                    Graphics.FillPolygon(c.ToBrush(), Potential.Process(positions));
                }

            //for (int u = 0; u < USamples; u++)
            //    for (int v = 0; v < VSamples; v++)
            //    {
            //        PointF position = new PointF(u / (USamples - 1f), v / (VSamples - 1f));
            //        PointF vek = Vektorfeld(position.X, position.Y);
            //        vek = vek.add(1, 1).div(2);
            //        Color c = Color.FromArgb((int)(255 * vek.X), 0, (int)(255 * vek.Y));
            //        Graphics.FillRectangle(c.ToBrush(), new RectangleF(position, new SizeF(1 / (USamples - 1f), 1 / (VSamples - 1f))).mul(Picture.Size));
            //    }

            pictureBox1.Refresh();
        }
    }
}
