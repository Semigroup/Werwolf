using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Texts;
using Werwolf.Inhalt;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

namespace Werwolf.Karten.Alchemie
{
    public class StarTitle : WolfBox
    {
        private StarBox StarBox;
        public int Stars { get; set; } = 4;

        public StarTitle(Karte Karte, float PPm) : base(Karte, PPm)
        {

        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            RectangleF MovedInnenBox = InnenBox.move(Box.Location);
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

            SizeF size = new SizeF(InnenBox.Width, HintergrundDarstellung.MarginTop * Faktor);

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
