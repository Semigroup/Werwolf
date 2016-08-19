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

        public WolfTitel(Karte Karte) : base(Karte)
        {
        }

        public override void OnKarteChanged()
        {
            Text t = new Text();
            t.preferedFont = TitelDarstellung.FontMeasurer;
            t.addRegex(Karte.Schreibname);
            this.Titel = Titel.GetTitel(Karte.Fraktion.TitelArt, 
                t, 
                TitelDarstellung.Rand.Height,
                TitelDarstellung.RandFarbe.ToPen(Faktor / 5),
                TitelDarstellung.Farbe.ToBrush());
            this.Titel.Scaling = Ppm;
        }
        public override void OnPpmChanged()
        {
            this.Titel.Scaling = Ppm;
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
            OnKarteChanged();
            OnPpmChanged();
            Titel.update();
        }

        public override void setup(System.Drawing.RectangleF box)
        {
            Titel.setup(box);
        }

        public override void draw(Assistment.Texts.DrawContext con)
        {
            Titel.draw(con);
        }
    }
}
