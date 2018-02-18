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
                Text.Alignment = 1;
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
        public override void Update()
        {
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

            Stufen = new DrawBox[Karte.Entwicklungen.Length];
            if (Stufen.Length > 0)
            {
                for (int i = 0; i < Stufen.Length; i++)
                {
                    Karte Ausbau = new Karte();
                    Karte.Entwicklungen[i].Assimilate(Ausbau);
                    //Ausbau.HintergrundDarstellung = Karte.Universe.HintergrundDarstellungen["Globaler_Ausbau"];
                    Stufen[i] = new StandardKarte(Ausbau, ppm).Geometry(abstand * Faktor, 0);
                    //Bitmap Image = Ausbau.GetImage(ppm);
                    //Image.Filter(HintergrundDarstellung.Farbe, HintergrundDarstellung.Farbe.A/255f);
                    //Stufen[i] = new ImageBox(Ausbau.HintergrundDarstellung.Size.Width * Faktor, Image)
                    //    .Geometry(abstand * Faktor, 0);
                }
                foreach (var item in Stufen)
                    item.Setup(0);
                float breite = Stufen.Map(x => x.Size.Width).Sum();
                float rest = MovedInnenBox.Width - breite;
                float part = rest / (Stufen.Length + 1);
                float hohe = (MovedInnenBox.Height - Stufen[0].Box.Height) / 2;
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
            foreach (var item in Stufen)
                item.Move(ToMove);
            Text.Move(ToMove);
        }

        public override void Draw(DrawContext con)
        {
            foreach (var item in Stufen)
            {
                //item.draw(con);
                StandardKarte sk = (item as GeometryBox).DrawBox as StandardKarte;
                Bitmap b = sk.Karte.GetImage(ppm, true);
                b.Filter(HintergrundDarstellung.Farbe, HintergrundDarstellung.Farbe.A / 255f);
                con.DrawImage(b, sk.Box);
                sk.drawRand(con);
            }
            Text.Draw(con);
        }
    }
}
