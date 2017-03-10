using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Assistment.form;
using Assistment.Extensions;
using Assistment.Xml;

using Werwolf.Inhalt;

namespace Werwolf.Forms
{
    public class StartForm<U> : Form where U : Universe, new()
    {
        private U universe;
        public U Universe
        {
            get { return universe; }
            private set { SetUniverse(value); }
        }

        private ToolTip ToolTip = new ToolTip();
        private SteuerBox SteuerBox = new SteuerBox("Spiel");
        private ButtonReihe BilderButtons = new ButtonReihe(false,
            "Hauptbilder Sammeln",
            "Hintergrundbilder Sammeln",
            "Textbilder Sammeln",
            "Rückseitenbilder Sammeln");
        protected ButtonReihe ElementMengenButtons = new ButtonReihe(false,
            "Gesinnungen Bearbeiten",
            "Fraktionen Bearbeiten",
            "Karten Bearbeiten",
            "Decks Bearbeiten");
        private ButtonReihe BildMengenButtons = new ButtonReihe(false,
           "Hauptbilder Bearbeiten",
           "Hintergrundbilder Bearbeiten",
           "Rückseitenbilder Bearbeiten",
           "Textbilder Bearbeiten");
        private ButtonReihe DarstellungenButtons = new ButtonReihe(false,
            "Bilddarstellungen Bearbeiten",
           "Hintergrunddarstellungen Bearbeiten",
           "Textdarstellungen Bearbeiten",
           "Titeldarstellungen Bearbeiten",
           "Infodarstellungen Bearbeiten",
           "Layoutdarstellungen Bearbeiten");
        private ScrollList ScrollList = new ScrollList();
        private CheckBox checkBox1 = new CheckBox();
        private CheckBox checkBox2 = new CheckBox();
        private TextBox textBox1 = new TextBox();
        private Button SettingsButton = new Button();
        private Button PrintDeck = new Button();
        protected ViewKarte ViewKarte;

        private OpenFileDialog OpenFileDialog = new OpenFileDialog();

        public StartForm()
        {
            BuildUp();
        }

