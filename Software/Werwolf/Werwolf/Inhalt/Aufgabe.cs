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

        public Aufgabe(string roherText)
        {
            this.roherText = roherText.Split(new string[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
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

        public override string ToString()
        {
            return roherText.SumText("\r\n\r\n");
        }
    }
}
