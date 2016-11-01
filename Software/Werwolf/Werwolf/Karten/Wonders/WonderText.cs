using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistment.Extensions;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten
{
    public class WonderText : WolfBox
    {
        private DrawBox Text;
        private FixedBox FixedBox;

        private xFont LastFont;
        private string LastAufgabe;

        private RectangleF MovedAussenBox;

        private float EntwicklungsBreite;

        public WonderText(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override bool Visible()
        {
            return base.Visible() && Karte.MeineAufgaben.Anzahl > 0;
        }
        public override void update()
        {
        }
        public void SetEntwicklungsBreite(float Breite)
        {
            this.EntwicklungsBreite = Breite;
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Size = AussenBox.Size;
            string aufgabe = Karte.MeineAufgaben.ToString();
            if (!(LastAufgabe == aufgabe
                && LastFont == TextDarstellung.FontMeasurer))
            {
                LastAufgabe = aufgabe;
                LastFont = TextDarstellung.FontMeasurer;
                Text = Karte.MeineAufgaben.ProduceTexts(TextDarstellung.FontMeasurer)[0];
            }
            RectangleF MovedInnenBox = InnenBox.move(box.Location);
            MovedInnenBox = MovedInnenBox.move(HintergrundDarstellung.Anker.mul(Faktor));
            MovedInnenBox.Size = MovedInnenBox.Size.sub(HintergrundDarstellung.Anker.mul(Faktor).ToSize());
            MovedInnenBox.Width -= EntwicklungsBreite;
            FixedBox = new FixedBox(MovedInnenBox.Size, Text.Geometry(TextDarstellung.Rand.mul(Faktor)));
            FixedBox.Alignment = new SizeF(0.5f, 1);
            FixedBox.setup(MovedInnenBox);

            MovedAussenBox = AussenBox.move(box.Location);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            FixedBox.Move(ToMove);
            MovedAussenBox = MovedAussenBox.move(ToMove);
        }
        public override void draw(DrawContext con)
        {
            MovedAussenBox.Height -= Text.Top;
            MovedAussenBox.Y += Text.Top;
            con.fillRectangle(TextDarstellung.Farbe.ToBrush(), MovedAussenBox);
            FixedBox.draw(con);
        }
    }
}
