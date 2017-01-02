using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;

using Werwolf.Karten;
using Werwolf.Inhalt;

namespace Werwolf.Printing
{
    public class WolfSinglePaper : WolfBox
    {
        public iTextSharp.text.Rectangle PageSize { get; set; }
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
        public Color TrennlinienFarbe { get; set; }
        public bool Zwischenplatz { get; set; }
        public bool Swapped { get; set; }
        public bool TrennlinieVorne { get; set; }
        public bool TrennlinieHinten { get; set; }

        private List<DrawBox> Karten = new List<DrawBox>();

        private Size NumberOfCards = new Size();

        public WolfSinglePaper(Universe Universe, float ppm)
            : base(Universe.Karten.Standard, ppm)
        {
            this.PageSize = iTextSharp.text.PageSize.A4;
        }
        public WolfSinglePaper(Job Job)
            : this(Job.Universe, Job.Ppm)
        {
            this.Zwischenplatz = Job.Zwischenplatz;
            this.TrennlinienFarbe = Job.TrennlinienFarbe;
            this.TrennlinieVorne = Job.TrennlinieVorne;
            this.TrennlinieHinten = Job.TrennlinieHinten;
        }

        public override void update()
        {
        }

        public bool TryAdd(DrawBox Karte)
        {
            if (NumberOfCards.IsEmpty)
            {
                Karten.Add(Karte);
                Karte.setup(0);
                if (Karte.Size.Width > Seite.Width || Karte.Size.Height > Seite.Height)
                    PageSize = iTextSharp.text.PageSize.A3;
                SizeF n = Seite.Size.div(Karte.box.Size);
                NumberOfCards = new Size((int)Math.Floor(n.Width), (int)Math.Floor(n.Height));
                return true;
            }
            else if (Karten.Count < NumberOfCards.Height * NumberOfCards.Width)
            {
                Karten.Add(Karte);
                return true;
            }
            else
                return false;
        }

        public override float getMin()
        {
            return Seite.Width;
        }
        public override float getMax()
        {
            return Seite.Width;
        }
        public override float getSpace()
        {
            return Seite.Width * Seite.Height;
        }

        public override void setup(RectangleF box)
        {
            if (Karten.Count == 0)
                return;

            foreach (var item in Karten)
                item.setup(box);

            SizeF karte = Karten.First().box.Size;

            SizeF n = Seite.Size.div(karte);
            NumberOfCards = new Size((int)Math.Floor(n.Width), (int)Math.Floor(n.Height));
            SizeF Offset;
            SizeF Zwischen;
            if (Zwischenplatz)
            {
                Offset = Seite.Size.sub(karte.mul(NumberOfCards));
                Offset = Offset.div(new SizeF(1, 1).add(NumberOfCards));
                Zwischen = Offset;
            }
            else
            {
                Zwischen = new SizeF();
                Offset = Seite.Size.sub(karte.mul(NumberOfCards));
                Offset = Offset.div(2);
            }
            SizeF Platz = karte.add(Zwischen);

            this.box = Seite;
            if (Swapped)
            {
                Offset.Width = Seite.Width - karte.Width - Offset.Width;
                Platz.Width = -Platz.Width;
            }

            IEnumerator<DrawBox> db = Karten.GetEnumerator();
            for (int y = 0; y < NumberOfCards.Height; y++)
                for (int x = 0; x < NumberOfCards.Width; x++)
                    if (db.MoveNext())
                    {
                        PointF off = Offset.ToPointF();
                        off = off.add(Platz.mul(x, y).ToPointF());
                        db.Current.Move(off);
                    }
        }

        public override void draw(DrawContext con)
        {
            foreach (var item in Karten)
            {
                item.draw(con);
                if ((TrennlinieVorne && !Swapped) || (TrennlinieHinten && Swapped))
                    con.drawRectangle(new Pen(TrennlinienFarbe, 0.35f), item.box);
            }
        }

        /// <summary>
        /// Size in DrawBox Maß
        /// </summary>
        /// <param name="Size"></param>
        /// <returns></returns>
        public static Size GetNumberOfCards(SizeF Size)
        {
            SizeF Seite = (4).DinA(true);
            Seite = Seite.mul(Faktor);

            if (Size.Width > Seite.Width || Size.Height > Seite.Height)
            {
                Seite = (3).DinA(true);
                Seite = Seite.mul(Faktor);
            }
            SizeF n = Seite.div(Size);
            return new Size((int)Math.Floor(n.Width), (int)Math.Floor(n.Height));
        }
    }
}