        protected void BuildUp()
        {
            SteuerBox.NeuClicked += new EventHandler(SteuerBox_NeuClicked);
            SteuerBox.SpeichernClicked += new EventHandler(SteuerBox_SpeichernClicked);
            SteuerBox.LadenClicked += new EventHandler(SteuerBox_LadenClicked);
            Controls.Add(SteuerBox);

            ToolTip.SetToolTip(checkBox1, "Sollen alle benutzten Bilder gesammelt und in einen Ordner mit dem Spiel abgespeichert werden?");
            ToolTip.SetToolTip(checkBox2, "Sollen Hintergrundbilder und Rückseitenbilder ferner in Jpg mit 90% Qualität konvertiert werden, um Speicherplatz zu sparen?");
            checkBox2.Enabled = false;
            checkBox1.Text = "Bilder extra Abspeichern? (Erhöht die Speicherdauer um ein paar Minuten...)";
            checkBox2.Text = "Bilder in Jpeg (90%) abspeichern? (Spart Speicherplatz)";
            checkBox1.AutoSize = checkBox2.AutoSize = true;
            Controls.Add(checkBox1);
            //Controls.Add(checkBox2);
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;

            //SteuerBox_NeuClicked(this, EventArgs.Empty);
            MachNeuesStandardUniverse();

            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            textBox1.Width = 200;

            BilderButtons.ButtonClick += new EventHandler(BilderButtons_ButtonClick);
            ToolTip.SetToolTip(BilderButtons, "Lädt mehrere Bilder auf einmal in die Datenbank des Spiels ein.");
            BilderButtons.SetToolTip("Lädt mehrere Hauptbilder, das sind Bilder die vorne auf der Vorderseite der Karte zu sehen sind, auf einmal in die Datenbank des Spiels ein.",
                "Lädt mehrere Hintergrund, das sind Bilder die hinten auf der Vorderseite der Karte zu sehen sind, auf einmal in die Datenbank des Spiels ein.",
                "Lädt mehrere Rückseitenbilder, das sind Bilder die hinten auf der Rückseite der Karte zu sehen sind, auf einmal in die Datenbank des Spiels ein.",
                "Lädt mehrere Textbilder, das sind Bilder die im Text der Karte angezeigt werden können, auf einmal in die Datenbank des Spiels ein.");

            ElementMengenButtons.ButtonClick += new EventHandler(ElementMengenButtons_ButtonClick);
            ToolTip.SetToolTip(ElementMengenButtons, "Ermöglicht es Karten, Kartendecks und andere Ressourcen des Spiels bearbeiten zu können.");
            ElementMengenButtons.SetToolTip("Ermöglicht es Gesinnungen, das ist der Text links unten bei einer Karte, des Spiels bearbeiten zu können.",
                "Ermöglicht es Fraktionen des Spiels bearbeiten zu können.",
                "Ermöglicht es Karten des Spiels bearbeiten zu können.",
                "Ermöglicht es Decks, das sind Zusammenstellungen von Karten, des Spiels bearbeiten zu können.");

            BildMengenButtons.ButtonClick += new EventHandler(BildMengenButtons_ButtonClick);
            ToolTip.SetToolTip(BildMengenButtons, "Ermöglicht es Bilder des Spiels bearbeiten zu können.");
            BildMengenButtons.SetToolTip("Ermöglicht es Hauptbilder, das sind Bilder die vorne auf der Vorderseite der Karte zu sehen sind, zu bearbeiten.",
              "Ermöglicht es Hintergrund, das sind Bilder die hinten auf der Vorderseite der Karte zu sehen sind, zu bearbeiten.",
              "Ermöglicht es Rückseitenbilder, das sind Bilder die hinten auf der Rückseite der Karte zu sehen sind, zu bearbeiten.",
              "Ermöglicht es Textbilder, das sind Bilder die im Text der Karte angezeigt werden können, zu bearbeiten.");

            DarstellungenButtons.ButtonClick += new EventHandler(DarstellungenButtons_ButtonClick);
            ToolTip.SetToolTip(DarstellungenButtons, "Ermöglicht es Darstellungen des Spiels bearbeiten zu können.");
            BildMengenButtons.SetToolTip("Ermöglicht es Bilddarstellungen, diese entscheiden, ob für eine Karte ein Hauptbild angezeigt werden soll, zu bearbeiten.",
              "Ermöglicht es Hintergrunddarstellungen, diese legen fest, wie groß eine Karte und ihr Rand ist, zu bearbeiten.",
              "Ermöglicht es Textdarstellungen, diese legen die Schriftart des Textblockes fest, zu bearbeiten.",
              "Ermöglicht es Titeldarstellungen, diese legen die Schriftart des Titels fest, zu bearbeiten.",
              "Ermöglicht es Infodarstellungen, diese legen die Schriftart des Infoteils fest, zu bearbeiten.",
              "Ermöglicht es Layoutdarstellungen, diese legen die Elemente des Layout fest, zu bearbeiten.");

            PrintDeck.AutoSize = true;
            PrintDeck.Text = "Ein Kartendeck in eine PDF-Datei verwandeln";
            PrintDeck.Click += new EventHandler(PrintDeck_Click);

            ScrollList.AddControl(textBox1, BilderButtons, ElementMengenButtons,
                PrintDeck, BildMengenButtons, DarstellungenButtons);
            Controls.Add(ScrollList);

            SettingsButton.AutoSize = true;
            SettingsButton.Text = "Einstellungen Ändern";
            SettingsButton.Click += new EventHandler(SettingsButton_Click);
            Controls.Add(SettingsButton);

            OpenFileDialog.Filter = "Bilder|*.jpg; *.jpeg; *.png; *.bmp; *.gif; *.tiff; *.tif; *.wmf";
            OpenFileDialog.Multiselect = true;

            this.ClientSize = new Size(1200, 800);
        }

