using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;

using Werwolf.Karten;
using Werwolf.Inhalt;
using System.Drawing.Imaging;
using System.IO;

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
        public bool FillUpRemainder { get; set; }

        private List<DrawBox> Karten = new List<DrawBox>();
        private List<DrawBox> ToPrint = new List<DrawBox>();

        private Size NumberOfCards = new Size();

        public string FileName { get; set; }

        public WolfSinglePaper(Universe Universe, float ppm, string FileName)
            : base(Universe.Karten.Standard, ppm)
        {
            this.PageSize = iTextSharp.text.PageSize.A4;
            this.FillUpRemainder = true;
            this.FileName = FileName;
        }
        public WolfSinglePaper(Job Job, string FileName)
            : this(Job.Universe, Job.Ppm, FileName)
        {
            this.Zwischenplatz = Job.Zwischenplatz;
            this.TrennlinienFarbe = Job.TrennlinienFarbe;
            this.TrennlinieVorne = Job.TrennlinieVorne;
            this.TrennlinieHinten = Job.TrennlinieHinten;
        }

        public override void Update()
        {
        }

        public bool TryAdd(DrawBox Karte)
        {
            if (NumberOfCards.IsEmpty)
            {
                Karten.Add(Karte);
                Karte.Setup(0);
                if (Karte.Size.Width > Seite.Width || Karte.Size.Height > Seite.Height)
                    PageSize = iTextSharp.text.PageSize.A3;
                SizeF n = Seite.Size.div(Karte.Box.Size);
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

        public override float Min => Seite.Width;
        public override float Max => Seite.Width;
        public override float Space => Seite.Width * Seite.Height;

        public override void Setup(RectangleF box)
        {
            if (Karten.Count == 0)
                return;

            foreach (var item in Karten)
                item.Setup(box);

            SizeF karte = Karten.First().Box.Size;

            SizeF n = Seite.Size.div(karte);
            NumberOfCards = new Size((int)Math.Floor(n.Width), (int)Math.Floor(n.Height));

            ToPrint = new List<DrawBox>(Karten);
            if (FillUpRemainder)
                for (int i = Karten.Count; i < NumberOfCards.Width * NumberOfCards.Height; i++)
                    ToPrint.Add(new Whitespace(karte.Width, karte.Height, false));

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

            this.Box = Seite;
            if (Swapped)
            {
                Offset.Width = Seite.Width - karte.Width - Offset.Width;
                Platz.Width = -Platz.Width;
            }

            IEnumerator<DrawBox> db = ToPrint.GetEnumerator();
            for (int y = 0; y < NumberOfCards.Height; y++)
                for (int x = 0; x < NumberOfCards.Width; x++)
                    if (db.MoveNext())
                    {
                        PointF off = Offset.ToPointF();
                        off = off.add(Platz.mul(x, y).ToPointF());
                        db.Current.Move(off);
                    }
        }

        public override void Draw(DrawContext con)
        {
            float top = 1;
            float left = 0;
            float bottom = Seite.Height - 1;
            float right = Seite.Width;

            bool linie = (TrennlinieVorne && !Swapped) || (TrennlinieHinten && Swapped);
            Pen LinePen = new Pen(TrennlinienFarbe, 0.35f);
            if (linie)
                foreach (var item in ToPrint)
                {
                    float x1 = item.Box.Left - 10 * Faktor;
                    x1 = Math.Max(x1, left);
                    float x2 = item.Box.Right + 10 * Faktor;
                    x2 = Math.Min(x2, right);
                    float y1 = item.Box.Top - 10 * Faktor;
                    y1 = Math.Max(top, y1);
                    float y2 = item.Box.Bottom + 10 * Faktor;
                    y2 = Math.Min(bottom, y2);
                    con.DrawLine(LinePen, new PointF(x1, top), new PointF(x1, bottom));
                    con.DrawLine(LinePen, new PointF(x2, top), new PointF(x2, bottom));
                    con.DrawLine(LinePen, new PointF(left, y1), new PointF(right, y1));
                    con.DrawLine(LinePen, new PointF(left, y2), new PointF(right, y2));
                }
            List<string> toDelete = new List<string>();
            int i = 0;
            foreach (var item in ToPrint)
                //if (con is DrawContextDocument)
                //{
                //    string file = FileName + "." + i;
                //    i++;
                //    item.CreateImage(file, ImageFormat.Jpeg);
                //    file = file + ".jpg";
                //    using (Image img = Image.FromFile(file))
                //        con.DrawImage(img, item.Box);
                //    toDelete.Add(file);
                //    System.Windows.Forms.MessageBox.Show("Test");
                //}
                //else
                    item.Draw(con);
            if (linie)
                foreach (var item in ToPrint)
                    con.DrawRectangle(LinePen, item.Box);
            foreach (var item in toDelete)
                File.Delete(item);
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
