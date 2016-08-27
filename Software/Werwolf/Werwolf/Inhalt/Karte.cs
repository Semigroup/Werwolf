using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using Assistment.Xml;

namespace Werwolf.Inhalt
{
    public class Karte : XmlElement
    {
        public Aufgabe Aufgaben { get; set; }
        public Aufgabe MeineAufgaben
        {
            get
            {
                return Fraktion.StandardAufgaben+ Aufgaben ;
            }
        }
        public Fraktion Fraktion { get; set; }
        public Gesinnung Gesinnung { get; set; }
        public Bild Bild { get; set; }

        public HintergrundDarstellung HintergrundDarstellung { get; set; }
        public TextDarstellung TextDarstellung { get; set; }
        public TitelDarstellung TitelDarstellung { get; set; }
        public BildDarstellung BildDarstellung { get; set; }
        public InfoDarstellung InfoDarstellung { get; set; }

        public Karte()
            : base("Karte", true)
        {

        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Aufgaben = new Aufgabe("Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum.");
            Fraktion = Universe.Fraktionen.Standard;
            Gesinnung = Universe.Gesinnungen.Standard;
            Bild = Universe.Bilder.Standard;

            HintergrundDarstellung = Universe.HintergrundDarstellungen.Standard;
            TextDarstellung = Universe.TextDarstellungen.Standard;
            TitelDarstellung = Universe.TitelDarstellungen.Standard;
            BildDarstellung = Universe.BildDarstellungen.Standard;
            InfoDarstellung = Universe.InfoDarstellungen.Standard;
        }

        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);

            Aufgaben = Loader.GetAufgabe("Aufgaben");
            Fraktion = Loader.GetFraktion();
            Gesinnung = Loader.GetGesinnung();
            Bild = Loader.GetBild("Bild");

            HintergrundDarstellung = Loader.GetHintergrundDarstellung();
            TextDarstellung = Loader.GetTextDarstellung();
            TitelDarstellung = Loader.GetTitelDarstellung();
            InfoDarstellung = Loader.GetInfoDarstellung();
            BildDarstellung = Loader.GetBildDarstellung();
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.writeAttribute("Fraktion", Fraktion.Name);
            XmlWriter.writeAttribute("Gesinnung", Gesinnung.Name);
            XmlWriter.writeAttribute("Aufgaben", Aufgaben.ToString());
            XmlWriter.writeAttribute("Bild", Bild.Name);

            XmlWriter.writeAttribute("HintergrundDarstellung", HintergrundDarstellung.Name);
            XmlWriter.writeAttribute("TextDarstellung", TextDarstellung.Name);
            XmlWriter.writeAttribute("TitelDarstellung", TitelDarstellung.Name);
            XmlWriter.writeAttribute("InfoDarstellung", InfoDarstellung.Name);
            XmlWriter.writeAttribute("BildDarstellung", BildDarstellung.Name);
        }
    }
}
