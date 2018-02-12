using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Inhalt;
using Assistment.Drawing.Geometries;
using System.Drawing;

namespace Designer.RahmenCreator
{
    public class State
    {
        /// <summary>
        /// Pixel Size of Bitmap
        /// </summary>
        public Size Size;

        public float PPM;
        public string UniverseName;
        public string HintergrundBildName;
        public string HintergrundDarstellungName;
        public string Name;
        public float MarginLeft, MarginRight, MarginTop, MarginBottom;
        public float Radius;
        public float FragmentDicke;
        public int FragmentZahl;
        public Fragments.Style FragmentStyle;
        public int Samples;
        public bool InvertLeft;
        public bool InvertRight;

        public Color PenColor;
        public float PenWidth;
    }
}
