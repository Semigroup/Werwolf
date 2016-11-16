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
        public bool Quer;

        protected string LastFilePath;
        protected SizeF LastFeldSize;
        protected float LastPpm;
        protected string LastText;
        protected Font LastFont;
        protected Size LastSize;
        protected bool DrawBoxChanged;

        protected new Size Size;
        protected Image BearbeitetesBild;
        protected DrawBox DrawBox { get; set; }
        protected FixedBox FixedBox;

        public WonderTextFeld(Karte Karte, float Ppm, bool Oben, bool Quer, Bild FeldBild)
            : base(Karte, Ppm)
        {
            this.Oben = Oben;
            this.Quer = Quer;
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

            if (Quer)
                DrawBox.setup(FeldBild.Size.mul(Faktor).permut());
            else
                DrawBox.setup(FeldBild.Size.mul(Faktor));

            SizeF Size1 = FeldBild.Size.mul(Ppm);
            SizeF Size2 = DrawBox.Size.mul(Ppm / Faktor);
            if (Quer)
                Size2 = Size2.permut();
            Size = Size1.Max(Size2).ToSize();
            this.box.Size = ((SizeF)Size).mul(Faktor / Ppm);

            RectangleF Rectangle = new RectangleF(new PointF(), this.box.Size);
            if (Quer)
            {
                Rectangle.X = -Size.Height * Faktor / Ppm;
                Rectangle.Size = Rectangle.Size.permut();
            }

            FixedBox = new FixedBox(Rectangle.Size, DrawBox);
            if (Oben)
                FixedBox.Alignment = new SizeF(0.5f, 1);
            else
                FixedBox.Alignment = new SizeF(0.5f, 0);
            if (Quer)
            {
                FixedBox.Alignment = new SizeF(1 - FixedBox.Alignment.Height, FixedBox.Alignment.Width);
            }

            FixedBox.setup(Rectangle);
        }
        /// <summary>
        /// falls oben: linker, oberer Punkt wird gleich dem LotPunkt gesetzt
        /// <para>falls unten: linker, unterer Punkt wird gleich dem LotPunkt gesetzt</para>
        /// </summary>
        /// <param name="LotPunkt"></param>
        public virtual void SetLot(PointF LotPunkt)
        {
            if (DrawBox == null)
                return;
            float Abstand = Quer ? DrawBox.Size.Width : DrawBox.Size.Height;
            float Rest = box.Size.Height - Abstand;
            this.box.X = LotPunkt.X;
            if (Oben)
                this.box.Y = LotPunkt.Y - Rest;
            else
                this.box.Y = LotPunkt.Y - box.Size.Height + Rest;
        }

        public override void draw(DrawContext con)
        {
            if (DrawBoxChanged ||
                !(LastPpm == Ppm
                && TitelDarstellung.Font.Equals(LastFont)
                && FeldBild.FilePath.Equals(LastFilePath)
                && FeldBild.Size.Equal(LastFeldSize)
                && Size == LastSize
                ))
            {
                this.LastPpm = Ppm;
                this.LastFont = TitelDarstellung.Font;
                this.LastFeldSize = FeldBild.Size;
                this.LastFilePath = FeldBild.FilePath;
                this.LastSize = Size;
                DrawBoxChanged = false;
                Bearbeite();
            }
            con.drawImage(BearbeitetesBild, box);
        }
        public virtual void Bearbeite()
        {
            SizeF Rest = box.Size.sub(FeldBild.Size.mul(Faktor)).mul(0.5f, Oben ? 1 : 0).mul(Ppm / Faktor);
            Point P = new Point((int)Rest.Width, (int)Rest.Height);
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
