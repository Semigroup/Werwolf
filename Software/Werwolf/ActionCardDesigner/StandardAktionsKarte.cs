using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Werwolf.Inhalt;
using Werwolf.Karten;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace ActionCardDesigner
{
    public class StandardAktionsKarte : StandardKarte
    {
        private Text[] Texts;
        AktionsHeader ah;

        public StandardAktionsKarte(AktionsKarte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }
        public override void BuildUp()
        {
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte != null)
            {
                Texts = Karte.MeineAufgaben.ProduceTexts(Karte.TextDarstellung.FontMeasurer);
                ah = new AktionsHeader(Karte as AktionsKarte, Ppm);
            }
        }

        public override void setup(System.Drawing.RectangleF box)
        {
            this.box = box;
            this.box.Size = AussenBox.Size;
            ah.setup(box);
            RectangleF TextBox = InnenBox.move(0, ah.box.Height);
            foreach (var item in Texts)
            {
                item.setup(TextBox);
                TextBox = TextBox.move(0, item.box.Height);
            }
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            ah.Move(ToMove);
            foreach (var item in Texts)
                item.Move(ToMove);
        }
        public override void draw(DrawContext con)
        {
            if (Karte == null)
                return;

            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(-1, -1);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();


            if (HintergrundDarstellung.Existiert)
            {
                con.fillRectangle(HintergrundDarstellung.Farbe.ToBrush(), MovedInnenBox);
                con.DrawCenteredImage(Karte.Fraktion.HintergrundBild, MovedAussenBoxCenter, MovedInnenBox);
            }

            ah.draw(con);
            foreach (var item in Texts)
            {
                con.drawLine(TextDarstellung.RandFarbe.ToPen(1), InnenBox.Left, item.Top, InnenBox.Right, item.Top);
                item.draw(con);
            }

            if (HintergrundDarstellung.Rand.Inhalt() > 0)
            {
                HintergrundDarstellung.MakeRandBild(ppm);
                con.drawImage(HintergrundDarstellung.RandBild, MovedAussenBox);
            }
        }
    }
    public class StandardAktionsDeck : StandardDeck
    {
        private AktionsDeck deck;
        public AktionsDeck AktionsDeck
        {
            get { return deck; }
            set { deck = value; }
        }

        public StandardAktionsDeck(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override void OnKarteChanged()
        {
            if (deck == null)
                return;
            CString cs = new CString();
            foreach (var item in deck.Universe.Fraktionen)
            {
                IEnumerable<KeyValuePair<AktionsKarte, int>> frak = deck.Karten.Where(x => x.Key.Fraktion == item.Value);
                bool empty = true;
                Text t = new Text("\\d" + item.Value.Schreibname, new FontGraphicsMeasurer("Calibri", 22));
                foreach (var karte in frak)
                {
                    if (karte.Value > 0)
                    {
                        t.addAbsatz();
                        empty = false;
                    }
                    if (karte.Value > 1)
                        t.addWort(karte.Value + "x ");
                    if (karte.Value > 0)
                    {
                        t.addRegex(karte.Key.Schreibname);
                        //Text.add(new StandardKarte(item.Key, Ppm));
                    }
                }
                if (!empty)
                    cs.add(t.Geometry(10));
            }
            Text = cs;
        }
    }
}
