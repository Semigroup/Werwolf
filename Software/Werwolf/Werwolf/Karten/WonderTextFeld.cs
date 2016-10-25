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
    public class WonderTextFeld : WolfBox
    {
        public bool Oben;
        public Bild FeldBild;
        public string Text;

        private string LastFilePath;
        private SizeF LastFeldSize;
        private float LastPpm;
        private string LastText;
        private Font LastFont;
        private SizeF LastTitelRand;

        private Image BearbeitetesBild;
        //private float Abstand;
        private DrawBox DrawBox;
        private FixedBox FixedBox;

        public WonderTextFeld(Karte Karte, float Ppm, bool Oben, Bild FeldBild, string Text)
            : base(Karte, Ppm)
        {
            this.Oben = Oben;
            this.FeldBild = FeldBild;
            this.Text = Text;
        }

        public override void update()
        {
        }

        public override bool Visible()
        {
            return base.Visible() && HintergrundDarstellung.Existiert && FeldBild != null && Text.Length > 0;
        }

        public override void setup(RectangleF box)
        {
            this.box = AussenBox;
            this.box.Location = box.Location;
            this.box.Size = FeldBild.Size.mul(Faktor);

            RectangleF Rectangle = new RectangleF();
            Rectangle.Size = FeldBild.Size.mul(Faktor).permut();
            Rectangle.X -= Rectangle.Size.Width;
            Rectangle = Rectangle.Inner(Karte.TitelDarstellung.Rand.mul(Faktor));

            this.DrawBox = new Text(Text, TitelDarstellung.FontMeasurer);
            FixedBox = new FixedBox(Rectangle.Size, DrawBox);
            FixedBox.Alignment = new SizeF(1, 0.5f);
            FixedBox.setup(Rectangle);
        }
        /// <summary>
        /// falls oben: linker, oberer Punkt wird gleich dem LotPunkt gesetzt
        /// <para>falls unten: linker, unterer Punkt wird gleich dem LotPunkt gesetzt</para>
        /// </summary>
        /// <param name="LotPunkt"></param>
        public void SetLot(PointF LotPunkt)
        {
            float Abstand = (FeldBild.Size.Height - TitelDarstellung.Rand.Width) * Faktor;
            if (DrawBox != null)
                Abstand -= DrawBox.box.Width;
            this.box.X = LotPunkt.X;
            if (Oben)
                this.box.Y = LotPunkt.Y - Abstand;
            else
                this.box.Y = LotPunkt.Y - FeldBild.Size.Height * Faktor + Abstand;
        }

        public override void draw(DrawContext con)
        {
            if (!(LastPpm == Ppm
                && TitelDarstellung.Font.Equals(LastFont)
                && Text.Equals(LastText)
                && FeldBild.FilePath.Equals(LastFilePath)
                && FeldBild.Size.Equal(LastFeldSize)
                && Karte.TitelDarstellung.Rand.Equal(LastTitelRand)))
            {
                this.LastPpm = Ppm;
                this.LastFont = TitelDarstellung.Font;
                this.LastText = Text;
                this.LastFeldSize = FeldBild.Size;
                this.LastFilePath = FeldBild.FilePath;
                this.LastTitelRand = Karte.TitelDarstellung.Rand;
                Bearbeite();
            }
            con.drawImage(BearbeitetesBild, box);
        }
        public void Bearbeite()
        {
            BearbeitetesBild = new Bitmap((int)(FeldBild.Size.Width * Ppm), (int)(FeldBild.Size.Height * Ppm));
            using (Graphics g = BearbeitetesBild.GetHighGraphics())
            {
                using (Image img = Image.FromFile(FeldBild.TotalFilePath))
                    g.DrawImage(img, new Rectangle(0, 0, BearbeitetesBild.Width, BearbeitetesBild.Height));
                g.ScaleTransform(Ppm / Faktor, Ppm / Faktor);
                g.RotateTransform(-90);

                using (DrawContextGraphics dcg = new DrawContextGraphics(g))
                    FixedBox.draw(dcg);
            }
        }
    }
}
