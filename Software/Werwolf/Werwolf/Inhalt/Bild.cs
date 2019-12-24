using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.IO;
using System.Windows.Media.Imaging;
using System.Drawing.Imaging;

using Assistment.Drawing.LinearAlgebra;
using Assistment.Xml;
using Assistment.Extensions;

using Werwolf.Karten;
using Werwolf.Inhalt.Data;

namespace Werwolf.Inhalt
{
    public abstract class Bild : XmlElement
    {
        private static Image LeerBild = new Bitmap(1, 1);
        private static Image FehlerBild = Settings.ErrorImage;

        /// <summary>
        /// name.png
        /// </summary>
        public string Identifier { get; private set; }
        /// <summary>
        /// C:\asdad\asdasda.png
        /// oder relativ
        /// </summary>
        public string FilePath { get; private set; }

        public string TotalFilePath
        {
            get
            {
                if (HasIdentifier())
                {
                    string result = Path.Combine(Universe.DirectoryName, GetSubfolder(), Identifier);
                    if (File.Exists(result))
                        return result;
                }
                if (HasFilePath())
                {
                    if (File.Exists(FilePath))
                        return Path.GetFullPath(FilePath);
                    else
                    {
                        string result = Path.Combine(Universe.DirectoryName, FilePath);
                        if (File.Exists(result))
                            return result;
                    }
                }
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

        /// <summary>
        /// gibt an, ob TotalFilePath Länge gleich Null hat
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            return TotalFilePath.Length == 0;
        }
        public bool HasIdentifier()
        {
            return Identifier != null && Identifier.Length > 0;
        }
        public bool HasFilePath()
        {
            return FilePath != null && FilePath.Length > 0;
        }
        public Konflikt Konflikt { get; private set; }
        public bool TryNewIdentifier { get; set; }

        public virtual Image Image
        {
            get
            {
                string tfp = TotalFilePath;
                if (TotalFilePath.Length == 0)
                    return LeerBild.Clone() as Image;
                if (File.Exists(tfp))
                    try
                    {
                        return Image.FromFile(tfp);
                    }
                    catch (Exception e)
                    {
                        System.Windows.Forms.MessageBox.Show(
                            "Es gab einen Fehler beim Laden des Bildes:\r\n"
                            + tfp + "\r\n\r\n"
                            + "Die Bilddatei ist vielleicht zu groß oder kaputt.\r\n"
                            +"(Manchmal werden Bilder unter einem falschen Dateitypen abgespeichert (.jpg statt .heic z.Bsp.)," +
                            "was sie korrumpiert.\r\n" +
                            "Dieses Problem kann man lösen, indem man die betroffenen Bilddateien mit Paint öffnet und unter einem anderen Dateityp abspeichert.)"
                            );
                        return FehlerBild;
                    }
                else
                    return FehlerBild;
            }
        }

        public Bild(string XmlName)
            : base(XmlName, false)
        {
        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Artist = "";
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            this.FilePath = Loader.XmlReader.GetString("FilePath");
            this.Identifier = Loader.XmlReader.GetString("Identifier");
            this.Artist = Loader.XmlReader.GetString("Artist");
            this.Zentrum = Loader.XmlReader.GetPointF("Zentrum");
            this.Size = Loader.XmlReader.GetSizeF("Size");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.WriteAttribute("Identifier", Identifier);
            XmlWriter.WriteAttribute("FilePath", FilePath);
            XmlWriter.WriteAttribute("Artist", Artist);
            XmlWriter.WriteSize("Size", Size);
            XmlWriter.WritePoint("Zentrum", Zentrum);
        }

        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            Bild b = Element as Bild;
            b.FilePath = FilePath;
            b.Identifier = Identifier;
            b.Artist = Artist;
            b.Size = Size;
            b.Zentrum = Zentrum;
            b.TryNewIdentifier = TryNewIdentifier;
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
        public void SetAutoSize()
        {
            SizeF KartenSize = Universe.HintergrundDarstellungen.Standard.Size;
            this.Size = new SizeF(KartenSize.Width, KartenSize.Width / Size.ratio());
        }
        public Image GetImageByHeight(float Height)
        {
            Image img;
            using (Image Img = this.Image)
            {
                RectangleF r = new RectangleF();
                r.Size = new SizeF(Img.Size.Width * Height / Img.Size.Height, Height).Max(1, 1).ToSize();
                img = new Bitmap((int)r.Width, (int)r.Height);
                using (Graphics g = img.GetHighGraphics())
                    g.DrawImage(Img, r);
            }
            return img;
        }
        /// <summary>
        /// "Bilder/" + XmlName + "er/"
        /// </summary>
        /// <returns></returns>
        public string GetSubfolder()
        {
            return "Bilder\\" + XmlName + "er\\";
        }
        ///// <summary>
        ///// Hier werden Bilder abgespeichert
        ///// <para>gibt Null zurück, wenn Universe nicht gespeichert ist</para>
        ///// </summary>
        ///// <returns></returns>
        //public string GetInternetDirectory()
        //{
        //    if (Universe.Pfad == null) return null;
        //    return Path.Combine(Universe.DirectoryName, GetSubfolder());
        //}

        public void Lokalisieren(string destinyDirectory)
        {
            string NewFilePath;
            switch (Konflikt.LosungArt)
            {
                case Konflikt.Losung.NichtErsetzen:
                    this.FilePath = Konflikt.DestinyFile;
                    this.Identifier = Path.GetFileName(this.FilePath);
                    this.TryNewIdentifier = false;
                    return;
                case Konflikt.Losung.Ersetzen:
                    NewFilePath = Konflikt.DestinyFile;
                    break;
                case Konflikt.Losung.Umbennen:
                    NewFilePath = Konflikt.DestinyFile.DecollideFilename();
                    break;
                case Konflikt.Losung.Keine:
                default:
                    throw new NotImplementedException();
            }
            File.Copy(TotalFilePath, NewFilePath, true);
            this.FilePath = NewFilePath;
            this.Identifier =  Path.GetFileName(this.FilePath);
            this.TryNewIdentifier = false;
        }
        public void SetKonflikt(string destinyDirectory)
        {
            string source = TotalFilePath;

            string destiny = Path.Combine(
                destinyDirectory, 
                GetSubfolder(),
                (TryNewIdentifier || !HasIdentifier()
                ? Name.ToFileName() + Path.GetExtension(FilePath)
                : Identifier));
            this.Konflikt = new Konflikt(this, source, destiny);
        }

        public void SetFilePath(string FilePath)
        {
            this.FilePath = FilePath;
            this.Identifier = null;
            this.TryNewIdentifier = true;
        }
        public override void AdaptToCard(Karte Karte)
        {
            throw new NotImplementedException();
        }
        public override void Rescue()
        {
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
            Karte.Aufgaben = new Aufgabe(this);
        }
        public override object Clone()
        {
            TextBild b = new TextBild();
            Assimilate(b);
            return b;
        }
    }
    public class TrafoTextBild : TextBild
    {
        public override System.Drawing.Image Image
        {
            get
            {
                Image img = Original.Image;
                img.RotateFlip(Transformation);
                return img;
            }
        }

        public RotateFlipType Transformation { get; set; }
        public TextBild Original { get; set; }

        public TrafoTextBild(RotateFlipType Transformation, TextBild Original)
        {
            this.Transformation = Transformation;
            this.Original = Original;
            Original.Assimilate(this);
        }
        public override void Init(Universe Universe)
        {
            throw new NotImplementedException();
            //base.Init(Universe);
            //Size = Universe.HintergrundDarstellungen.Standard.Size;
            //Zentrum = new PointF(0.5f, 0.5f);
            //this.Original = Universe.TextBilder.Standard;
            //this.Transformation = RotateFlipType.RotateNoneFlipNone;
        }

        public override void AdaptToCard(Karte Karte)
        {
            Karte.Aufgaben = new Aufgabe(this);
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            TrafoTextBild b = Element as TrafoTextBild;
            b.Transformation = Transformation;
            b.Original = Original;
        }
        public override object Clone()
        {
            TrafoTextBild b = new TrafoTextBild(Transformation, Original);
            Assimilate(b);
            return b;
        }
    }
}
