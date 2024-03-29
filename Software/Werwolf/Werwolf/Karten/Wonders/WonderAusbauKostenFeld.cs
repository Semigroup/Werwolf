﻿using System;
using System.Linq;
using System.Drawing;
using Assistment.Extensions;
using Werwolf.Inhalt;
using Assistment.Texts;
using System.IO;
using Assistment.Drawing.Geometries.Extensions;

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
                    durchmesser = Symbole.First().Min;
                klein = n < 4;
            }

            public override float Space => Symbole.Space;
            public override float Min
            {
                get
                {
                    if (klein)
                        return durchmesser;
                    else
                        return durchmesser + hohe;
                }
            }

            public override float Max => Max;
            public override void Update()
            {
            }
            public override void Setup(RectangleF box)
            {
                this.Box = box;
                PointF p1, p2;
                if (klein)
                    p1 = p2 = new PointF(0, durchmesser + abstand);
                else
                {
                    p1 = new PointF(hohe, (durchmesser + abstand) / 2);
                    if (Symbole.Count() > 5)
                        p1.Y = p1.Y * 0.85f;
                    p2 = new PointF(-p1.X, p1.Y);
                }
                int i = 0;
                foreach (var item in Symbole)
                {
                    item.Setup(box);
                    box = box.move(i % 2 == 0 ? p1 : p2);
                    i++;
                }
                this.Box.Width = Min;
                this.Box.Height = box.Y - this.Box.Y + abstand;
                if (!klein)
                    this.Box.Height += (durchmesser + abstand) / 2;
            }
            public override void Draw(DrawContext con)
            {
                foreach (var item in Symbole)
                    item.Draw(con);
            }
            public override DrawBox Clone()
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

            SizeF Rest = Box.Size.sub(FeldBild.Size.mul(Faktor)).mul(0.5f, Oben ? 1 : 0).mul(Ppm / Faktor);
            Point P = new Point((int)Rest.Width, (int)(Rest.Height + 0.5f * Ppm));
            BearbeitetesBild = new Bitmap(Size.Width, Size.Height);
            using (Graphics g = BearbeitetesBild.GetHighGraphics())
            {
                if (File.Exists(FeldBild.TotalFilePath))
                    using (Image img = Image.FromFile(FeldBild.TotalFilePath))
                        g.DrawImage(img, new Rectangle(P, FeldBild.Size.mul(Ppm).ToSize()));
                else
                {
                    //TODO draw error image here
                }
                g.ScaleTransform(Ppm / Faktor, Ppm / Faktor);
                if (Quer)
                    g.RotateTransform(-90);
                using (DrawContextGraphics dcg = new DrawContextGraphics(g))
                    FixedBox.Draw(dcg);
            }
        }
    }
}
