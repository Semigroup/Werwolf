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
        public bool Ruckseite { get; set; }

        public FixedFontKarte(Karte Karte, float ppm, bool Ruckseite)
            : base(Karte, ppm)
        {
            this.Ruckseite = Ruckseite;
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
            if (Ruckseite)
                using (Image image = Karte.GetBackImage(Ppm, Color.Black, true))
                    con.drawImage(image, AussenBox.move(box.Location));
            else
                using (Image image = Karte.GetImage(Ppm, true))
                    con.drawImage(image, AussenBox.move(box.Location));
        }
        public override void Move(PointF ToMove)
        {
            this.box = this.box.move(ToMove);
        }
    }
}
