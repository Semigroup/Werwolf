using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Assistment.Extensions;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public abstract class Menge : XmlElement
    {
        public Menge(string XmlName)
            : base(XmlName)
        {

        }
        public override void AdaptToCard(Karte Karte)
        {
            throw new NotImplementedException();
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }

    public class ElementMenge<T> : Menge, IDictionary<string, T> where T : XmlElement, new()
    {
        private SortedDictionary<string, T> dictionary = new SortedDictionary<string, T>();

        public T Standard { get; private set; }

        public ElementMenge(string XmlName, Universe Universe)
            : base(XmlName)
        {
            this.Init(Universe);
        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Standard = new T();
            Standard.Init(Universe);
            Standard.Unzerstorbar = true;
            Add(Standard);
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            string standardName = Loader.XmlReader.getString("Standard");

            Clear();
            int depth = Loader.XmlReader.Depth;
            Loader.XmlReader.Next();
            while (Loader.XmlReader.Depth > depth)
            {
                T NeuesElement = new T();
                NeuesElement.Read(Loader);
                dictionary.Add(NeuesElement.Name, NeuesElement);
                Loader.XmlReader.Next();
            }

            if (ContainsKey(standardName))
                Standard = this[standardName];
            else if (ContainsKey(Standard.Name))
                Standard = this[Standard.Name];
            else
                this.Add(Standard);
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeAttribute("Standard", Standard.Name);

            foreach (var item in dictionary.Values)
                item.Write(XmlWriter);
        }

        public void Change(string oldElement, T NewValues)
        {
            T Element = this[oldElement];
            NewValues.Assimilate(Element);
            Remove(oldElement);
            AddPolymorph(Element);
        }
        public void Add(T value)
        {
            Add(value.Name, value);
        }
        public void AddPolymorph(T value)
        {
            string name = value.Name;
            int i = 2;
            while (ContainsKey(name))
            {
                name = value.Name + "_" + i;
                i++;
            }
            value.Name = name;
            Add(value);
        }
        public void Add(string key, T value)
        {
            dictionary.Add(key, value);
        }
        public bool ContainsKey(string key)
        {
            return dictionary.ContainsKey(key);
        }
        public ICollection<string> Keys
        {
            get { return dictionary.Keys; }
        }
        public bool Remove(string key)
        {
            return dictionary.Remove(key);
        }
        public bool TryGetValue(string key, out T value)
        {
            return dictionary.TryGetValue(key, out value);
        }
        public ICollection<T> Values
        {
            get { return dictionary.Values; }
        }
        public T this[string key]
        {
            get
            {
                T value;
                if (!dictionary.TryGetValue(key, out value))
                    value = Standard;
                return value;
            }
            set
            {
                dictionary[key] = value;
            }
        }
        public void Add(KeyValuePair<string, T> item)
        {
            dictionary.Add(item.Key, item.Value);
        }
        public void Clear()
        {
            dictionary.Clear();
        }
        public bool Contains(KeyValuePair<string, T> item)
        {
            return dictionary.Contains(item);
        }
        public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get { return dictionary.Count; }
        }
        public bool IsReadOnly
        {
            get { return false; }
        }
        public bool Remove(KeyValuePair<string, T> item)
        {
            return dictionary.Remove(item.Key);
        }
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
