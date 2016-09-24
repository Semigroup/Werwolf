using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Werwolf.Forms;
using Werwolf.Inhalt;

namespace ActionCardDesigner
{
    public class AktionsStartForm : StartForm<AktionsUniverse>
    {
        protected override void BuildViewKarte()
        {
            ViewKarte = new AktionsViewKarte();
            Controls.Add(ViewKarte);
            ViewKarte.Karte = Universe.AktionsKarten.Standard;
        }

        protected override void ElementMengenButtons_ButtonClick(object sender, EventArgs e)
        {
            switch (ElementMengenButtons.Message)
            {
                case "Gesinnungen Bearbeiten":
                    new ElementAuswahlForm<Gesinnung>(Universe.Gesinnungen).ShowDialog();
                    Changed(true);
                    break;
                case "Fraktionen Bearbeiten":
                    new ElementAuswahlForm<Fraktion>(Universe.Fraktionen).ShowDialog();
                    Changed(true);
                    break;
                case "Karten Bearbeiten":
                    new AktionsAuswahlForm<AktionsKarte>(
                        Universe.AktionsKarten.Standard,
                        Universe.AktionsKarten,
                        Universe.AktionsKarten.Standard,
                        new AktionsViewKarte(), true
                        ).ShowDialog();
                    Changed(true);
                    break;
                case "Decks Bearbeiten":
                    new AktionsAuswahlForm<AktionsDeck>(
                        Universe.AktionsKarten.Standard,
                        Universe.AktionsDecks,
                        Universe.AktionsDecks.Standard,
                        new AktionsViewDeck(),true
                        ).ShowDialog();
                    Changed(true);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
