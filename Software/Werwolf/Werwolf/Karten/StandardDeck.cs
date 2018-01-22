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
                Text t = new Text("\\d" + item.Value.Schreibname, new FontGraphicsMeasurer("Consolas", 22));
                foreach (var karte in frak)
                {
                    if (karte.Value > 0)
                    {
                        t.AddAbsatz();
                        empty = false;
                    }
                    if (karte.Value > 1)
                        t.AddWort(karte.Value + "x ");
                    if (karte.Value > 0)
                    {
                        t.AddRegex(karte.Key.Schreibname);
                        //Text.add(new StandardKarte(item.Key, Ppm));
                    }
                }
                if (!empty)
                    cs.Add(t.Geometry(10));
            }
            Text = cs;
        }

        public override void Update()
        {
            Text.Update();
        }

        public override void Setup(RectangleF box)
        {
            Text.Setup(box);
        }
        public override void Draw(DrawContext con)
        {
            //con.fillRectangle(Brushes.White, 0,0,1000,1000);
            Text.Draw(con);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Text.Move(ToMove);
        }

        public override float Max => Text.Max;
        public override float Min => Text.Min;
        public override float Space => Text.Space;
    }
}
