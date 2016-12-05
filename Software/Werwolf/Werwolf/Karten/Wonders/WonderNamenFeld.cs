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
        public xFont Font;

        public WonderNamenFeld(Karte Karte, float Ppm)
            : this(Karte, false, false, Ppm)
        {

        }
        public WonderNamenFeld(Karte Karte, bool oben, bool AufKopf, float Ppm)
            : base(Karte, Ppm, oben, true, AufKopf,
            AufKopf ? Karte.Universe.TextBilder["GroßesNamenfeldTransponiert"] : Karte.Universe.TextBilder["GroßesNamenfeld"])
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
                UpdateDrawBox();
            }
        }
        public void UpdateDrawBox()
        {
            DrawBox = new Text(Karte.Schreibname, Font == null ? Karte.TitelDarstellung.FontMeasurer : Font)
                    .FirstLine()
                    .Geometry(Faktor, Faktor, Faktor * 5, Faktor);
            DrawBoxChanged = true;
        }
    }
}
