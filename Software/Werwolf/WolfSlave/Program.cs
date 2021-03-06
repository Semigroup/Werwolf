﻿using System;
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

namespace WolfSlave
{
    public class Program
    {
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

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

            SetProcessDpiAwareness((int)DpiAwareness.None); //PerMonitorAware makes the Line Height higher (why?)
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
            if (!Job.KonsolenAnzeigen)
                HideConsole();

            try
            {
                Console.WriteLine(Job.MyMode + ", " + JobNumber);
                Console.WriteLine(Job.MachBilder ? "Bilder" : "PDF");
                if (Job.MachBilder)
                    CreateImage();
                else
                    CreatePDF();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
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
            Text t = new Text();
            t.AddRegex(Karte.Name);
            string s = t.ToString().ToFileName("_");
            s = s.Replace(" ", "");
            s = Path.Combine(Path.GetDirectoryName(JobPath), s + ".jpg");
            using (Image img = Karte.GetImage(Job.Ppm, true))
                img.Save(s, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        static void HideConsole()
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);

            // Show
            //ShowWindow(handle, SW_SHOW);
        }

        static void AddKarten(Job Job, int JobNumber, WolfSinglePaper wsp)
        {
            int numberProSheet = Job.GetNumberProSheet();

            switch (Job.MyMode)
            {
                case Job.RuckBildMode.Keine:
                    foreach (var item in Job.Deck.GetKarten(Job.FullSortedDeckList,JobNumber * numberProSheet, numberProSheet))
                        for (int i = 0; i < item.Value; i++)
                            wsp.TryAdd(GetKarte(item.Key, Job));
                    break;
                case Job.RuckBildMode.Einzeln:
                    foreach (var item in Job.Deck.GetKarten(Job.FullSortedDeckList, (JobNumber / 2) * numberProSheet, numberProSheet))
                        for (int i = 0; i < item.Value; i++)
                            if (JobNumber % 2 == 0)
                                wsp.TryAdd(GetKarte(item.Key, Job));
                            else
                                wsp.TryAdd(GetRuckseite(item.Key, Job));
                    wsp.Swapped = JobNumber % 2 == 1;
                    break;
                case Job.RuckBildMode.Nur:
                    foreach (var item in Job.Deck.GetKarten(Job.FullSortedDeckList, JobNumber * numberProSheet, numberProSheet))
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
