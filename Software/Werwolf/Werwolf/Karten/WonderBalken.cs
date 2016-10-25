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
    public class WonderBalken : WolfBox
    {
        private string FilePath;
        private Color AlteFarbe;
        private Image BearbeitetesBild;

        public WonderBalken(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override void update()
        {
        }

        public override bool Visible()
        {
            return base.Visible() && HintergrundDarstellung.Existiert;
        }

        public override void setup(RectangleF box)
        {
            this.box = AussenBox;
            this.box.Location = box.Location;
        }

        public override void draw(DrawContext con)
        {
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(-1, -1);
            if (Karte.Fraktion.HintergrundBild.FilePath.Length == 0)
                return;
            if (!Karte.Fraktion.HintergrundBild.FilePath.Equals(FilePath)
                || !Karte.HintergrundDarstellung.Farbe.Equals(AlteFarbe))
            {
                this.AlteFarbe = Karte.HintergrundDarstellung.Farbe;
                this.FilePath = Karte.Fraktion.HintergrundBild.FilePath;
                using (Bitmap Vorlage = new Bitmap(Karte.Fraktion.HintergrundBild.TotalFilePath))
                    Bearbeite(Vorlage, AlteFarbe);
            }
            PointF Zentrum = MovedInnenBox.Center();//Karte.TitelDarstellung.Rand.add(Karte.HintergrundDarstellung.Rand).mul(Faktor).ToPointF();
            con.DrawCenteredImage(Karte.Fraktion.HintergrundBild, BearbeitetesBild, Zentrum, MovedInnenBox); // 
        }


        public void Bearbeite(Bitmap Vorlage, Color GrundFarbe)
        {
            Bitmap b = new Bitmap(Vorlage);
            BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, b.PixelFormat);
            int bufferSize = data.Height * data.Stride;
            byte[] bytes = new byte[bufferSize]; //BGRA
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            for (int i = 0; i < bufferSize; i += 4)
            {
                if (bytes[i + 3] > 0)
                {
                    bytes[i] = GrundFarbe.B;
                    bytes[i + 1] = GrundFarbe.G;
                    bytes[i + 2] = GrundFarbe.R;
                    bytes[i + 3] = GrundFarbe.A;
                }
                else
                    bytes[i] = bytes[i + 1] = bytes[i + 2] = bytes[i + 3] = 0;
            }
            Marshal.Copy(bytes, 0, data.Scan0, bytes.Length);
            b.UnlockBits(data);
            using (Graphics g = b.GetHighGraphics())
                g.DrawImage(Vorlage, new Rectangle(new Point(), Vorlage.Size));
            this.BearbeitetesBild = b;
        }
    }
}
