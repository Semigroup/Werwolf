using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts.Paper;
using Assistment.Texts;
using System.Drawing;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class KartenPDFMacher : IKartenmacher
    {
        public string GetName()
        {
            return "Alle Karten in eine DinA4 PDF";
        }

        public void MakeKarten(Universe Universe, string Speicherort, float ppm)
        {
            MakeAt(0, Universe, Speicherort, ppm);
            //GC.Collect();
            //MakeAt(1, Universe, Speicherort, ppm);
            //GC.Collect();
            //MakeAt(2, Universe, Speicherort, ppm);
            //GC.Collect();
            //MakeAt(3, Universe, Speicherort, ppm);
            //GC.Collect();
        }
        public void MakeAt(int i, Universe Universe, string Speicherort, float ppm)
        {
            CardSheet cs = new CardSheet(3, 3, Universe.HintergrundDarstellungen.Standard.Size.mul(WolfBox.Faktor));
            foreach (var item in Universe.Karten.Values.Skip(9 * i).Take(9))
            {
                StandardKarte sk = new StandardKarte(item, ppm);
                cs.add(sk);
            }
            //cs.createImage(Path.Combine(Speicherort, "Karten" + i));
            cs.createPDF(Path.Combine(Speicherort, "Karten" + i));
        }
    }
}
