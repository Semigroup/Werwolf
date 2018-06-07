using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellRanger.Structures
{
    public class Spell
    {
        public string casting_time { get; set; }
        public List<string> classes { get; set; }
        public Components components { get; set; }
        public string description { get; set; }
        public string duration { get; set; }
        public string level { get; set; }
        public string name { get; set; }
        public string range { get; set; }
        public bool ritual { get; set; }
        public string school { get; set; }
        public List<string> tags { get; set; }
        public string type { get; set; }
    }
}
