using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Assistment.form;

using Werwolf.Inhalt;

namespace Werwolf.Forms
{
    public class UnterDarstellungForm<T> : PreForm<T> where T : UnterDarstellung
    {
        public override void UpdateWerteListe()
        {
            WerteListe.SetValue("Existiert", element.Existiert);
            WerteListe.SetValue("Font", element.Font);
            WerteListe.SetValue("HatRand", element.HatRand);
            WerteListe.SetValue("Rand", element.Rand);
        }
        public override void UpdateElement()
        {
            element.Existiert = WerteListe.GetValue<bool>("Existiert");
            element.Font = WerteListe.GetValue<Font>("Font");
            element.HatRand = WerteListe.GetValue<bool>("HatRand");
            element.Rand = WerteListe.GetValue<SizeF>("Rand");
        }
        public override void BuildWerteListe()
        {
            WerteListe.AddSizeFBox(new SizeF(), "Rand");
            WerteListe.AddFontBox(null, "Font");
            WerteListe.AddBoolBox(true, "Existiert");
            WerteListe.AddBoolBox(true, "HatRand");
        }
        public override void Redraw()
        {
        }
    }
}
