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
        public RectangleF OuterBox;
        public RectangleF InnerBox;
        public RectangleF TextBox;

        private Text[] Texts;

        public float InnenRadius { get { return TextDarstellung.InnenRadius; } }
        public float BalkenDicke { get { return TextDarstellung.BalkenDicke; } }
        public SizeF Rand { get { return TextDarstellung.Rand; } }

        private Image Back;

        public WolfText(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            update();
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            update();
        }
        public override bool Visible()
        {
            return base.Visible() && Karte.MeineAufgaben.Anzahl() > 0;
        }
        public override void update()
        {
            Aufgabe Aufgaben = Karte.MeineAufgaben;

            if (Karte == null || Aufgaben.Anzahl() == 0)
                return;

            OuterBox = InnenBox;
            InnerBox = OuterBox.Inner(Rand.add(BalkenDicke, BalkenDicke).mul(Faktor));
            TextBox = InnerBox.Inner(InnenRadius * Faktor, InnenRadius * Faktor);

            Texts = new Text[Aufgaben.Anzahl()];
            int i = 0;
            foreach (var item in Aufgaben)
            {
                Texts[i] = new Text(item, TextDarstellung.FontMeasurer);
                Texts[i++].setup(TextBox);
            }

            float Bottom = TextBox.Bottom;
            for (i = Texts.Length - 1; i >= 0; i--)
            {
                Texts[i].Bottom = Bottom;
                Bottom = Texts[i].Top - Faktor * (BalkenDicke + 2 * InnenRadius);
            }

            TextBox.Height = TextBox.Bottom - Texts[0].Top;
            TextBox.Y = Texts[0].Top;
            InnerBox.Height = InnerBox.Bottom - (TextBox.Top - InnenRadius * Faktor);
            InnerBox.Y = TextBox.Top - InnenRadius * Faktor;
            OuterBox.Height = OuterBox.Bottom - (InnerBox.Top - Faktor * (BalkenDicke + Rand.Height));
            OuterBox.Y = InnerBox.Top - Faktor * (BalkenDicke + Rand.Height);

            Mal();
        }

        private Brush[] GetBrushes()
        {
            Brush[] br = new Brush[10];
            for (int i = 0; i < br.Length; i++)
                br[i] = new SolidBrush(Color.FromArgb((br.Length - i) * 255 / br.Length, TextDarstellung.RandFarbe));
            return br;
        }

        private void Mal()
        {
            //PointF Offset = OuterBox.Location.mul(-1);
            //Size s = OuterBox.Size.mul(ppm / Faktor).ToSize();
            RectangleF outBox = OuterBox.mul(1 / Faktor);
            PointF Offset = outBox.Location.mul(-1);
            RectangleF innBox = InnerBox.mul(1 / Faktor);
            Size s = outBox.Size.mul(ppm).ToSize();
            Back = new Bitmap(s.Width, s.Height);

            using (Graphics g = Back.GetHighGraphics(ppm))
            {
                OrientierbarerWeg OrientierbarerWeg = Rund(innBox.move(Offset), BalkenDicke);
                Hohe h = t => OrientierbarerWeg.normale(t).SKP(Rand.ToPointF()) * Random.NextFloat();

                int L = (int)OrientierbarerWeg.L;
                Shadex.malBezierhulle(g, GetBrushes(), OrientierbarerWeg, h, L * 10, L );
                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                Brush Brush = TextDarstellung.Farbe.ToBrush();
                foreach (var item in Texts)
                {
                    OrientierbarerWeg = Rund(item.box.mul(1 / Faktor).move(Offset), InnenRadius);
                    L = (int)OrientierbarerWeg.L;
                    g.FillPolygon(Brush, OrientierbarerWeg.getPolygon(L, 0, 1));
                }
            }
        }
        private OrientierbarerWeg Rund(RectangleF Rectangle, float AussenRadius)
        {
            return OrientierbarerWeg.RundesRechteck(Rectangle.Inner(-AussenRadius, -AussenRadius), AussenRadius);
        }

        public void KorrigierUmInfo(float InfoHeight)
        {
            OuterBox = OuterBox.move(0, -InfoHeight);
            InnerBox = OuterBox.move(0, -InfoHeight);
            TextBox = OuterBox.move(0, -InfoHeight);
            foreach (var item in Texts)
                item.Move(0, -InfoHeight);
        }
        public override void Move(PointF ToMove)
        {
            this.box = box.move(ToMove);
            foreach (var item in Texts)
                item.Move(ToMove);
        }
        public override void setup(RectangleF box)
        {
            Move(box.Location.sub(this.box.Location));
        }
        public override void draw(DrawContext con)
        {
            con.drawImage(Back, OuterBox.move(box.Location));
            foreach (var item in Texts)
                item.draw(con);
        }
    }
}
