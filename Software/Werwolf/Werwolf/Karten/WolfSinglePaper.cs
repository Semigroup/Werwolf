using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;

using Werwolf.Karten;
using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class WolfSinglePaper : WolfBox
    {
        public iTextSharp.text.Rectangle PageSize { get; set; }
        /// <summary>
        /// in mm
        /// </summary>
        public SizeF Zwischenplatz { get; set; }
        /// <summary>
        /// in Pixel
        /// </summary>
        public RectangleF Seite
        {
            get
            {
                return new RectangleF(0, 0,
                    PageSize.Width / DrawContextDocument.factor, PageSize.Height / DrawContextDocument.factor);
            }
        }
        public Color HintergrundFarbe { get; set; }
        public Color TrennlinienFarbe { get; set; }

        private List<DrawBox> Karten = new List<DrawBox>();

        private Size NumberOfCards;

        public WolfSinglePaper(Universe Universe, float ppm)
            : base(Universe.Karten.Standard, ppm)
        {
            this.PageSize = iTextSharp.text.PageSize.A4;
        }

        public override void update()
        {
        }

        public bool TryAdd(DrawBox Karte)
        {
            if (Karten.Count < 9)
            {
                Karten.Add(Karte);
                return true;
            }
            else
                return false;
        }

        public override void setup(RectangleF box)
        {
            if (Karten.Count == 0)
                return;

            foreach (var item in Karten)
                item.setup(box);

            SizeF zwischen = Zwischenplatz.mul(ppm);
            SizeF karte = Karten.First().box.Size;
            SizeF Platz = karte.add(zwischen);
            SizeF n = Seite.Size.add(zwischen).div(Platz);
            NumberOfCards = new Size((int)Math.Floor(n.Width), (int)Math.Floor(n.Height));

            this.box = Seite;
            SizeF Rest = Zwischenplatz;
            Rest = Rest.mul(2);
            Rest = Rest.add(karte.mul(3));
            Rest = Seite.Size.sub(Rest);
            Rest = Rest.div(2);

            IEnumerator<DrawBox> db = Karten.GetEnumerator();

            for (int y = 0; y < NumberOfCards.Height; y++)
                for (int x = 0; x < NumberOfCards.Width; x++)
                    if (db.MoveNext())
                    {
                        PointF off = Rest.ToPointF();
                        off = off.add(Platz.mul(x, y).ToPointF());
                        db.Current.Move(off);
                    }
        }

        public override void draw(DrawContext con)
        {
            foreach (var item in Karten)
                item.draw(con);
        }
    }
}
