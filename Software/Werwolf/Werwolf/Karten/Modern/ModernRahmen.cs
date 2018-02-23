using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;
using Werwolf.Inhalt;

namespace Werwolf.Karten.Modern
{
    public class ModernRahmen : WolfBox
    {
        public PointF MovedAussenBoxCenter { get; set; }
        public RectangleF MovedInnenBox { get; set; }

        public ModernRahmen(Karte Karte, float PPm) : base(Karte, PPm)
        {

        }

        public override bool Visible()
        {
            return HintergrundDarstellung.Existiert;
        }

        public override void Setup(RectangleF box)
        {
            RectangleF MovedAussenBox = AussenBox.move(Box.Location);
            MovedInnenBox = InnenBox.move(Box.Location).Inner(-1, -1);
            MovedAussenBoxCenter = MovedAussenBox.Center();
        }

        public override void Update()
        {
        }

        public override void Draw(DrawContext con)
        {
            HintergrundBild hintergrundBild = HintergrundDarstellung.Quer 
                ? Karte.Fraktion.HintergrundBildQuer 
                : Karte.Fraktion.HintergrundBild;
            con.DrawCenteredImage(hintergrundBild, MovedAussenBoxCenter, MovedInnenBox);
        }
    }
}
