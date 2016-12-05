using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistment.Extensions;
using Werwolf.Inhalt;
using System.Drawing.Drawing2D;

namespace Werwolf.Karten
{
    public class WondersDoppelBild : WolfBox
    {
        private Karte Reich1, Reich2;
        private bool NeuZeichnen;
        private Image Bild;

        public WondersDoppelBild(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override bool Visible()
        {
            return base.Visible() && BildDarstellung.Existiert;
        }

        public override void update()
        {
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            if (Karte != null)
            {
                Size s = Karte.HintergrundDarstellung.Size
                    .sub(Karte.HintergrundDarstellung.Rand.mul(2))
                    .mul(ppm).ToSize();
                if ((Bild == null || !Bild.Size.Equals(s)))
                    Bild = new Bitmap(s.Width, s.Height);
            }
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
            {
                Bild = null;
                return;
            }

            if (Karte.Entwicklungen.Length > 0)
            {
                if (Reich1 != Karte.Entwicklungen[0])
                    NeuZeichnen = true;
                Reich1 = Karte.Entwicklungen[0];
            }
            else
            {
                if (Reich1 != null)
                    NeuZeichnen = true;
                Reich1 = null;
            }
            if (Karte.Entwicklungen.Length > 1)
            {
                if (Reich2 != Karte.Entwicklungen[1])
                    NeuZeichnen = true;
                Reich2 = Karte.Entwicklungen[1];
            }
            else
            {
                if (Reich2 != null)
                    NeuZeichnen = true;
                Reich2 = null;
            }
            OnPpmChanged();
        }
        public override void setup(RectangleF box)
        {
            this.box = AussenBox.move(box.Location);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
        }

        public override void draw(DrawContext con)
        {
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(0, 0);
            if (NeuZeichnen && Bild != null)
            {
                ErstelleBild();
                con.drawImage(Bild, MovedInnenBox);
            }
        }
        private void ErstelleBild()
        {
            Size s = Bild.Size;
            float h = HintergrundDarstellung.Anker.Y * ppm;
            float heigh = s.Height - h;
            PointF Zenter = new PointF(s.Width / 4, heigh / 4);

            using (Graphics g = Bild.GetHighGraphics())
            {
                if (Reich1 != null)
                {
                    SizeF FeldSize = Reich1.HintergrundDarstellung.Size;
                    float skal = heigh / FeldSize.Height;
                    RectangleF Rec = Reich1.HauptBild.Rectangle.mul(skal / Faktor);
                    Rec = Rec.move(Zenter);
                    using (Image img = Reich1.HauptBild.Image)
                        g.DrawImage(img, Rec);
                }

                GraphicsPath gp = new GraphicsPath();
                gp.AddLine(0, s.Height, 0, s.Height - h);
                gp.AddLine(0, s.Height - h, s.Width, h);
                gp.AddLine(s.Width, h, s.Width, s.Height);
                gp.AddLine(s.Width, s.Height, 0, s.Height);
                gp.CloseFigure();
                g.Clip = new Region(gp);
                g.TranslateTransform(s.Width / 2, s.Height / 2);
                g.RotateTransform(180);
                g.TranslateTransform(-s.Width / 2, -s.Height / 2);
                if (Reich2 != null)
                {
                    SizeF FeldSize = Reich2.HintergrundDarstellung.Size;
                    float skal = heigh / FeldSize.Height;
                    RectangleF Rec = Reich2.HauptBild.Rectangle.mul(skal / Faktor);
                    Rec = Rec.move(Zenter);
                    using (Image img = Reich2.HauptBild.Image)
                        g.DrawImage(img, Rec);
                }

            }
        }
    }
}
