using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

using Assistment.form;

using Werwolf.Inhalt;

namespace Werwolf.Forms
{
    public class SettingsForm : Form
    {
        private Button Speichern = new Button();
        private Button Abbrechen = new Button();

        private WerteListe werteListe1;

        public SettingsForm()
        {
            BuildUp();
            this.ClientSize = new Size(1000, 800);
        }

        public void BuildUp()
        {
            this.Text = "Einstellungen Bearbeiten";

            werteListe1 = new WerteListe();

            werteListe1.AddIntBox(Settings.DelayTime, "DelayTime");
            werteListe1.AddSizeFBox(Settings.MaximumKarteSize, "MaximumKarteSize");
            werteListe1.AddIntBox(Settings.MaximumNumberOfCores, "MaximumNumberOfCores");
            werteListe1.AddIntBox(Settings.MaximumImageArea, "MaximumImageArea");
            werteListe1.AddFloatBox(Settings.MaximumPpm, "MaximumPpm");
            werteListe1.AddIntBox(Settings.SleepTime, "SleepTime");
            werteListe1.AddFloatBox(Settings.WolfBoxFaktor, "WolfBoxFaktor");
            werteListe1.AddFloatBox(Settings.ViewPpm, "ViewPpm");
            werteListe1.AddImageBox(Settings.ErrorImagePath, "ErrorImagePath");
            werteListe1.AddDirectoryBox(Settings.ArtOfMtgLibraryRoot, "ArtOfMtgLibraryRoot");

            werteListe1.AddListener(UserChangedValue);
            werteListe1.AddInvalidListener(UserChangedValue);
            werteListe1.Setup();
            Controls.Add(werteListe1);

            Speichern.Text = "Einstellungen Speichern";
            Speichern.AutoSize = true;
            Speichern.Click += new EventHandler(Speichern_Click);
            Controls.Add(Speichern);

            Abbrechen.Text = "Abbrechen";
            Abbrechen.AutoSize = true;
            Abbrechen.Click += new EventHandler(Speichern_Click);
            Controls.Add(Abbrechen);
        }

        void UserChangedValue(object sender, EventArgs e)
        {
            this.Text = "Einstellungen Bearbeiten*";

            Speichern.Enabled = werteListe1.Valid() &&
                File.Exists(werteListe1.GetValue<string>("ErrorImagePath"));
        }

        void Speichern_Click(object sender, EventArgs e)
        {
            if (sender == Speichern)
            {
                Settings.DelayTime = werteListe1.GetValue<int>("DelayTime");
                Settings.MaximumNumberOfCores = werteListe1.GetValue<int>("MaximumNumberOfCores");
                Settings.SleepTime = werteListe1.GetValue<int>("SleepTime");
                Settings.MaximumPpm = werteListe1.GetValue<float>("MaximumPpm");
                Settings.WolfBoxFaktor = werteListe1.GetValue<float>("WolfBoxFaktor");
                Settings.ViewPpm = werteListe1.GetValue<float>("ViewPpm");
                Settings.MaximumKarteSize = werteListe1.GetValue<SizeF>("MaximumKarteSize");
                Settings.MaximumImageArea = werteListe1.GetValue<int>("MaximumImageArea");
                Settings.ArtOfMtgLibraryRoot = werteListe1.GetValue<string>("ArtOfMtgLibraryRoot");
                Settings.TryLoadArtOfMtgLibrary();

                string s = werteListe1.GetValue<string>("ErrorImagePath");
                werteListe1.Dispose();
                if (File.Exists(s))
                    using (FileStream fs = new FileStream(s, FileMode.Open))
                    {
                        using (Image image = Image.FromStream(fs))
                            Settings.ErrorImage = new Bitmap(image);
                        fs.Close();
                    }
                Settings.Save();
            }
            this.Close();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            werteListe1.Location = new Point(0, 0);
            werteListe1.Size = new Size(ClientSize.Width, ClientSize.Height - 100);

            Speichern.Location = new Point(50, ClientSize.Height - 50);
            Abbrechen.Location = new Point(Speichern.Right + 50, ClientSize.Height - 50);
        }
    }
}
