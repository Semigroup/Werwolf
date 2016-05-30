using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Assistment.Extensions;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Aufgabe
    {
        private string[] roherText;

        public Aufgabe(string[] roherText)
        {
            this.roherText = roherText;
        }
    }
}
