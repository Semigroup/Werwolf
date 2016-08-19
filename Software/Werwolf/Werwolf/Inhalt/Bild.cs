using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;

using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Bild : XmlElement
    {
        public string FilePath
        {
            get { return filePath; }
            set
            {
                filePath = value;
                if (value == null || value.Length == 0)
                    Image = new Bitmap(1, 1);
                else if (File.Exists(filePath))
                    Image = Image.FromFile(value);
                else
                    Image = Image.FromFile(Path.Combine(Universe.rootBilder, value));
            }
        }
        public string Artist { get; private set; }
        public SizeF Size { get; set; }
        public PointF Zentrum { get; set; }

        public Image Image { get; private set; }
        private string filePath;

        public Bild()
            : base("Bild", true)
        {

        }
        public Bild(string Name, string FilePath, string Artist, Universe Universe)
            : base("Bild", true)
        {
            this.Universe = Universe;
            this.Name = Name;
            this.FilePath = FilePath;
            this.Artist = Artist;
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            this.FilePath = Loader.XmlReader.getString("FilePath");
            this.Artist = Loader.XmlReader.getString("Artist");
            this.Zentrum = Loader.XmlReader.getPointF("Zentrum");
            this.Size = Loader.XmlReader.getSizeF("Size");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.writeAttribute("FilePath", FilePath);
            XmlWriter.writeAttribute("Artist", Artist);
            XmlWriter.writeSize("Size", Size);
            XmlWriter.writePoint("Zentrum", Zentrum);
        }
    }
}
