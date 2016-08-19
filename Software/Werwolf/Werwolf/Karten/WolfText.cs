using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;
using Assistment.Drawing.Geometries;
using Assistment.Drawing;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class WolfText : WolfBox
    {
        private RectangleF AussenBox;
        private RectangleF InnenBox;
        private RectangleF TextBox;

        private Text[] Texts;

        public float InnenRadius { get { return Darstellung.Text.InnenRadius; } }
        public float BalkenDicke { get { return Darstellung.Text.BalkenDicke; } }
        public SizeF Rand { get { return Darstellung.Text.Rand; } }

        private Image Back;

        public WolfText(Karte Karte)
            : base(Karte)
        {
            OnKarteChanged();
        }

        public override void OnKarteChanged()
        {
            update();
        }
        public override void OnPpmChanged()
        {
            update();
        }

        public override float getSpace()
        {
            return AussenBox.Size.Inhalt();
        }

        public override float getMin()
        {
            return AussenBox.Width;
        }

        public override float getMax()
        {
            return AussenBox.Width;
        }

        public override void update()
        {
            if (Karte == null || Darstellung == null || Karte.Aufgaben.Anzahl() == 0)
                return;

            AussenBox.Location = Darstellung.Hintergrund.Rand.ToPointF();
            AussenBox.Size = Darstellung.Size.sub(Darstellung.Hintergrund.Rand.mul(2));

            InnenBox.Location = AussenBox.Location.add(Rand.ToPointF());
            InnenBox.Location = InnenBox.Location.add(new PointF(1, 1).mul(BalkenDicke));
            InnenBox.Size = AussenBox.Size.sub(InnenBox.Location.sub(AussenBox.Location).ToSize().mul(2));

            TextBox.Location = InnenBox.Location.add(new PointF(1, 1).mul(InnenRadius));
            TextBox.Size = InnenBox.Size.sub(TextBox.Location.sub(InnenBox.Location).mul(2).ToSize());

            AussenBox = AussenBox.mul(Faktor);
            InnenBox = InnenBox.mul(Faktor);
            TextBox = TextBox.mul(Faktor);

            Texts = new Text[Karte.Aufgaben.Anzahl()];
            int i = 0;
            foreach (var item in Karte.Aufgaben)
            {
                Texts[i] = new Text();
                Texts[i].addRegex(item, Darstellung.Text.FontMeasurer);
                Texts[i++].setup(TextBox);
            }

            float Bottom = TextBox.Bottom;
            for (i = Texts.Length - 1; i >= 0; i--)
            {
                Texts[i].Bottom = Bottom;
                Bottom = Texts[i].Top - Faktor * (BalkenDicke + 2 * InnenRadius);
                Texts[i].setup(Texts[i].box);
            }

            TextBox.Height = TextBox.Bottom - Texts[0].Top;
            TextBox.Y = Texts[0].Top;
            InnenBox.Height = InnenBox.Bottom - (TextBox.Top - InnenRadius * Faktor);
            InnenBox.Y = TextBox.Top - InnenRadius * Faktor;
            AussenBox.Height = AussenBox.Bottom - (InnenBox.Top - Faktor * (BalkenDicke + Rand.Height));
            AussenBox.Y = InnenBox.Top - Faktor * (BalkenDicke + Rand.Height);

            Mal();
        }

        private Brush[] GetBrushes()
        {
            Brush[] br = new Brush[10];
            for (int i = 0; i < br.Length; i++)
                br[i] = new SolidBrush(Color.FromArgb((br.Length - i) * 255 / br.Length, 0, 0, 0));
            return br;
        }

        private void Mal()
        {
            PointF Offset = AussenBox.Location.mul(-1);
            Size s = AussenBox.Size.mul(ppm).ToSize();
            Back = new Bitmap(s.Width, s.Height);
            Graphics g = Back.GetHighGraphics();
            g.ScaleTransform(ppm, ppm);
            OrientierbarerWeg OrientierbarerWeg = Rund(InnenBox.move(Offset), BalkenDicke * Faktor);
            Hohe h = t =>
                OrientierbarerWeg.normale(t).SKP(Rand.ToPointF())
                * Faktor
                * Random.NextFloat()
                * (1 - (float)Math.Pow(2 * t - 1, 100));

            int L = (int)OrientierbarerWeg.L;
            Shadex.malBezierhulle(g, GetBrushes(), OrientierbarerWeg, h, L / 1, L / 10);
            //g.FillPolygon(Brushes.Black, OrientierbarerWeg.getPolygon(100, 0, 1));

            g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            Brush Brush = Color.FromArgb(128, 255, 255, 255).ToBrush();
            foreach (var item in Texts)
            {
                OrientierbarerWeg = Rund(item.box.move(Offset), InnenRadius * Faktor);
                L = (int)OrientierbarerWeg.L;
                g.FillPolygon(Brush, OrientierbarerWeg.getPolygon(L, 0, 1));
            }
        }

        private OrientierbarerWeg Rund(RectangleF Rectangle, float AussenRadius)
        {
            Rectangle.Location = Rectangle.Location.sub(AussenRadius, AussenRadius);
            Rectangle.Size = Rectangle.Size.sub(new SizeF(-2, -2).mul(AussenRadius));
            return OrientierbarerWeg.RundesRechteck(Rectangle, AussenRadius);
        }

        public override void setup(RectangleF box)
        {
            this.box = AussenBox;
            //this.box.Size = AussenBox.Size;
            //PointF Diff = box.Location.sub(AussenBox.Location);
            //foreach (var item in Texts)
            //    item.box.Location = item.box.Location.add(Diff);
        }

        public override void draw(DrawContext con)
        {
            //System.Windows.Forms.MessageBox.Show(box + ", " + Back.Size);
            if (Karte.Aufgaben.Anzahl() > 0)
            {
                con.drawImage(Back, box);
                foreach (var item in Texts)
                    item.draw(con);
            }
        }
    }
}
