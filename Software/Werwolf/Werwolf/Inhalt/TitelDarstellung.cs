using System.Drawing;

namespace Werwolf.Inhalt
{
    public class TitelDarstellung : Darstellung
    {
        public TitelDarstellung()
            : base("TitelDarstellung")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Farbe = Color.White;
            Font = new Font("Exocet", 14);
        }
        public override void AdaptToCard(Karte Karte)
        {
            Karte.TitelDarstellung = this;
        }
        public override object Clone()
        {
            TitelDarstellung hg = new TitelDarstellung();
            Assimilate(hg);
            return hg;
        }
    }
}
