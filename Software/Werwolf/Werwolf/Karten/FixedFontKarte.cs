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
        public bool Rotieren { get; set; }

        public FixedFontKarte(Karte Karte, float ppm, bool Ruckseite, bool Rotieren)
            : base(Karte, ppm)
        {
            this.Ruckseite = Ruckseite;
            this.Rotieren = Rotieren;
        }

        public override float getMax()
        {
            return Rotieren ? AussenBox.Height : AussenBox.Width;
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
            if (Rotieren)
                this.box.Size = this.box.Size.permut();
        }
        public override void draw(DrawContext con)
        {
            if (Ruckseite)
                using (Image image = Karte.GetBackImage(Ppm, Color.Black, true))
                {
                    if (Rotieren)
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    con.drawImage(image, box);
                }
            else
                using (Image image = Karte.GetImage(Ppm, true))
                {
                    if (Rotieren)
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    con.drawImage(image, box);
                }
        }
        public override void Move(PointF ToMove)
        {
            this.box = this.box.move(ToMove);
        }
    }
}
