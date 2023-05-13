using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Texts;
using Assistment.form;
using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Forms
{
    public class BildDarstellungForm : DarstellungForm<BildDarstellung>
    {
        public BildDarstellungForm( Karte Karte)
            : base( Karte)
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();

            UpdatingWerteListe = true;

            WerteListe.AddEnumBox(BildDarstellung.Filter.Keiner, "Farb Filter");
            WerteListe.AddColorBox(Color.White, "Farbe erster Dimension");
            WerteListe.AddColorBox(Color.White, "Farbe zweiter Dimension");

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }
        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
            if (element == null)
                return;
            UpdatingWerteListe = true;

            WerteListe.SetValue("Farb Filter", element.MyFilter as object);
            WerteListe.SetValue("Farbe erster Dimension", element.ErsteFilterFarbe);
            WerteListe.SetValue("Farbe zweiter Dimension", element.ZweiteFilterFarbe);

            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            base.UpdateElement();
            if (element == null || UpdatingWerteListe)
                return;

            element.MyFilter = (BildDarstellung.Filter)WerteListe.GetValue<object>("Farb Filter");
            element.ErsteFilterFarbe = WerteListe.GetValue<Color>("Farbe erster Dimension");
            element.ZweiteFilterFarbe = WerteListe.GetValue<Color>("Farbe zweiter Dimension");
        }
    }
}
