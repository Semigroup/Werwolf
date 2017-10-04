using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

using Werwolf.Inhalt;
using Werwolf.Karten;

using Assistment.Texts;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Forms
{
    public abstract partial class ViewBox : UserControl
    {
        public bool Active
        {
            get { return timer1.Enabled; }
            set { timer1.Enabled = value; }
        }

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
        private bool Drawing;
        private bool Dirty;

        protected DrawContextGraphics DrawContext;
        protected WolfBox WolfBox;

        protected PictureBox PictureBox { get { return pictureBox1; } }

        public ViewBox()
            : base()
        {
            InitializeComponent();
            WolfBox = GetWolfBox(Karte, ppm);
            pictureBox1.MouseDown += (x, e) => OnMouseDown(e);
            pictureBox1.MouseUp += (x, e) => OnMouseUp(e);
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
            if (karte == null) return;
            if (!Drawing)
            {
                Drawing = true;
                ChangeSize();
                //g.Clear(Color.White);
                WolfBox.Karte = karte;
                RectangleF r = new RectangleF();
                r.Size = LastSize;
                r.Size = r.Size.mul(ppm / WolfBox.Faktor);
                WolfBox.setup(r);
                WolfBox.draw(DrawContext);
                //await Task.Run( () => );
                Drawing = false;
            }
            else
                Dirty = true;
        }
        public override void Refresh()
        {
            Draw();
            if (this.IsHandleCreated)
                this.Invoke((MethodInvoker)(() => base.Refresh()));
            //base.Refresh();
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
        public PointF GetCurrentPictureSize()
        {
            PointF res = new PointF(LastSize.Width * 1f / pictureBox1.Width,
                LastSize.Height * 1f / pictureBox1.Height);
            return new PointF(LastSize.Width, LastSize.Height).div(
                res.X > res.Y ? res.X : res.Y);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (CurrentDelay > 1)
                CurrentDelay--;
            else if (CurrentDelay == 1)
            {
                Dirty = false;
                this.Refresh();
                CurrentDelay = 0;
            }
            else if(Dirty)
            {
                Dirty = false;
                OnKarteChanged();
            }
        }
    }
    public class ViewKarte : ViewBox
    {
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            if (Karte == null)
                return new StandardKarte(Karte, Ppm);
            else
                return Karte.GetVorderSeite(Ppm);
        }
    }
    public class ViewRuckseitenBild : ViewBox
    {
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            if (Karte == null)
                return new StandardRuckseite(Karte, Ppm);
            else
                return Karte.GetRuckSeite(Ppm);
        }
    }
    public class ViewDeck : ViewBox
    {
        public override void ChangeKarte(XmlElement ChangedElement)
        {
            ((StandardDeck)WolfBox).Deck = ChangedElement as Deck;
            OnKarteChanged();
        }
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            return new StandardDeck(Karte, Ppm);
        }
        protected override bool ChangeSize()
        {
            SizeF size = PictureBox.Size;
            Size Size = size.Max(1, 1).ToSize();
            if (Size.Equals(LastSize))
                return false;
            LastSize = Size;
            PictureBox.Image = new Bitmap(Size.Width, Size.Height);
            g = PictureBox.Image.GetHighGraphics(WolfBox.Faktor / ppm);
            DrawContext = new DrawContextGraphics(g, Brushes.White);
            return true;
        }
    }
}
