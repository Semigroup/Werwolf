using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Werwolf.Inhalt;
using Assistment.Texts;

namespace Werwolf.Karten
{
    public class WonderNamenFeld : WonderTextFeld
    {
        private string LastName;

        public WonderNamenFeld(Karte Karte, float Ppm)
            : base(Karte, Ppm, false, true, Karte.Universe.TextBilder["GroßesNamenfeld"])
        {

        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
            {
                DrawBox = null;
                return;
            }
            else if (!Karte.Name.Equals(LastName))
            {
                this.LastName = Karte.Name;
                DrawBox = new Text(Karte.Schreibname, Karte.TitelDarstellung.FontMeasurer)
                    .Geometry(Faktor, Faktor, Faktor * 5, Faktor);
                DrawBoxChanged = true;
            }
        }
    }
}
