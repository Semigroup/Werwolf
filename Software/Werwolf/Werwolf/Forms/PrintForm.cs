﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Threading;
using System.Diagnostics;

using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;
using Assistment.PDF;
using Assistment.Mathematik;
using Assistment.Texts;

using Werwolf.Inhalt;
using Werwolf.Karten;
using Werwolf.Printing;

namespace Werwolf.Forms
{
    public partial class PrintForm : Form
    {
        private string TargetPath;
        private Job Job = new Job();

        private Universe universe;
        private Deck deck;
        public Deck Deck
        {
            get { return deck; }
            set
            {
                deck = value;
                label1.Text = deck.Schreibname;
                int n = deck.TotalCount();
                if (n > 99)
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

        private void FetchJob()
        {
            Job.RuckBildMode Mode = Printing.Job.RuckBildMode.Keine;
            if (radioButton1.Checked)
                Mode = Printing.Job.RuckBildMode.Einzeln;
            else if (radioButton2.Checked)
                Mode = Printing.Job.RuckBildMode.Gemeinsam;
            else if (radioButton4.Checked)
                Mode = Printing.Job.RuckBildMode.Nur;

            Job.Init(deck,
                colorBox1.GetValue(),
                colorBox2.GetValue(),
                ppmBox1.GetValue(),
                checkBox1.Checked,
                Mode);
            TargetPath = Path.Combine(saveFileDialog1.FileName);
            Job.Schreibname = Path.GetFileNameWithoutExtension(TargetPath);
        }

        private void DeckButton_Click(object sender, EventArgs e)
        {
            ElementAuswahlForm<Deck> form = new ElementAuswahlForm<Deck>(
                Universe.Karten.Standard,
                Universe.Decks,
                deck,
                new ViewDeck(),
                false);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                Deck = form.Element;
        }
        private void Drucken_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = Deck.Schreibname;
            if (saveFileDialog1.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                return;

            FetchJob();

            Printer.RunWorkerAsync();
        }
        private void Printer_DoWork(object sender, DoWorkEventArgs e)
        {
            int numberOfCards = Deck.TotalCount();
            int numberOfJobs = (int)Math.Ceiling(numberOfCards / 9f);
            int solvedJobs = 0;
            if (Job.MyMode == Printing.Job.RuckBildMode.Einzeln)
                numberOfJobs *= 2;

            progressBar1.Invoke((MethodInvoker)delegate
            {
                progressBar1.Value = 0;
                progressBar1.Maximum = numberOfJobs;
            });
            Drucken.Invoke((MethodInvoker)delegate { Drucken.Enabled = false; });

            string JobPath = Path.Combine(Path.GetDirectoryName(TargetPath), Job.Schreibname + ".job.xml");
            using (XmlWriter writer = XmlWriter.Create(JobPath))
            {
                writer.WriteStartDocument();
                Job.Write(writer);
                writer.WriteEndDocument();
                writer.Close();
            }

            Queue<int> jobs = new Queue<int>();
            for (int i = 0; i < numberOfJobs; i++)
                jobs.Enqueue(i);

            int maxWorkers = FastMath.Min(Settings.MaximumNumberOfCores, numberOfJobs, Environment.ProcessorCount);
            Process[] workers = new Process[maxWorkers];

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = Path.Combine(Directory.GetCurrentDirectory(), "WolfSlave.exe");
            psi.Arguments = '"' + Universe.Pfad + "\" \"" + JobPath + "\"";

            while (solvedJobs < numberOfJobs)
            {
                for (int i = 0; i < maxWorkers; i++)
                    if (workers[i] == null && jobs.Count > 0)
                        workers[i] = Process.Start(psi.FileName, psi.Arguments + " " + jobs.Dequeue());
                    else if (workers[i] != null && workers[i].HasExited)
                    {
                        solvedJobs++;
                        workers[i].Close();
                        workers[i] = null;
                        progressBar1.Invoke((MethodInvoker)delegate { progressBar1.PerformStep(); });
                    }
                Thread.Sleep(500);
            }

            string[] files = new string[numberOfJobs];
            for (int i = 0; i < numberOfJobs; i++)
                files[i] = Path.Combine(Path.GetDirectoryName(JobPath), Job.Schreibname + "." + i + ".pdf");

            PDFHelper.Concat(Path.Combine(Path.GetDirectoryName(JobPath), Job.Schreibname), files);
            foreach (var item in files)
                File.Delete(item);
            File.Delete(JobPath);

            Drucken.Invoke((MethodInvoker)delegate { Drucken.Enabled = true; });
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = !Drucken.Enabled;
            base.OnClosing(e);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            FetchJob();
            WolfSinglePaper wsp = new WolfSinglePaper(Job);
            foreach (var item in deck.GetKarten(0, 9))
                for (int i = 0; i < item.Value; i++)
                    if (Job.MyMode == Printing.Job.RuckBildMode.Nur)
                        wsp.TryAdd(new StandardRuckseite(item.Key, Job.Ppm));
                    else
                        wsp.TryAdd(new StandardKarte(item.Key, Job.Ppm));

            wsp.Swapped = Job.MyMode == Printing.Job.RuckBildMode.Nur;

            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            Size s = wsp.Seite.Size.mul(Job.Ppm / WolfBox.Faktor).ToSize();
            Bitmap b = new Bitmap(s.Width, s.Height);
            using (Graphics g = b.GetHighGraphics(Job.Ppm / WolfBox.Faktor))
            using (DrawContextGraphics dcg = new DrawContextGraphics(g, Job.HintergrundFarbe.ToBrush()))
            {
                g.Clear(Job.HintergrundFarbe);
                wsp.setup(wsp.Seite);
                wsp.draw(dcg);
            }
            pictureBox1.Image = b;
        }
    }
}
