using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing;

using Assistment.Xml;
using Assistment.Extensions;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Mathematik;

namespace Werwolf.Inhalt
{
    public class Deck : XmlElement
    {
        public SortedDictionary<Karte, int> Karten { get; private set; }

        public Deck()
            : base("Deck", true)
        {
            Karten = new SortedDictionary<Karte, int>();
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            //string dump = Loader.XmlReader.DumpInfo();
            string s = Loader.XmlReader.ReadString();
            foreach (var item in s.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                int i = item.IndexOf(' ');
                int n = int.Parse(item.Substring(0, i));
                string name = item.Substring(i + 1, item.Length - i - 1);
                Karten.Add(Universe.Karten[name], n);
            }
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            StringBuilder sb = new StringBuilder();
            foreach (var item in Karten)
                sb.AppendLine(item.Value + " " + item.Key.Name);
            XmlWriter.WriteRaw(sb.ToString());
        }

        public List<KeyValuePair<Karte, int>> GetKarten(List<KeyValuePair<Karte, int>> fullSortedList, int ab, int Anzahl)
        {
            List<KeyValuePair<Karte, int>> l = new List<KeyValuePair<Karte, int>>();
            int selected = 0;
            foreach (var item in fullSortedList)
            {
                ab -= item.Value;
                int take = FastMath.Min(Anzahl - selected, -ab, item.Value);
                l.Add(new KeyValuePair<Karte, int>(item.Key, take));
                selected += take;
                if (selected >= Anzahl)
                    break;
            }
            return l;
        }
        public List<KeyValuePair<Karte, int>> GetSortedList()
        {
            List<KeyValuePair<Karte, int>> l = new List<KeyValuePair<Karte, int>>();
            foreach (var item in Karten)
                l.Add(item);
            l.Sort((x, y) => x.Key.OldCompareTo(y.Key));
            return l;
        }
        public int UniqueCount()
        {
            int n = 0;
            foreach (var item in Karten)
                if (item.Value > 0)
                    n++;
            return n;
        }
        public int TotalCount()
        {
            int n = 0;
            foreach (var item in Karten)
                n += item.Value;
            return n;
        }
        public int this[Karte Karte]
        {
            get
            {
                if (Karten.ContainsKey(Karte))
                    return Karten[Karte];
                else
                    return 0;
            }
            set { SetKarte(Karte, value); }
        }
        public void SetKarte(Karte Karte, int Number)
        {
            if (Number > 0)
            {
                if (Karten.ContainsKey(Karte))
                    Karten[Karte] = Number;
                else
                    Karten.Add(Karte, Number);
            }
            else
                if (Karten.ContainsKey(Karte))
                    Karten.Remove(Karte);
        }

        public override void AdaptToCard(Karte Karte)
        {
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            Deck d = Element as Deck;
            foreach (var item in Universe.Karten.Values)
                d[item] = this[item];
        }
        public override object Clone()
        {
            Deck d = new Deck();
            Assimilate(d);
            return d;
        }
        public override string ToString()
        {
            SortedDictionary<Fraktion, int> dic = Karten.SumLeft(x => x.Fraktion);
            StringBuilder sb = new StringBuilder();
            int tot = 0;
            foreach (var item in dic)
            {
                sb.AppendLine(item.Value + "x " + item.Key.Schreibname);
                tot += item.Value;
            }
            sb.Append(tot + "x Karten");
            return sb.ToString();
        }
        public override void Rescue()
        {
            ElementMenge<Karte> Menge = Universe.Karten;
            foreach (var item in Karten.Keys)
                Menge.Rescue(item);
        }

        /// <summary>
        /// returns the size of the first card of the deck in millimeters
        /// <para>if this deck is empty, (0,0) will be returned</para>
        /// </summary>
        /// <returns></returns>
        public SizeF GetKartenSize()
        {
            foreach (var item in Karten)
                if (item.Value > 0)
                    return item.Key.HintergrundDarstellung.Size;
            return new SizeF();
        }
    }
}
