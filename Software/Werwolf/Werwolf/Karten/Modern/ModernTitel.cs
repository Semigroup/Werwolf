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

namespace Werwolf.Karten.Modern
{
    public class ModernTitel : WolfBox
    {
        private ShadowBox ShadowBox;

        public ModernTitel(Karte Karte, float PPm) : base(Karte, PPm)
        {

        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            RectangleF MovedInnenBox = InnenBox.move(Box.Location);
            ShadowBox.Setup(MovedInnenBox);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            ShadowBox.Move(ToMove);
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
                Alignment = 0.5f
            };
            FixedBox fixedBox = new FixedBox(size, text)
            {
                Alignment = new SizeF(0f, 0.5f)
            };
            ShadowBox = new ShadowBox(fixedBox,
                TitelDarstellung.TextFarbe.ToBrush(), 
                TitelDarstellung.Farbe.ToBrush(), 
                TitelDarstellung.Rand.mul(Faktor).ToPointF());
        }
        public override void Draw(DrawContext con) => ShadowBox.Draw(con);
    }
}
