using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Assistment.form;

using Werwolf.Inhalt;

namespace Werwolf.Forms
{
    public abstract class PreForm<T> : Form where T : XmlElement
    {
        public Label KartenBild = new Label();
        public WerteListe WerteListe = new WerteListe();
        public Button OkButton = new Button();
        public Button AbbrechenButton = new Button();
        protected T element;
        public T Element { get { return element; } set { element = value; UpdateWerteListe(); } }
        public event EventHandler UserValueChanged = delegate { };

        public PreForm()
        {
            KartenBild.Size = new Size(500, 700);
            KartenBild.Image = Image.FromFile(@"D:\CSArbeiten\Github\Werwolf\Software\Werwolf\Werwolf\Resources\Dorfbewohner.JPG");
            Controls.Add(KartenBild);

            WerteListe.Size = new Size(500, 650);
            WerteListe.Location = new Point(500, 50);
            BuildWerteListe();
            WerteListe.Setup();
            WerteListe.UserValueChanged += (sender, e) => OnUserValueChanged();
            Controls.Add(WerteListe);

            OkButton.Size = new Size(100, 40);
            OkButton.Location = new Point(500, 710);
            OkButton.Text = "Übernehmen";
            OkButton.Click += new EventHandler(OkButton_Click);
            Controls.Add(OkButton);

            AbbrechenButton.Size = new Size(100, 40);
            AbbrechenButton.Location = new Point(600, 710);
            AbbrechenButton.Text = "Abbrechen";
            AbbrechenButton.Click += new EventHandler(AbbrechenButton_Click);
            Controls.Add(AbbrechenButton);

            this.ClientSize = new Size(1000, 800);
            this.Shown += new EventHandler(PreForm_Shown);
            this.UserValueChanged += (sender, e) => UpdateElement();
            this.UserValueChanged += (sender, e) => Redraw();
        }

        void AbbrechenButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
        void PreForm_Shown(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        public abstract void UpdateWerteListe();
        public abstract void UpdateElement();
        public abstract void BuildWerteListe();
        public abstract void Redraw();

        public void OnUserValueChanged()
        {
            UserValueChanged(this, EventArgs.Empty);
        }
    }
}
