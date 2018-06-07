using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellRanger.Structures
{
    public class Components
    {
        public bool material { get; set; }
        public List<string> materials_needed { get; set; }
        public string raw { get; set; }
        public bool somatic { get; set; }
        public bool verbal { get; set; }
    }
}
