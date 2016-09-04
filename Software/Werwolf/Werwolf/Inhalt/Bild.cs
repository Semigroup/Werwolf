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
    public abstract class Bild : XmlElement
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
        public string Artist { get; set; }
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
        public string TotalFilePath
        {
            get
            {
                if (File.Exists(filePath))
                    return filePath;
                else if (filePath != null)
                    return Path.Combine(Universe.RootBilder, filePath);
                else
                    return "";
            }
        }

        public Bild(string XmlName)
            : base(XmlName)
        {
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

       
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            Bild b = Element as Bild;
            b.FilePath = FilePath;
            b.Artist = Artist;
            b.Size = Size;
            b.Zentrum = Zentrum;
            b.image = image;
        }
        public SizeF StandardSize(Image image)
        {
            float w = Universe.HintergrundDarstellungen.Standard.Size.Width;
            if (image != null)
                return new SizeF(w, w / ((SizeF)image.Size).ratio());
            else
                return new SizeF(1, 1);
        }
    }

    public class HauptBild : Bild
    {
        public HauptBild()
            : base("HauptBild")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            FilePath = "";
            Artist = "";
            Size = Universe.HintergrundDarstellungen.Standard.Size;
            Zentrum = new PointF(0.5f, 0.25f);
        }

        public override void AdaptToCard(Karte Karte)
        {
            Karte.HauptBild = this;
        }
        public override object Clone()
        {
            HauptBild b = new HauptBild();
            Assimilate(b);
            return b;
        }
    }
    public class HintergrundBild : Bild
    {
        public HintergrundBild()
            : base("HintergrundBild")
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

        public override void AdaptToCard(Karte Karte)
        {
            Karte.Fraktion.HintergrundBild = this;
        }
        public override object Clone()
        {
            HintergrundBild b = new HintergrundBild();
            Assimilate(b);
            return b;
        }
    }
    public class RuckseitenBild : Bild
    {
        public RuckseitenBild()
            : base("RuckseitenBild")
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
 
        public override void AdaptToCard(Karte Karte)
        {
            Karte.Fraktion.RuckseitenBild = this;
        }
        public override object Clone()
        {
            RuckseitenBild b = new RuckseitenBild();
            Assimilate(b);
            return b;
        }
    }
    public class TextBild : Bild
    {
        public TextBild()
            : base("TextBild")
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

        public override void AdaptToCard(Karte Karte)
        {
            throw new NotImplementedException();
        }
        public override object Clone()
        {
            TextBild b = new TextBild();
            Assimilate(b);
            return b;
        }
    }
}
