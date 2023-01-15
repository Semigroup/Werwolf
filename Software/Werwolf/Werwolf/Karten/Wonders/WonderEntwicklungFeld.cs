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
        //private static string[] PWeiss;
        //private static string[] PSchwarz;

        private string LastName;
        public readonly int Index;
        private FontGraphicsMeasurer SmallFont;
        public override Bild FeldBild
        {
            get { return Karte.LayoutDarstellung.KleinesNamenfeld; }
        }

        //static WonderEntwicklungFeld()
        //{
        //    PSchwarz = new string[14];
        //    PSchwarz.CountMap(i => "P" + i);
        //    PWeiss = PSchwarz.Map(x => x + "W").ToArray();
        //}

        public WonderEntwicklungFeld(Karte Karte, float Ppm, int Index)
            : base(Karte, Ppm, false, true, false )
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
                Karte Entwicklung = Karte.Entwicklungen[Index];
                SmallFont = new FontGraphicsMeasurer(Entwicklung.TitelDarstellung.Font.Name, 8);
                FontGraphicsMeasurer EffektFont = new FontGraphicsMeasurer(Entwicklung.TitelDarstellung.Font.Name, 8);
                this.LastName = Entwicklung.Schreibname;
                string color = Entwicklung.HintergrundDarstellung.Farbe.tween(Color.Black, 0.5f).ToHexString();

                Text Text = new Text();
                //if (Entwicklung.Effekt.Anzahl > 0)
                //{
                //    Text Effekt = Entwicklung.Effekt.Replace(PWeiss, PSchwarz).ProduceTexts(EffektFont)[0];
                //    Effekt.RemoveWhitespaces();
                //    Text.addRange(Effekt);
                //    Text.addWhitespace(EffektFont.getWhitespace());
                //}
                Text.AddFormat(@"\c" + color + Entwicklung.Schreibname, SmallFont);
                DrawBox = Text.Geometry(0.5f * Faktor, 0.5f * Faktor, 2.5f * Faktor, 0.5f * Faktor);
                DrawBoxChanged = true;
            }
        }
    }
}
