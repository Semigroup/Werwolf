using System.Linq;
using System.Drawing;
using Assistment.Extensions;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.Geometries.Extensions;

namespace Werwolf.Karten
{
    public abstract class WonderTextFeld : WolfBox
    {
        public bool Oben;
        public abstract Bild FeldBild { get; }
        public string Text;
        public bool Quer;
        public bool AufKopf;

        protected string LastFilePath;
        protected SizeF LastFeldSize;
        protected float LastPpm;
        protected string LastText;
        protected Font LastFont;
        protected Size LastSize;
        protected bool DrawBoxChanged;

        protected new Size Size;
        protected Image BearbeitetesBild;
        public DrawBox DrawBox { get; set; }
        public FixedBox FixedBox;
        protected Region Clip;

        public float Drehung
        {
            get
            {
                return (Quer ? -90 : 0)
                    + (AufKopf ? 180 : 0);
            }
        }

        public WonderTextFeld(Karte Karte, float Ppm, bool Oben, bool Quer, bool AufKopf)
            : base(Karte, Ppm)
        {
            this.Oben = Oben;
            this.Quer = Quer;
            this.AufKopf = AufKopf;
            //this.FeldBild = FeldBild;
        }

        public override void Update()
        {
        }

        public override bool Visible()
        {
            return base.Visible() && HintergrundDarstellung.Existiert && FeldBild != null && DrawBox != null;
        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            if (Quer)
                DrawBox.Setup(FeldBild.Size.mul(Faktor).permut());
            else
                DrawBox.Setup(FeldBild.Size.mul(Faktor));

            SizeF Size1 = FeldBild.Size.mul(Ppm);
            SizeF Size2 = DrawBox.Size.mul(Ppm / Faktor);
            if (Quer)
                Size2 = Size2.permut();
            Size = Size1.Max(Size2).ToSize();
            this.Box.Size = ((SizeF)Size).mul(Faktor / Ppm);

            RectangleF Rectangle = new RectangleF(new PointF(), this.Box.Size);
            if (Quer)
                Rectangle.Size = Rectangle.Size.permut();

            FixedBox = new FixedBox(Rectangle.Size, DrawBox);
            if (Oben ^ AufKopf)
                FixedBox.Alignment = new SizeF(0.5f, 1);
            else
                FixedBox.Alignment = new SizeF(0.5f, 0);
            if (Quer)
                FixedBox.Alignment = new SizeF(1 - FixedBox.Alignment.Height, FixedBox.Alignment.Width);

            FixedBox.Setup(Rectangle);
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
            float Rest = Box.Size.Height - Abstand;
            this.Box.X = LotPunkt.X;
            if (Oben)
                this.Box.Y = LotPunkt.Y - Rest;
            else
                this.Box.Y = LotPunkt.Y - Box.Size.Height + Rest;

            if (Oben)
                Clip = new Region(new RectangleF(0, Box.Height - Abstand, Box.Width, Abstand).mul(Ppm / Faktor));
            else
                Clip = new Region(new RectangleF(0, 0, Box.Width, Abstand).mul(Ppm / Faktor));
        }

        public override void Draw(DrawContext con)
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
            con.DrawImage(BearbeitetesBild, Box);
        }
        public virtual void Bearbeite()
        {
            SizeF Rest = Box.Size.sub(FeldBild.Size.mul(Faktor)).mul(0.5f, Oben ? 1 : 0).mul(Ppm / Faktor);
            Point P = new Point((int)Rest.Width, (int)Rest.Height);
            BearbeitetesBild = new Bitmap(Size.Width, Size.Height);
            using (Graphics g = BearbeitetesBild.GetHighGraphics())
            {
                g.Clip = Clip;
                using (Image img = FeldBild.Image)//Image.FromFile(FeldBild.TotalFilePath))
                    g.DrawImage(img, new Rectangle(P, FeldBild.Size.mul(Ppm).ToSize()));

                g.TranslateTransform(Size.Width / 2, Size.Height / 2);
                g.RotateTransform(Drehung);
                if (Quer)
                    g.TranslateTransform(-Size.Height / 2, -Size.Width / 2);
                else
                    g.TranslateTransform(-Size.Width / 2, -Size.Height / 2);

                g.ScaleTransform(Ppm / Faktor, Ppm / Faktor);
                //if (Quer)
                //    g.RotateTransform(-90);
                using (DrawContextGraphics dcg = new DrawContextGraphics(g))
                    FixedBox.Draw(dcg);
            }
        }
    }
}
