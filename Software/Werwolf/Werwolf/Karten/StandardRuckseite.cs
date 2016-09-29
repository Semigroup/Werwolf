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
        public override float getSpace()
        {
            return AussenBox.Size.Inhalt();
        }
        public override float getMin()
        {
            return AussenBox.Size.Width;
        }
        public override float getMax()
        {
            return getMin();
        }

        public override void update()
        {
        }

        public StandardRuckseite(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }


        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Size = AussenBox.Size;
        }
        public override void draw(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(-1, -1);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            if (HintergrundDarstellung.Rand.Inhalt() > 0)
            {
                HintergrundDarstellung.MakeRandBild(ppm);
                con.drawImage(HintergrundDarstellung.RandBild, MovedAussenBox);
            }
            con.DrawCenteredImage(Karte.Fraktion.RuckseitenBild, MovedAussenBoxCenter, MovedInnenBox);
        }
    }
}
