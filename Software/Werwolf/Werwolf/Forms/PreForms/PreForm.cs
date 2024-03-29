﻿using System;
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
    public abstract class PreForm<T> : Form where T : XmlElement, new()
    {
        public Universe Universe { get { return Karte.Universe; } }

        protected bool UpdatingWerteListe = false;

        public T Element { get { return element; } set { SetElement(value); } }
        protected T element;

        public WerteListe WerteListe = new WerteListe();
        public Button OkButton = new Button();
        public Button AbbrechenButton = new Button();

        public ViewBox ViewBox;
        private Karte Karte;

        public event EventHandler UserValueChanged = delegate { };

        public PreForm(Karte Karte, ViewBox ViewBox)
            : base()
        {
            this.Karte = Karte;
            this.ViewBox = ViewBox;
            BuildUp();
        }
        protected virtual void BuildUp()
        {
            ViewBox.Dock = DockStyle.Left;
            ViewBox.Karte = Karte;
            Controls.Add(ViewBox);

            BuildWerteListe();
            WerteListe.UserValueChanged += (sender, e) => OnUserValueChanged(e);
            WerteListe.InvalidChange += (sender, e) => OnInvalidChange(e);
            Controls.Add(WerteListe);

            BuildButtons();

            this.ClientSize = new Size(1000, 800);
        }
        protected virtual void BuildButtons()
        {
            OkButton.Size = new Size(100, 40);
            OkButton.Text = "Übernehmen";
            OkButton.Click += OkButton_Click;
            Controls.Add(OkButton);

            AbbrechenButton.Size = new Size(100, 40);
            AbbrechenButton.Text = "Abbrechen";
            AbbrechenButton.Click += OkButton_Click;
            Controls.Add(AbbrechenButton);
        }

        public void SetElement(T Element)
        {
            this.element = Element.Clone() as T;
            //SetVisibles();
            ViewBox.ChangeKarte(element);
            UpdateWerteListe();
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            if (sender == OkButton)
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            else
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// WerteListe wird aufgebaut
        /// </summary>
        public virtual void BuildWerteListe()
        {
            UpdatingWerteListe = true;
            WerteListe.AddStringBox("", "Name");
            WerteListe.Setup();
            UpdatingWerteListe = false;
        }
        /// <summary>
        /// WerteListe übernimmt Werte von Element
        /// </summary>
        public virtual void UpdateWerteListe()
        {
            UpdatingWerteListe = true;
            WerteListe.SetValue("Name", element.Schreibname);
            this.UpdateFormTitle();
            UpdatingWerteListe = false;
        }
        /// <summary>
        /// Element übernimmt Werte von WerteListe
        /// </summary>
        public virtual void UpdateElement()
        {
            if (element == null || UpdatingWerteListe)
                return;
            element.Name = element.Schreibname = WerteListe.GetValue<string>("Name");
            this.UpdateFormTitle();
        }
        public void UpdateFormTitle()
            => this.Text = element.XmlName + " namens " + element.Schreibname + " bearbeiten...";

        protected void BuildArrayBox<I>(string Name, ElementMenge<I> Menge) where I : XmlElement, new()
        {
            ElementArrayBox<I> b = new ElementArrayBox<I>(Menge);
            WerteListe.AddWertePaar<I[]>(b, new I[0], Name);
        }
        protected void BuildWertBox<I>(string Name, ElementMenge<I> Menge) where I : XmlElement, new()
        {
            ElementButton<I> b = new ElementButton<I>(Menge, Karte);
            WerteListe.AddWertePaar<I>(b, Menge.Standard, Name);
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            ViewBox.Width = ClientSize.Width / 2;

            OkButton.Location = new Point(ViewBox.Right + 20, ClientSize.Height - 50);
            AbbrechenButton.Location = new Point(OkButton.Right + 20, OkButton.Top);

            WerteListe.Location = new Point(ViewBox.Right, 0);
            WerteListe.Size = new Size(ClientSize.Width / 2, ClientSize.Height - 70);
            WerteListe.Refresh();
        }

        protected virtual void OnUserValueChanged(EventArgs e)
        {
            UserValueChanged(this, EventArgs.Empty);
            UpdateElement();
            ViewBox.ChangeKarte(element);
            //OkButton.Enabled = WerteListe.Valid();
        }
        protected virtual void OnInvalidChange(EventArgs e)
        {
            //OkButton.Enabled = WerteListe.Valid();
        }

        protected virtual void SetVisibles()
        {
        }
        protected virtual void SetVisible(Karte.KartenModus DesiredModus, params string[] Namen)
        {
            WerteListe.ShowBox(((Karte.Modus & DesiredModus) != 0), Namen);
        }

        protected override void OnLoad(EventArgs e)
        {
            SetVisibles();
            WerteListe.Setup();
        }
    }
}
