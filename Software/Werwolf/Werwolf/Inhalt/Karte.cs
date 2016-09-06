using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Drawing;

using Assistment.Texts;
using Assistment.Xml;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

using Werwolf.Karten;

namespace Werwolf.Inhalt
{
    public class Karte : XmlElement
    {
        public Aufgabe Aufgaben { get; set; }
        public Aufgabe MeineAufgaben
        {
            get
            {
                return Fraktion.StandardAufgaben + Aufgaben;
            }
        }
        public Fraktion Fraktion { get; set; }
        public Gesinnung Gesinnung { get; set; }
        public HauptBild HauptBild { get; set; }

        public HintergrundDarstellung HintergrundDarstellung { get; set; }
        public TextDarstellung TextDarstellung { get; set; }
        public TitelDarstellung TitelDarstellung { get; set; }
        public BildDarstellung BildDarstellung { get; set; }
        public InfoDarstellung InfoDarstellung { get; set; }

        public Karte()
            : base("Karte")
        {

        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Schreibname = "Beispielkarte";

            Aufgaben = new Aufgabe(
@"\r rot \g grün \b blau \o orange \y gelb \w weiß \v violett \l sattelbraun \e grau \cff0abcde FF0ABCDE \s schwarz
\+
\d fett \d \i kursiv \i \u Unterstricht \u \a Oberstrich \a \f Linksstrich \f \j Rechtstrich \j \x Horizontalstrich \x");
            Fraktion = Universe.Fraktionen.Standard;
            Gesinnung = Universe.Gesinnungen.Standard;
            HauptBild = Universe.HauptBilder.Standard;

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
            HauptBild = Loader.GetHauptBild();

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
            XmlWriter.writeAttribute("HauptBild", HauptBild.Name);

            XmlWriter.writeAttribute("HintergrundDarstellung", HintergrundDarstellung.Name);
            XmlWriter.writeAttribute("TextDarstellung", TextDarstellung.Name);
            XmlWriter.writeAttribute("TitelDarstellung", TitelDarstellung.Name);
            XmlWriter.writeAttribute("InfoDarstellung", InfoDarstellung.Name);
            XmlWriter.writeAttribute("BildDarstellung", BildDarstellung.Name);
        }

        public override void AdaptToCard(Karte Karte)
        {
            Assimilate(Karte);
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            Karte Karte = Element as Karte;
            Karte.Aufgaben = Aufgaben;
            Karte.Fraktion = Fraktion;
            Karte.Gesinnung = Gesinnung;
            Karte.HauptBild = HauptBild;

            Karte.HintergrundDarstellung = HintergrundDarstellung;
            Karte.TextDarstellung = TextDarstellung;
            Karte.TitelDarstellung = TitelDarstellung;
            Karte.BildDarstellung = BildDarstellung;
            Karte.InfoDarstellung = InfoDarstellung;
        }
        public override object Clone()
        {
            Karte k = new Karte();
            AdaptToCard(k);
            return k;
        }

        public Size GetPictureSize(float ppm)
        {
            return HintergrundDarstellung.Size.mul(ppm).Max(1, 1).ToSize();
        }
        public Image GetImage(float ppm, Color BackColor)
        {
            Size s = GetPictureSize(ppm);
            Image img = new Bitmap(s.Width, s.Height);
            using (Graphics g = img.GetHighGraphics(ppm / WolfBox.Faktor))
            using (DrawContextGraphics dcg = new DrawContextGraphics(g))
            {
                g.Clear(BackColor);
                StandardKarte sk = new StandardKarte(this, ppm);
                sk.setup(0);
                sk.draw(dcg);
            }
            return img;
        }
        public Image GetImage(float ppm)
        {
            return GetImage(ppm, Color.FromArgb(0));
        }
        public Image GetImageByHeight(float Height)
        {
            float ppm = Height / HintergrundDarstellung.Size.Height;
            return GetImage(ppm, Color.FromArgb(0));
        }
    }
}
