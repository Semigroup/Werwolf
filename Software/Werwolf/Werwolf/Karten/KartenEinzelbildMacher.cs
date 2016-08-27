using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class KartenEinzelbildMacher : IKartenmacher
    {
        public string GetName()
        {
            return "Jede Karte als eigene Bilddatei";
        }

        public void MakeKarten(Universe Universe, string Speicherort, float ppm)
        {
            string path = Path.GetDirectoryName(Speicherort);
            foreach (var item in Universe.Karten.Values)
            {
                StandardKarte sk = new StandardKarte(item, ppm);
                sk.createImage(path + "\\" + item.Name, ppm / WolfBox.Faktor, Color.White);
            }
        }
    }
}
