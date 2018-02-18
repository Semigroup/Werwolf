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
        public override Bild FeldBild
        {
            get { return Karte.LayoutDarstellung.GetGrossesNamenfeld(AufKopf); }
        }
        private string LastName;
        public xFont Font;
        public xFont FontToUse
        {
            get
            {
                return Font == null ? TitelDarstellung.FontMeasurer : Font;
            }
        }

        public WonderNamenFeld(Karte Karte, float Ppm)
            : this(Karte, false, false, Ppm)
        {

        }
        public WonderNamenFeld(Karte Karte, bool oben, bool AufKopf, float Ppm)
            : base(Karte, Ppm, oben, true, AufKopf)
        {

        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null || !Karte.TitelDarstellung.Existiert)
            {
                DrawBox = null;
                return;
            }
            else if (!Karte.Name.Equals(LastName) || !FontToUse.GetFont().Equals(LastFont))
            {
                this.LastName = Karte.Name;
                this.LastFont = FontToUse.GetFont();
                UpdateDrawBox();
            }
        }
   
        public void UpdateDrawBox()
        {
            DrawBox = new Text(Karte.Schreibname, FontToUse)
                    .FirstLine()
                    .Geometry(Faktor, Faktor, Faktor * 5, Faktor);
            DrawBoxChanged = true;
        }
    }
}
