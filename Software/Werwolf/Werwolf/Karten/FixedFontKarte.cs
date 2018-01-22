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

        public override float Max => Rotieren ? AussenBox.Height : AussenBox.Width;
        public override float Min => Max;
        public override float Space => AussenBox.Size.Inhalt();

        public override void Update()
        {
        }
        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox.move(box.Location);
            if (Rotieren)
                this.Box.Size = this.Box.Size.permut();
        }
        public override void Draw(DrawContext con)
        {
            if (Ruckseite)
                using (Image image = Karte.GetBackImage(Ppm, Color.Black, true))
                {
                    if (Rotieren)
                        image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    con.drawImage(image, Box);
                }
            else
                using (Image image = Karte.GetImage(Ppm, true))
                {
                    if (Rotieren)
                        image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    con.drawImage(image, Box);
                }
        }
        public override void Move(PointF ToMove)
        {
            this.Box = this.Box.move(ToMove);
        }
    }
}
