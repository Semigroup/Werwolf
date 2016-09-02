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

namespace Werwolf.Forms
{
    public partial class ViewCard : UserControl
    {
        private int DelayStep = 1;
        private int CurrentDelay = 0;

        private float ppm = 8;

        private Karte karte;
        public Karte Karte
        {
            get { return karte; }
            set
            {
                karte = value;
                Size s = karte.GetPictureSize(ppm);
                pictureBox1.Image = new Bitmap(s.Width, s.Height);
                DrawContext = new DrawContextGraphics(pictureBox1.Image.GetHighGraphics(ppm / WolfBox.Faktor), Brushes.White);
                StandardKarte = new StandardKarte(karte, ppm);
                OnKarteChanged();
            }
        }
        private DrawContextGraphics DrawContext;
        private StandardKarte StandardKarte;

        public ViewCard()
        {
            InitializeComponent();
        }
        public void ChangeKarte(XmlElement ChangedElement)
        {
            if (ChangedElement != null)
            {
                ChangedElement.AdaptToCard(karte);
                OnKarteChanged();
            }
        }
        public void OnKarteChanged()
        {
            CurrentDelay = DelayStep;
        }
        public override void Refresh()
        {
            DrawContext.g.Clear(Color.White);
            StandardKarte.OnKarteChanged();
            StandardKarte.setup(0);
            StandardKarte.draw(DrawContext);
            base.Refresh();
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
}
