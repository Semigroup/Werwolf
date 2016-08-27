using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using Assistment.form;
using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Forms
{
    public abstract class PreForm<T> : Form where T : XmlElement, new()
    {
        private DrawContextGraphics DrawContext;
        private StandardKarte StandardKarte;
        private Karte karte;
        public Karte Karte
        {
            get { return karte; }
            set
            {
                karte = value;
                OnKarteChanged(EventArgs.Empty);
            }
        }
     
        protected T element;
        public T Element
        {
            get { return element; }
            set
            {
                element = value;
                UpdateWerteListe();
            }
        }
        public ElementMenge<T> Menge { get; set; }

        public PictureBox KartenBox = new PictureBox();
        public WerteListe WerteListe = new WerteListe();
        public Button OkButton = new Button();
        public Button AbbrechenButton = new Button();

        public event EventHandler UserValueChanged = delegate { };

        public PreForm()
        {
            StandardKarte.Ppm = 10;
            KartenBox.Dock = DockStyle.Left;
            KartenBox.SizeMode = PictureBoxSizeMode.StretchImage;
            Controls.Add(KartenBox);

            WerteListe.Size = new Size(500, 650);
            WerteListe.Location = new Point(500, 50);
            BuildWerteListe();
            WerteListe.Setup();
            WerteListe.UserValueChanged += (sender, e) => OnUserValueChanged(e);
            Controls.Add(WerteListe);

            OkButton.Size = new Size(100, 40);
            OkButton.Location = new Point(500, 710);
            OkButton.Text = "Übernehmen";
            OkButton.Click += new EventHandler(OkButton_Click);
            Controls.Add(OkButton);

            AbbrechenButton.Size = new Size(100, 40);
            AbbrechenButton.Location = new Point(600, 710);
            AbbrechenButton.Text = "Abbrechen";
            AbbrechenButton.Click += new EventHandler(AbbrechenButton_Click);
            Controls.Add(AbbrechenButton);

            this.ClientSize = new Size(1000, 800);
        }

        void AbbrechenButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        public abstract void UpdateWerteListe();
        public abstract void UpdateElement();
        public abstract void BuildWerteListe();
        public virtual void Redraw()
        {
            if (karte != null)
            {
                StandardKarte.setup(StandardKarte.InnenBox.Width);
                StandardKarte.draw(DrawContext);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (karte == null)
                return;
            KartenBox.Width = (int)(Karte.HintergrundDarstellung.Size.ratio() * KartenBox.Height);
            StandardKarte.Ppm = 10;//((SizeF)KartenBox.Size).div(karte.Darstellung.Size).Min() / WolfBox.Faktor;
            Size s = StandardKarte.InnenBox.Size.ToSize();
            KartenBox.Image = new Bitmap(s.Width, s.Height);
            DrawContext = new DrawContextGraphics(KartenBox.Image.GetHighGraphics());
            Redraw();
        }

        protected virtual void OnUserValueChanged(EventArgs e)
        {
            UserValueChanged(this, EventArgs.Empty);
            UpdateElement();
            Redraw();
        }
        protected virtual void OnKarteChanged(EventArgs e)
        {
            StandardKarte.Karte = karte;
            OnSizeChanged(e);
            Redraw();
        }
    }
}
