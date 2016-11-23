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
    public class WonderGlobalesReich : WolfBox
    {
        DrawBox Text;
        DrawBox[] Stufen;
        /// <summary>
        /// Mindestabstand zwischen zwei ausbaustufen
        /// </summary>
        float abstand = 2;

        public WonderGlobalesReich(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte != null)
            {
                Text Text = new Text(Karte.Schreibname, Karte.TitelDarstellung.FontMeasurer);
                Text.alignment = 1;
                this.Text = Text.Colorize(TitelDarstellung.Farbe).Geometry(TitelDarstellung.Rand.mul(Faktor));
            }
            else
            {
                Text = null;
            }
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
        }
        public override void update()
        {
        }

        public override bool Visible()
        {
            return base.Visible();
        }

        public override void setup(RectangleF box)
        {

            this.box = AussenBox;
            this.box.Location = box.Location;

            Stufen = new DrawBox[Karte.Entwicklungen.Length];
            if (Stufen.Length > 0)
            {
                for (int i = 0; i < Stufen.Length; i++)
                {
                    Karte Ausbau = new Karte();
                    Karte.Entwicklungen[i].Assimilate(Ausbau);
                    Ausbau.BildDarstellung = Karte.Universe.BildDarstellungen["Schwarz-Weiß"];
                    Stufen[i] = new StandardKarte(Ausbau, ppm).Geometry(abstand * Faktor, 0);
                }
                foreach (var item in Stufen)
                    item.setup(0);
                float breite = Stufen.Map(x => x.Size.Width).Sum();
                float rest = this.HintergrundDarstellung.Size.Width * Faktor - breite;
                float part = rest / (Stufen.Length + 1);
                float hohe = (this.box.Height - Stufen[0].box.Height) / 2;
                PointF loc = new PointF(part, hohe);
                if (rest < 0)
                {
                    loc = new PointF(rest / 2, hohe);
                    part = 0;
                }
                foreach (var item in Stufen)
                {
                    item.Move(loc);
                    loc = loc.add(item.Size.Width + part, 0);
                }
            }
            Text.setup(this.box);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            foreach (var item in Stufen)
                item.Move(ToMove);
            Text.Move(ToMove);
        }

        public override void draw(DrawContext con)
        {
            foreach (var item in Stufen)
                item.draw(con);
            Text.draw(con);
        }
    }
}
