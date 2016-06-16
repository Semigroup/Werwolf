using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public abstract class Karte
    {
        /// <summary>
        /// Größe in Millimeter
        /// </summary>
        public SizeF Size { get; private set; }

        public Rolle Rolle { get; private set; }
    }
}
