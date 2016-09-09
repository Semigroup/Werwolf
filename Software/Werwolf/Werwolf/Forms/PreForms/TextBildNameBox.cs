using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

using Assistment.form;
using Werwolf.Inhalt;

namespace Werwolf.Forms
{
    public class TextBildNameBox : TextBox, IWertBox<string>
    {
        private string OldValue;
        private ElementMenge<TextBild> TextBilder;

        private string Value = "";

        public event EventHandler UserValueChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        public TextBildNameBox(ElementMenge<TextBild> TextBilder)
        {
            OldValue = "";
            this.TextBilder = TextBilder;
        }

        public string GetValue()
        {
            return Value;
        }
        public void SetValue(string Value)
        {
            this.OldValue = Value;
            this.Value = Value;
            this.Text = Value;
        }
        public void AddListener(EventHandler Handler)
        {
            UserValueChanged += Handler;
        }
        public bool Valid()
        {
            return ForeColor == Color.Black;
        }
        public void AddInvalidListener(EventHandler Handler)
        {
            InvalidChange += Handler;
        }
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            if (Text.Equals(OldValue) || !TextBilder.ContainsKey(Text))
            {
                this.ForeColor = Color.Black;
                this.Value = Text;
                UserValueChanged(this, e);
            }
            else
            {
                this.ForeColor = Color.Red;
                InvalidChange(this, e);
            }
        }
        public void DDispose()
        {
            this.Dispose();
        }
    }
}
