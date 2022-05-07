using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Assistment.form;
using ArtOfMagicCrawler;
using Werwolf.Inhalt;
using System.IO;

namespace Werwolf.Forms
{
    public class ArtLibrarySelectionButton : Button, IWertBox<string>
    {
        public LibraryImageSelectionDialog ImageSelectionDialog { get; set; } = new LibraryImageSelectionDialog();

        private string path;
        public string ImagePath
        {
            get
            {
                return path;
            }
            set
            {
                path = value;
                valid = File.Exists(value);
                ImageChanged(this, new EventArgs());
                if (!valid)
                    InvalidChange(this, new EventArgs());
            }
        }

        public event EventHandler ImageChanged = delegate { };
        public event EventHandler InvalidChange = delegate { };

        private bool valid = true;

        public ArtLibrarySelectionButton()
        {
            this.AutoSize = true;
            this.Text = "Bild aus Bibliothek Auswählen";
            this.Enabled = Settings.ArtOfMtgLibrary != null;
            if (this.Enabled)
                ImageSelectionDialog.SetLibrary(Settings.ArtOfMtgLibrary);

            this.Click += ArtLibrarySelectionButton_Click;
        }

        private void ArtLibrarySelectionButton_Click(object sender, EventArgs e)
        {
            if (ImageSelectionDialog.ShowDialog() == DialogResult.OK)
            {
                this.ImagePath = ImageSelectionDialog.SelectedTile.Art.AbsoluteImagePath;
            }
        }

        public void AddInvalidListener(EventHandler Handler)
        {
            this.InvalidChange += Handler;
        }

        public void AddListener(EventHandler Handler)
        {
            this.ImageChanged += Handler;
        }

        public void DDispose()
        {
            this.ImageSelectionDialog.Dispose();
            this.Dispose();
        }

        public string GetValue() => path;

        public void SetValue(string Value)
        {
            path = Value;
        }

        public bool Valid() => valid;
    }
}
