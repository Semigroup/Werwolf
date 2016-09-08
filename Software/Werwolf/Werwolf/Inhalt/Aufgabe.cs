using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Assistment.Extensions;
using Assistment.Xml;
using Assistment.Texts;

using Werwolf.Karten;

namespace Werwolf.Inhalt
{
    public class Aufgabe
    {
        private class Fragment
        {
            public bool BlockBreak;
            public string regex;
            public TextBild Bild;

            public Fragment(bool BlockBreak)
            {
                this.BlockBreak = BlockBreak;
            }
            public Fragment(string regex)
            {
                this.regex = regex;
            }
            public Fragment(TextBild Bild)
            {
                this.Bild = Bild;
            }

            public void AddDrawBox(DrawContainer Container)
            {
                if (regex != null)
                    Container.addRegex(regex);
                else if (Bild != null)
                    Container.add(new WolfTextBild(Bild, Container.preferedFont));// Container.addWort(Bild.Name); 
                else
                    throw new NotImplementedException();
            }
        }

        public Universe Universe { get; private set; }

        private IEnumerable<Fragment> Fragments;
        public int Anzahl { get; private set; }

        public Aufgabe()
            : this("", null)
        {

        }
        public Aufgabe(string roherText, Universe Universe)
        {
            this.Universe = Universe;
            ConsumeString(roherText);
        }
        private Aufgabe(Aufgabe A1, Aufgabe A2)
        {
            if (A1.Anzahl == 0)
            {
                this.Fragments = A2.Fragments;
                this.Anzahl = A2.Anzahl;
            }
            else if (A2.Anzahl == 0)
            {
                this.Fragments = A1.Fragments;
                this.Anzahl = A1.Anzahl;
            }
            else
            {
                this.Fragments = A1.Fragments
              .Concat(new Fragment[] { new Fragment(true) })
              .Concat(A2.Fragments);
                this.Anzahl = A1.Anzahl + A2.Anzahl;
            }

            if (A1.Universe != null)
                this.Universe = A1.Universe;
            else
                this.Universe = A2.Universe;
        }
        public Aufgabe(TextBild Test)
        {
            List<Fragment> Fragments = new List<Fragment>();
            Fragments.Add(new Fragment(@"What's your \rfavourite "));
            Fragments.Add(new Fragment(Test));
            Fragments.Add(new Fragment(" ide\ba?"));

            Fragments.Add(new Fragment(true));
            Fragments.Add(new Fragment(@"Mine is being \i\oc\br\ge\ya\rt\vi\lv\oe "));
            Fragments.Add(new Fragment(Test));
            Fragments.Add(new Fragment(@" \d!"));

            Fragments.Add(new Fragment(true));
            Fragments.Add(new Fragment(@"\xHow do you "));
            Fragments.Add(new Fragment(Test));
            Fragments.Add(new Fragment(@" \xget the idea?"));

            Fragments.Add(new Fragment(true));
            Fragments.Add(new Fragment(Test));
            Fragments.Add(new Fragment(@"\dI just try to think \rc\br\ge\oa\vt\li\ev\ye\rl\by!"));

            this.Fragments = Fragments;
            this.Anzahl = 4;
        }

        private void ConsumeString(string roherText)
        {
            List<Fragment> Fragments = new List<Fragment>();
            string[] blocks = roherText.Split(new string[] { @"\+" }, StringSplitOptions.RemoveEmptyEntries);
            this.Anzahl = blocks.Length;

            if (Anzahl > 0)
                ConsumeLine(blocks[0], Fragments);
            for (int i = 1; i < Anzahl; i++)
            {
                Fragments.Add(new Fragment(true));
                ConsumeLine(blocks[i], Fragments);
            }
            this.Fragments = Fragments;
        }
        private void ConsumeLine(string Line, List<Fragment> Fragments)
        {
            string[] fragments = Line.Split(new string[] { "::" }, StringSplitOptions.None);
            bool bild = false;
            foreach (var item in fragments)
            {
                if (bild)
                    Fragments.Add(new Fragment(Universe.TextBilder[item]));
                else
                    Fragments.Add(new Fragment(item));
                bild = !bild;
            }
        }

        public Text[] ProduceTexts(xFont Font)
        {
            Text[] t = new Text[Anzahl];
            t.SelfMap(x => new Text("", Font));
            int i = 0;
            foreach (var item in Fragments)
                if (item.BlockBreak)
                    i++;
                else
                    item.AddDrawBox(t[i]);
            return t;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder(Anzahl * 10);
            foreach (var item in Fragments)
                if (item.BlockBreak)
                    sb.Append("\\+");
                else if (item.Bild != null)
                    sb.Append("::" + item.Bild.Name + "::");
                else
                    sb.Append(item.regex);
            return sb.ToString();
        }
        public static Aufgabe operator +(Aufgabe Aufgabe1, Aufgabe Aufgabe2)
        {
            return new Aufgabe(Aufgabe1, Aufgabe2);
        }
    }
}
