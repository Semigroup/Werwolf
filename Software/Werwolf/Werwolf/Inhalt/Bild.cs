using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Windows.Media.Imaging;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Xml;

using Werwolf.Karten;

namespace Werwolf.Inhalt
{
    public abstract class Bild : XmlElement
    {
        public string FilePath { get; set; }
        public string TotalFilePath
        {
            get
            {
                if (File.Exists(FilePath))
                    return FilePath;
                else if (FilePath != null && Universe.RootBilder != null)
                    return Path.Combine(Universe.RootBilder, FilePath);
                else
                    return "";
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

        public Image Image
        {
            get
            {
                string tot = TotalFilePath;
                if (File.Exists(tot))
                    return Image.FromFile(tot);
                else
                    return new Bitmap(1, 1);
            }
        }

        public Bild(string XmlName)
            : base(XmlName)
        {
        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            FilePath = "";
            Artist = "Artist";
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
        }
        public SizeF StandardSize(Image image)
        {
            float w = Universe.HintergrundDarstellungen.Standard.Size.Width;
            if (image != null)
                return new SizeF(w, w / ((SizeF)image.Size).ratio());
            else
                return new SizeF(1, 1);
        }
        public Size GetImageSize()
        {
            string tot = TotalFilePath;
            if (File.Exists(tot))
                using (var imageStream = File.OpenRead(TotalFilePath))
                {
                    BitmapDecoder decoder = BitmapDecoder.Create(imageStream,
                        BitmapCreateOptions.IgnoreColorProfile,
                        BitmapCacheOption.None);
                    return new Size(decoder.Frames[0].PixelWidth, decoder.Frames[0].PixelHeight);
                }
            else
                return new Size(0, 1);
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
            Size = Universe.HintergrundDarstellungen.Standard.Size;
            Zentrum = new PointF(0.5f, 0.5f);
        }

        public override void AdaptToCard(Karte Karte)
        {
        }
        public override object Clone()
        {
            TextBild b = new TextBild();
            Assimilate(b);
            return b;
        }
    }
}
