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
    public class LayoutDarstellungForm : DarstellungForm<LayoutDarstellung>
    {
        public LayoutDarstellungForm(Karte Karte)
            : base( Karte)
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();

            UpdatingWerteListe = true;

            BuildWertBox("Kleines Namenfeld", Universe.TextBilder);
            BuildWertBox("Großes Namenfeld", Universe.TextBilder);

            BuildWertBox("Störung", Universe.TextBilder);
            BuildWertBox("Reichweite", Universe.TextBilder);
            BuildWertBox("Initiative", Universe.TextBilder);
            BuildWertBox("Felder", Universe.TextBilder);

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }
        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
            if (element == null)
                return;
            UpdatingWerteListe = true;

            WerteListe.SetValue("Kleines Namenfeld", element.KleinesNamenfeld);
            WerteListe.SetValue("Großes Namenfeld", element.GrossesNamenfeld);

            WerteListe.SetValue("Störung", element.Storung);
            WerteListe.SetValue("Reichweite", element.Reichweite);
            WerteListe.SetValue("Initiative", element.Initiative);
            WerteListe.SetValue("Felder", element.Felder);

            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            base.UpdateElement();
            if (element == null || UpdatingWerteListe)
                return;

            element.KleinesNamenfeld = WerteListe.GetValue<TextBild>("Kleines Namenfeld");
            element.GrossesNamenfeld = WerteListe.GetValue<TextBild>("Großes Namenfeld");

            element.Storung = WerteListe.GetValue<TextBild>("Störung");
            element.Reichweite = WerteListe.GetValue<TextBild>("Reichweite");
            element.Initiative = WerteListe.GetValue<TextBild>("Initiative");
            element.Felder = WerteListe.GetValue<TextBild>("Felder");
        }
    }
}
