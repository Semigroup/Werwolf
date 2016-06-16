using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using Assistment.Texts;
using Assistment.Extensions;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Darstellung : XmlElement
    {
        public HintergrundDarstellung Hintergrund { get; private set; }
        public TitelDarstellung Titel { get; private set; }
        public BildDarstellung Bild { get; private set; }
        public TextDarstellung Text { get; private set; }
        public InfoDarstellung Info { get; private set; }

        public UnterDarstellung[] UnterDarstellungen { get { return new UnterDarstellung[] { Hintergrund, Titel, Bild, Text, Info }; } }

        /// <summary>
        /// Größe in Millimeter
        /// </summary>
        public SizeF Size { get; private set; }

        public Darstellung()
            : base("Darstellung", false)
        {
            this.Hintergrund = new HintergrundDarstellung();
            this.Titel = new TitelDarstellung();
            this.Bild = new BildDarstellung();
            this.Text = new TextDarstellung();
            this.Info = new InfoDarstellung();
        }

        protected override void ReadIntern(Loader Loader)
        {
            Size = Loader.XmlReader.getSizeF("Size");
            while (Loader.XmlReader.Next())
                switch (Loader.XmlReader.NodeType)
                {
                    case XmlNodeType.Element:
                        bool Found = false;
                        foreach (var item in UnterDarstellungen)
                            if (item.XmlName.Equals(Loader.XmlReader.Name))
                            {
                                item.Read(Loader);
                                Found = true;
                            }
                        if (!Found)
                            throw new NotImplementedException();
                        break;

                    case XmlNodeType.EndElement:
                        if (this.XmlName.Equals(Loader.XmlReader.Name))
                            return;
                        else
                            throw new NotImplementedException();

                    default:
                        throw new NotImplementedException();
                }
        }
    }

    public abstract class UnterDarstellung : XmlElement
    {
        /// <summary>
        /// in Millimeter
        /// </summary>
        public SizeF Rand { get; private set; }
        public xFont Font { get; private set; }
        public bool Existiert { get; private set; }
        public bool HatRand { get; private set; }

        public UnterDarstellung(string XmlName)
            : base(XmlName, true)
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            Existiert = Loader.XmlReader.getBoolean("Existiert");
            HatRand = Loader.XmlReader.getBoolean("HatRand");
            Font = Loader.GetFont("Font");
            Rand = Loader.XmlReader.getSizeF("Rand");
        }

        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            XmlWriter.writeBoolean("Existiert", Existiert);
        }
    }
    public class HintergrundDarstellung : UnterDarstellung
    {
        public HintergrundDarstellung()
            : base("Hintergrund")
        {

        }
    }
    public class TitelDarstellung : UnterDarstellung
    {
        public Color HintergrundFarbe { get; private set; }

        public TitelDarstellung()
            : base("Titel")
        {
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            HintergrundFarbe = Loader.XmlReader.getColorHexARGB("HintergrundFarbe");
        }
    }
    public class BildDarstellung : UnterDarstellung
    {
        public SizeF Alignment { get; private set; }

        public BildDarstellung()
            : base("Bild")
        {

        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            Alignment = Loader.XmlReader.getSizeF("Alignment");
        }
    }
    public class TextDarstellung : UnterDarstellung
    {
        public Color HintergrundFarbe { get; private set; }
        public bool BalkenProBlock { get; private set; }

        public TextDarstellung()
            : base("Text")
        {
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            HintergrundFarbe = Loader.XmlReader.getColorHexARGB("HintergrundFarbe");
            BalkenProBlock = Loader.XmlReader.getBoolean("BalkenProBlock");
        }
    }
    public class InfoDarstellung : UnterDarstellung
    {
        public InfoDarstellung()
            : base("Info")
        {
        }
    }

}
