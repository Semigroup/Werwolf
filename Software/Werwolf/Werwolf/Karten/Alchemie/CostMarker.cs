using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Texts;
using Werwolf.Inhalt;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten.Alchemie
{
    public class CostMarker : WolfBox
    {
        private RectangleF CostBox;
        private TextBild Cost;

        public CostMarker(Karte Karte, float PPm) : base(Karte, PPm)
        {
        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            float size = Karte.HintergrundDarstellung.Anker.Y * Faktor;
            CostBox = new RectangleF(Box.Location, new SizeF(size, size));
            float d = Karte.HintergrundDarstellung.MarginTop + Karte.HintergrundDarstellung.Rand.Height;
            d = d * Faktor / 2 - size / 2;
            CostBox = CostBox.move(d, d);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            CostBox.move(ToMove);
        }
        public override void Update()
        {
        }

        public override bool Visible()
        {
            return base.Visible()
                && InfoDarstellung.Existiert
                && (Cost != null);
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
                return;

            var bilder = Karte.Kosten.GetTextBilder();
            Cost = bilder.FirstOrDefault();
        }

        public override void Draw(DrawContext con)
        {
            con.DrawImage(Cost.Image, CostBox);
        }
    }
}
