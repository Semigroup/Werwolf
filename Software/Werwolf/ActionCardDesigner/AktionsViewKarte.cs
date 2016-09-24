using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Werwolf.Karten;
using Werwolf.Forms;
using Werwolf.Inhalt;

namespace ActionCardDesigner
{
    public class AktionsViewKarte : ViewKarte
    {
        public AktionsViewKarte()
            : base()
        {

        }
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            if (Karte == null)
                return new StandardAktionsKarte(Karte as AktionsKarte, Ppm);
            else
                return Karte.GetVorderSeite(Ppm);
        }
    }
    public class AktionsViewDeck : ViewDeck
    {
        public override void ChangeKarte(XmlElement ChangedElement)
        {
            ((StandardAktionsDeck)WolfBox).AktionsDeck = ChangedElement as AktionsDeck;
            OnKarteChanged();
        }
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            return new StandardAktionsDeck(Karte, Ppm);
        }
    }
}
