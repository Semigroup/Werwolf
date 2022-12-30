using Assistment.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Inhalt;
using Werwolf.Printing;

namespace Skinner
{
    /// <summary>
    /// A UniverseManager collects universes (specified by a root).
    /// 
    /// It stores in a dictionary where each universe is keyed by its name.
    /// 
    /// Jobs can specify the universe they need by the name of the universe
    /// instead of its filepath (which is not device independent).
    /// </summary>
    public class UniverseManager
    {
        public IDictionary<string, string> Universes = new SortedDictionary<string, string>();

        public void CollectUniverses(string root)
        {
            foreach (var xmlFile in Directory.EnumerateFiles(root, "*.xml", SearchOption.AllDirectories))
            {
                try
                {
                    using (Loader loader = new DummyLoader(xmlFile))
                    {
                        loader.XmlReader.Next();
                        if (loader.XmlReader.Name.Equals("Universe"))
                        {
                            var name = loader.XmlReader.GetAttribute("Name");
                            Console.Write("Trying to add " + name + ":" + xmlFile + "...");
                            if (!Universes.ContainsKey(name))
                                Universes.Add(name, xmlFile);
                            Console.WriteLine(" Success!");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(" Fail!");
                    Console.WriteLine(e.ToString());
                    Console.ForegroundColor = ConsoleColor.Gray;
                }
            }
        }

        public Universe LoadUniverse(Job job)
        {
            string universePath;
            if (job.UniverseName == null || job.UniverseName.Length == 0)
                universePath = job.UniversePath;
            else
                universePath = Universes[job.UniverseName];

            try
            {
                return new Universe(universePath);
            }
            catch (Exception e)
            {
                Program.LogError("Couldnt load " + universePath + "!");
                Program.LogError(e);
                return null;
            }
        }
    }
}
