using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Werwolf.Inhalt;
using Werwolf.Karten;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using Assistment.form;
using Assistment.Extensions;

namespace Werwolf.Forms
{
    public class ElementAuswahlForm<T> : Form where T : XmlElement, new()
    {
        private Karte Karte;
        public T Element { get; private set; }
        private ElementMenge<T> ElementMenge;

        private ViewCard ViewCard = new ViewCard();
        private ScrollList List = new ScrollList();
        private ButtonReihe SteuerButton = new ButtonReihe(true, "Übernehmen", "Abbrechen");

        public ElementAuswahlForm(Karte Karte, ElementMenge<T> ElementMenge)
            : this(Karte, ElementMenge, ElementMenge.Standard)
        {
        }
        public ElementAuswahlForm(Karte Karte, ElementMenge<T> ElementMenge, T Element)
        {
            this.Karte = Karte.Clone() as Karte;
            this.ElementMenge = ElementMenge;
            this.Element = Element;
            BuildUp();
        }

        private void BuildUp()
        {
            ViewCard.Dock = DockStyle.Left;
            ViewCard.Karte = Karte;
            Controls.Add(ViewCard);

            List.AddControl(ElementMenge.Values.Map(x => GetButton(x)));
            Controls.Add(List);

            SteuerButton.ButtonClick += new EventHandler(SteuerButton_ButtonClick);
            Controls.Add(SteuerButton);

            this.ClientSize = new Size(1000, 800);
            Auswahlen(Element);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ViewCard.Width = ClientSize.Width / 2;

            SteuerButton.Location = new Point(ViewCard.Right + 20, ClientSize.Height - 50);

            List.Location = new Point(ViewCard.Right, 0);
            List.Size = new Size(ClientSize.Width / 2, ClientSize.Height - 70);
        }
        private void SteuerButton_ButtonClick(object sender, EventArgs e)
        {
            if (SteuerButton.Message == "Übernehmen")
                DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private Control GetButton(T element)
        {
            ElementAuswahlButton<T> b = new ElementAuswahlButton<T>(element, Karte, ElementMenge);
            b.Ausgewahlt += (s, e) => Auswahlen(element);
            b.Entfernt += (s, e) => Entfernen(b);
            b.Bearbeitet += (s, e) => Bearbeiten(b);
            b.Geklont += (s, e) => Klonen(element);
            return b;
        }
        private void Auswahlen(T element)
        {
            this.Element = element;
            ViewCard.ChangeKarte(Element);
            foreach (var item in List.ControlList)
            {
                ElementAuswahlButton<T> b = item as ElementAuswahlButton<T>;
                b.BackColor = Element == b.Element ?
                    Color.LightYellow : Color.Transparent;
            }
            this.Text = element.Schreibname + " ist ausgewählt als " + element.XmlName;
        }
        private void Entfernen(ElementAuswahlButton<T> b)
        {
            if (Element == b.Element)
                Auswahlen(ElementMenge.Standard);
            ElementMenge.Remove(b.Element.Name);
            List.ControlList.Remove(b);
            List.SetUp();
        }
        private void Bearbeiten(ElementAuswahlButton<T> b)
        {
            PreForm<T> pf = GetPreform(b.Element);
            if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ElementMenge.Change(b.Element.Name, pf.Element);
                Auswahlen(b.Element);
            }
            foreach (var item in List.ControlList)
                item.Refresh();
        }
        private PreForm<T> GetPreform(T Element)
        {
            PreForm<T> pf = GetPreform();
            pf.Element = Element;
            return pf;
        }
        private PreForm<T> GetPreform()
        {
            switch (typeof(T).Name)
            {
                case "Bild":
                    return new BildForm(Karte) as PreForm<T>;
                case "Karte":
                    return new KartenForm(Karte) as PreForm<T>;
                case "Gesinnung":
                    return new GesinnungForm(Karte) as PreForm<T>;
                case "Fraktion":
                    return new FraktionForm(Karte) as PreForm<T>;

                case "BildDarstellung":
                    return new BildDarstellungForm(Karte) as PreForm<T>;
                case "HintergrundDarstellung":
                    return new HintergrundDarstellungForm(Karte) as PreForm<T>;
                case "InfoDarstellung":
                    return new InfoDarstellungForm(Karte) as PreForm<T>;
                case "TextDarstellung":
                    return new TextDarstellungForm(Karte) as PreForm<T>;
                case "TitelDarstellung":
                    return new TitelDarstellungForm(Karte) as PreForm<T>;
                default:
                    throw new NotImplementedException();
            }
        }
        private void Klonen(T element)
        {
            T Neu = element.Clone() as T;
            ElementMenge.AddPolymorph(Neu);
            List.AddControl(GetButton(Neu));
        }
    }
}
