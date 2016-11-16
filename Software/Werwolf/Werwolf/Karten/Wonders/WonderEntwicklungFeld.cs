using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;
using System.Drawing;

namespace Werwolf.Karten
{
    public class WonderEntwicklungFeld : WonderTextFeld
    {
        private string LastName;
        public readonly int Index;
        private FontGraphicsMeasurer SmallFont;

        public WonderEntwicklungFeld(Karte Karte, float Ppm, int Index)
            : base(Karte, Ppm, false, true, Karte.Universe.TextBilder["KleinesNamenfeld"])
        {
            this.Index = Index;
            OnKarteChanged();
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null || Karte.Entwicklungen.Length <= Index)
            {
                DrawBox = null;
                return;
            }
            else if (!Karte.Entwicklungen[Index].Schreibname.Equals(LastName))
            {
                //SmallFont = (Karte.Entwicklungen[Index].TitelDarstellung.FontMeasurer as FontGraphicsMeasurer) * 0.5f;
                SmallFont = new FontGraphicsMeasurer(Karte.Entwicklungen[Index].TitelDarstellung.Font.Name, 6);
                this.LastName = Karte.Entwicklungen[Index].Schreibname;
                string color = Karte.Entwicklungen[Index].HintergrundDarstellung.Farbe.tween(Color.Black, 0.5f).ToHexString();
                DrawBox = new Text(@"\c" + color + Karte.Entwicklungen[Index].Schreibname, SmallFont) // + ""
                    .Geometry(0.5f * Faktor, 0.5f * Faktor, 2.5f * Faktor, 0.5f * Faktor); // 
                DrawBoxChanged = true;
            }
        }
    }
}
