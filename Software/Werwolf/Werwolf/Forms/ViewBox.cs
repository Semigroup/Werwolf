using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Werwolf.Inhalt;
using Werwolf.Karten;

using Assistment.Texts;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Forms
{
    public abstract partial class ViewBox : UserControl
    {
        private int DelayStep = 10;
        private int CurrentDelay = 0;

        protected float ppm = 8;

        protected Karte karte;
        public Karte Karte
        {
            get { return karte; }
            set
            {
                karte = value;
                OnKarteChanged();
            }
        }

        protected Size LastSize = new Size();
        protected Graphics g;

        public ViewBox()
        {
            InitializeComponent();
        }
        public void ChangeKarte(XmlElement ChangedElement)
        {
            if (ChangedElement != null)
                ChangedElement.AdaptToCard(karte);
            OnKarteChanged();
        }
        public void OnKarteChanged()
        {
            CurrentDelay = DelayStep;
        }
        protected virtual void Draw()
        {
            ChangeSize();
            g.Clear(Color.White);
        }
        public override void Refresh()
        {
            Draw();
            base.Refresh();
        }

        protected virtual bool ChangeSize()
        {
            Size Size = karte.GetPictureSize(ppm);
            if (Size.Equals(LastSize))
                return false;
            LastSize = Size;
            pictureBox1.Image = new Bitmap(Size.Width, Size.Height);
            g = pictureBox1.Image.GetHighGraphics(ppm / WolfBox.Faktor);
            return true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CurrentDelay > 1)
                CurrentDelay--;
            else if (CurrentDelay == 1)
            {
                this.Refresh();
                CurrentDelay = 0;
            }
        }
    }
    public class ViewKarte : ViewBox
    {
        private DrawContextGraphics DrawContext;
        private StandardKarte StandardKarte;

        protected override void Draw()
        {
            base.Draw();
            StandardKarte.OnKarteChanged();
            StandardKarte.setup(0);
            StandardKarte.draw(DrawContext);
        }
        protected override bool ChangeSize()
        {
            if (!base.ChangeSize())
                return false;
            DrawContext = new DrawContextGraphics(g, Brushes.White);
            StandardKarte = new StandardKarte(karte, ppm);
            return true;
        }
    }
    public class ViewRuckseitenBild : ViewBox
    {
        protected override void Draw()
        {
            base.Draw();
            g.Clear(Karte.HintergrundDarstellung.Farbe);
            Bild b = Karte.Fraktion.RuckseitenBild;
            if (b.Image == null)
                return;
            PointF Zentrum = ((SizeF)LastSize).div(2).ToPointF();
            RectangleF Rectangle = b.Rectangle.mul(ppm / WolfBox.Faktor).move(Zentrum);
            g.DrawImage(b.Image, Rectangle);
        }
    }
    public class ViewTextBild : ViewBox
    {
        protected override void Draw()
        {
            base.Draw();
            
        }
    }
    public class ViewDeck : ViewBox
    {
        protected override void Draw()
        {
            base.Draw();

        }
    }
}
