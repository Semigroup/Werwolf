using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assistment.Xml;
using Assistment.Extensions;

namespace Werwolf.Inhalt
{
    public class Deck : XmlElement, ICollection<Karte>
    {
        private List<Karte> Karten = new List<Karte>();

        public Deck()
            : base("Deck")
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Loader.XmlReader.Next();
            string s = Loader.XmlReader.ReadString();
            foreach (var item in s.Split("\r\n".ToArray(), StringSplitOptions.RemoveEmptyEntries))
                Karten.Add(Universe.Karten[item]);
            Loader.XmlReader.Next();
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.WriteString(Karten.SumText("\r\n"));
        }

        public override void AdaptToCard(Karte Karte)
        {
            throw new NotImplementedException();
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            Deck d = Element as Deck;
            d.Karten.AddRange(Karten);
        }
        public override object Clone()
        {
            Deck d = new Deck();
            Assimilate(d);
            return d;
        }

        public void Add(Karte item)
        {
            Karten.Add(item);
        }
        public void Clear()
        {
            Karten.Clear();
        }
        public bool Contains(Karte item)
        {
            return Karten.Contains(item);
        }
        public void CopyTo(Karte[] array, int arrayIndex)
        {
            Karten.CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get { return Karten.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public bool Remove(Karte item)
        {
            return Karten.Remove(item);
        }
        public IEnumerator<Karte> GetEnumerator()
        {
            return Karten.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
