using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Texts;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class StandardDeck : WolfBox
    {
        private Deck deck;
        public Deck Deck
        {
            get { return deck; }
            set { deck = value; }
        }

        private Text Text;

        public StandardDeck(Karte Karte, float ppm)
            : base(Karte, ppm)
        {
        }

        public override void OnKarteChanged()
        {
            if (Deck == null)
                return;

            Text = new Text("", new FontMeasurer("Calibri", 11));
            foreach (var item in Deck.Karten)
            {
                if (item.Value > 1)
                    Text.addWort(item.Value + "x");
                if (item.Value > 0)
                    Text.add(new StandardKarte(item.Key, Ppm));
            }
        }

        public override void update()
        {
            Text.update();
        }

        public override void setup(RectangleF box)
        {
            Text.setup(box);
        }
        public override void draw(DrawContext con)
        {
            Text.draw(con);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Text.Move(ToMove);
        }

        public override float getMax()
        {
            return Text.getMax();
        }
        public override float getMin()
        {
            return Text.getMin();
        }
        public override float getSpace()
        {
            return Text.getSpace();
        }
    }
}
