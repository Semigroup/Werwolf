using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;

using SpellRanger.Structures;
using SpellRanger.Latex;

namespace SpellRanger
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<Spell> spells;

            using (StreamReader file = File.OpenText("./Spells/dndSpells.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                spells = (List<Spell>)serializer.Deserialize(file, typeof(List<Spell>));
            }
            spells = spells.Where(x => x.tags.Contains("cleric")|| x.tags.Contains("cleric (trickery)"));


            LatexWriter lw = new LatexWriter("./spells.txt");


            lw.WriteLine("\\section{Zaubetricks}");
            IEnumerable<Spell> cantrips = spells.Where(x => x.level.ToLower().Contains("cantrip"));
            foreach (var item in cantrips)
            {
                item.Setup();
                lw.WriteSpell(item);
            lw.WriteLine();
            }
            lw.WriteLine();
            lw.WriteLine();

            string[] levels = {"1", "2", "3" };

            List<Spell> list = spells.Where(x => levels.Contains(x.level)).ToList();
            foreach (var item in list)
                item.Setup();

            list.Sort();

            IEnumerable<Spell> noRitual = list.Where(x => !x.ritual);

            lw.WriteLine("\\section{Kampfzauber}");
            foreach (var item in noRitual.Where(x =>x.Time.Seconds <= 6))
            {
                lw.WriteSpell(item);
                lw.WriteLine();
            }
            lw.WriteLine("\\section{Zauber}");
            foreach (var item in noRitual.Where(x => x.Time.Seconds > 6))
            {
                lw.WriteSpell(item);
                lw.WriteLine();
            }

            IEnumerable<Spell> ritual = list.Where(x => x.ritual);
            lw.WriteLine("\\section{Rituale}");
            foreach (var item in ritual)
            {
                lw.WriteSpell(item);
                lw.WriteLine();
            }

            lw.Flush();
            lw.Close();

            List<string> castTimes = new List<string>();

            foreach (var item in spells)
            {
                foreach (var tag in item.tags)
                if (!castTimes.Contains(tag))
                    castTimes.Add(tag);
            }
            foreach (var item in castTimes)
            {
                Console.Write("*");
                Console.Write(item);
                Console.Write("*");
                Console.WriteLine();
            }
            Console.ReadKey();

        }
    }
}
