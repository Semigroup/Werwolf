using System.Drawing;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.Geometries.Extensions;

namespace Werwolf.Karten
{
    public class WonderEffekt : WolfBox
    {
        DrawBox DrawBox;

        public WonderEffekt(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override void Update()
        {
        }
        public override bool Visible()
        {
            return base.Visible() && Karte.Effekt.Anzahl > 0;
        }
        public override void Setup(RectangleF box)
        {
            this.Box = box;
            this.Box.Size = AussenBox.Size;
            RectangleF MovedInnenBox = InnenBox.move(box.Location);
            SizeF Size = new SizeF(InnenBox.Width, Karte.HintergrundDarstellung.Anker.Y * Faktor);
            Text t = Karte.Effekt.ProduceTexts(Karte.TextDarstellung.EffektFontMeasurer)[0];
            DrawBox = new FixedBox(Size, t);
            (DrawBox as FixedBox).Alignment = new SizeF(0.5f, 0.5f);
            DrawBox.Setup(MovedInnenBox);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            DrawBox.Move(ToMove);
        }
        public override void Draw(DrawContext con)
        {
            DrawBox.Draw(con);
        }
    }
}
