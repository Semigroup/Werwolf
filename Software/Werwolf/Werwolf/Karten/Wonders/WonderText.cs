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
    public class WonderText : WolfBox
    {
        private DrawBox Text;
        private FixedBox FixedBox;
        private Whitespace WhiteSpace = new Whitespace(0, 0, true);
        private DrawBox Spieleranzahl;

        private FontGraphicsMeasurer LastFont;
        private FontGraphicsMeasurer LastFlavourFont;
        private string LastAufgabe;

        private RectangleF MovedAussenBox;

        private float EntwicklungsBreite;

        public WonderText(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override bool Visible()
        {
            return base.Visible() && Karte.MeineAufgaben.Anzahl > 0;
        }
        public override void Update()
        {
        }
        public void SetEntwicklungsBreite(float Breite)
        {
            this.EntwicklungsBreite = Breite;
        }
        public override void Setup(RectangleF box)
        {
            this.Box = box;
            this.Box.Size = AussenBox.Size;

            RectangleF MovedInnenBox = InnenBox.move(box.Location);
            MovedInnenBox = MovedInnenBox.move(HintergrundDarstellung.Anker.mul(Faktor));
            MovedInnenBox.Size = MovedInnenBox.Size.sub(HintergrundDarstellung.Anker.mul(Faktor).ToSize());
            MovedInnenBox.Width -= EntwicklungsBreite;

            string aufgabe = Karte.MeineAufgaben.ToString();
            if (!(LastAufgabe == aufgabe
                && LastFont == TextDarstellung.FontMeasurer))
            {
                LastAufgabe = aufgabe;
                LastFont = TextDarstellung.FontMeasurer as FontGraphicsMeasurer;
                LastFlavourFont = LastFont * 0.8f;
                Text Text1 = Karte.MeineAufgaben.ProduceTexts(LastFont)[0];
                if (Karte.MeineAufgaben.Anzahl == 1)
                    Text = Text1;
                else if (Karte.MeineAufgaben.Anzahl == 2)
                {
                    Text Text2 = Karte.MeineAufgaben.ProduceTexts(LastFlavourFont)[1];
                    Text2.Alignment = 0.5f;
                    Text = new CString() { Text1, WhiteSpace, Text2 };
                }
                else if (Karte.MeineAufgaben.Anzahl > 2)
                {
                    List<string> lines = Karte.MeineAufgaben.GetLines();
                    Text anekdote = "„" + lines[1] +"“";
                    Text autor = "—" + lines[2];
                    anekdote.Alignment = autor.Alignment = 0.5f;
                    Text = new CString() { Text1, WhiteSpace, anekdote, WhiteSpace, autor };
                }
            }

            WhiteSpace.Box.Width = MovedInnenBox.Width - TextDarstellung.Rand.Width * Faktor * 2;
            WhiteSpace.Box.Height = Faktor;

            DrawBox TextBox = Text.Geometry(TextDarstellung.Rand.mul(Faktor));
            FixedBox = new FixedBox(MovedInnenBox.Size, TextBox);
            FixedBox.Alignment = new SizeF(0.5f, 1);
            FixedBox.Setup(MovedInnenBox);

            MovedAussenBox = AussenBox.move(box.Location);

            Text SaT = Karte.Gesinnung.GetText(InfoDarstellung.FontMeasurer);
            if (SaT != null)
            {
                Spieleranzahl = SaT
                .Geometry(InfoDarstellung.Rand.mul(Faktor))
                .Colorize(InfoDarstellung.Farbe.ToBrush());
                Spieleranzahl.Setup(MovedInnenBox);
                Spieleranzahl.Bottom = Text.Top;
                Spieleranzahl.Left = MovedInnenBox.Left;
            }
            else
                Spieleranzahl = null;

        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            FixedBox.Move(ToMove);
            MovedAussenBox = MovedAussenBox.move(ToMove);
            if (Spieleranzahl != null)
                Spieleranzahl.Move(ToMove);
        }
        public override void Draw(DrawContext con)
        {
            float off = Text.Top - MovedAussenBox.Top;
            MovedAussenBox.Height -= off;
            MovedAussenBox.Y += off;
            con.FillRectangle(TextDarstellung.Farbe.ToBrush(), MovedAussenBox);
            FixedBox.Draw(con);
            if (Spieleranzahl != null)
                Spieleranzahl.Draw(con);
        }
    }
}
