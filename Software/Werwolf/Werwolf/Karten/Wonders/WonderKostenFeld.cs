using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistment.Extensions;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten
{
    public class WonderKostenFeld : WonderTextFeld
    {
        private string LastKosten;
        private static FontGraphicsMeasurer Font = new FontGraphicsMeasurer("Calibri", 14); // 16

        public WonderKostenFeld(Karte Karte, float Ppm)
            : base(Karte, Ppm, true, false, false, Karte.Universe.TextBilder["KleinesNamenfeldTransponiert"])
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
            else
            {
                string Kosten = Karte.Kosten.ToString();
                if (!Kosten.Equals(LastKosten))
                {
                    this.LastKosten = Kosten;
                    Text[] KostenText = Karte.Kosten.ProduceTexts(Font);
                    if (KostenText.Length > 0)
                    {
                        for (int i = KostenText[0].Count() - 1; i >= 0; i--)
                            KostenText[0].insert(i, new Whitespace(1 * Faktor, 1 * Faktor, true));
                        DrawBox = KostenText[0].Geometry(Faktor, Faktor, Faktor, Faktor * 5);
                    }
                    else
                        DrawBox = null;
                    DrawBoxChanged = true;
                }
            }
        }
    }
}
