using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Texts;
using Werwolf.Inhalt;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

namespace Werwolf.Karten.Figur
{
    public class StatInfo : WolfBox
    {
        public TextBild Rustung => Karte.LayoutDarstellung.Rustung;
        public TextBild Leben => Karte.LayoutDarstellung.Leben;
        public TextBild LebenLeer => Karte.LayoutDarstellung.LebenLeer;

        public StatInfo(Karte Karte, float PPm) : base(Karte, PPm)
        {

        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            RectangleF MovedInnenBox = InnenBox.move(Box.Location);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
        }
        public override void Update()
        {
        }

        public override bool Visible()
        {
            return base.Visible();
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
                return;
        }
        public override void Draw(DrawContext con)
        {
            xFont font = Karte.InfoDarstellung.FontMeasurer;

            if (Karte.Rustung != 0)
            {
                PointF p = new PointF(Karte.HintergrundDarstellung.MarginLeft, Karte.HintergrundDarstellung.MarginTop).mul(Faktor / 2);
                p.Y += HintergrundDarstellung.Anker.Y * Faktor;
                con.DrawCenteredImage(Rustung, p, InnenBox);
                string s = Karte.Rustung.ToString();
                SizeF fontSize = font.Size(s);
                p = p.sub(fontSize.Width / 2, fontSize.Height / 2);
                con.DrawString(s, font.GetFont(), Brushes.Black, p, fontSize.Height);
            }
            if (Karte.Leben != 0)
            {
                PointF p = new PointF(Karte.HintergrundDarstellung.MarginRight, Karte.HintergrundDarstellung.MarginTop).mul(Faktor / 2);
                p.Y += HintergrundDarstellung.Anker.Y * Faktor;
                p.X = InnenBox.Right - p.X;
                con.DrawCenteredImage(Leben, p, InnenBox);

                string s = Karte.Leben.ToString();
                SizeF fontSize = font.Size(s);
                PointF p2 = p.sub(fontSize.Width / 2, fontSize.Height / 2);
                con.DrawString(s, font.GetFont(), Brushes.Black, p2, fontSize.Height);

                p.Y += Leben.Size.Width * Faktor/ 2 + LebenLeer.Size.Height * Faktor / 2;
                con.DrawCenteredImage(LebenLeer, p, InnenBox);
            }
        }
    }
}
