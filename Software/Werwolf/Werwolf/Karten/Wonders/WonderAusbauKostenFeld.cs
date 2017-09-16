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
    public class WonderAusbauKostenFeld : WonderTextFeld
    {
        private bool Klein = true;
        private string LastKosten;
        private static FontGraphicsMeasurer Font = new FontGraphicsMeasurer("Calibri", 15); // 16
        public override Bild FeldBild
        {
            get
            {
                if (Klein)
                    return Karte.LayoutDarstellung.GetKleinesNamenfeld(false);
                else
                    return Karte.LayoutDarstellung.GetGrossesNamenfeld(false);
            }
        }

        private class KostenBox : DrawBox
        {
            private float abstand;
            private float durchmesser;
            private int n;
            public bool klein;
            private DrawContainer Symbole;
            private float hohe { get { return (float)(Math.Sqrt(3.0 / 4.0) * (durchmesser + abstand)); } }

            /// <summary>
            /// Abstand in mm
            /// <para>nimmt an, dass alle symbole kreise derselben größe sind</para>
            /// </summary>
            /// <param name="Abstand"></param>
            /// <param name="Symbole"></param>
            public KostenBox(float Abstand, DrawContainer Symbole)
            {
                this.Symbole = Symbole;
                abstand = Abstand * Faktor;
                n = Symbole.Count();
                if (n > 0)
                    durchmesser = Symbole.First().getMin();
                klein = n < 4;
            }

            public override float getSpace()
            {
                return Symbole.getSpace();
            }
            public override float getMin()
            {
                if (klein)
                    return durchmesser;
                else
                    return durchmesser + hohe;
            }
            public override float getMax()
            {
                return getMax();
            }
            public override void update()
            {
            }
            public override void setup(RectangleF box)
            {
                this.box = box;
                PointF p1, p2;
                if (klein)
                    p1 = p2 = new PointF(0, durchmesser + abstand);
                else
                {
                    p1 = new PointF(hohe, (durchmesser + abstand) / 2);
                    p2 = new PointF(-p1.X, p1.Y);
                }
                int i = 0;
                foreach (var item in Symbole)
                {
                    item.setup(box);
                    box = box.move(i % 2 == 0 ? p1 : p2);
                    i++;
                }
                this.box.Width = getMin();
                this.box.Height = box.Y - this.box.Y + abstand;
                if (!klein)
                    this.box.Height += (durchmesser + abstand) / 2;
            }
            public override void draw(DrawContext con)
            {
                foreach (var item in Symbole)
                    item.draw(con);
            }
            public override DrawBox clone()
            {
                throw new NotImplementedException();
            }
        }

        public WonderAusbauKostenFeld(Karte Karte, float Ppm)
            : base(Karte, Ppm, false, false, false)
        {

        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
            {
                DrawBox = null;
                return;
            }
            else
            {
                string Kosten = Karte.Kosten.ToString();
                if (!Kosten.Equals(LastKosten))
                {
                    this.LastKosten = Kosten;
                    Text[] KostenText = Karte.Kosten.ProduceTexts(Font);
                    if (KostenText.Length > 0)
                    {
                        KostenBox KostenBox = new KostenBox(1, KostenText[0]);
                        DrawBox = KostenBox;
                        this.Klein = KostenBox.klein;

                    }
                    else
                        DrawBox = null;
                    DrawBoxChanged = true;
                }
            }
        }
        public override void Bearbeite()
        {
            Bild FeldBild = this.FeldBild;

            SizeF Rest = box.Size.sub(FeldBild.Size.mul(Faktor)).mul(0.5f, Oben ? 1 : 0).mul(Ppm / Faktor);
            Point P = new Point((int)Rest.Width, (int)(Rest.Height + 0.5f * Ppm));
            BearbeitetesBild = new Bitmap(Size.Width, Size.Height);
            using (Graphics g = BearbeitetesBild.GetHighGraphics())
            {
                using (Image img = Image.FromFile(FeldBild.TotalFilePath))
                    g.DrawImage(img, new Rectangle(P, FeldBild.Size.mul(Ppm).ToSize()));
                g.ScaleTransform(Ppm / Faktor, Ppm / Faktor);
                if (Quer)
                    g.RotateTransform(-90);
                using (DrawContextGraphics dcg = new DrawContextGraphics(g))
                    FixedBox.draw(dcg);
            }
        }
    }
}
