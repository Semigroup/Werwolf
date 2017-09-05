using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Inhalt;
using Assistment.Texts;

namespace Werwolf.Karten.CyberAktion
{
    public class CyberWaffenHeader : CyberAktionHeader
    {
        public CyberWaffenHeader(Karte Karte, float Ppm) 
            : base(Karte, Ppm, 5)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null|| Lines == null) return;
            Text[] schaden = Karte.Effekt.ProduceTexts(Font);
            Text[] gesinnung = Karte.Gesinnung.Aufgabe.ProduceTexts(Font);
            this[2, 1] = schaden.Length > 0 ? schaden[0].Geometry(InnenRadius * Faktor) : null;
            this[3, 0] = GetZielSicherheiten().Geometry(InnenRadius * Faktor);
            this[4, 0] = gesinnung.Length > 0 ? gesinnung[0].Geometry(InnenRadius * Faktor) : null;
            this[4, 1] = gesinnung.Length > 1 ? gesinnung[1].Geometry(InnenRadius * Faktor) : null;
        }

        private Text GetZielSicherheiten()
        {
            Text text = new Text("", Font);
            text.add(new WolfTextBild(Layout.ZielSicherheitenSchutze, Font));
            string pure = Karte.ZielSicherheiten.Replace(" ", "");
            for (int i = 0; i < pure.Length; i++)
            {
                if (i % 4 == 0)
                    text.addWort(" ");
                text.add(GetZielSicherheit(pure[i]));
            }
            return text;
        }
        private WolfTextBild GetZielSicherheit(char a)
        {
            TextBild tb;
            if ('0' <= a && a <= '9')
                tb = Layout.ZielSicherheiten[a - '0' + 9];
            else if ('a' <= a && a <= 'i')
                tb = Layout.ZielSicherheiten['a' - a + 8];
            else if (a == '+')
                tb = Layout.ZielSicherheiten[10];
            else if (a == '+')
                tb = Layout.ZielSicherheiten[8];
            else
                throw new NotImplementedException();

            return new WolfTextBild(tb, Font);
        }
    }
}
