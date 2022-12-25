using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;

using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;
using Werwolf.Printing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using iTextSharp.text;

namespace WolfSlave
{
    public class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        //const int SW_HIDE = 0;
        //const int SW_SHOW = 5;

        static Job Job;
        static int JobNumber;
        static string JobPath;
        static Universe Universe;

        [DllImport("Shcore.dll")]
        static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        // According to https://msdn.microsoft.com/en-us/library/windows/desktop/dn280512(v=vs.85).aspx
        private enum DpiAwareness
        {
            None = 0,
            SystemAware = 1,
            PerMonitorAware = 2
        }

        static void Main(string[] args)
        {
            //#if !DEBUG
            //            HideConsole();
            //#endif
            Application.SetCompatibleTextRenderingDefault(false);

            SetProcessDpiAwareness((int)DpiAwareness.PerMonitorAware); //PerMonitorAware makes the Line Height higher (why?)
            //Has been fixed by changes in FontGraphicsMeasurer in Assistment.Texts

            string UniversePath = args[0];
            JobPath = args[1];
            JobNumber = int.Parse(args[2]);

            Console.WriteLine(UniversePath);
            Console.WriteLine(JobPath);
            Console.WriteLine(JobNumber);

            Universe = new Universe(UniversePath);
            Console.WriteLine("Universe read");
            Job = new Job(Universe, JobPath);
            Console.WriteLine("Job read");
            //if (!Job.KonsolenAnzeigen)
            //    HideConsole();

            try
            {
                Console.WriteLine(Job.MyMode + ", " + JobNumber);
                //Console.WriteLine(Job.MachBilder ? "Bilder" : "PDF");
                Console.WriteLine(Job.OutputFileType);
                switch (Job.OutputFileType)
                {
                    case Job.OutputType.PDFDocument:
                        CreatePDF();
                        break;
                    case Job.OutputType.JPGImages:
                    case Job.OutputType.TTSData:
                        CreateImage();
                        break;
                    case Job.OutputType.JPGAtlas:
                        CreateAtlas();
                        break;
                    default:
                        throw new NotImplementedException("Unknown enum for OutputType: " + Job.OutputFileType);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
        }

        private static (int rows, int columns) Distribute(int numberCards)
        {

            switch (numberCards)
            {
                case 0:
                    return (0, 0);
                case 1:
                case 2:
                case 3:
                case 5:
                    return (1, numberCards);

                case 4:
                case 6:
                case 7:
                case 8:
                case 10:
                    return (2, (numberCards + 1) / 2);

                case 9:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 17:
                case 18:
                case 21:
                    return (3, (numberCards + 2) / 3);

                case 16:
                case 19:
                case 20:
                case 22:
                case 23:
                case 24:
                case 26:
                case 27:
                case 28:
                case 31:
                case 32:
                    return (4, (numberCards + 3) / 4);

                case 25:
                case 29:
                case 30:
                case 33:
                case 34:
                case 35:
                    return (5, (numberCards + 4) / 5);

                case 36:
                    return (6, (numberCards + 4) / 5);
            }

            double a = Math.Sqrt(numberCards / (16.0 * 9.0));
            int columns = (int)Math.Ceiling(16 * a);
            int rows = (int)Math.Ceiling(9 * a);

            bool changed = true;
            while (changed)
            {
                changed = false;
                if ((columns - 1) * rows >= numberCards)
                {
                    columns--;
                    changed = true;
                }
                if (columns * (rows - 1) >= numberCards)
                {
                    rows--;
                    changed = true;
                }
            }

            return (rows, columns);
        }
        private static void CreateAtlas()
        {
            //ToDo
            int numberProSheet = 24;

            var cards = Deck.GetKarten(Job.FullSortedDeckList, JobNumber * numberProSheet, numberProSheet);

            var numberCards = 24;// cards.Map(item => item.Value).Sum();

            (int rows, int columns) = Distribute(numberCards);

            SizeF sizeCard = Job.Deck.GetKartenSize().mul(Job.Ppm);
            //Console.WriteLine(sizeCard.ToString());
            Size sizeAtlas = sizeCard.mul(columns, rows).ToSize();
            Console.WriteLine("Trying to create a bitmap of size " + sizeAtlas.ToString());
            Bitmap atlas = new Bitmap(sizeAtlas.Width, sizeAtlas.Height);
            Console.WriteLine("Success!");

            using (Graphics g = atlas.GetGraphics(Job.Ppm / WolfBox.Faktor, Job.HintergrundFarbe, true))
            using (DrawContextGraphics context = new DrawContextGraphics(g))
            {
                Point index = new Point(0, 0);
                int itemNumber = 0;
                foreach (var item in cards)
                    if (item.Value > 0)
                    {
                        Console.WriteLine(itemNumber + " of " + numberCards);
                        itemNumber++;
                        var card = item.Key;
                        var sprite = new StandardKarte(card, Job.Ppm);
                        sprite.Setup(0);
                        for (int i = 0; i < item.Value; i++)
                        {
                            var offset = sizeCard.mul(index).mul(WolfBox.Faktor / Job.Ppm).ToPointF();
                            //Console.WriteLine("index: " + index.ToString());
                            //Console.WriteLine(offset.ToString());
                            g.TranslateTransform(offset.X, offset.Y);
                            g.SetClip(new RectangleF(new PointF(), sizeCard.mul(WolfBox.Faktor / Job.Ppm)));
                            //sprite.Move(offset);
                            sprite.Draw(context);
                            //card.DrawOnGraphics(g, Job.Ppm, sprite, false);
                            g.TranslateTransform(-offset.X, -offset.Y);

                            index.X++;
                            if (index.X == columns)
                            {
                                index.X = 0;
                                index.Y++;
                            }
                        }
                    }
            }

            var atlasFilePath = Path.Combine(Path.GetDirectoryName(JobPath), Job.Schreibname + (JobNumber + 1) + ".jpg");
            atlas.Save(atlasFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        static void CreatePDF()
        {
            string p = Path.Combine(Path.GetDirectoryName(JobPath), Job.Schreibname + "." + JobNumber);

            WolfSinglePaper wsp = new WolfSinglePaper(Job, p);
            AddKarten(Job, JobNumber, wsp);

            Console.WriteLine(p);
            Console.WriteLine(wsp.Seite.div(WolfBox.Faktor) + "");
            wsp.CreatePDF(p, wsp.Min, float.MaxValue, wsp.PageSize, Job.HintergrundFarbe);
        }
        static void CreateImage()
        {
            int i = 0;
            Karte Karte = null;
            foreach (var item in Job.Deck.Karten)
                if (item.Value > 0)
                    if (i++ == JobNumber)
                    {
                        Karte = item.Key;
                        break;
                    }

            var filepath = Job.MapCardNameToFileName(Karte, JobPath);
            using (System.Drawing.Image img = Karte.GetImage(Job.Ppm, true))
                img.Save(filepath, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        //static void HideConsole()
        //{
        //    var handle = GetConsoleWindow();

        //    Hide
        //    ShowWindow(handle, SW_HIDE);

        //    Show
        //    ShowWindow(handle, SW_SHOW);
        //}

        static void AddKarten(Job Job, int JobNumber, WolfSinglePaper wsp)
        {
            int numberProSheet = Job.GetNumberProSheet();

            switch (Job.MyMode)
            {
                case Job.RuckBildMode.Keine:
                    foreach (var item in Deck.GetKarten(Job.FullSortedDeckList, JobNumber * numberProSheet, numberProSheet))
                        for (int i = 0; i < item.Value; i++)
                            wsp.TryAdd(GetKarte(item.Key, Job));
                    break;
                case Job.RuckBildMode.Einzeln:
                    foreach (var item in Deck.GetKarten(Job.FullSortedDeckList, (JobNumber / 2) * numberProSheet, numberProSheet))
                        for (int i = 0; i < item.Value; i++)
                            if (JobNumber % 2 == 0)
                                wsp.TryAdd(GetKarte(item.Key, Job));
                            else
                                wsp.TryAdd(GetRuckseite(item.Key, Job));
                    wsp.Swapped = JobNumber % 2 == 1;
                    break;
                case Job.RuckBildMode.Nur:
                    foreach (var item in Deck.GetKarten(Job.FullSortedDeckList, JobNumber * numberProSheet, numberProSheet))
                        for (int i = 0; i < item.Value; i++)
                            wsp.TryAdd(GetRuckseite(item.Key, Job));
                    wsp.Swapped = true;
                    break;
                case Job.RuckBildMode.Gemeinsam:
                default:
                    throw new NotImplementedException();
            }
        }
        public static WolfBox GetKarte(Karte Karte, Job Job)
        {
            if (Job.FixedFont)
                return new FixedFontKarte(Karte, Job.Ppm, false, Job.Rotieren);
            else
                return new StandardKarte(Karte, Job.Ppm);
        }
        public static WolfBox GetRuckseite(Karte Karte, Job Job)
        {
            if (Job.FixedFont)
                return new FixedFontKarte(Karte, Job.Ppm, true, Job.Rotieren);
            else
                return new StandardRuckseite(Karte, Job.Ppm);
        }
    }
}
