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
    public class WonderKostenFeld : WonderTextFeld
    {
        private string LastKosten;
        private static FontGraphicsMeasurer Font = new FontGraphicsMeasurer("Calibri", 14); // 16

        public WonderKostenFeld(Karte Karte, float Ppm)
            : base(Karte, Ppm, true,false, Karte.Universe.TextBilder["KleinesNamenfeld"])
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
                        for (int i = KostenText[0].Count() - 1; i >= 0; i--)
                            KostenText[0].insert(i, new Whitespace(1 * Faktor, 1 * Faktor, true));
                        DrawBox = KostenText[0].Geometry(Faktor, Faktor, Faktor, Faktor * 5);
                    }
                    else
                        DrawBox = null;
                    DrawBoxChanged = true;
                }
            }
        }
        //public override void setup(RectangleF box)
        //{
        //    this.box = AussenBox;
        //    this.box.Location = box.Location;

        //    DrawBox.setup(FeldBild.Size.mul(Faktor));

        //    SizeF Size1 = FeldBild.Size.mul(Ppm);
        //    SizeF Size2 = DrawBox.Size.mul(Ppm / Faktor).add(TitelDarstellung.Rand.mul(Ppm * 2).permut());
        //    Size = Size1.Max(Size2).ToSize();
        //    this.box.Size = ((SizeF)Size).mul(Faktor / Ppm);

        //    RectangleF Rectangle = new RectangleF();
        //    Rectangle.Size = ((SizeF)Size).mul(Faktor / Ppm);

        //    SizeF Rand = TitelDarstellung.Rand.mul(Faktor).permut();
        //    if (Oben)
        //    {
        //        FixedBox = new FixedBox(Rectangle.Size,
        //               DrawBox.Geometry(Rand.Width, 0, Rand.Width, Rand.Height));
        //        FixedBox.Alignment = new SizeF(0.5f, 1);
        //    }
        //    else
        //    {
        //        FixedBox = new FixedBox(Rectangle.Size,
        //        DrawBox.Geometry(Rand.Width, Rand.Height, Rand.Width, 0));
        //        FixedBox.Alignment = new SizeF(0.5f, 0);
        //    }
        //    FixedBox.setup(Rectangle);
        //}
        //public override void Bearbeite()
        //{
        //    SizeF Rest = box.Size.sub(FeldBild.Size.mul(Faktor)).mul(0.5f, Oben ? 1 : 0).mul(Ppm / Faktor);
        //    Point P = new Point((int)Rest.Width, (int)Rest.Height);
        //    BearbeitetesBild = new Bitmap(Size.Width, Size.Height);
        //    using (Graphics g = BearbeitetesBild.GetHighGraphics())
        //    {
        //        using (Image img = Image.FromFile(FeldBild.TotalFilePath))
        //            g.DrawImage(img, new Rectangle(P, FeldBild.Size.mul(Ppm).ToSize()));
        //        g.ScaleTransform(Ppm / Faktor, Ppm / Faktor);
        //        using (DrawContextGraphics dcg = new DrawContextGraphics(g))
        //            FixedBox.draw(dcg);
        //    }
        //}
        //public override void SetLot(PointF LotPunkt)
        //{
        //    float Abstand = (FeldBild.Size.Height - TitelDarstellung.Rand.Width) * Faktor;
        //    if (DrawBox != null)
        //        Abstand -= DrawBox.box.Height;
        //    this.box.X = LotPunkt.X;
        //    if (Oben)
        //        this.box.Y = LotPunkt.Y - Abstand;
        //    else
        //        this.box.Y = LotPunkt.Y - FeldBild.Size.Height * Faktor + Abstand;
        //}
    }
}
