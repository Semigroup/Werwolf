﻿using System;
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
        public string Pfad { get; set; }

        public Menge(string XmlName)
            : base(XmlName, false)
        {

        }
        public void Open(string Pfad)
        {
            this.Pfad = Pfad;
            Loader l = Universe.CreateLoader(Pfad);
            this.Read(l);
            l.XmlReader.Close();
        }
        public void Save()
        {
            XmlWriterSettings s = new XmlWriterSettings();
            s.NewLineOnAttributes = true;
            s.Indent = true;
            s.IndentChars = new string(' ', 4);
            XmlWriter writer = XmlWriter.Create(Pfad, s);
            writer.WriteStartDocument();
            Write(writer);
            writer.WriteEndDocument();
            writer.Close();
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
            Add(Standard);
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            string standardName = Loader.XmlReader.getString("Standard");

            Loader.XmlReader.Next();
            Clear();
            while (!Loader.XmlReader.EOF)
            {
                T NeuesElement = new T();
                NeuesElement.Read(Loader);
                dictionary.Add(NeuesElement.Name, NeuesElement);
            }

            T standard = new T();
            if (!TryGetValue(standardName, out standard))
                this.Add(Standard);
            else
                Standard = standard;
        }
        protected override void WriteIntern(System.Xml.XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeAttribute("Standard", Standard.Name);

            foreach (var item in dictionary.Values)
                item.Write(XmlWriter);
        }

        public void Add(T value)
        {
            Add(value.Name, value);
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
