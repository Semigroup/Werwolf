using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Assistment.Extensions;
using Assistment.Xml;
using Assistment.Texts;

namespace Werwolf.Inhalt
{
    public class Aufgabe : IEnumerable<string>
    {
        private string[] roherText;

        public Aufgabe(string roherText)
        {
            this.roherText = roherText.Split(new string[] { @"\+" }, StringSplitOptions.RemoveEmptyEntries);
        }
        public Aufgabe(params string[] roherText)
        {
            this.roherText = roherText;
        }
        public Aufgabe(IEnumerable<string> roherText)
        {
            this.roherText = roherText.ToArray();
        }

        public int Anzahl()
        {
            return roherText.Length;
        }

        public IEnumerable<Text> GetTexts(xFont Font)
        {
            return roherText.Map(x => new Text(x, Font));
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
            return roherText.SumText("\r\n\\+\r\n");
        }

        public static Aufgabe operator +(Aufgabe Aufgabe1, Aufgabe Aufgabe2)
        {
            return new Aufgabe(Aufgabe1.roherText.Concat(Aufgabe2.roherText));
        }
    }
}
