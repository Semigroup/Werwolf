using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assistment.Texts;

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

        public WolfBox(Karte Karte)
        {
            this.Karte = Karte;
        }

        public virtual void OnKarteChanged()
        {
        }
        public virtual void OnPpmChanged()
        {
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }
    }
}
