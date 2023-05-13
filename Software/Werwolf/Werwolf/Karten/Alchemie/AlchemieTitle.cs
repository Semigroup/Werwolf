using System.Drawing;
using Assistment.Texts;
using Werwolf.Inhalt;
using Assistment.Drawing.Geometries.Extensions;

namespace Werwolf.Karten.Alchemie
{
    public class AlchemieTitle : WolfBox
    {
        private StarBox StarBox;
        public int Stars { get; set; } = 4;
        private float WidthOff;

        public AlchemieTitle(Karte Karte, float PPm) 
            : base(Karte, PPm)
        {

        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            RectangleF MovedInnenBox = InnenBox.move(Box.Location);
            MovedInnenBox.Width -= WidthOff;
            MovedInnenBox.X += WidthOff;
            StarBox.Setup(MovedInnenBox);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            StarBox.Move(ToMove);
        }
        public override void Update()
        {
        }

        public override bool Visible()
        {
            return base.Visible()
                && TitelDarstellung.Existiert
                && Karte.Schreibname.Length > 0;
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
                return;

            WidthOff = Karte.HintergrundDarstellung.MarginLeft;
            if (!Karte.Kosten.IsEmpty)
                WidthOff += HintergrundDarstellung.Anker.X;
            WidthOff *= Faktor;

            SizeF size = new SizeF(InnenBox.Width - WidthOff, HintergrundDarstellung.MarginTop * Faktor);

            Text text = new Text(Karte.Schreibname, TitelDarstellung.FontMeasurer)
            {
                Alignment = 0f
            };
            FixedBox fixedBox = new FixedBox(size, text)
            {
                Alignment = new SizeF(0f, 0.5f)
            };
            StarBox = new StarBox(fixedBox,
                TitelDarstellung.TextFarbe.ToBrush(),
                TitelDarstellung.Farbe.ToBrush(),
                TitelDarstellung.Rand.mul(Faktor).ToPointF(),
                Stars);
        }
        public override void Draw(DrawContext con) => StarBox.Draw(con);
    }
}
