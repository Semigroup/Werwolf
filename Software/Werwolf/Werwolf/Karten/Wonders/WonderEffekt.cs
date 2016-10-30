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
    public class WonderEffekt : WolfBox
    {
        DrawBox DrawBox;

        public WonderEffekt(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override void update()
        {
        }
        public override bool Visible()
        {
            return base.Visible() && Karte.Effekt.Anzahl > 0;
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Size = AussenBox.Size;
            RectangleF MovedInnenBox = InnenBox.move(box.Location);
            SizeF Size = new SizeF(InnenBox.Width, Karte.HintergrundDarstellung.Anker.Y * Faktor);
            Text t = Karte.Effekt.ProduceTexts(Karte.TextDarstellung.EffektFontMeasurer)[0];
            DrawBox = new FixedBox(Size, t);
            (DrawBox as FixedBox).Alignment = new SizeF(0.5f, 0.5f);
            DrawBox.setup(MovedInnenBox);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            DrawBox.Move(ToMove);
        }
        public override void draw(DrawContext con)
        {
            DrawBox.draw(con);
        }
    }
}
