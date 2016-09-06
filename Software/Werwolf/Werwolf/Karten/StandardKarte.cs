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
    public class StandardKarte : WolfBox
    {
        public WolfTitel Titel { get; set; }
        public WolfText Text { get; set; }
        public WolfInfo Info { get; set; }

        private WolfBox[] WolfBoxs { get { return new WolfBox[] { Titel, Text, Info }; } }

        public override float getSpace()
        {
            return AussenBox.Size.Inhalt();
        }
        public override float getMin()
        {
            return AussenBox.Size.Width;
        }
        public override float getMax()
        {
            return getMin();
        }

        public StandardKarte(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
            Titel = new WolfTitel(Karte, ppm);
            Text = new WolfText(Karte, ppm);
            Info = new WolfInfo(Karte, ppm);
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            foreach (var item in WolfBoxs)
                if (item != null)
                    item.Karte = Karte;
            update();
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            foreach (var item in WolfBoxs)
                if (item != null && item.Visible())
                    item.OnPpmChanged();
            update();
        }

        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.Move(ToMove);
        }
        public override void update()
        {
            foreach (var item in WolfBoxs)
                if (item != null && item.Visible())
                    item.update();
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Size = AussenBox.Size;

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.setup(box);

            if (Text.Visible() && Info.Visible())
                Text.KorrigierUmInfo(Info.Kompositum.box.Height);
        }
        public override void draw(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(-1, -1);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            float top = Titel.Visible() ? Titel.Titel.Bottom : MovedInnenBox.Top;
            float bottom = MovedInnenBox.Bottom;
            if (Text.Visible())
                bottom = Text.OuterBox.Top + box.Location.Y;
            else if (Info.Visible())
                bottom = Info.Kompositum.Top;

            PointF PointOfInterest = new PointF(MovedAussenBoxCenter.X, (3 * top + bottom) / 4);

            if (HintergrundDarstellung.Existiert)
            {
                con.fillRectangle(HintergrundDarstellung.Farbe.ToBrush(), MovedInnenBox);
                con.DrawCenteredImage(Karte.Fraktion.HintergrundBild, MovedAussenBoxCenter, MovedInnenBox);
            }
            if (BildDarstellung.Existiert)
                con.DrawCenteredImage(Karte.HauptBild, PointOfInterest, MovedInnenBox);

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.draw(con);

            if (HintergrundDarstellung.Rand.Inhalt() > 0)
            {
                HintergrundDarstellung.MakeRandBild(ppm);
                con.drawImage(HintergrundDarstellung.RandBild, MovedAussenBox);
            }
        }
    }
}
