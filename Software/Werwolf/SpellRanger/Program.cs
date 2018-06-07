using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.IO;

using SpellRanger.Structures;

namespace SpellRanger
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Spell> spells;

            using (StreamReader file = File.OpenText("./Spells/dndSpells.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                spells = (List<Spell>)serializer.Deserialize(file, typeof(List<Spell>));
            }
            List<string> castTimes = new List<string>();

            foreach (var item in spells)
            {
                if (!castTimes.Contains(item.casting_time))
                    castTimes.Add(item.casting_time);
            }
            foreach (var item in castTimes)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();

        }
    }
}
