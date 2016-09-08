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

using Werwolf.Inhalt;

namespace Werwolf.Forms
{
    public class StartForm : Form
    {
        private Universe universe;
        public Universe Universe
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
        private ButtonReihe ElementMengenButtons = new ButtonReihe(false,
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
           "Infodarstellungen Bearbeiten");
        private ScrollList ScrollList = new ScrollList();
        private CheckBox checkBox1 = new CheckBox();
        private CheckBox checkBox2 = new CheckBox();
        private TextBox textBox1 = new TextBox();
        private Button SettingsButton = new Button();
        private Button PrintDeck = new Button();
        private ViewKarte ViewKarte;

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
            Controls.Add(checkBox2);
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;

            SteuerBox_NeuClicked(this, EventArgs.Empty);

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
              "Ermöglicht es Infodarstellungen, diese legen die Schriftart des Infoteils fest, zu bearbeiten.");


            PrintDeck.AutoSize = true;
            PrintDeck.Text = "Ein Kartendeck in eine PDF-Datei verwandeln";
            PrintDeck.Click += new EventHandler(PrintDeck_Click);

            ScrollList.AddControl(textBox1, BilderButtons, ElementMengenButtons,
                PrintDeck, BildMengenButtons, DarstellungenButtons);
            ScrollList.SetUp();
            Controls.Add(ScrollList);

            SettingsButton.AutoSize = true;
            SettingsButton.Text = "Einstellungen Ändern";
            SettingsButton.Click += new EventHandler(SettingsButton_Click);
            Controls.Add(SettingsButton);

            OpenFileDialog.Filter = "Bilder|*.jpg; *.jpg; *.png; *.bmp; *.gif; *.tiff; *.tif; *.wmf";
            OpenFileDialog.Multiselect = true;

            this.ClientSize = new Size(1200, 800);
        }

        private void PrintDeck_Click(object sender, EventArgs e)
        {
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
                    break;
                case "Hintergrundbilder Bearbeiten":
                    new ElementAuswahlForm<HintergrundBild>(Universe.HintergrundBilder).ShowDialog();
                    break;
                case "Textbilder Bearbeiten":
                    new ElementAuswahlForm<TextBild>(Universe.TextBilder).ShowDialog();
                    break;
                case "Rückseitenbilder Bearbeiten":
                    new ElementAuswahlForm<RuckseitenBild>(Universe.RuckseitenBilder).ShowDialog();
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
                    break;
                case "Hintergrundbilder Sammeln":
                    LadeBilder<HintergrundBild>();
                    break;
                case "Textbilder Sammeln":
                    LadeBilder<TextBild>();
                    break;
                case "Rückseitenbilder Sammeln":
                    LadeBilder<RuckseitenBild>();
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
                    break;
                case "Titeldarstellungen Bearbeiten":
                    new ElementAuswahlForm<TitelDarstellung>(Universe.TitelDarstellungen).ShowDialog();
                    break;
                case "Textdarstellungen Bearbeiten":
                    new ElementAuswahlForm<TextDarstellung>(Universe.TextDarstellungen).ShowDialog();
                    break;
                case "Infodarstellungen Bearbeiten":
                    new ElementAuswahlForm<InfoDarstellung>(Universe.InfoDarstellungen).ShowDialog();
                    break;
                case "Hintergrunddarstellungen Bearbeiten":
                    new ElementAuswahlForm<HintergrundDarstellung>(Universe.HintergrundDarstellungen).ShowDialog();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        private void LadeBilder<T>() where T : Bild, new()
        {
            if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ElementMenge<T> Menge = Universe.GetElementMenge<T>();
                foreach (var item in OpenFileDialog.FileNames)
                {
                    T bild = new T();
                    bild.Init(Universe);
                    bild.FilePath = item;
                    bild.SetAutoSize();
                    bild.Name = bild.Schreibname = Path.GetFileNameWithoutExtension(item);
                    Menge.AddPolymorph(bild);
                }
            }
        }
        private void ElementMengenButtons_ButtonClick(object sender, EventArgs e)
        {
            switch (ElementMengenButtons.Message)
            {
                case "Gesinnungen Bearbeiten":
                    new ElementAuswahlForm<Gesinnung>(Universe.Gesinnungen).ShowDialog();
                    break;
                case "Fraktionen Bearbeiten":
                    new ElementAuswahlForm<Fraktion>(Universe.Fraktionen).ShowDialog();
                    break;
                case "Karten Bearbeiten":
                    new ElementAuswahlForm<Karte>(Universe.Karten).ShowDialog();
                    break;
                case "Decks Bearbeiten":
                    new ElementAuswahlForm<Deck>(Universe.Decks).ShowDialog();
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        private void SteuerBox_LadenClicked(object sender, EventArgs e)
        {
            Universe = new Universe(SteuerBox.Speicherort);
        }
        private void SteuerBox_SpeichernClicked(object sender, EventArgs e)
        {
            Changed(false);
            Universe.Root(SteuerBox.Speicherort);
            if (checkBox1.Checked)
                Universe.Lokalisieren(checkBox2.Checked);
            Universe.Save(SteuerBox.Speicherort);
        }
        private void SteuerBox_NeuClicked(object sender, EventArgs e)
        {
            Universe = new Universe(Path.Combine(Directory.GetCurrentDirectory(), "Ressourcen/Universe.xml"));
        }

        private void SetUniverse(Universe Universe)
        {
            this.universe = Universe;
            this.Text = this.textBox1.Text = universe.Schreibname;
            if (ViewKarte == null)
            {
                ViewKarte = new ViewKarte();
                Controls.Add(ViewKarte);
                ViewKarte.Karte = Universe.Karten.Standard;
            }
            SteuerBox.Speicherort = textBox1.Text + ".xml";
            Changed(false);
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
            SteuerBox.Speicherort = textBox1.Text + ".xml";
            Changed(true);
        }
        private void Changed(bool changed)
        {
            SteuerBox.SpeichernNotwendig = changed;
            if (changed)
                this.Text = Universe.Schreibname + "*";
            else
                this.Text = Universe.Schreibname;
        }
    }
}
