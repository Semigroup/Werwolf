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
    public class WonderAusbauStufe : WolfBox
    {
        public WonderBalken Balken { get; set; }
        public WonderAusbauKostenFeld Kosten { get; set; }
        public WonderEffekt WonderEffekt { get; set; }

        private WolfBox[] WolfBoxs
        {
            get
            {
                if (Karte != null)
                    return new WolfBox[] { Balken, WonderEffekt };
                else
                    return new WolfBox[] { };
            }
        }

        public WonderAusbauStufe(Karte AusbauKarte, float Ppm)
            : base(AusbauKarte, Ppm)
        {
            Balken = new WonderBalken(AusbauKarte, ppm);
            Kosten = new WonderAusbauKostenFeld(AusbauKarte, ppm);
            WonderEffekt = new WonderEffekt(AusbauKarte, ppm);
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
                item.OnPpmChanged();
            update();
        }

        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.Move(ToMove);
            Kosten.Move(ToMove);
        }
        public override void update()
        {
            foreach (var item in WolfBoxs)
                if (item != null)
                    item.update();
        }

        public override void setup(RectangleF box)
        {
            RectangleF MovedInnenBox = InnenBox.move(box.Location);
            this.box = box;
            this.box.Size = AussenBox.Size;

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.setup(box);

            Kosten.Karte = Karte;
            Kosten.Ppm = ppm;
            if (Kosten.Visible())
            {
                Kosten.setup(box);
                float rest = Faktor * HintergrundDarstellung.Anker.X - Kosten.Size.Width;
                Kosten.SetLot(new PointF(MovedInnenBox.Left + rest / 2,
                    MovedInnenBox.Top + HintergrundDarstellung.Anker.Y * Faktor));
            }
        }
        public override void draw(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(0, 0);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.draw(con);
            if (Kosten.Visible())
                Kosten.draw(con);
            if (HintergrundDarstellung.Rand.Inhalt() > 0)
            {
                HintergrundDarstellung.MakeRandBild(ppm);
                con.drawImage(HintergrundDarstellung.RandBild, MovedAussenBox);
            }
        }
    }
}
