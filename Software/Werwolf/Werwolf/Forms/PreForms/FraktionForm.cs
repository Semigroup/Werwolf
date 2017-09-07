﻿using System;
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
    public class FraktionForm : PreForm<Fraktion>
    {
        public FraktionForm(Karte Karte)
            : base(Karte, new ViewKarte())
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();
            UpdatingWerteListe = true;

            BuildWertBox("Bild Hintergrund", Universe.HintergrundBilder);
            WerteListe.AddEnumBox(Titel.Art.Rund, "Titel Art");
            BuildWertBox("Bild Rückseite", Universe.RuckseitenBilder);
            WerteListe.AddBigStringBox("", "Fraktionstext");
            BuildWertBox("Symbol", Universe.TextBilder);

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }
        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
            if (element == null)
                return;
            UpdatingWerteListe = true;

            WerteListe.SetValue("Bild Hintergrund", element.HintergrundBild);
            WerteListe.SetValue("Titel Art", element.TitelArt as object);
            WerteListe.SetValue("Bild Rückseite", element.RuckseitenBild);
            WerteListe.SetValue("Fraktionstext", element.StandardAufgaben.ToString());
            WerteListe.SetValue("Symbol", element.Symbol);

            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            base.UpdateElement();
            if (element == null || UpdatingWerteListe)
                return;

            element.HintergrundBild = WerteListe.GetValue<HintergrundBild>("Bild Hintergrund");
            element.TitelArt = (Titel.Art)WerteListe.GetValue<object>("Titel Art");
            element.RuckseitenBild = WerteListe.GetValue<RuckseitenBild>("Bild Rückseite");
            element.StandardAufgaben = new Aufgabe(WerteListe.GetValue<string>("Fraktionstext"),Universe);
            element.Symbol = WerteListe.GetValue<TextBild>("Symbol");
        }

        protected override void SetVisibles()
        {
            SetVisible(Karte.KartenModus.Werwolfkarte, "Titel Art");
            SetVisible(Karte.KartenModus.CyberWaffenKarte, "Symbol");
        }
    }
}
