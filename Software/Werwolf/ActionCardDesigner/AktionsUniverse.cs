using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Werwolf.Inhalt;

namespace ActionCardDesigner
{
    public class AktionsUniverse : Universe
    {
        public ElementMenge<AktionsKarte> AktionsKarten { get; private set; }
        public ElementMenge<AktionsDeck> AktionsDecks { get; private set; }

        public AktionsUniverse()
        {
            AktionsKarten = new ElementMenge<AktionsKarte>("AktionsKarten", this);
            AktionsDecks = new ElementMenge<AktionsDeck>("AktionsDecks", this);
        }

        protected override Menge[] GetMengen()
        {
            Menge[] m = base.GetMengen();
            m[11] = AktionsKarten;
            m[12] = AktionsDecks;
            return m;
        }
        public override void Rescue()
        {
            foreach (var item in new Menge[] { AktionsDecks, AktionsKarten, Fraktionen, Gesinnungen })
                item.Rescue();
        }
    }
}
