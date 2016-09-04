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
    public class DarstellungForm<T> : PreForm<T> where T : Darstellung, new()
    {
        public DarstellungForm(Karte Karte)
            : base(Karte, new ViewKarte())
        {

        }

        public override void BuildWerteListe()
        {
            UpdatingWerteListe = true;

            WerteListe.AddStringBox("", "Name");
            WerteListe.AddBoolBox(true, "Existiert");

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }
        public override void UpdateWerteListe()
        {
            if (element == null)
                return;
            UpdatingWerteListe = true;

            WerteListe.SetValue("Name", element.Name);
            WerteListe.SetValue("Existiert", element.Existiert);

            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            if (element == null || UpdatingWerteListe)
                return;

            element.Name = element.Schreibname = WerteListe.GetValue<string>("Name");
            element.Existiert = WerteListe.GetValue<bool>("Existiert");
        }
    }
}