        private void PrintDeck_Click(object sender, EventArgs e)
        {
            PrintForm pf = new PrintForm();
            pf.Universe = Universe;
            pf.ShowDialog();
        }
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
            ViewKarte.OnKarteChanged();
        }
        private void BildMengenButtons_ButtonClick(object sender, EventArgs e)
        {
            switch (BildMengenButtons.Message)
            {
                case "Hauptbilder Bearbeiten":
                    new ElementAuswahlForm<HauptBild>(Universe.HauptBilder).ShowDialog();
                    Changed(true);
                    break;
                case "Hintergrundbilder Bearbeiten":
                    new ElementAuswahlForm<HintergrundBild>(Universe.Karten.Standard.DeepClone(), Universe.HintergrundBilder).ShowDialog();
                    Changed(true);
                    break;
                case "Textbilder Bearbeiten":
                    new ElementAuswahlForm<TextBild>(Universe.TextBilder).ShowDialog();
                    Changed(true);
                    break;
                case "Rückseitenbilder Bearbeiten":
                    new ElementAuswahlForm<RuckseitenBild>(Universe.Karten.Standard.DeepClone(), Universe.RuckseitenBilder).ShowDialog();
                    Changed(true);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void BilderButtons_ButtonClick(object sender, EventArgs e)
        {
            switch (BilderButtons.Message)
            {
                case "Hauptbilder Sammeln":
                    LadeBilder<HauptBild>();
                    Changed(true);
                    break;
                case "Hintergrundbilder Sammeln":
                    LadeBilder<HintergrundBild>();
                    Changed(true);
                    break;
                case "Textbilder Sammeln":
                    LadeBilder<TextBild>();
                    Changed(true);
                    break;
                case "Rückseitenbilder Sammeln":
                    LadeBilder<RuckseitenBild>();
                    Changed(true);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void DarstellungenButtons_ButtonClick(object sender, EventArgs e)
        {
            switch (DarstellungenButtons.Message)
            {
                case "Bilddarstellungen Bearbeiten":
                    new ElementAuswahlForm<BildDarstellung>(Universe.BildDarstellungen).ShowDialog();
                    Changed(true);
                    break;
                case "Titeldarstellungen Bearbeiten":
                    new ElementAuswahlForm<TitelDarstellung>(Universe.TitelDarstellungen).ShowDialog();
                    Changed(true);
                    break;
                case "Textdarstellungen Bearbeiten":
                    new ElementAuswahlForm<TextDarstellung>(Universe.TextDarstellungen).ShowDialog();
                    Changed(true);
                    break;
                case "Infodarstellungen Bearbeiten":
                    new ElementAuswahlForm<InfoDarstellung>(Universe.InfoDarstellungen).ShowDialog();
                    Changed(true);
                    break;
                case "Hintergrunddarstellungen Bearbeiten":
                    new ElementAuswahlForm<HintergrundDarstellung>(Universe.HintergrundDarstellungen).ShowDialog();
                    Changed(true);
                    break;
                case "Layoutdarstellungen Bearbeiten":
                    new ElementAuswahlForm<LayoutDarstellung>(Universe.LayoutDarstellungen).ShowDialog();
                    Changed(true);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void LadeBilder<T>() where T : Bild, new()
        {
            if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                List<T> tooBig = new List<T>();
                ElementMenge<T> Menge = Universe.GetElementMenge<T>();
                foreach (var item in OpenFileDialog.FileNames)
                {
                    T bild = new T();
                    bild.Init(Universe);
                    bild.FilePath = item;
                    Size s = bild.GetImageSize();
                    if (s.Width * s.Height > Settings.MaximumImageArea)
                        tooBig.Add(bild);
                    else
                    {
                        bild.SetAutoSize();
                        bild.Name = bild.Schreibname = Path.GetFileNameWithoutExtension(item);
                        Menge.AddPolymorph(bild);
                    }
                }
                if (!tooBig.Empty())
                {
                    MessageBox.Show("Die Bilder der Adressen\r\n\r\n"
                        + tooBig.Map(x => x.TotalFilePath).SumText("\r\n")
                        + "\r\n\r\nwurden nicht reingeladen, da ihre Größen jeweils das maximale Pixelprodukt von Breite x Höhe <= "
                        + Settings.MaximumImageArea + " überschreiten!",
                        "Memory-Überschreitung",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
        protected virtual void ElementMengenButtons_ButtonClick(object sender, EventArgs e)
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
                    new ElementAuswahlForm<Karte>(Universe.Karten).ShowDialog();
                    Changed(true);
                    break;
                case "Decks Bearbeiten":
                    new ElementAuswahlForm<Deck>(Universe.Decks).ShowDialog();
                    Changed(true);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void SteuerBox_LadenClicked(object sender, EventArgs e)
        {
            U u = new U();
            u.Open(SteuerBox.Speicherort);
            Universe = u;
        }
        private void SteuerBox_SpeichernClicked(object sender, EventArgs e)
        {
            Changed(false);
            Universe.Rescue();
            if (checkBox1.Checked)
                Universe.Lokalisieren(checkBox2.Checked, Path.GetDirectoryName(SteuerBox.Speicherort));
            Universe.Save(SteuerBox.Speicherort);
        }
        private void SteuerBox_NeuClicked(object sender, EventArgs e)
        {
            //U u = new U();
            //u.Open(Path.Combine(Directory.GetCurrentDirectory(), "Ressourcen/Universe.xml"));
            //Universe = u;
            ElementMenge<U> Us = new ElementMenge<U>("Universen", null);
            Loader l = new Loader(null, "Ressourcen/Universen.xml");
            l.XmlReader.Next();
            Us.Read(l);
            foreach (var item in Us)
                item.Value.Root(Path.Combine(Directory.GetCurrentDirectory(), "Ressourcen/Universen.xml"));
            ElementAuswahlForm<U> f = new ElementAuswahlForm<U>(Us.Standard.Karten.Standard.Clone() as Karte, Us, false);
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Universe = f.Element;
                PrintDeck.Enabled = false;
                SteuerBox.Speicherort = null;
            }
        }

        private void MachNeuesStandardUniverse()
        {
            ElementMenge<U> Us = new ElementMenge<U>("Universen", null);
            Loader l = new Loader(null, "Ressourcen/Universen.xml");
            l.XmlReader.Next();
            Us.Read(l);
            Us.Standard.Root(Path.Combine(Directory.GetCurrentDirectory(), "Ressourcen/Universen.xml"));
            Universe = Us.Standard;
            PrintDeck.Enabled = false;
            SteuerBox.Speicherort = null;
        }
        private void SetUniverse(U Universe)
        {
            this.universe = Universe;
            this.Text = this.textBox1.Text = universe.Schreibname;
            BuildViewKarte();
            Changed(false);
        }
        protected virtual void BuildViewKarte()
        {
            if (ViewKarte == null)
            {
                ViewKarte = new ViewKarte();
                Controls.Add(ViewKarte);
            }
            ViewKarte.Karte = Universe.Karten.Standard;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            ViewKarte.Size = new Size(ClientSize.Width / 3, ClientSize.Height);

            SteuerBox.Location = new Point(ViewKarte.Right + 20, ClientSize.Height - 150);
            checkBox1.Location = new Point(SteuerBox.Right - 250, ClientSize.Height - 120);
            checkBox2.Location = new Point(SteuerBox.Right - 250, ClientSize.Height - 100);
            SettingsButton.Location = new Point(ViewKarte.Right + 20, ClientSize.Height - 50);

            ScrollList.Location = new Point(ViewKarte.Right + 20, 20);
            ScrollList.Size = new Size(ClientSize.Width - ScrollList.Left - 20, SteuerBox.Top - 40);
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !SteuerBox.CanExit();
            base.OnClosing(e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checkBox2.Enabled = checkBox1.Checked;
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.Text = Universe.Name = Universe.Schreibname = textBox1.Text;
            //if (SteuerBox.Speicherort != null)
            //    SteuerBox.Speicherort = Path.Combine(Path.GetDirectoryName(SteuerBox.Speicherort), textBox1.Text + ".xml");
            Changed(true);
        }
        protected void Changed(bool changed)
        {
            ViewKarte.OnKarteChanged();
            SteuerBox.SpeichernNotwendig = changed;
            if (changed)
                this.Text = Universe.Schreibname + "*";
            else
                this.Text = Universe.Schreibname;
            PrintDeck.Enabled = !changed;
        }
    }
}
