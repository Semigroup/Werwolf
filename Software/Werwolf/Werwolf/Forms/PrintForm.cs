using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;

using Assistment.Extensions;
using Assistment.Drawing.Geometries.Extensions;
using Assistment.Texts;

using Werwolf.Inhalt;
using Werwolf.Karten;
using Werwolf.Printing;

namespace Werwolf.Forms
{
    public partial class PrintForm : Form
    {
        private string TargetPath;
        private readonly Job Job = new Job();

        private Universe universe;
        private Deck deck;
        private List<KeyValuePair<Karte, int>> fullSortedDeckList;
        public Deck Deck
        {
            get { return deck; }
            set
            {
                deck = value;
                label1.Text = deck.Schreibname;
                fullSortedDeckList = deck.GetSortedList();
                int n = deck.TotalCount();
                if (n > 99 && !Environment.Is64BitProcess)
                    MessageBox.Show("Achtung. Das Deck " + deck.Schreibname + " besitzt " + n + " Karten.\r\n"
                        + "Decks, die echt mehr als 99 Karten besitzen, können auf Grund von Memory-Gründen zu einem Programmabsturz führen.");
            }
        }
        public Universe Universe
        {
            get { return universe; }
            set
            {
                universe = value;
                Deck = universe.Decks.Standard;
            }
        }

        public PrintForm()
        {
            InitializeComponent();
            this.ppmBox1.PpmMaximum = Settings.MaximumPpm;
        }

        private void DeckButton_Click(object sender, EventArgs e)
        {
            ElementAuswahlForm<Deck> form = new ElementAuswahlForm<Deck>(
                Universe.Karten.Standard,
                Universe.Decks,
                deck,
                new ViewDeck(),
                false);
            if (form.ShowDialog() == DialogResult.OK)
                Deck = form.Element;
        }

        private void FetchJob(Job.OutputType outputType, bool CleanJob)
        {
            Job.RuckBildMode Mode = Job.RuckBildMode.Keine;
            if (radioButton1.Checked)
                Mode = Job.RuckBildMode.Einzeln;
            else if (radioButton2.Checked)
                Mode = Job.RuckBildMode.Gemeinsam;
            else if (radioButton4.Checked)
                Mode = Job.RuckBildMode.Nur;

            Job.Init(deck,
                colorBox1.GetValue(),
                colorBox2.GetValue(),
                ppmBox1.GetValue(),
                checkBox1.Checked,
                Mode,
                checkBox2.Checked,
                checkBox4.Checked,
                checkBox3.Checked,
                dinA4ForcedBox.Checked,
                floatBox1.UserValue,
                outputType,
                consoleBox.Checked,
                CleanJob);
            TargetPath = Path.Combine(saveFileDialog1.FileName);
            Job.Schreibname = Path.GetFileNameWithoutExtension(TargetPath);
            Job.MaxCPU = int.MaxValue;
        }

        private void Printer_DoWork(object sender, DoWorkEventArgs e)
        {
            Drucken.Invoke((MethodInvoker)delegate { Drucken.Enabled = false; });
            DruckenBilder.Invoke((MethodInvoker)delegate { DruckenBilder.Enabled = false; });
            PrintButtonAtlas.Invoke((MethodInvoker)delegate { PrintButtonAtlas.Enabled = false; });
            DruckenJob.Invoke((MethodInvoker)delegate { DruckenJob.Enabled = false; });
            PrintButtonTTS.Invoke((MethodInvoker)delegate { PrintButtonTTS.Enabled = false; });
            button1.Invoke((MethodInvoker)delegate { button1.Enabled = false; });
            PrintButtonDataSheet.Invoke((MethodInvoker)delegate { PrintButtonDataSheet.Enabled = false; });

            string JobPath = Job.Save(TargetPath);
            JobTickerProgressBar jobTickerProgressBar = new JobTickerProgressBar(progressBar1);
            Job.DistributedPrint(JobPath, jobTickerProgressBar);

            Drucken.Invoke((MethodInvoker)delegate { Drucken.Enabled = true; });
            DruckenBilder.Invoke((MethodInvoker)delegate { DruckenBilder.Enabled = true; });
            PrintButtonAtlas.Invoke((MethodInvoker)delegate { PrintButtonAtlas.Enabled = true; });
            DruckenJob.Invoke((MethodInvoker)delegate { DruckenJob.Enabled = true; });
            PrintButtonTTS.Invoke((MethodInvoker)delegate { PrintButtonTTS.Enabled = true; });
            button1.Invoke((MethodInvoker)delegate { button1.Enabled = true; });
            PrintButtonDataSheet.Invoke((MethodInvoker)delegate { PrintButtonDataSheet.Enabled = true; });
        }

        private void Drucken_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Deck.Schreibname;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            FetchJob(Job.OutputType.PDFDocument, true);

            Printer.RunWorkerAsync();
        }

        private void DruckenBilder_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Deck.Schreibname;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            FetchJob(Job.OutputType.JPGImages, true);

            Printer.RunWorkerAsync();
        }

        private void DruckenJob_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Deck.Schreibname;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            FetchJob(Job.OutputType.PDFDocument, false);
            Job.MaxCPU = 1;
            Job.Save(TargetPath);
        }

        private void PrintButtonAtlas_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Deck.Schreibname;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            FetchJob(Job.OutputType.JPGAtlas, true);

            Printer.RunWorkerAsync();
        }


        private void PrintButtonTTS_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Deck.Schreibname;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            FetchJob(Job.OutputType.TTSData, true);

            Printer.RunWorkerAsync();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            FetchJob(Job.OutputType.PDFDocument, true);
            WolfSinglePaper wsp = new WolfSinglePaper(Job, "");
            foreach (var item in Deck.GetKarten(fullSortedDeckList, 0, 9))
                for (int i = 0; i < item.Value; i++)
                    if (Job.MyMode == Job.RuckBildMode.Nur)
                        wsp.TryAdd(new StandardRuckseite(item.Key, Job.Ppm));
                    else
                        wsp.TryAdd(new StandardKarte(item.Key, Job.Ppm));

            wsp.Swapped = Job.MyMode == Job.RuckBildMode.Nur;

            pictureBox1.Image?.Dispose();

            Size s = wsp.Seite.Size.mul(Job.Ppm / WolfBox.Faktor).ToSize();
            Bitmap b = new Bitmap(s.Width, s.Height);
            using (Graphics g = b.GetHighGraphics(Job.Ppm / WolfBox.Faktor))
            using (DrawContextGraphics dcg = new DrawContextGraphics(g, Job.HintergrundFarbe.ToBrush()))
            {
                g.Clear(Job.HintergrundFarbe);
                wsp.Setup(wsp.Seite);
                wsp.Draw(dcg);
            }
            pictureBox1.Image = b;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !Drucken.Enabled;
            base.OnClosing(e);
        }

        private void PrintButtonDataSheet_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Deck.Schreibname;
            if (saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            FetchJob(Job.OutputType.DataSheet, true);

            Printer.RunWorkerAsync();
        }
    }
}
