using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten
{
    public class WonderBasenFeld : WonderTextFeld
    {
        private string LastName;
        public int Index;
        private FontGraphicsMeasurer SmallFont;

        public WonderBasenFeld(Karte Karte, float Ppm, int Index)
            : base(Karte, Ppm, true, true, Karte.Universe.TextBilder["KleinesNamenfeld"])
        {
            this.Index = Index;
            OnKarteChanged();
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null || Karte.Basen.Length <= Index)
                DrawBox = null;
            else if (!Karte.Basen[Index].Schreibname.Equals(LastName))
            {
                //SmallFont = (Karte.Basen[Index].TitelDarstellung.FontMeasurer as FontGraphicsMeasurer) * 0.4f;
                SmallFont = new FontGraphicsMeasurer(Karte.Basen[Index].TitelDarstellung.Font.Name, 6);
                this.LastName = Karte.Basen[Index].Schreibname;
                string color = Karte.Basen[Index].HintergrundDarstellung.Farbe.ToHexString();
                DrawBox = new Text("\\c" + color + LastName, SmallFont)
                    .Geometry(2.5f * Faktor, 0.5f * Faktor, 0.5f * Faktor, 0.5f * Faktor);
                DrawBoxChanged = true;
            }
        }
    }
}
