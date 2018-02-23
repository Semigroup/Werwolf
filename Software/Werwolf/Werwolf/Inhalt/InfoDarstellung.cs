namespace Werwolf.Inhalt
{
    public class InfoDarstellung : Darstellung
    {
        public InfoDarstellung()
            : base("InfoDarstellung")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
        }
        public override void AdaptToCard(Karte Karte)
        {
            Karte.InfoDarstellung = this;
        }
        public override object Clone()
        {
            InfoDarstellung hg = new InfoDarstellung();
            Assimilate(hg);
            return hg;
        }
    }
}
