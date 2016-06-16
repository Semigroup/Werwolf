using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Assistment.Texts;

namespace Werwolf.Karten
{
    public class Pipeline
    {
        private DrawBox[] Layers = new DrawBox[7];

        public DrawBox Hintergrund
        {
            get { return Layers[0]; }
            set { Layers[0] = value; }
        }
        public DrawBox Bild
        {
            get { return Layers[1]; }
            set { Layers[1] = value; }
        }
        public DrawBox Text
        {
            get { return Layers[2]; }
            set { Layers[2] = value; }
        }
        public DrawBox Titel
        {
            get { return Layers[3]; }
            set { Layers[3] = value; }
        }
        public DrawBox Gesinnung
        {
            get { return Layers[4]; }
            set { Layers[4] = value; }
        }
        public DrawBox Artist
        {
            get { return Layers[5]; }
            set { Layers[5] = value; }
        }
        public DrawBox Rand
        {
            get { return Layers[6]; }
            set { Layers[6] = value; }
        }
    }
}
