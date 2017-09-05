using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Inhalt;
using Assistment.Texts;

namespace Werwolf.Karten.CyberAktion
{
    public abstract class CyberAktionHeader : DoppelTabTextBox
    {
        public xFont Font => Karte.TitelDarstellung.FontMeasurer;
        public LayoutDarstellung Layout => Karte.LayoutDarstellung;
        public override Darstellung Darstellung => Karte.TitelDarstellung;
        public virtual float InnenRadius => 0.3f;

        public CyberAktionHeader(Karte Karte, float Ppm, int lines)
            : base(Karte, Ppm, lines)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null||Lines == null) return;
            Text[] texts = Karte.AktionsName.ProduceTexts(Font);
            Text[] kosten = Karte.Kosten.ProduceTexts(Font);
            this[0, 0] = texts.Length > 0 ? texts[0].Geometry(InnenRadius * Faktor) : null ;
            this[0, 1] = new Text(Karte.Fraktion.Schreibname, Font).Geometry(InnenRadius * Faktor);
            this[1, 0] = texts.Length > 1 ? texts[1].Geometry(InnenRadius * Faktor) : null;
            this[1, 1] = kosten.Length > 0 ? kosten[0].Geometry(InnenRadius * Faktor) : null;
            Text inRen = new Text("", Font);
            inRen.addWort(Karte.Initiative.ToString("F1"));
            inRen.add(new WolfTextBild(Layout.Initiative, Font));
            inRen.addWort(" ");
            inRen.addWort(Karte.Felder);
            inRen.add(new WolfTextBild(Layout.Felder, Font));
            this[2, 0] = inRen.Geometry(InnenRadius * Faktor);
        }
    }
}
