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
        public Manifest Manifest { get; private set; }
    }
}
