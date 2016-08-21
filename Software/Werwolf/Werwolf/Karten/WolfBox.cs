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

        public const float Faktor = 5;

        protected Karte karte;
        public Karte Karte { get { return karte; } set { karte = value; OnKarteChanged(); } }

        protected float ppm = 1;
        public float Ppm { get { return ppm; } set { ppm = value; OnPpmChanged(); } }

        public Darstellung Darstellung { get { return karte.Darstellung; } }
        public TitelDarstellung TitelDarstellung { get { return karte.Darstellung.Titel; } }
        public HintergrundDarstellung HintergrundDarstellung { get { return karte.Darstellung.Hintergrund; } }
        public TextDarstellung TextDarstellung { get { return karte.Darstellung.Text; } }
        public BildDarstellung BildDarstellung { get { return karte.Darstellung.Bild; } }
        public InfoDarstellung InfoDarstellung { get { return karte.Darstellung.Info; } }

        public RectangleF AussenBox { get; private set; }
        public RectangleF InnenBox { get; private set; }

        public WolfBox(Karte Karte)
        {
            this.Karte = Karte;
        }

        public virtual void OnKarteChanged()
        {
            AussenBox = new RectangleF(new PointF(), Darstellung.Size).mul(Faktor);
            InnenBox = new RectangleF(HintergrundDarstellung.Rand.ToPointF(), 
                AussenBox.Size.sub(HintergrundDarstellung.Rand.mul(2))).mul(Faktor);
        }
        public virtual void OnPpmChanged()
        {
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
