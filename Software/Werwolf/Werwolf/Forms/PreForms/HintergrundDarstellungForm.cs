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
            WerteListe.AddChainedSizeFBox(new SizeF(), "Größe in mm", true, Settings.MaximumKarteSize);
            WerteListe.AddPointFBox(new PointF(), "Anker");

            WerteListe.AddFloatBox(0, "Margin Links");
            WerteListe.AddFloatBox(0, "Margin Rechts");
            WerteListe.AddFloatBox(0, "Margin Oben");
            WerteListe.AddFloatBox(0, "Margin Unten");

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
            WerteListe.SetValue("Größe in mm", element.Size);
            WerteListe.SetValue("Anker", element.Anker);

            WerteListe.SetValue("Margin Links", element.MarginLeft);
            WerteListe.SetValue("Margin Rechts", element.MarginRight);
            WerteListe.SetValue("Margin Oben", element.MarginTop);
            WerteListe.SetValue("Margin Unten", element.MarginBottom);

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
            element.Size = WerteListe.GetValue<SizeF>("Größe in mm");
            element.Anker = WerteListe.GetValue<PointF>("Anker");

            element.MarginLeft = WerteListe.GetValue<float>("Margin Links");
            element.MarginRight = WerteListe.GetValue<float>("Margin Rechts");
            element.MarginTop = WerteListe.GetValue<float>("Margin Oben");
            element.MarginBottom = WerteListe.GetValue<float>("Margin Unten");
        }

        protected override void SetVisibles()
        {
            SetVisible(0, "Hat Runde Ecken?");
            SetVisible(Karte.KartenModus.ModernWolfKarte, "Margin Links", "Margin Rechts", "Margin Oben", "Margin Unten");
        }
    }
}
