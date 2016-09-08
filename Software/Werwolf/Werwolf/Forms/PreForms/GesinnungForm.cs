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
    public class GesinnungForm : PreForm<Gesinnung>
    {
        public GesinnungForm(Karte Karte)
            : base(Karte, new ViewKarte())
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();
            UpdatingWerteListe = true;

            WerteListe.AddBigStringBox("","Text");

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }
        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
            if (element == null)
                return;
            UpdatingWerteListe = true;

            WerteListe.SetValue("Text", element.Aufgabe.ToString());

            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            base.UpdateElement();
            if (element == null || UpdatingWerteListe)
                return;

            element.Aufgabe = new Aufgabe(WerteListe.GetValue<string>("Text"), Universe);
        }
    }
}
