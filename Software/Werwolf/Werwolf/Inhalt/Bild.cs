using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Werwolf.Inhalt
{
    public class Bild
    {
        public static Bild Leer { get; private set; }

        static Bild()
        {
            Leer = new Bild(new Bitmap(0, 0), "", new SizeF());
        }

        public Image Image { get; private set; }
        public string Artist { get; private set; }
        /// <summary>
        /// Größe in Millimeter
        /// </summary>
        public SizeF Size { get; private set; }

        public Bild(Image Image, string Artist, SizeF Size)
        {
            this.Image = Image;
            this.Artist = Artist;
            this.Size = Size;
        }
    }
}
