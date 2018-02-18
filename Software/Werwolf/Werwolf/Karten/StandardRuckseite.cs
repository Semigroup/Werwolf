using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Assistment.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class StandardRuckseite : WolfBox
    {
        public override float Space => AussenBox.Size.Inhalt();
        public override float Min => AussenBox.Size.Width;
        public override float Max => Min;

        public override void Update()
        {
        }

        public StandardRuckseite(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }


        public override void Setup(RectangleF box)
        {
            this.Box = box;
            this.Box.Size = AussenBox.Size;
        }
        public override void Draw(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(Box.Location);
            RectangleF MovedInnenBox = InnenBox.move(Box.Location).Inner(-1, -1);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            if (HintergrundDarstellung.Rand.Inhalt() > 0)
            {
                HintergrundDarstellung.MakeRandBild(ppm);
                con.DrawImage(HintergrundDarstellung.RandBild, MovedAussenBox);
            }
            con.FillRectangle(HintergrundDarstellung.RuckseitenFarbe.ToBrush(), MovedInnenBox);
            con.DrawCenteredImage(Karte.Fraktion.RuckseitenBild, MovedAussenBoxCenter, MovedInnenBox);
        }
    }
}
