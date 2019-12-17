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
    public class GoldBox : WolfBox
    {
        private DrawBox DrawBox;
        private RectangleF BGImageRegion;

        public GoldBox(Karte Karte, float PPm) : base(Karte, PPm)
        {

        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            BGImageRegion = new RectangleF(InfoDarstellung.Position2, InfoDarstellung.Rand2).mul(Faktor).move(box.Location);
            DrawBox.Setup(BGImageRegion.Inner(InfoDarstellung.Grosse2.mul(Faktor)));
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            DrawBox.Move(ToMove);
        }
        public override void Update()
        {
        }

        public override bool Visible()
        {
            return base.Visible()
                && LayoutDarstellung.KostenFeld != null
                && Karte.Geldkosten.Length > 0
                && DrawBox != null;
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
                return;

            BGImageRegion = new RectangleF(InfoDarstellung.Position2, InfoDarstellung.Rand2).mul(Faktor);
            Word word = new Word(
                Karte.Geldkosten.ToString(),
                InfoDarstellung.TextFarbe2.ToBrush(),
                InfoDarstellung.FontMeasurer2,
                0, null);
            Text text = new Text()
            {
                Alignment = 1,
            };
            text.Add(word);
            this.DrawBox = text;
        }
        public override void Draw(DrawContext con)
        {
            con.DrawImage(LayoutDarstellung.KostenFeld.Image, BGImageRegion);
            DrawBox.Draw(con);
        }
    }
}
