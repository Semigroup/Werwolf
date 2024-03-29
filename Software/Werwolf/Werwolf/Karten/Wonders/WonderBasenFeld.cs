﻿using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.Geometries.Extensions;
using System.Drawing;

namespace Werwolf.Karten
{
    public class WonderBasenFeld : WonderTextFeld
    {
        private string LastName;
        public int Index;
        private FontGraphicsMeasurer SmallFont;
        public override Bild FeldBild
        {
            get { return Karte.LayoutDarstellung.KleinesNamenfeldTransponiert; }
        }

        public WonderBasenFeld(Karte Karte, float Ppm, int Index)
            : base(Karte, Ppm, true, true, false)
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
                Karte Basis = Karte.Basen[Index];
                SmallFont = new FontGraphicsMeasurer(Basis.TitelDarstellung.Font.Name, 8);
                this.LastName = Basis.Schreibname;
                string color = Basis.HintergrundDarstellung.Farbe.tween(Color.Black, 0.5f).ToHexString();
                DrawBox = new Text("\\c" + color + LastName, SmallFont)
                    .Geometry(2.5f * Faktor, 0.5f * Faktor, 0.5f * Faktor, 0.5f * Faktor);
                DrawBoxChanged = true;
            }
        }
    }
}
