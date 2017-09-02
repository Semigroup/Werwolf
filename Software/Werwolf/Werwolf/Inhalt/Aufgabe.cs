using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

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
            public bool Fehler;
            public string FehlerhafteName;

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
            public Fragment(ElementMenge<TextBild> Bilder, string name)
            {
                if (Bilder.ContainsKey(name))
                    this.Bild = Bilder[name];
                else
                {
                    Fehler = true;
                    FehlerhafteName = name;
                }
            }

            public void AddDrawBox(DrawContainer Container)
            {
                if (regex != null)
                    Container.addRegex(regex);
                else if (Bild != null)
                    Container.add(new WolfTextBild(Bild, Container.preferedFont));
                else if (Fehler)
                {
                    Image Image = Settings.NotFoundImage;
                    float height = Container.preferedFont.yMass('_');
                    Container.addImage(Image, Image.Size.Width * height / Image.Size.Height, height);
                }
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

        private void ConsumeLine(string line, ICollection<Fragment> fragments)
        {
            var separatedFragments = line.Split(new[] { "::" }, StringSplitOptions.None);
            for (var i = 0; i < separatedFragments.Length; i++)
            {
                //separatedFragments is an alternating collection of text and pictures
                var fragment = separatedFragments[i];
                fragments.Add(i % 2 == 1 ? new Fragment(Universe.TextBilder, fragment) : new Fragment(fragment));
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
                else if (item.Fehler)
                    sb.Append("::" + item.FehlerhafteName + "::");
                else
                    sb.Append(item.regex);
            return sb.ToString();
        }
        public static Aufgabe operator +(Aufgabe Aufgabe1, Aufgabe Aufgabe2)
        {
            return new Aufgabe(Aufgabe1, Aufgabe2);
        }

        public void Rescue()
        {
            foreach (var item in Fragments)
                if (item.Bild != null)
                    Universe.TextBilder.Rescue(item.Bild);
        }

        public Aufgabe Replace(string[] oldBilder, string[] newBilder)
        {
            Aufgabe a = new Aufgabe();
            a.Anzahl = Anzahl;
            a.Fragments = this.Fragments.Map(fragment =>
            {
                if (fragment.Bild == null)
                    return fragment;
                else
                {
                    int i = oldBilder.IndexOfTrue(x => x == fragment.Bild.Name);
                    if (i >= 0)
                        return new Fragment(fragment.Bild.Universe.TextBilder, newBilder[i]);
                    else
                        return fragment;
                }
            });
            return a;
        }

    }
}
