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
    public class HintergrundDarstellungForm : DarstellungForm<HintergrundDarstellung>
    {
        public HintergrundDarstellungForm(Karte Karte)
            : base(Karte)
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();

            UpdatingWerteListe = true;
            WerteListe.AddChainedSizeFBox(new SizeF(), "Randgröße in mm", true, Settings.MaximumKarteSize);

            WerteListe.AddColorBox(Color.Black, "Randfarbe");
            WerteListe.AddColorBox(Color.White, "Hintergrundfarbe");
            WerteListe.AddColorBox(Color.White, "Farbe Rückseite");

            WerteListe.AddBoolBox(false, "Hat Runde Ecken?");
            WerteListe.AddBoolBox(false, "Aktionskarte");
            WerteListe.AddChainedSizeFBox(new SizeF(), "Größe in mm", true, Settings.MaximumKarteSize);
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
            WerteListe.SetValue("Farbe Rückseite", element.RuckseitenFarbe);

            WerteListe.SetValue("Hat Runde Ecken?", element.RundeEcken);
            WerteListe.SetValue("Aktionskarte", element.AktionsKarte);
            WerteListe.SetValue("Größe in mm", element.Size);
            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            if (UpdatingWerteListe)
                return;
            base.UpdateElement();
            element.Rand = WerteListe.GetValue<SizeF>("Randgröße in mm");

            element.RandFarbe = WerteListe.GetValue<Color>("Randfarbe");
            element.Farbe = WerteListe.GetValue<Color>("Hintergrundfarbe");
            element.RuckseitenFarbe = WerteListe.GetValue<Color>("Farbe Rückseite");

            element.RundeEcken = WerteListe.GetValue<bool>("Hat Runde Ecken?");
            element.AktionsKarte = WerteListe.GetValue<bool>("Aktionskarte");
            element.Size = WerteListe.GetValue<SizeF>("Größe in mm");
        }
    }
}
