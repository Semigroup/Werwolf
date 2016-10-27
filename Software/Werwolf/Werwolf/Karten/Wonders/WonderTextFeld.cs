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
    public abstract class WonderTextFeld : WolfBox
    {
        public bool Oben;
        public Bild FeldBild;
        public string Text;

        protected string LastFilePath;
        protected SizeF LastFeldSize;
        protected float LastPpm;
        protected string LastText;
        protected Font LastFont;
        protected SizeF LastTitelRand;
        protected Size LastSize;
        protected bool DrawBoxChanged;

        protected new Size Size;
        protected Image BearbeitetesBild;
        protected DrawBox DrawBox { get; set; }
        protected FixedBox FixedBox;

        public WonderTextFeld(Karte Karte, float Ppm, bool Oben, Bild FeldBild)
            : base(Karte, Ppm)
        {
            this.Oben = Oben;
            this.FeldBild = FeldBild;
        }

        public override void update()
        {
        }

        public override bool Visible()
        {
            return base.Visible() && HintergrundDarstellung.Existiert && FeldBild != null && DrawBox != null;
        }

        public override void setup(RectangleF box)
        {
            this.box = AussenBox;
            this.box.Location = box.Location;

            DrawBox.setup(FeldBild.Size.mul(Faktor).permut());

            SizeF Size1 = FeldBild.Size.mul(Ppm);
            SizeF Size2 = DrawBox.Size.mul(Ppm / Faktor).add(TitelDarstellung.Rand.mul(Ppm * 2)).permut();
            Size = Size1.Max(Size2).ToSize();
            this.box.Size = ((SizeF)Size).mul(Faktor / Ppm);

            RectangleF Rectangle = new RectangleF();
            Rectangle.Size = ((SizeF)Size).mul(Faktor / Ppm).permut();

            SizeF Rand = TitelDarstellung.Rand.mul(Faktor);
            if (Oben)
            {
                FixedBox = new FixedBox(Rectangle.Size,
                       DrawBox.Geometry(Rand.Width, Rand.Height, 0, Rand.Height));
                FixedBox.Alignment = new SizeF(0, 0.5f);
                FixedBox.setup(Rectangle);
                FixedBox.Move(-Size.Height * Faktor / Ppm, 0);
            }
            else
            {
                FixedBox = new FixedBox(Rectangle.Size,
                DrawBox.Geometry(0, Rand.Height, Rand.Width, Rand.Height));
                FixedBox.Alignment = new SizeF(1, 0.5f);
                FixedBox.setup(Rectangle);
                FixedBox.Move(-Size.Height * Faktor / Ppm, 0);
            }
        }
        /// <summary>
        /// falls oben: linker, oberer Punkt wird gleich dem LotPunkt gesetzt
        /// <para>falls unten: linker, unterer Punkt wird gleich dem LotPunkt gesetzt</para>
        /// </summary>
        /// <param name="LotPunkt"></param>
        public virtual void SetLot(PointF LotPunkt)
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
            if (DrawBoxChanged ||
                !(LastPpm == Ppm
                && TitelDarstellung.Font.Equals(LastFont)
                && FeldBild.FilePath.Equals(LastFilePath)
                && FeldBild.Size.Equal(LastFeldSize)
                && Karte.TitelDarstellung.Rand.Equal(LastTitelRand)
                && Size == LastSize
                ))
            {
                this.LastPpm = Ppm;
                this.LastFont = TitelDarstellung.Font;
                this.LastFeldSize = FeldBild.Size;
                this.LastFilePath = FeldBild.FilePath;
                this.LastTitelRand = Karte.TitelDarstellung.Rand;
                this.LastSize = Size;
                Bearbeite();
            }
            con.drawImage(BearbeitetesBild, box);
        }
        public virtual void Bearbeite()
        {
            SizeF Rest = box.Size.sub(FeldBild.Size.mul(Faktor)).mul(0.5f ,Oben ? 1 : 0).mul(Ppm / Faktor);
            Point P = new Point((int)Rest.Width, (int)Rest.Height);
            BearbeitetesBild = new Bitmap(Size.Width, Size.Height);
            using (Graphics g = BearbeitetesBild.GetHighGraphics())
            {
                //g.Clear(Color.Red);
                using (Image img = Image.FromFile(FeldBild.TotalFilePath))
                    g.DrawImage(img, new Rectangle(P, FeldBild.Size.mul(Ppm).ToSize()));
                g.ScaleTransform(Ppm / Faktor, Ppm / Faktor);
                g.RotateTransform(-90);
                using (DrawContextGraphics dcg = new DrawContextGraphics(g))
                    FixedBox.draw(dcg);
            }
        }
    }
}
