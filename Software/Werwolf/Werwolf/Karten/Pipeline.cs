using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class Pipeline : WolfBox
    {
        private WolfBox[] Layers = new WolfBox[7];

        public BildBox Hintergrund
        {
            get { return (BildBox)Layers[0]; }
            set { Layers[0] = value; }
        }
        public BildBox Bild
        {
            get { return (BildBox)Layers[1]; }
            set { Layers[1] = value; }
        }
        public WolfText Text
        {
            get { return (WolfText)Layers[2]; }
            set { Layers[2] = value; }
        }
        public WolfTitel Titel
        {
            get { return (WolfTitel)Layers[3]; }
            set { Layers[3] = value; }
        }
        public InfoBox Gesinnung
        {
            get { return (InfoBox)Layers[4]; }
            set { Layers[4] = value; }
        }
        public InfoBox Artist
        {
            get { return (InfoBox)Layers[5]; }
            set { Layers[5] = value; }
        }
        public RandBox Rand
        {
            get { return (RandBox)Layers[6]; }
            set { Layers[6] = value; }
        }

        public RectangleF Innenbox { get; private set; }
        public RectangleF InnenboxFaktor { get; private set; }

        public Pipeline(Karte Karte)
            : base(Karte)
        {
            this.Hintergrund = new BildBox(Karte, Karte.Fraktion.Bild, BildBox.Modus.Hintergrund);
            this.Bild = new BildBox(Karte, Karte.Bild, BildBox.Modus.Hauptbild);
            this.Text = new WolfText(Karte);
            this.Titel = new WolfTitel(Karte);
            this.Gesinnung = new InfoBox(Karte);
            this.Artist = new InfoBox(Karte);
            this.Rand = new RandBox(Karte);
        }

        public override float getSpace()
        {
            return Karte.Darstellung.Size.Inhalt() * Faktor * Faktor;
        }
        public override float getMin()
        {
            return Karte.Darstellung.Size.Width * Faktor;
        }
        public override float getMax()
        {
            return Karte.Darstellung.Size.Width * Faktor;
        }
        public override void update()
        {
            foreach (var item in Layers)
                item.update();
        }

        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Height = Karte.Darstellung.Size.Height * Faktor;
            this.Innenbox = new RectangleF(Darstellung.Hintergrund.Rand.ToPointF(), box.Size.sub(Darstellung.Hintergrund.Rand.mul(2)));
            this.InnenboxFaktor = Innenbox.mul(Faktor);

            SetupHintergrund();
            SetupTitel();
            SetupInfo();
            SetupText();
            SetupBild();
        }
        public void SetupHintergrund()
        {
            Hintergrund.setup(InnenboxFaktor);
        }
        public void SetupBild()
        {
            Bild.setup(InnenboxFaktor);
        }
        public void SetupText()
        {
            Text.setup(InnenboxFaktor);
        }
        public void SetupInfo()
        {
            CString cs = new CString();
            cs.add(Artist);
            cs.add(Gesinnung);
            cs.setup(InnenboxFaktor);

            Artist.box.Y += InnenboxFaktor.Height - Artist.box.Height;
            Gesinnung.box.Y += InnenboxFaktor.Height - Gesinnung.box.Height;
            Gesinnung.box.X += InnenboxFaktor.Width - Gesinnung.box.Width;
        }
        public void SetupTitel()
        {
            Titel.setup(InnenboxFaktor);
            Titel.box.X += (InnenboxFaktor.Width - Titel.box.Width) * 0.5f;
        }

        public override void draw(DrawContext con)
        {
            for (int i = 0; i < Layers.Length; i++)
                Layers[i].draw(con);
        }

        public override void OnKarteChanged()
        {
            foreach (var item in Layers)
                if (item != null)
                    item.OnKarteChanged();
        }
        public override void OnPpmChanged()
        {
            foreach (var item in Layers)
                item.OnPpmChanged();
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
}
