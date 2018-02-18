using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class StandardBild : WolfBox
    {
        public override float Space => AussenBox.Size.Inhalt();
        public override float Min => AussenBox.Size.Width;
        public override float Max => Min;

        private Bild bild;
        public Bild Bild
        {
            get { return bild; }
            set { bild = value; }
        }

        public StandardBild(Karte Karte, float ppm)
            : base(Karte, ppm)
        {
        }

        public override void Update()
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

            if (bild != null && Bild.Image != null)
                con.DrawCenteredImage(Bild, MovedAussenBoxCenter, MovedInnenBox);
        }
    }
}
