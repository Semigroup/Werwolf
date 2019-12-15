using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using Assistment.form;
using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Forms
{
    public class InfoDarstellungForm : DarstellungForm<InfoDarstellung>
    {
        public InfoDarstellungForm(Karte Karte)
            : base(Karte)
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();
            UpdatingWerteListe = true;

            WerteListe.AddPointFBox(new PointF(), "Position");
            WerteListe.AddChainedSizeFBox(new SizeF(), "Randgröße in mm");
            WerteListe.AddFontBox(new Font("Calibri", 11), "Font");
            WerteListe.AddColorBox(Color.White, "Hintergrundfarbe");
            WerteListe.AddColorBox(Color.Black, "Textfarbe");
            WerteListe.AddChainedSizeFBox(new SizeF(), "Größe in mm");

            WerteListe.AddLabelBox("", "");

            WerteListe.AddPointFBox(new PointF(), "Position2");
            WerteListe.AddChainedSizeFBox(new SizeF(), "Randgröße2 in mm");
            WerteListe.AddFontBox(new Font("Calibri", 11), "Font2");
            WerteListe.AddColorBox(Color.White, "Hintergrundfarbe2");
            WerteListe.AddColorBox(Color.Black, "Textfarbe2");
            WerteListe.AddChainedSizeFBox(new SizeF(), "Größe2 in mm");
            UpdatingWerteListe = false;
            WerteListe.Setup();
        }
        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
            UpdatingWerteListe = true;
            WerteListe.SetValue("Hintergrundfarbe", element.Farbe);
            WerteListe.SetValue("Textfarbe", element.TextFarbe);
            WerteListe.SetValue("Position", element.Position);
            WerteListe.SetValue("Randgröße in mm", element.Rand);
            WerteListe.SetValue("Font", element.Font);
            WerteListe.SetValue("Größe in mm", element.Grosse);

            WerteListe.SetValue("Hintergrundfarbe2", element.Farbe2);
            WerteListe.SetValue("Textfarbe2", element.TextFarbe2);
            WerteListe.SetValue("Position2", element.Position2);
            WerteListe.SetValue("Randgröße2 in mm", element.Rand2);
            WerteListe.SetValue("Font2", element.Font2);
            WerteListe.SetValue("Größe2 in mm", element.Grosse2);
            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            if (UpdatingWerteListe)
                return;
            base.UpdateElement();
            element.Position = WerteListe.GetValue<PointF>("Position");
            element.Rand = WerteListe.GetValue<SizeF>("Randgröße in mm");
            element.Farbe = WerteListe.GetValue<Color>("Hintergrundfarbe");
            element.TextFarbe = WerteListe.GetValue<Color>("Textfarbe");
            element.Font = WerteListe.GetValue<Font>("Font");
            element.Grosse = WerteListe.GetValue<SizeF>("Größe in mm");

            element.Position2 = WerteListe.GetValue<PointF>("Position2");
            element.Rand2 = WerteListe.GetValue<SizeF>("Randgröße2 in mm");
            element.Farbe2 = WerteListe.GetValue<Color>("Hintergrundfarbe2");
            element.TextFarbe2 = WerteListe.GetValue<Color>("Textfarbe2");
            element.Font2 = WerteListe.GetValue<Font>("Font2");
            element.Grosse2 = WerteListe.GetValue<SizeF>("Größe2 in mm");
        }

        protected override void SetVisibles()
        {
            SetVisible(Karte.KartenModus.ModernWolfKarte | Karte.AlchemieIrgendwas, "Textfarbe");
            SetVisible(Karte.AlchemieIrgendwas, "Position", "Größe in mm",
                "Randgröße2 in mm", "Position2", "Hintergrundfarbe2", "Textfarbe2", "Font2", "Größe2 in mm");
        }
    }
}
