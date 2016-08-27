using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Xml;

using Werwolf.Karten;

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
                image = null;
            }
        }
        public string Artist { get; private set; }
        public SizeF Size { get; set; }
        /// <summary>
        /// Relativ
        /// </summary>
        public PointF Zentrum { get; set; }

        /// <summary>
        /// Absolut in Millimetern
        /// </summary>
        public PointF ZentrumAbsolut
        {
            get { return Zentrum.mul(Size); }
            set { Zentrum = value.div(Size.ToPointF()); }
        }
        /// <summary>
        /// (-Zentrum, Size) * Faktor
        /// </summary>
        public RectangleF Rectangle
        {
            get { return new RectangleF(ZentrumAbsolut.mul(-WolfBox.Faktor), Size.mul(WolfBox.Faktor)); }
        }

        private Image image;
        public Image Image
        {
            get
            {
                if (image == null)
                {
                    if (filePath == null || filePath.Length == 0)
                        image = new Bitmap(1, 1);
                    else if (File.Exists(filePath))
                        image = Image.FromFile(filePath);
                    else
                        image = Image.FromFile(Path.Combine(Universe.RootBilder, filePath));  
                }
                return image;
            }
        }
        private string filePath;

        public Bild()
            : base("Bild", true)
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            FilePath = "";
            Artist = "";
            Size = Universe.HintergrundDarstellungen.Standard.Size;
            Zentrum = new PointF(0.5f, 0.5f);
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            this.FilePath = Loader.XmlReader.getString("FilePath");
            this.Artist = Loader.XmlReader.getString("Artist");
            this.Zentrum = Loader.XmlReader.getPointF("Zentrum");
            this.Size = Loader.XmlReader.getSizeF("Size");

            Loader.XmlReader.Next();
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
