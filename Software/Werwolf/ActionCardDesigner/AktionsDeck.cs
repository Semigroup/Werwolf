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

using Werwolf.Inhalt;

namespace ActionCardDesigner
{
    public class AktionsDeck : XmlElement
    {
        public SortedDictionary<AktionsKarte, int> Karten { get; private set; }

        public new AktionsUniverse Universe { get { return base.Universe as AktionsUniverse; } }

        public AktionsDeck()
            : base("Deck")
        {
            Karten = new SortedDictionary<AktionsKarte, int>();
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            string s = Loader.XmlReader.ReadString();
            foreach (var item in s.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                int i = item.IndexOf(' ');
                int n = int.Parse(item.Substring(0, i));
                string name = item.Substring(i + 1, item.Length - i - 1);
                Karten.Add(Universe.AktionsKarten[name], n);
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

        public List<KeyValuePair<AktionsKarte, int>> GetKarten(int ab, int Anzahl)
        {
            List<KeyValuePair<AktionsKarte, int>> l = new List<KeyValuePair<AktionsKarte, int>>();
            int selected = 0;
            foreach (var item in Karten)
            {
                ab -= item.Value;
                int take = FastMath.Min(Anzahl - selected, -ab, item.Value);
                l.Add(new KeyValuePair<AktionsKarte, int>(item.Key, take));
                selected += take;
                if (selected >= Anzahl)
                    break;
            }
            return l;
        }
        public int TotalCount()
        {
            int n = 0;
            foreach (var item in Karten)
                n += item.Value;
            return n;
        }
        public int this[AktionsKarte Karte]
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
        public void SetKarte(AktionsKarte Karte, int Number)
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
            AktionsDeck d = Element as AktionsDeck;
            foreach (var item in Universe.AktionsKarten.Values)
                d[item] = this[item];
        }
        public override object Clone()
        {
            AktionsDeck d = new AktionsDeck();
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
            ElementMenge<AktionsKarte> Menge = Universe.AktionsKarten;
            foreach (var item in Karten.Keys)
                Menge.Rescue(item);
        }
    }
}
