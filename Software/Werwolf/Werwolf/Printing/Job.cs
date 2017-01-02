using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Threading;
using System.Diagnostics;

using Assistment.Xml;
using Assistment.Extensions;
using Assistment.Mathematik;
using Assistment.PDF;
using Assistment.Drawing.LinearAlgebra;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Printing
{
    public class Job : XmlElement
    {
        public enum RuckBildMode
        {
            Keine,
            Einzeln,
            Nur,
            Gemeinsam
        }

        public Deck Deck { get; set; }
        public Color HintergrundFarbe { get; set; }
        public Color TrennlinienFarbe { get; set; }
        public float Ppm { get; set; }
        public bool Zwischenplatz { get; set; }
        public RuckBildMode MyMode { get; set; }
        public bool FixedFont { get; set; }
        public bool TrennlinieVorne { get; set; }
        public bool TrennlinieHinten { get; set; }
        /// <summary>
        /// Dateigröße in MB
        /// </summary>
        public float MaximaleGrose { get; set; }

        public Job(Universe Universe, string Pfad)
            : this()
        {
            this.Init(Universe);
            using (Loader l = new Loader(Universe, Pfad))
            {
                l.XmlReader.Next();
                this.Read(l);
                l.Dispose();
            }
        }
        public Job()
            : base("Job", true)
        {
        }
        public void Init(Deck Deck, Color HintergrundFarbe, Color TrennlinienFarbe,
            float Ppm, bool Zwischenplatz, RuckBildMode MyMode,
            bool FixedFont, bool TrennlinieVorne, bool TrennlinieHinten,
            float MaximaleGrose)
        {
            this.Deck = Deck;
            this.HintergrundFarbe = HintergrundFarbe;
            this.TrennlinienFarbe = TrennlinienFarbe;
            this.Ppm = Ppm;
            this.Zwischenplatz = Zwischenplatz;
            this.MyMode = MyMode;
            this.FixedFont = FixedFont;
            this.TrennlinieVorne = TrennlinieVorne;
            this.TrennlinieHinten = TrennlinieHinten;
            this.MaximaleGrose = MaximaleGrose;

            this.Init(Deck.Universe);
            this.Name = this.Schreibname = Deck.Name + "-Job";
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            this.Deck = Loader.GetDeck();
            this.HintergrundFarbe = Loader.XmlReader.getColorHexARGB("HintergrundFarbe");
            this.TrennlinienFarbe = Loader.XmlReader.getColorHexARGB("TrennlinienFarbe");
            this.Ppm = Loader.XmlReader.getFloat("Ppm");
            this.Zwischenplatz = Loader.XmlReader.getBoolean("Zwischenplatz");
            this.MyMode = Loader.XmlReader.getEnum<RuckBildMode>("MyMode");
            this.FixedFont = Loader.XmlReader.getBoolean("FixedFont");
            this.TrennlinieVorne = Loader.XmlReader.getBoolean("TrennlinieVorne");
            this.TrennlinieHinten = Loader.XmlReader.getBoolean("TrennlinieHinten");
            this.MaximaleGrose = Loader.XmlReader.getFloat("MaximaleGrose");
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("Deck", Deck.Name);
            XmlWriter.writeColorHexARGB("HintergrundFarbe", HintergrundFarbe);
            XmlWriter.writeColorHexARGB("TrennlinienFarbe", TrennlinienFarbe);
            XmlWriter.writeFloat("Ppm", Ppm);
            XmlWriter.writeBoolean("Zwischenplatz", Zwischenplatz);
            XmlWriter.writeBoolean("FixedFont", FixedFont);
            XmlWriter.writeEnum<RuckBildMode>("MyMode", MyMode);
            XmlWriter.writeBoolean("TrennlinieVorne", TrennlinieVorne);
            XmlWriter.writeBoolean("TrennlinieHinten", TrennlinieHinten);
            XmlWriter.writeFloat("MaximaleGrose", MaximaleGrose);
        }

        public override void AdaptToCard(Karte Karte)
        {
            throw new NotImplementedException();
        }
        public override void Rescue()
        {
            throw new NotImplementedException();
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public int GetNumberProSheet()
        {
            return WolfSinglePaper.GetNumberOfCards(Deck.GetKartenSize().mul(WolfBox.Faktor)).Inhalt();
        }

        public void DistributedPrint(string TargetPath, ProgressBar progressBar1)
        {
            int numberProSheet = GetNumberProSheet();
            int numberOfCards = Deck.TotalCount();
            int numberOfJobs = (int)Math.Ceiling(numberOfCards * 1f / numberProSheet);
            int solvedJobs = 0;
            if (MyMode == Printing.Job.RuckBildMode.Einzeln)
                numberOfJobs *= 2;

            if (progressBar1 != null)
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = numberOfJobs;
                });

            string JobPath = Path.Combine(Path.GetDirectoryName(TargetPath), Schreibname + ".job.xml");
            using (System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(JobPath))
            {
                writer.WriteStartDocument();
                Write(writer);
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
                        if (progressBar1 != null)
                            progressBar1.Invoke((MethodInvoker)delegate { progressBar1.PerformStep(); });
                    }
                Thread.Sleep(500);
            }

            string[] files = new string[numberOfJobs];
            for (int i = 0; i < numberOfJobs; i++)
                files[i] = Path.Combine(Path.GetDirectoryName(JobPath), Schreibname + "." + i + ".pdf");

            if (MyMode == RuckBildMode.Einzeln)
                PDFHelper.ConcatSplitDoppelseitig(Path.Combine(Path.GetDirectoryName(JobPath), Schreibname), (long)(MaximaleGrose * (1 << 20)), files);
            else
                PDFHelper.ConcatSplit(Path.Combine(Path.GetDirectoryName(JobPath), Schreibname), (long)(MaximaleGrose * (1 << 20)), files);
            foreach (var item in files)
                File.Delete(item);
            File.Delete(JobPath);
        }
    }
}
