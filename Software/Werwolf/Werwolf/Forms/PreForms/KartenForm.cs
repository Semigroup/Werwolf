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

            BuildWertBox("Bild Darstellung", Universe.BildDarstellungen);
            BuildWertBox("Titel Darstellung", Universe.TitelDarstellungen);
            BuildWertBox("Text Darstellung", Universe.TextDarstellungen);
            BuildWertBox("Hintergrund Darstellung", Universe.HintergrundDarstellungen);
            BuildWertBox("Info Darstellung", Universe.InfoDarstellungen);

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

            WerteListe.SetValue("Bild Darstellung", element.BildDarstellung);
            WerteListe.SetValue("Titel Darstellung", element.TitelDarstellung);
            WerteListe.SetValue("Text Darstellung", element.TextDarstellung);
            WerteListe.SetValue("Hintergrund Darstellung", element.HintergrundDarstellung);
            WerteListe.SetValue("Info Darstellung", element.InfoDarstellung);

            UpdatingWerteListe = false;
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

            element.BildDarstellung = WerteListe.GetValue<BildDarstellung>("Bild Darstellung");
            element.TitelDarstellung = WerteListe.GetValue<TitelDarstellung>("Titel Darstellung");
            element.TextDarstellung = WerteListe.GetValue<TextDarstellung>("Text Darstellung");
            element.HintergrundDarstellung = WerteListe.GetValue<HintergrundDarstellung>("Hintergrund Darstellung");
            element.InfoDarstellung = WerteListe.GetValue<InfoDarstellung>("Info Darstellung");
        }
    }
}
