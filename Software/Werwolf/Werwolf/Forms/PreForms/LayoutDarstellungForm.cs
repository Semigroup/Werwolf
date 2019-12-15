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
            UpdatingWerteListe = true;
            WerteListe.AddStringBox("", "Name");

            BuildWertBox("Kleines Namenfeld", Universe.TextBilder);
            BuildWertBox("Großes Namenfeld", Universe.TextBilder);

            BuildWertBox("Störung", Universe.TextBilder);
            BuildWertBox("Reichweite", Universe.TextBilder);
            BuildWertBox("Initiative", Universe.TextBilder);
            BuildWertBox("Felder", Universe.TextBilder);

            BuildWertBox("Leben", Universe.TextBilder);
            BuildWertBox("Leben Leer", Universe.TextBilder);
            BuildWertBox("Rüstung", Universe.TextBilder);

            BuildWertBox("KostenFeld", Universe.TextBilder);

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }
        public override void UpdateWerteListe()
        {
            if (element == null)
                return;
            UpdatingWerteListe = true;
            WerteListe.SetValue("Name", element.Name);

            WerteListe.SetValue("Kleines Namenfeld", element.KleinesNamenfeld);
            WerteListe.SetValue("Großes Namenfeld", element.GrossesNamenfeld);

            WerteListe.SetValue("Störung", element.Storung);
            WerteListe.SetValue("Reichweite", element.Reichweite);
            WerteListe.SetValue("Initiative", element.Initiative);
            WerteListe.SetValue("Felder", element.Felder);

            WerteListe.SetValue("Leben", element.Leben);
            WerteListe.SetValue("Leben Leer", element.LebenLeer);
            WerteListe.SetValue("Rüstung", element.Rustung);

            WerteListe.SetValue("KostenFeld", element.KostenFeld);

            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            if (element == null || UpdatingWerteListe)
                return;
            element.Name = element.Schreibname = WerteListe.GetValue<string>("Name");

            element.KleinesNamenfeld = WerteListe.GetValue<TextBild>("Kleines Namenfeld");
            element.GrossesNamenfeld = WerteListe.GetValue<TextBild>("Großes Namenfeld");

            element.Storung = WerteListe.GetValue<TextBild>("Störung");
            element.Reichweite = WerteListe.GetValue<TextBild>("Reichweite");
            element.Initiative = WerteListe.GetValue<TextBild>("Initiative");
            element.Felder = WerteListe.GetValue<TextBild>("Felder");

            element.Leben = WerteListe.GetValue<TextBild>("Leben");
            element.LebenLeer = WerteListe.GetValue<TextBild>("Leben Leer");
            element.Rustung = WerteListe.GetValue<TextBild>("Rüstung");

            element.KostenFeld = WerteListe.GetValue<TextBild>("KostenFeld");
        }
        protected override void SetVisibles()
        {
            SetVisible(Karte.KartenModus.AktionsKarte, "Störung", "Reichweite", "Initiative", "Felder");
            SetVisible(Karte.WondersIrgendwas, "Kleines Namenfeld", "Großes Namenfeld");
            SetVisible(Karte.KartenModus.RollenspielFigur, "Rüstung", "Leben", "Leben Leer");

            SetVisible(Karte.AlchemieIrgendwas, "KostenFeld");
        }
    }
}
