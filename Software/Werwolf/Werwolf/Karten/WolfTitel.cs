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
        public TitelProxy Titel { get; private set; }

        private string LastSchreibname;
        private Font LastFont;
        private Titel.Art LastTitelArt;
        private float LastRandHeight;
        private Color LastRandFarbe;
        private Color LastFarbe;
        private float LastPpm;

        public WolfTitel(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte != null && Titel == null)
                this.Titel = new TitelProxy(Karte.Fraktion.TitelArt,
                Karte.Schreibname, TitelDarstellung.FontMeasurer,
                TitelDarstellung.Rand.Height * Faktor,
                TitelDarstellung.RandFarbe.ToPen(Faktor / 5),
                TitelDarstellung.Farbe.ToBrush());
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
            if (karte == null ||
                (Karte.Schreibname.Equals(LastSchreibname)
                && TitelDarstellung.Font.Equals(LastFont)
                && Karte.Fraktion.TitelArt.Equals(LastTitelArt)
                && TitelDarstellung.Rand.Height.Equals(LastRandHeight)
                && TitelDarstellung.RandFarbe.Equals(LastRandFarbe)
                && TitelDarstellung.Farbe.Equals(LastFarbe)
                && Ppm.Equals(LastPpm)))
                return;

            LastSchreibname = Karte.Schreibname;
            LastFont = TitelDarstellung.Font;
            LastTitelArt = Karte.Fraktion.TitelArt;
            LastRandHeight = TitelDarstellung.Rand.Height;
            LastFarbe = TitelDarstellung.Farbe;
            LastRandFarbe = TitelDarstellung.RandFarbe;
            LastPpm = ppm;

            //Text t = new Text(Karte.Schreibname, TitelDarstellung.FontMeasurer);
            //t.alignment = 0.5f;
            this.Titel.SetArt(Karte.Fraktion.TitelArt);
            this.Titel.SetText(Karte.Schreibname, TitelDarstellung.FontMeasurer);
            this.Titel.RandHohe = TitelDarstellung.Rand.Height * Faktor;
            this.Titel.RandFarbe = TitelDarstellung.RandFarbe.ToPen(Faktor / 5);
            this.Titel.HintergrundFarbe = TitelDarstellung.Farbe.ToBrush();
            //this.Titel = new TitelProxy(Karte.Fraktion.TitelArt,
            //    Karte.Schreibname,TitelDarstellung.FontMeasurer,
            //    TitelDarstellung.Rand.Height * Faktor,
            //    TitelDarstellung.RandFarbe.ToPen(Faktor / 5),
            //    TitelDarstellung.Farbe.ToBrush());
            this.Titel.Scaling = Ppm / Faktor;
            Titel.update();
        }
        public override bool Visible()
        {
            return base.Visible() && TitelDarstellung.Existiert && Karte.Schreibname.Length > 0 && !Titel.Empty();
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            Titel.setup(InnenBox.move(box.Location).Inner(Faktor, Faktor));
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Titel.Move(ToMove);
        }

        public override void draw(DrawContext con)
        {
            Titel.draw(con);
        }
    }
}
