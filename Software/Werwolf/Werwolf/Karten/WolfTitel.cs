using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class WolfTitel : WolfBox
    {
        public Titel Titel { get; private set; }

        public WolfTitel(Karte Karte,float Ppm)
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
            base.OnKarteChanged();
            update();
        }

        public override float getSpace()
        {
            return Titel.getSpace();
        }
        public override float getMin()
        {
            return Titel.getMin();
        }
        public override float getMax()
        {
            return Titel.getMax();
        }

        public override void update()
        {
            Text t = new Text(Karte.Schreibname, TitelDarstellung.FontMeasurer);
            t.alignment = 0.5f;
            this.Titel = Titel.GetTitel(Karte.Fraktion.TitelArt,
                t,
                TitelDarstellung.Rand.Height * Faktor,
                TitelDarstellung.RandFarbe.ToPen(Faktor / 5),
                TitelDarstellung.Farbe.ToBrush());
            this.Titel.Scaling = Ppm / Faktor;
            Titel.update();
        }
        public override bool Visible()
        {
            return base.Visible() && TitelDarstellung.Existiert && Karte.Name.Length > 0;
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            Titel.setup(InnenBox.move(box.Location).Inner(Faktor, Faktor));
        }

        public override void draw(DrawContext con)
        {
            Titel.draw(con);
        }
    }
}
