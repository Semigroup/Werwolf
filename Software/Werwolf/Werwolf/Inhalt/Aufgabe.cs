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
using Assistment.Texts.Fonts;

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
                    Container.AddRegex(regex);
                else if (Bild != null)
                    Container.Add(new WolfTextBild(Bild, Container.PreferedFont));
                else if (Fehler)
                {
                    Image Image = Settings.NotFoundImage;
                    float height = Container.PreferedFont.YMass('_');
                    Container.AddImage(Image, Image.Size.Width * height / Image.Size.Height, height);
                }
                else
                    throw new NotImplementedException();
            }
        }

        public Universe Universe { get; private set; }

        private IEnumerable<Fragment> Fragments;
        public int Anzahl { get; private set; }
        public bool IsEmpty => Anzahl == 0;

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
            List<Fragment> Fragments = new List<Fragment>
            {
                new Fragment(@"What's your \rfavourite "),
                new Fragment(Test),
                new Fragment(" ide\ba?"),

                new Fragment(true),
                new Fragment(@"Mine is being \i\oc\br\ge\ya\rt\vi\lv\oe "),
                new Fragment(Test),
                new Fragment(@" \d!"),

                new Fragment(true),
                new Fragment(@"\xHow do you "),
                new Fragment(Test),
                new Fragment(@" \xget the idea?"),

                new Fragment(true),
                new Fragment(Test),
                new Fragment(@"\dI just try to think \rc\br\ge\oa\vt\li\ev\ye\rl\by!")
            };

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

        public DrawContainer[] ProduceDrawContainers(xFont font, bool istKomplex)
        {
            if (istKomplex)
                return ProduceCStrings(font);
            else
                return ProduceTexts(font);
        }
        private CString[] ProduceCStrings(xFont Font)
        {
            CString[] t = new CString[Anzahl];
            t.SelfMap(x => new CString(Font));
            AddToTexts(t);
            return t;
        }
        public Text[] ProduceTexts(xFont Font)
        {
            Text[] t = new Text[Anzahl];
            t.SelfMap(x => new Text("", Font));
            AddToTexts(t);
            return t;
        }
        private void AddToTexts(DrawContainer[] t)
        {
            int i = 0;
            foreach (var item in Fragments)
                if (item.BlockBreak)
                    i++;
                else
                    item.AddDrawBox(t[i]);
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
        public string GetFlatString()
        {
            var font = new MonoFont(1, 1);
            var texts = ProduceTexts(font);
            StringBuilder sb = new StringBuilder(texts.Length);
            foreach (var text in texts)
            {
                sb.AppendLine(text.ToString());
                //Console.WriteLine(text.Explain());
                //Console.WriteLine();
            }
            //Console.ReadKey();
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
            Aufgabe a = new Aufgabe
            {
                Anzahl = Anzahl,
                Fragments = this.Fragments.Map(fragment =>
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
                })
            };
            return a;
        }
        public List<string> GetLines()
        {
            List<string> result = new List<string>();
            StringBuilder sb = new StringBuilder(Anzahl);
            foreach (var item in Fragments)
                if (item.BlockBreak)
                {
                    result.Add(sb.ToString());
                    sb = new StringBuilder(Anzahl);
                }
                else if (item.Bild != null)
                    sb.Append("::" + item.Bild.Name + "::");
                else if (item.Fehler)
                    sb.Append("::" + item.FehlerhafteName + "::");
                else
                    sb.Append(item.regex);
            result.Add(sb.ToString());
            return result;
        }
        public List<TextBild> GetTextBilder()
        {
            List<TextBild> result = new List<TextBild>();
            foreach (var item in Fragments)
                if (item.Bild != null)
                    result.Add(item.Bild);
            return result;
        }
    }
}
