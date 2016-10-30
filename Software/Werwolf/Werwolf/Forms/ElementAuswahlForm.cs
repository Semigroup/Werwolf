using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Threading;

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
        protected Karte Karte;
        public T Element { get; private set; }
        private ElementMenge<T> ElementMenge;

        private ViewBox ViewBox;
        private ScrollList List = new ScrollList();
        private ButtonReihe SteuerButton = new ButtonReihe(true, "Übernehmen", "Abbrechen");

        private BackgroundWorker RefreshThread = new BackgroundWorker();
        private Queue RefreshJobs;
        private Queue SynchronizedRefreshJobs;

        private bool CancelThread = false;

        public bool BearbeitungErlaubt { get; private set; }

        public ElementAuswahlForm(ElementMenge<T> ElementMenge, bool BearbeitungErlaubt)
            : this(ElementMenge.Universe.Karten.Standard.Clone() as Karte, ElementMenge, ElementMenge.Standard, GetViewBox(), BearbeitungErlaubt)
        {

        }
        public ElementAuswahlForm(ElementMenge<T> ElementMenge, T Element)
            : this(ElementMenge.Universe.Karten.Standard.Clone() as Karte, ElementMenge, Element)
        {

        }
        public ElementAuswahlForm(ElementMenge<T> ElementMenge)
            : this(ElementMenge.Universe.Karten.Standard.Clone() as Karte, ElementMenge)
        {

        }
        public ElementAuswahlForm(Karte Karte, ElementMenge<T> ElementMenge)
            : this(Karte, ElementMenge, ElementMenge.Standard)
        {

        }
        public ElementAuswahlForm(Karte Karte, ElementMenge<T> ElementMenge, T Element)
            : this(Karte, ElementMenge, Element, GetViewBox())
        {

        }
        public ElementAuswahlForm(Karte Karte, ElementMenge<T> ElementMenge, T Element, ViewBox ViewBox)
            : this(Karte, ElementMenge, Element, ViewBox, true)
        {
        }
        public ElementAuswahlForm(Karte Karte, ElementMenge<T> ElementMenge, T Element, ViewBox ViewBox, bool BearbeitungErlaubt)
            : base()
        {
            this.Karte = Karte.Clone() as Karte;
            this.ElementMenge = ElementMenge;
            this.Element = Element;
            this.ViewBox = ViewBox;
            this.RefreshJobs = new Queue();
            this.SynchronizedRefreshJobs = Queue.Synchronized(RefreshJobs as Queue);
            this.BearbeitungErlaubt = BearbeitungErlaubt;
            BuildUp();
        }

        private void BuildUp()
        {
            ViewBox.Dock = DockStyle.Left;
            ViewBox.Karte = Karte.Clone() as Karte;
            Controls.Add(ViewBox);

            List.AddControl(ElementMenge.Values.Map(x => GetButton(x)));
            List.Scroll += new ScrollEventHandler(List_Scroll);
            Controls.Add(List);

            SteuerButton.ButtonClick += new EventHandler(SteuerButton_ButtonClick);
            Controls.Add(SteuerButton);

            RefreshThread.DoWork += new DoWorkEventHandler(RefreshThread_DoWork);

            this.ClientSize = new Size(1000, 800);
            Auswahlen(Element);

            StartRefreshing();
        }

        private void RefreshThread_DoWork(object sender, DoWorkEventArgs e)
        {
            ElementAuswahlButton<T> item;
            while (!CancelThread)
            {
                lock (SynchronizedRefreshJobs.SyncRoot)
                    if (RefreshJobs.Count > 0)
                        item = RefreshJobs.Dequeue() as ElementAuswahlButton<T>;
                    else
                        return;
                if (item.Dirty)
                {
                    if (item.IsHandleCreated)
                        item.Invoke((MethodInvoker)item.Refresh);
                    Thread.Sleep(Settings.SleepTime);
                }
            }
            CancelThread = false;
        }
        private void List_Scroll(object sender, ScrollEventArgs e)
        {
            Rectangle R = ClientRectangle;
            R.Location = R.Location.sub(List.Location.add(List.ControlList.Location));
            foreach (ElementAuswahlButton<T> item in List.ControlList)
                if (item.Dirty)
                {
                    Rectangle r = new Rectangle(item.Location, item.Size);
                    r.Intersect(R);
                    if (!r.Size.IsEmpty)
                        item.Refresh();
                }
        }
        private void SteuerButton_ButtonClick(object sender, EventArgs e)
        {
            if (SteuerButton.Message == "Übernehmen")
                DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ViewBox.Width = ClientSize.Width / 2;

            SteuerButton.Location = new Point(ViewBox.Right + 20, ClientSize.Height - 50);

            List.Location = new Point(ViewBox.Right, 0);
            List.Size = new Size(ClientSize.Width / 2, ClientSize.Height - 70);
        }

        private Control GetButton(T element)
        {
            ElementAuswahlButton<T> b = new ElementAuswahlButton<T>(element, Karte, ElementMenge, BearbeitungErlaubt);
            b.Ausgewahlt += (s, e) => Auswahlen(element);
            b.Entfernt += (s, e) => Entfernen(b);
            b.Bearbeitet += (s, e) => Bearbeiten(b);
            b.Geklont += (s, e) => Klonen(element);
            return b;
        }
        private void Auswahlen(T element)
        {
            this.Element = element;
            ViewBox.ChangeKarte(Element);
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
            CancelThread = true;
            if (pf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ElementMenge.Change(b.Element.Name, pf.Element);
                Auswahlen(b.Element);
            }
            if (b.Element.Deep && Settings.RefreshDirtyButtons)
                StartRefreshing();
            else
            {
                b.Dirty = true;
                b.Refresh();
            }
        }
        private void Klonen(T element)
        {
            T Neu = element.Clone() as T;
            ElementMenge.AddPolymorph(Neu);
            ElementAuswahlButton<T> b = GetButton(Neu) as ElementAuswahlButton<T>;
            b.Refresh();
            List.AddControl(b);
            List.SetUp();
        }

        private void StartRefreshing()
        {
            lock (SynchronizedRefreshJobs.SyncRoot)
            {
                RefreshJobs.Clear();

                Rectangle R = ClientRectangle;
                R.Location = R.Location.sub(List.Location.add(List.ControlList.Location));
                foreach (ElementAuswahlButton<T> item in List.ControlList)
                {
                    item.Dirty = true;
                    Rectangle r = new Rectangle(item.Location, item.Size);
                    r.Intersect(R);
                    if (!r.Size.IsEmpty)
                        item.Refresh();
                    else
                        RefreshJobs.Enqueue(item);
                }
            }
            if (!RefreshThread.IsBusy)
                RefreshThread.RunWorkerAsync();
        }

        protected PreForm<T> GetPreform(T Element)
        {
            PreForm<T> pf = GetPreform();
            pf.Element = Element;
            return pf;
        }
        protected virtual PreForm<T> GetPreform()
        {
            switch (typeof(T).Name)
            {
                case "HauptBild":
                    return new HauptBildForm(Karte) as PreForm<T>;
                case "HintergrundBild":
                    return new HintergrundBildForm(Karte) as PreForm<T>;
                case "RuckseitenBild":
                    return new RuckseitenBildForm(Karte) as PreForm<T>;
                case "TextBild":
                    return new TextBildForm(Karte) as PreForm<T>;

                case "Karte":
                    return new KartenForm(Karte) as PreForm<T>;
                case "Gesinnung":
                    return new GesinnungForm(Karte) as PreForm<T>;
                case "Fraktion":
                    return new FraktionForm(Karte) as PreForm<T>;
                case "Deck":
                    return new DeckForm(Karte) as PreForm<T>;

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

        private static ViewBox GetViewBox()
        {
            switch (typeof(T).Name)
            {
                case "HauptBild":
                case "HintergrundBild":
                case "TextBild":
                case "Karte":
                case "Gesinnung":
                case "Fraktion":
                case "BildDarstellung":
                case "HintergrundDarstellung":
                case "InfoDarstellung":
                case "TextDarstellung":
                case "TitelDarstellung":
                    return new ViewKarte();

                case "RuckseitenBild":
                    return new ViewRuckseitenBild();

                case "Deck":
                    return new ViewDeck();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
