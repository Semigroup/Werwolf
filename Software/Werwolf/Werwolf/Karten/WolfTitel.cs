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
                Karte.Schreibname,
                TitelDarstellung.FontMeasurer,
                TitelDarstellung.Rand.Height * Faktor,
                TitelDarstellung.RandFarbe.ToPen(Faktor / 5),
                TitelDarstellung.Farbe.ToBrush());
            Update();
        }
        public override void OnPpmChanged()
        {
            base.OnKarteChanged();
            Update();
        }

        public override float Space => Titel.Space;
        public override float Min => Titel.Min;
        public override float Max => Titel.Max;

        public override void Update()
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

            this.Titel.SetArt(Karte.Fraktion.TitelArt);
            this.Titel.SetText(Karte.Schreibname, TitelDarstellung.FontMeasurer);
            this.Titel.RandHohe = TitelDarstellung.Rand.Height * Faktor;
            this.Titel.RandFarbe = TitelDarstellung.RandFarbe.ToPen(Faktor / 5);
            this.Titel.HintergrundFarbe = TitelDarstellung.Farbe.ToBrush();
            this.Titel.Scaling = Ppm / Faktor;
            Titel.Update();
        }
        public override bool Visible()
        {
            return base.Visible() 
                && TitelDarstellung.Existiert
                && Karte.Schreibname.Length > 0
                && !Titel.Empty();
        }
        public override void Setup(RectangleF box)
        {
            this.Box = box;
            Titel.Setup(InnenBox.move(box.Location).Inner(Faktor, Faktor));
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Titel.Move(ToMove);
        }

        public override void Draw(DrawContext con)
        {
            Titel.Draw(con);
        }
    }
}
