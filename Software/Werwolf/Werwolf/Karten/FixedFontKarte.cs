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
    public class FixedFontKarte : WolfBox
    {
        public FixedFontKarte(Karte Karte, float ppm)
            : base(Karte, ppm)
        {
        }

        public override float getMax()
        {
            return AussenBox.Width;
        }
        public override float getMin()
        {
            return getMax();
        }
        public override float getSpace()
        {
            return AussenBox.Size.Inhalt();
        }

        public override void update()
        {
        }
        public override void setup(RectangleF box)
        {
            this.box = AussenBox.move(box.Location);
        }
        public override void draw(DrawContext con)
        {
            using (Image image = Karte.GetImage(Ppm))
                con.drawImage(image, AussenBox.move(box.Location));
        }
        public override void Move(PointF ToMove)
        {
            this.box = this.box.move(ToMove);
        }
    }
}
