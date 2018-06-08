using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellRanger.Structures
{
    public class Spell : IComparable<Spell>
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
        public string higher_levels { get; set; }
        public string school { get; set; }
        public List<string> tags { get; set; }
        public string type { get; set; }

        public Time Time { get; set; }
        public SpellType SpellType { get; set; }
        public Range Range { get; set; }
        public Time Duration { get; set; }

        public int CompareTo(Spell other)
        {
            if (this.Time.CompareTo(other.Time) > 0)
                return 1;
            else if (this.Time.CompareTo(other.Time) < 0)
                return -1;
            else if (this.SpellType.Level > other.SpellType.Level)
                return 1;
            else if (this.SpellType.Level < other.SpellType.Level)
                return -1;
            else
                return this.name.CompareTo(other.name);
        }

        public void Setup()
        {
            this.Time = new Time(casting_time);
            this.SpellType = new SpellType(this);
            this.Range = new Range(range);
            this.Duration = new Time(duration);
        }


    }
}
