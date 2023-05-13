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
            BuildWertBox("Bild Hintergrund Quer", Universe.HintergrundBilder);
            WerteListe.AddEnumBox(Titel.Art.Rund, "Titel Art");
            BuildWertBox("Bild Rückseite", Universe.RuckseitenBilder);
            WerteListe.AddBigStringBox("", "Fraktionstext");
            BuildWertBox("Symbol", Universe.TextBilder);
            WerteListe.AddEnumBox(Fraktion.RuckseitenArt.Normal, "Rückseiten Art");
            WerteListe.AddBoolBox(false, "Text hat komplexe Struktur?");

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
            WerteListe.SetValue("Bild Hintergrund Quer", element.HintergrundBildQuer);
            WerteListe.SetValue("Titel Art", element.TitelArt as object);
            WerteListe.SetValue("Bild Rückseite", element.RuckseitenBild);
            WerteListe.SetValue("Fraktionstext", element.StandardAufgaben.ToString());
            WerteListe.SetValue("Symbol", element.Symbol);
            WerteListe.SetValue("Rückseiten Art", element.RuckArt as object);
            WerteListe.SetValue("Text hat komplexe Struktur?", element.IstKomplex);

            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            base.UpdateElement();
            if (element == null || UpdatingWerteListe)
                return;

            element.HintergrundBild = WerteListe.GetValue<HintergrundBild>("Bild Hintergrund");
            element.HintergrundBildQuer = WerteListe.GetValue<HintergrundBild>("Bild Hintergrund Quer");
            element.TitelArt = (Titel.Art)WerteListe.GetValue<object>("Titel Art");
            element.RuckseitenBild = WerteListe.GetValue<RuckseitenBild>("Bild Rückseite");
            element.StandardAufgaben = new Aufgabe(WerteListe.GetValue<string>("Fraktionstext"),Universe);
            element.Symbol = WerteListe.GetValue<TextBild>("Symbol");
            element.RuckArt = (Fraktion.RuckseitenArt)WerteListe.GetValue<object>("Rückseiten Art");
            element.IstKomplex = WerteListe.GetValue<bool>("Text hat komplexe Struktur?");
        }

        protected override void SetVisibles()
        {
            SetVisible(Karte.KartenModus.Werwolfkarte, "Titel Art");
            SetVisible(Karte.KartenModus.ModernWolfKarte | Karte.AlchemieIrgendwas, "Bild Hintergrund Quer");
            SetVisible(Karte.CyberIrgendwas, "Symbol");
            SetVisible(Karte.KartenModus.ModernWolfEreignisKarte, "Text hat komplexe Struktur?");
        }
    }
}
