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
    public class ElementButton<T> : Button, IWertBox<T> where T : XmlElement, new()
    {
        private T Value;
        private ElementMenge<T> ElementMenge;
        private Karte Karte;
        public event EventHandler UserValueChanged = delegate { };

        public ElementButton(ElementMenge<T> ElementMenge, Karte Karte)
        {
            this.ElementMenge = ElementMenge;
            this.Karte = Karte;
            this.AutoSize = true;
            Value = ElementMenge.Standard;
        }

        public T GetValue()
        {
            return Value;
        }
        public void SetValue(T Value)
        {
            this.Value = Value;
            this.Text = Value.Schreibname;
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
        protected virtual void OnUserValueChanged(object sender, EventArgs e)
        {
            UserValueChanged(sender, e);
        }
        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            ElementAuswahlForm<T> Form = new ElementAuswahlForm<T>(Karte, ElementMenge, Value);
            if (Form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Value = Form.Element;
                Refresh();
                OnUserValueChanged(this, e);
            }
        }
        public override void Refresh()
        {
            base.Refresh();
            this.Text = Value.Schreibname;
        }
        public void DDispose()
        {
            this.Dispose();
        }
    }
}
