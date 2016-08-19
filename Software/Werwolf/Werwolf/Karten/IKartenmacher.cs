using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public interface IKartenmacher
    {
        string GetName();
        void MakeKarten(Universe Universe, string Speicherort, float ppm);
    }
}
