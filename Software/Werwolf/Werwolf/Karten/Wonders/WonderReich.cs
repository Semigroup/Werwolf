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
    public class WonderReich : WolfBox
    {
        Karte SubKarte = new Karte();
        WonderBalken Balken;
        WonderEffekt Effekt;
        DrawBox Text;
        DrawBox[] Stufen;
        /// <summary>
        /// Mindestabstand zwischen zwei ausbaustufen
        /// </summary>
        float abstand = 2;

        public WonderReich(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte != null)
            {
                if (Karte.Basen.Length > 0)
                {
                    Karte.Basen[0].Assimilate(SubKarte);
                    SubKarte.Schreibname = "";
                    SubKarte.HintergrundDarstellung = SubKarte.HintergrundDarstellung.Clone() as HintergrundDarstellung;
                    SubKarte.HintergrundDarstellung.Rand = new SizeF();

                    if (Balken == null)
                    {
                        Balken = new WonderBalken(SubKarte, Ppm);
                        Effekt = new WonderEffekt(SubKarte, Ppm);
                    }
                    else
                    {
                        Balken.Karte = SubKarte;
                        Effekt.Karte = SubKarte;
                    }
                    Text Text = new Text(Karte.Schreibname, Karte.TitelDarstellung.FontMeasurer);
                    Text.alignment = 1;
                    this.Text = Text.Colorize(TitelDarstellung.Farbe).Geometry(TitelDarstellung.Rand.mul(Faktor));
                    if (Karte.MeineAufgaben.Anzahl > 0)
                    {
                        Text t2 = Karte.MeineAufgaben.ProduceTexts(Karte.TextDarstellung.EffektFontMeasurer)[0];
                        t2.alignment = 1;
                        this.Text *= t2;
                    }
                }
            }
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            Balken.Ppm = ppm;
            Effekt.Ppm = ppm;
        }
        public override void update()
        {
            if (Balken != null)
            {
                Balken.update();
                Effekt.update();
            }
        }

        public override bool Visible()
        {
            return base.Visible();
        }

        public override void setup(RectangleF box)
        {
            if (SubKarte.HintergrundDarstellung == null)
                return;

            this.box = AussenBox;
            this.box.Location = box.Location;

            RectangleF SmallBox = new RectangleF(box.Location, SubKarte.HintergrundDarstellung.Size).mul(Faktor);
            SmallBox = SmallBox.move(HintergrundDarstellung.Anker.mul(Faktor));
            Balken.setup(SmallBox);
            Effekt.setup(SmallBox);
            Stufen = new DrawBox[Karte.Entwicklungen.Length];
            if (Stufen.Length > 0)
            {
                Stufen.CountMap(i => new WonderAusbauStufe(Karte.Entwicklungen[i], ppm).Geometry(abstand * Faktor, 0));
                foreach (var item in Stufen)
                    item.setup(0);
                float breite = Stufen.Map(x => x.Size.Width).Sum();
                float rest = this.HintergrundDarstellung.Size.Width * Faktor - breite;
                float part = rest / (Stufen.Length + 1);
                float hohe = (this.HintergrundDarstellung.Size.Height - 23) * Faktor;
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
            if (SubKarte.HintergrundDarstellung == null)
                return;

            base.Move(ToMove);
            Balken.Move(ToMove);
            Effekt.Move(ToMove);
            foreach (var item in Stufen)
                item.Move(ToMove);
            Text.Move(ToMove);
        }

        public override void draw(DrawContext con)
        {
            if (SubKarte.HintergrundDarstellung == null)
                return;

            Balken.draw(con);
            Effekt.draw(con);
            foreach (var item in Stufen)
                item.draw(con);
            Text.draw(con);
        }
    }
}
