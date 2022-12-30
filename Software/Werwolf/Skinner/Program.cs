using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;

using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;
using Werwolf.Printing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using Assistment.Xml;

namespace Skinner
{
    class Program
    {
        static MyJobTicker Ticker;

        //static string JobDirectory;
        //static string UniverseDirectory;
        //static string PDFDirectory;

        static void Main(string[] args)
        {
            var um = new UniverseManager();
            string jobRoot;

            if (args.Length > 1)
            {
                jobRoot = args[1];
                um.CollectUniverses(args[0]);
            }
            else
                jobRoot = args[0];
            //Console.ReadKey();
            MainCollectStartJobs(um, jobRoot);
        }

        static void MainCollectStartJobs(UniverseManager um, string root)
        {
            Ticker = new MyJobTicker();
            var jobs = Directory.EnumerateFiles(root, "*.job.xml", SearchOption.AllDirectories).ToArray();
            for (int i = 0; i < jobs.Length; i++)
            {
                var path = jobs[i];
                Console.WriteLine("Starting to process job <" + path + ">, " + (i + 1) + " of " + jobs.Length);
                Console.WriteLine(path);
                if (!ProcessJob(um, path))
                    LogError("Job failed!");
            }
        }

        //static void Main2()
        //{
        //    Ticker = new MyJobTicker();

        //    JobDirectory = Directory.GetCurrentDirectory() + "\\Jobs\\";
        //    UniverseDirectory = Directory.GetCurrentDirectory() + "\\Universes\\";
        //    //PDFDirectory = Directory.GetCurrentDirectory() + "\\PDF\\";

        //    if (CheckIfDirectoriesExist())
        //        foreach (string jobFile in Directory.EnumerateFiles(JobDirectory))
        //        {
        //            Console.WriteLine("Fange an, Job <" + jobFile + "> zu bearbeiten.");
        //            if (ProcessJob(jobFile))
        //                Console.WriteLine("Job <" + jobFile + "> konnte erfolgreich vearbeitet werden.");
        //            else
        //                Console.WriteLine("Job <" + jobFile + "> konnte NICHT erfolgreich vearbeitet werden.");
        //        }
        //    Console.ReadKey();
        //}

       public static void LogError(object o)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("[Error] " + o);
            Console.ForegroundColor = ConsoleColor.Gray;
        }



        public static bool ProcessJob(UniverseManager um, string jobFile)
        {
            Job job = new Job();
            using (Loader loader = new DummyLoader(jobFile))
            {
                loader.XmlReader.Next();
                job.Read(loader);
            }

            //var working_directory = Path.GetDirectoryName(jobFile);
            //Directory.SetCurrentDirectory(working_directory);
            //Console.WriteLine("Changing current Directory to " + working_directory);

            //string universe = job.UniversePath.FileName() + ".xml";
            //universe = UniverseDirectory + universe;

            var Universe = um.LoadUniverse(job);
            if (Universe == null)
                return false;
            job = new Job(Universe, jobFile);

            try
            {
                job.DistributedPrint(jobFile, Ticker);
            }
            catch (Exception e)
            {
                LogError("Couldnt print " + jobFile + "!");
                LogError(e);
            }

            return !Ticker.ErrorOccured;
        }

        //public static bool CheckIfDirectoriesExist()
        //{
        //    if (!Directory.Exists(JobDirectory))
        //    {
        //        Console.WriteLine("Job-Ordner bei " + JobDirectory + " existiert nicht!");
        //        return false;
        //    }
        //    if (!Directory.Exists(UniverseDirectory))
        //    {
        //        Console.WriteLine("Universe-Ordner bei " + UniverseDirectory + " existiert nicht!");
        //        return false;
        //    }
        //    //if (!Directory.Exists(PDFDirectory))
        //    //    Directory.CreateDirectory(PDFDirectory);
        //    return true;
        //}
    }
}
