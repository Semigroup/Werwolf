using System;
using System.Linq;
using System.Drawing;
using Assistment.Extensions;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.Geometries.Extensions;

namespace Werwolf.Karten
{
    public class WonderReich : WolfBox
    {
        TextBild[,] AusbauStufen;

        WonderProduktionsFeld Produktion;
        DrawBox Text;
        DrawBox[] Stufen;
        /// <summary>
        /// Mindestabstand zwischen zwei ausbaustufen in mm
        /// </summary>
        float abstand = 2;

        public WonderReich(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }

        public override void OnKarteChanged()
        {
            if (Produktion == null)
                Produktion = new WonderProduktionsFeld(null, Ppm);

            base.OnKarteChanged();
            if (Karte != null)
            {
                if (AusbauStufen == null)
                {
                    AusbauStufen = new TextBild[4, 4];
                    for (int i = 0; i < 4; i++)
                        for (int j = 0; j < 4; j++)
                            AusbauStufen[i, j] = Karte.Universe.TextBilder["Ausbaustufe " + (i + 1) + "/" + (j + 1)];
                }

                Produktion.Karte = Karte.Basen.Length > 0 ? Karte.Basen[0] : null;

                Text Text = new Text(Karte.Schreibname, Karte.TitelDarstellung.FontMeasurer);
                Text.Alignment = 1;
                this.Text = Text.Colorize(TitelDarstellung.Farbe).Geometry(TitelDarstellung.Rand.mul(Faktor));
                if (Karte.MeineAufgaben.Anzahl > 0)
                {
                    Text t2 = Karte.MeineAufgaben.ProduceTexts(Karte.TextDarstellung.EffektFontMeasurer)[0];
                    t2.Alignment = 1;
                    this.Text *= t2;
                }
            }
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            Produktion.Ppm = ppm;
        }
        public override void Update()
        {
            Produktion.Update();
        }

        public override bool Visible()
        {
            return base.Visible();
        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;
            RectangleF MovedInnenBox = InnenBox.move(box.Location);

            if (Produktion.Visible())
                Produktion.Setup(MovedInnenBox);

            Stufen = new DrawBox[Karte.Entwicklungen.Length];
            if (Stufen.Length > 0)
            {
                Stufen.CountMap(i => new WonderAusbauStufe(Karte.Entwicklungen[i], ppm).Geometry(abstand * Faktor, 0));
                foreach (var item in Stufen)
                    item.Setup(0);
                float breite = Stufen.Map(x => x.Size.Width).Sum();
                float rest = MovedInnenBox.Width - breite;
                float part = rest / (Stufen.Length + 1);
                float hohe = MovedInnenBox.Height - 23 * Faktor;
                PointF loc = new PointF(part, hohe);
                PointF Rand = HintergrundDarstellung.Rand.ToPointF().mul(Faktor);
                if (rest < 0)
                {
                    loc = new PointF(rest / 2, hohe);
                    part = 0;
                }
                foreach (var item in Stufen)
                {
                    item.Move(loc);
                    item.Move(Rand);
                    loc = loc.add(item.Size.Width + part, 0);
                }
            }
            Text.Setup(MovedInnenBox);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Produktion.Move(ToMove);
            foreach (var item in Stufen)
                item.Move(ToMove);
            Text.Move(ToMove);
        }

        public override void Draw(DrawContext con)
        {
            if (Produktion.Visible())
                Produktion.Draw(con);

            int n = Stufen.Length;
            for (int i = 0; i < n; i++)
                Stufen[i].Draw(con);
            for (int i = 0; i < n; i++)
            {
                WonderAusbauStufe Stufe = (Stufen[i] as GeometryBox).DrawBox as WonderAusbauStufe;
                TextBild tb = AusbauStufen[i, n - 1];
                RectangleF r = new RectangleF(Stufe.Location, new SizeF());
                r.Height = 20 * Faktor;
                r.Width = tb.Size.ratio() * r.Height;
                r = r.move(Stufe.Box.Width - r.Width / 2, -r.Height / 2);
                r.X = Math.Min(r.X, Box.Location.X + AussenBox.Width - r.Width * 0.7f);
                //if (i + 1 < n)
                //    r.X = Math.Min(r.X, Stufen[i + 1].Left - r.Width);

                using (Image img = tb.Image)
                    con.DrawImage(img, r);
            }

            Text.Draw(con);
        }
    }
}
