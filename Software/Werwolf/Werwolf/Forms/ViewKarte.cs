
using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Forms
{
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
}
