﻿using System.Drawing;
using Assistment.form;
using Assistment.Drawing.Geometries.Extensions;

using Werwolf.Inhalt;

namespace Werwolf.Forms
{
    public class TextDarstellungForm : DarstellungForm<TextDarstellung>
    {
        public TextDarstellungForm(Karte Karte)
            : base(Karte)
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();
            UpdatingWerteListe = true;
            WerteListe.AddChainedSizeFBox(new SizeF(), "Randgröße in mm");
            WerteListe.AddColorBox(Color.Black, "Randfarbe");
            WerteListe.AddColorBox(Color.White, "Hintergrundfarbe");
            WerteListe.AddFontBox(new Font("Calibri", 8), "Font");
            WerteListe.AddFloatBox(1, "Balkendicke in mm");
            WerteListe.AddFloatBox(1, "Innenradius in mm");
            WerteListe.AddFontBox(new Font("Calibri", 8), "Effekt Font");
            WerteListe.AddRectangleFBox(new RectangleF(), "Text Region");
            WerteListe.AddLabelBox("", "");
            WerteListe.AddColorBox(Color.Black, "Schattenfarbe");
            WerteListe.AddChainedSizeFBox(new SizeF(), "Shattenversatz");
            WerteListe.AddBoolBox(false, "Textschatten Aktiv");

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }
        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
            UpdatingWerteListe = true;
            WerteListe.SetValue("Randgröße in mm", element.Rand);
            WerteListe.SetValue("Randfarbe", element.RandFarbe);
            WerteListe.SetValue("Hintergrundfarbe", element.Farbe);
            WerteListe.SetValue("Font", element.Font);
            WerteListe.SetValue("Balkendicke in mm", element.BalkenDicke);
            WerteListe.SetValue("Innenradius in mm", element.InnenRadius);
            WerteListe.SetValue("Effekt Font", element.EffektFont);
            WerteListe.SetValue("Text Region", element.TextRectangle);
            WerteListe.SetValue("Schattenfarbe", element.ShadowColor);
            WerteListe.SetValue("Shattenversatz", element.ShadowOffset.ToSize());
            WerteListe.SetValue("Textschatten Aktiv", element.ShadowIsActive);
            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            base.UpdateElement();
            if (UpdatingWerteListe)
                return;
            element.Rand = WerteListe.GetValue<SizeF>("Randgröße in mm");
            element.RandFarbe = WerteListe.GetValue<Color>("Randfarbe");
            element.Farbe = WerteListe.GetValue<Color>("Hintergrundfarbe");
            element.Font = WerteListe.GetValue<Font>("Font");
            element.EffektFont = WerteListe.GetValue<Font>("Effekt Font");
            element.BalkenDicke = WerteListe.GetValue<float>("Balkendicke in mm");
            element.InnenRadius = WerteListe.GetValue<float>("Innenradius in mm");
            element.TextRectangle = WerteListe.GetValue<RectangleF>("Text Region");
            element.ShadowColor = WerteListe.GetValue<Color>("Schattenfarbe");
            element.ShadowOffset = WerteListe.GetValue<SizeF>("Shattenversatz").ToPointF();
            element.ShadowIsActive = WerteListe.GetValue<bool>("Textschatten Aktiv");
        }

        protected override void SetVisibles()
        {
            SetVisible(Karte.KartenModus.WondersKarte, "Effekt Font");
            SetVisible(Karte.ModernIrgendwas | Karte.AlchemieIrgendwas, "Text Region");
            SetVisible(Karte.ModernIrgendwas | Karte.AlchemieIrgendwas, 
                "Schattenfarbe", "Shattenversatz", "Textschatten Aktiv");
        }
    }
}
