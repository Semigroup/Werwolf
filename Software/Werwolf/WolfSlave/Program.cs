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

using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;
using Werwolf.Printing;
using Assistment.Drawing.LinearAlgebra;

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

        static void Main(string[] args)
        {
            //HideConsole();

            string UniversePath = args[0];
            string JobPath = args[1];
            int JobNumber = int.Parse(args[2]);

            Console.WriteLine(UniversePath);
            Console.WriteLine(JobPath);
            Console.WriteLine(JobNumber);

            Universe Universe = new Universe(UniversePath);
            Console.WriteLine("Universe read");
            Job Job = new Job(Universe, JobPath);
            Console.WriteLine("Job read");

            try
            {
                Console.WriteLine(Job.MyMode + ", " + JobNumber);
                WolfSinglePaper wsp = new WolfSinglePaper(Job);
                AddKarten(Job, JobNumber, wsp);

                string p = Path.Combine(Path.GetDirectoryName(JobPath), Job.Schreibname + "." + JobNumber);
                Console.WriteLine(p);
                Console.WriteLine(wsp.Seite.div(WolfBox.Faktor) + "");
                wsp.createPDF(p, wsp.getMin(), float.MaxValue, wsp.PageSize, Job.HintergrundFarbe);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.ReadKey();
            }
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
            switch (Job.MyMode)
            {
                case Job.RuckBildMode.Keine:
                    foreach (var item in Job.Deck.GetKarten(JobNumber * 9, 9))
                        for (int i = 0; i < item.Value; i++)
                            wsp.TryAdd(GetKarte(item.Key, Job));
                    break;
                case Job.RuckBildMode.Einzeln:
                    foreach (var item in Job.Deck.GetKarten((JobNumber / 2) * 9, 9))
                        for (int i = 0; i < item.Value; i++)
                            if (JobNumber % 2 == 0)
                                wsp.TryAdd(GetKarte(item.Key, Job));
                            else
                                wsp.TryAdd(new StandardRuckseite(item.Key, Job.Ppm));
                    wsp.Swapped = JobNumber % 2 == 1;
                    break;
                case Job.RuckBildMode.Nur:
                    foreach (var item in Job.Deck.GetKarten(JobNumber * 9, 9))
                        for (int i = 0; i < item.Value; i++)
                            wsp.TryAdd(new StandardRuckseite(item.Key, Job.Ppm));
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
                return new FixedFontKarte(Karte, Job.Ppm);
            else
                return new StandardKarte(Karte, Job.Ppm);
        }
    }
}
