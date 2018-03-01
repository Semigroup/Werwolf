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

        private DrawBox[] Texts;

        public float InnenRadius { get { return TextDarstellung.InnenRadius; } }
        public float BalkenDicke { get { return TextDarstellung.BalkenDicke; } }
        public SizeF Rand { get { return TextDarstellung.Rand; } }

        private Image Back;
        private Size LastSize;
        private RectangleF LastoutBox;
        private RectangleF LastinnBox;
        private float LastBalkenDicke;
        private float LastInnenRadius;
        private float LastPpm;
        private SizeF LastRand;
        private Color LastRandFarbe;
        private Color LastFarbe;

        public WolfText(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            Update();
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            Update();
        }
        public override bool Visible()
        {
            return base.Visible() && Karte.MeineAufgaben.Anzahl > 0 && Karte.TextDarstellung.Existiert;
        }
        public override void Update()
        {
            if (!Visible())
                return;

            Aufgabe Aufgaben = Karte.MeineAufgaben;

            if (Karte == null || Aufgaben.Anzahl == 0)
                return;

            OuterBox = InnenBox;
            InnerBox = OuterBox.Inner(Rand.add(BalkenDicke, BalkenDicke).mul(Faktor));
            TextBox = InnerBox.Inner(InnenRadius * Faktor, InnenRadius * Faktor);

            Texts = Aufgaben.ProduceTexts(TextDarstellung.FontMeasurer);
            foreach (var item in Texts)
                item.Setup(TextBox);

            float Bottom = TextBox.Bottom;
            for (int i = Texts.Length - 1; i >= 0; i--)
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

            DrawRessources();
        }

        private Brush[] GetBrushes()
        {
            Brush[] br = new Brush[10];
            for (int i = 0; i < br.Length; i++)
                br[i] = new SolidBrush(Color.FromArgb((br.Length - i) * LastRandFarbe.A / br.Length, LastRandFarbe));
            return br;
        }

        public override void DrawRessources()
        {
            RectangleF outBox = OuterBox.mul(1 / Faktor);
            RectangleF innBox = InnerBox.mul(1 / Faktor);
            Size Size = outBox.Size.mul(ppm).Max(new SizeF(1, 1)).ToPointF().Ceil().ToSize().ToSize();

            float BalkenDicke = this.BalkenDicke;
            float InnenRadius = this.InnenRadius;

            if (LastSize == Size
                && LastoutBox.Equal(outBox)
                && LastinnBox.Equal(innBox)
                && LastBalkenDicke.Equal(BalkenDicke)
                && LastInnenRadius.Equal(InnenRadius)
                && LastPpm.Equal(ppm)
                && LastRand.Equal(Rand)
                && LastFarbe.Equals(TextDarstellung.Farbe)
                && LastRandFarbe.Equals(TextDarstellung.RandFarbe))
                return;

            LastoutBox = outBox;
            LastinnBox = innBox;
            LastSize = Size;
            LastBalkenDicke = BalkenDicke;
            LastInnenRadius = InnenRadius;
            LastPpm = ppm;
            LastRand = Rand;
            LastRandFarbe = TextDarstellung.RandFarbe;
            LastFarbe = TextDarstellung.Farbe;

            PointF Offset = outBox.Location.mul(-1);
            Back = new Bitmap(Size.Width, Size.Height);

            using (Graphics g = Back.GetHighGraphics(ppm))
            {
                int L;
                OrientierbarerWeg OrientierbarerWeg;
                if (!Rand.IsEmpty)
                {
                    OrientierbarerWeg = Rund(innBox.move(Offset), BalkenDicke);
                    Hohe h = t => OrientierbarerWeg.Normale(t).SKP(Rand.ToPointF()) * Random.NextFloat();
                    L = (int)OrientierbarerWeg.L;
                    Shadex.MalBezierhulle(g, GetBrushes(), OrientierbarerWeg, h, L * 10, L);
                }
                else
                    g.Clear(TextDarstellung.RandFarbe);

                g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                Brush Brush = LastFarbe.ToBrush();
                foreach (var item in Texts)
                {
                    RectangleF box = item.Box.mul(1 / Faktor).move(Offset);
                    box.Width = innBox.Width - 2 * InnenRadius;
                    OrientierbarerWeg = Rund(box, InnenRadius);
                    L = (int)OrientierbarerWeg.L;
                    g.FillPolygon(Brush, OrientierbarerWeg.GetPolygon(L, 0, 1));
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
            this.Box = Box.move(ToMove);
            foreach (var item in Texts)
                item.Move(ToMove);
        }
        public override void Setup(RectangleF box)
        {
            Move(box.Location.sub(this.Box.Location));
        }
        public override void Draw(DrawContext con)
        {
            //base.draw(con);

            con.DrawImage(Back, OuterBox.move(Box.Location));
            foreach (var item in Texts)
                item.Draw(con);
        }
    }
}
