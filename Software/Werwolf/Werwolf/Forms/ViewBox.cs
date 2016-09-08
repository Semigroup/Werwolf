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
        private int DelayStep { get { return Settings.DelayTime; } }
        private int CurrentDelay = 0;

        protected float ppm { get { return Settings.ViewPpm; } }

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

        protected DrawContextGraphics DrawContext;
        protected WolfBox WolfBox;

        public ViewBox()
            : base()
        {
            InitializeComponent();
            WolfBox = GetWolfBox(Karte, ppm);
        }
        public virtual void ChangeKarte(XmlElement ChangedElement)
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
            if (karte != null)
            {
                ChangeSize();
                g.Clear(Color.White);
                WolfBox.Karte = karte;
                WolfBox.setup(0);
                WolfBox.draw(DrawContext);
            }
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
            DrawContext = new DrawContextGraphics(g, Brushes.White);
            return true;
        }
        protected abstract WolfBox GetWolfBox(Karte Karte, float Ppm);

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
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            return new StandardKarte(Karte, Ppm);
        }
    }
    public class ViewRuckseitenBild : ViewBox
    {
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            return new StandardRuckseite(Karte, Ppm);
        }
    }
    public class ViewDeck : ViewBox
    {
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            throw new NotImplementedException();
        }
    }
}
