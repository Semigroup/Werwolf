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
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            update();
        }

        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
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
            this.Titel = new WolfTitel(Karte, Ppm);
            this.Text = new WolfText(Karte, Ppm);
            this.Info = new WolfInfo(Karte, Ppm);
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Size = AussenBox.Size;

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.setup(box);

            Text.KorrigierUmInfo(Info.Kompositum.box.Height);
        }
        public override void draw(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();
            PointF PointOfInterest = new PointF(MovedAussenBoxCenter.X,
                (Text.OuterBox.Top + box.Location.Y + Titel.Titel.Bottom) / 2);

            if (Karte.Fraktion.Bild != null)
                con.DrawCenteredImage(Karte.Fraktion.Bild, MovedAussenBoxCenter, MovedInnenBox);
            if (Karte.Bild != null)
                con.DrawCenteredImage(Karte.Bild, PointOfInterest, MovedInnenBox);

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.draw(con);

            HintergrundDarstellung.MakeRandBild(ppm);
            con.drawImage(HintergrundDarstellung.RandBild, MovedAussenBox);//,MovedAussenBox

            //con.fillEllipse(Brushes.Red, new RectangleF(PointOfInterest.sub(10, 10), new SizeF(20, 20)));
        }
    }
}
