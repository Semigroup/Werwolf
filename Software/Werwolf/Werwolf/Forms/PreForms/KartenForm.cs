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
    public class KartenForm : PreForm<Karte>
    {
        public KartenForm(Karte Karte)
            : base(Karte, new ViewKarte())
        {

        }

        public override void BuildWerteListe()
        {
            base.BuildWerteListe();
            UpdatingWerteListe = true;

            BuildWertBox("Bild", Universe.HauptBilder);
            BuildWertBox("Fraktion", Universe.Fraktionen);
            BuildWertBox("Gesinnung", Universe.Gesinnungen);
            WerteListe.AddBigStringBox("", "Text");

            //BuildWertBox("Bild Darstellung", Universe.BildDarstellungen);
            BuildWertBox("Titel Darstellung", Universe.TitelDarstellungen);
            BuildWertBox("Text Darstellung", Universe.TextDarstellungen);
            BuildWertBox("Hintergrund Darstellung", Universe.HintergrundDarstellungen);
            BuildWertBox("Info Darstellung", Universe.InfoDarstellungen);

            WerteListe.AddFloatBox(0, "Initiative");
            WerteListe.AddPointBox(new Point(), "Reichweite");
            WerteListe.AddIntBox(0, "Felder");
            WerteListe.AddIntBox(0, "Störung");

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }

        public override void UpdateWerteListe()
        {
            base.UpdateWerteListe();
            if (element == null)
                return;
            UpdatingWerteListe = true;

            WerteListe.SetValue("Bild", element.HauptBild);
            WerteListe.SetValue("Fraktion", element.Fraktion);
            WerteListe.SetValue("Gesinnung", element.Gesinnung);
            WerteListe.SetValue("Text", element.Aufgaben.ToString());

            // WerteListe.SetValue("Bild Darstellung", element.BildDarstellung);
            WerteListe.SetValue("Titel Darstellung", element.TitelDarstellung);
            WerteListe.SetValue("Text Darstellung", element.TextDarstellung);
            WerteListe.SetValue("Hintergrund Darstellung", element.HintergrundDarstellung);
            WerteListe.SetValue("Info Darstellung", element.InfoDarstellung);

            WerteListe.SetValue("Initiative", element.Initiative);
            WerteListe.SetValue("Reichweite", new Point(element.ReichweiteMin, element.ReichweiteMax));
            WerteListe.SetValue("Felder", element.Felder);
            WerteListe.SetValue("Störung", element.Storung);

            UpdatingWerteListe = false;
            SetVisibles();
        }
        public override void UpdateElement()
        {
            base.UpdateElement();
            if (element == null || UpdatingWerteListe)
                return;

            element.HauptBild = WerteListe.GetValue<HauptBild>("Bild");
            element.Fraktion = WerteListe.GetValue<Fraktion>("Fraktion");
            element.Gesinnung = WerteListe.GetValue<Gesinnung>("Gesinnung");
            element.Aufgaben = new Aufgabe(WerteListe.GetValue<string>("Text"), Universe);

            //element.BildDarstellung = WerteListe.GetValue<BildDarstellung>("Bild Darstellung");
            element.TitelDarstellung = WerteListe.GetValue<TitelDarstellung>("Titel Darstellung");
            element.TextDarstellung = WerteListe.GetValue<TextDarstellung>("Text Darstellung");
            element.HintergrundDarstellung = WerteListe.GetValue<HintergrundDarstellung>("Hintergrund Darstellung");
            element.InfoDarstellung = WerteListe.GetValue<InfoDarstellung>("Info Darstellung");

            Point r = WerteListe.GetValue<Point>("Reichweite");
            element.ReichweiteMin = r.X;
            element.ReichweiteMax = r.Y;
            element.Initiative = WerteListe.GetValue<float>("Initiative");
            element.Felder = WerteListe.GetValue<int>("Felder");
            element.Storung = WerteListe.GetValue<int>("Störung");

            SetVisibles();
        }

        public void SetVisibles()
        {
            WerteListe.ShowBox(
              element.HintergrundDarstellung.Modus == HintergrundDarstellung.KartenModus.AktionsKarte,
              "Initiative", "Reichweite", "Felder", "Störung");
        }
    }
}
