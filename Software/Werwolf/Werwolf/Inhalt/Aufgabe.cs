using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Assistment.Extensions;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Aufgabe : IEnumerable<string>
    {
        private string[] roherText;

        public Aufgabe(string[] roherText)
        {
            this.roherText = roherText;
        }

        public int Anzahl()
        {
            return roherText.Length;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)roherText).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
