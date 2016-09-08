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
    public abstract class WolfBox : DrawBox
    {
        protected static Random Random = new Random();

        public static float Faktor = Settings.WolfBoxFaktor;

        protected Karte karte;
        public Karte Karte { get { return karte; } set { karte = value; OnKarteChanged(); } }

        protected float ppm = 1;
        public float Ppm { get { return ppm; } set { ppm = value; OnPpmChanged(); } }

        public TitelDarstellung TitelDarstellung { get { return karte.TitelDarstellung; } }
        public HintergrundDarstellung HintergrundDarstellung { get { return karte.HintergrundDarstellung; } }
        public TextDarstellung TextDarstellung { get { return karte.TextDarstellung; } }
        public BildDarstellung BildDarstellung { get { return karte.BildDarstellung; } }
        public InfoDarstellung InfoDarstellung { get { return karte.InfoDarstellung; } }

        public RectangleF AussenBox { get; private set; }
        public RectangleF InnenBox { get; private set; }

        public WolfBox(Karte Karte)
        {
            this.Karte = Karte;
        }
        public WolfBox(Karte Karte, float Ppm)
        {
            this.ppm = Ppm;
            this.Karte = Karte;
        }

        public virtual void OnKarteChanged()
        {
            if (karte == null)
                return;
            AussenBox = new RectangleF(new PointF(), HintergrundDarstellung.Size).mul(Faktor);
            InnenBox = AussenBox.Inner(HintergrundDarstellung.Rand.mul(Faktor));
        }
        public virtual void OnPpmChanged()
        {
        }
        public virtual bool Visible()
        {
            return Karte != null && ppm > 0;
        }
        public virtual void DrawRessources()
        {
        }
        public override void draw(DrawContext con)
        {
            DrawRessources();
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }
        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }

        public override float getMax()
        {
            throw new NotImplementedException();
        }
        public override float getMin()
        {
            throw new NotImplementedException();
        }
        public override float getSpace()
        {
            throw new NotImplementedException();
        }
    }
}
