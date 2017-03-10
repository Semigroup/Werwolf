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
    public class ElementArrayBox<T> : UserControl, IWertBox<T[]> where T : XmlElement, new()
    {
        private ListBox ListBox = new ListBox();
        private Button Hinzufugen = new Button();
        private Button Entfernen = new Button();

        public ElementMenge<T> Menge { get; set; }

        public event EventHandler UserValueChanged = delegate { };

        public ElementArrayBox(ElementMenge<T> Menge)
        {
            this.Menge = Menge;

            this.Hinzufugen.Text = "Hinzufügen";
            this.Hinzufugen.AutoSize = false;
            this.Entfernen.Text = "Entfernen";
            this.Entfernen.AutoSize = false;

            this.Controls.Add(ListBox);
            this.Controls.Add(Hinzufugen);
            this.Controls.Add(Entfernen);
            this.Entfernen.Click += new EventHandler(Entfernen_Click);
            this.Hinzufugen.Click += new EventHandler(Hinzufugen_Click);

            this.Size = new Size(250, 120);
            OnResize(EventArgs.Empty);
        }

        void Hinzufugen_Click(object sender, EventArgs e)
        {
            ElementAuswahlForm<T> Form = new ElementAuswahlForm<T>(Menge, false);
            if (Form.ShowDialog() == DialogResult.OK)
                ListBox.Items.Add(Form.Element.Name);
            UserValueChanged(this, EventArgs.Empty);
        }
        void Entfernen_Click(object sender, EventArgs e)
        {
            if (ListBox.SelectedItem != null)
            {
                ListBox.Items.Remove(ListBox.SelectedItem);
                UserValueChanged(this, EventArgs.Empty);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.ListBox.Size = new Size(ClientSize.Width, ClientSize.Height - 50);
            this.Hinzufugen.Size = new Size(ClientSize.Width, 25);
            this.Entfernen.Size = new Size(ClientSize.Width, 25);
            this.ListBox.Location = new Point();
            this.Hinzufugen.Location = new Point(0, ListBox.Bottom);
            this.Entfernen.Location = new Point(0, Hinzufugen.Bottom);
        }

        public T[] GetValue()
        {
            T[] Items = new T[ListBox.Items.Count];
            int i = 0;
            foreach (var item in ListBox.Items)
                Items[i++] = Menge[item as string];
            return Items;
        }

        public void SetValue(T[] Value)
        {
            ListBox.Items.Clear();
            ListBox.Items.AddRange(Value.Map(t => t.Name).ToArray());
            UserValueChanged(this, EventArgs.Empty);
        }

        public void AddListener(EventHandler Handler)
        {
            UserValueChanged += Handler;
        }

        public bool Valid()
        {
            return true;
        }

        public void AddInvalidListener(EventHandler Handler)
        {
        }
        public void DDispose()
        {
            Menge = null;
            Dispose();
        }
    }
}
