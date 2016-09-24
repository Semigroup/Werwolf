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

        protected DrawBox Text;

        public StandardDeck(Karte Karte, float ppm)
            : base(Karte, ppm)
        {
        }

        public override void OnKarteChanged()
        {
            if (deck == null)
                return;
            CString cs = new CString();
            foreach (var item in deck.Universe.Fraktionen)
            {
                IEnumerable<KeyValuePair<Karte, int>> frak = deck.Karten.Where(x => x.Key.Fraktion == item.Value);
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
