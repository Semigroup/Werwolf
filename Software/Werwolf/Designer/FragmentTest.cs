using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assistment;
using Assistment.Drawing;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.Geometries.Extensions;

namespace Designer
{
    public partial class FragmentTest : Form
    {
        public Bitmap Picture { get; set; }
        public Graphics Graphics { get; set; }

        public FragmentTest()
        {
            InitializeComponent();

            Picture = new Bitmap(pictureBox1.Size.Width, pictureBox1.Size.Height);
            pictureBox1.Image = Picture;
            Graphics = Graphics.FromImage(Picture);

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
            Pen outline = Pens.Red;

            Graphics.Clear(Color.White);
            Graphics.DrawLine(outline, 0, Picture.Height / 2, (Picture.Width - pointFBox1.UserX) / 2, Picture.Height / 2);
            Graphics.DrawLine(outline, (pointFBox1.UserX +Picture.Width )/ 2, Picture.Height / 2, Picture.Width, Picture.Height / 2);
            Graphics.DrawRectangle(outline, (Picture.Width - pointFBox1.UserX) / 2, (Picture.Height) / 2 - pointFBox1.UserY, pointFBox1.UserX, 2*pointFBox1.UserY);

            OrientierbarerWeg frag = Fragments.GetFragment((Fragments.Style)enumBox1.UserValue , pointFBox1.UserX, pointFBox1.UserY);
            frag = frag + new PointF((Picture.Width - pointFBox1.UserX) / 2 , Picture.Height / 2);
            if (checkBox1.Checked)
                frag = frag.Spiegel(new Gerade(0, Picture.Height / 2, 1, 0));

            int n = Math.Max(intBox1.UserValue,2);
            PointF[] P = new PointF[n];
            for (int i = 0; i < n; i++)
            {
                float t = i / (n - 1f);
                P[i] = frag.Weg(t);
            }
            if (ShowNormales.Checked)
            {
                n = intBox2.UserValue;
                float hohe = floatBox1.UserValue;
                for (int i = 0; i < n; i++)
                {
                    float t = i / (n - 1f);
                    Color c = Color.Magenta.tween(Color.Orange, t);
                    PointF p = frag.Weg(t);
                    Graphics.DrawLine(new Pen(c), p, p.saxpy(hohe, frag.Normale(t)));
                }
            }
            if (ShowTangents.Checked)
            {
                n = NumberTangents.UserValue;
                float hohe = LengthTangents.UserValue;
                for (int i = 0; i < n; i++)
                {
                    float t = i / (n - 1f);
                    Color c = Color.Green.tween(Color.Blue, t);
                    PointF p = frag.Weg(t);
                    PointF target = p.saxpy(hohe, frag.Tangente(t));
                    Graphics.DrawLine(new Pen(c), p, target);
                    float r = 2;
                    Graphics.FillEllipse(new SolidBrush(c), target.X - r, target.Y - r, 2 * r, 2 * r);
                }
            }


            Graphics.DrawLines(new Pen(Color.Black, 2), P);

            pictureBox1.Refresh();
        }
    }
}
