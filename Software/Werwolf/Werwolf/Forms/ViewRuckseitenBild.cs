
using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Forms
{
    public class ViewRuckseitenBild : ViewBox
    {
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            if (Karte == null)
                return new StandardRuckseite(Karte, Ppm);
            else
                return Karte.GetRuckSeite(Ppm);
        }
    }
}
