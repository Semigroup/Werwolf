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
    public class Karte : XmlElement, IComparable<Karte>
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

        public float Initiative { get; set; }
        public int ReichweiteMin { get; set; }
        public int ReichweiteMax { get; set; }
        public int Felder { get; set; }
        public int Storung { get; set; }

        public Aufgabe Kosten { get; set; }
        public Aufgabe Effekt { get; set; }

        private string[] basen { get; set; }
        private Karte[] realBasen { get; set; }
        public Karte[] Basen
        {
            get
            {
                if (realBasen == null)
                    realBasen = basen.Map(s => Universe.Karten[s]).ToArray();
                return realBasen;
            }
            set
            {
                realBasen = value;
            }
        }

        private string[] entwicklungen { get; set; }
        private Karte[] realEntwicklungen { get; set; }
        public Karte[] Entwicklungen
        {
            get
            {
                if (realEntwicklungen == null)
                    realEntwicklungen = entwicklungen.Map(s => Universe.Karten[s]).ToArray();
                return realEntwicklungen;
            }
            set
            {
                realEntwicklungen = value;
            }
        }

        public Karte()
            : base("Karte", true)
        {

        }

        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            Schreibname = "Beispielkarte";

            Aufgaben = new Aufgabe(
@"\r rot \g grün \b blau \o orange \y gelb \w weiß \v violett \l sattelbraun \e grau \cff0abcde FF0ABCDE \s schwarz
\+
\d fett \d \i kursiv \i \u Unterstricht \u \a Oberstrich \a \f Linksstrich \f \j Rechtstrich \j \x Horizontalstrich \x"
, Universe);
            Fraktion = Universe.Fraktionen.Standard;
            Gesinnung = Universe.Gesinnungen.Standard;
            HauptBild = Universe.HauptBilder.Standard;

            HintergrundDarstellung = Universe.HintergrundDarstellungen.Standard;
            TextDarstellung = Universe.TextDarstellungen.Standard;
            TitelDarstellung = Universe.TitelDarstellungen.Standard;
            BildDarstellung = Universe.BildDarstellungen.Standard;
            InfoDarstellung = Universe.InfoDarstellungen.Standard;

            this.Kosten = new Aufgabe();
            this.Effekt = new Aufgabe();
            this.Basen = new Karte[0];
            this.Entwicklungen = new Karte[0];
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

            Initiative = Loader.XmlReader.getFloat("Initiative");
            ReichweiteMin = Loader.XmlReader.getInt("ReichweiteMin");
            ReichweiteMax = Loader.XmlReader.getInt("ReichweiteMax");
            Felder = Loader.XmlReader.getInt("Felder");
            Storung = Loader.XmlReader.getInt("Storung");

            Kosten = Loader.GetAufgabe("Kosten");
            Effekt = Loader.GetAufgabe("Effekt");
            basen = Loader.XmlReader.getString("Basen")
                .Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            entwicklungen = Loader.XmlReader.getString("Entwicklungen")
               .Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
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

            XmlWriter.writeFloat("Initiative", Initiative);
            XmlWriter.writeInt("ReichweiteMin", ReichweiteMin);
            XmlWriter.writeInt("ReichweiteMax", ReichweiteMax);
            XmlWriter.writeInt("Felder", Felder);
            XmlWriter.writeInt("Storung", Storung);

            XmlWriter.writeAttribute("Kosten", Kosten.ToString());
            XmlWriter.writeAttribute("Effekt", Effekt.ToString());
            XmlWriter.writeAttribute("Basen", Basen.Map(b => b.Name).SumText(";"));
            XmlWriter.writeAttribute("Entwicklungen", Entwicklungen.Map(b => b.Name).SumText(";"));
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

            Karte.Initiative = Initiative;
            Karte.ReichweiteMin = ReichweiteMin;
            Karte.ReichweiteMax = ReichweiteMax;
            Karte.Storung = Storung;
            Karte.Felder = Felder;

            Karte.Kosten = Kosten;
            Karte.Effekt = Effekt;
            Karte.Entwicklungen = Entwicklungen.FlatClone();
            Karte.Basen = Basen.FlatClone();
        }
        public override object Clone()
        {
            Karte k = new Karte();
            AdaptToCard(k);
            return k;
        }
        public Karte DeepClone()
        {
            Karte k = Clone() as Karte;
            k.Fraktion = Fraktion.Clone() as Fraktion;
            return k;
        }

        public Size GetPictureSize(float ppm)
        {
            return HintergrundDarstellung.Size.mul(ppm).Max(1, 1).ToSize();
        }
        public Image GetBackImage(float ppm, Color BackColor)
        {
            return GetImage(ppm, BackColor, new StandardRuckseite(this, ppm));
        }
        public Bitmap GetImage(float ppm, Color BackColor, WolfBox WolfBox)
        {
            Size s = GetPictureSize(ppm);
            Bitmap img = new Bitmap(s.Width, s.Height);
            using (Graphics g = img.GetHighGraphics(ppm / WolfBox.Faktor))
            using (DrawContextGraphics dcg = new DrawContextGraphics(g))
            {
                g.Clear(BackColor);
                WolfBox.setup(0);
                WolfBox.draw(dcg);
            }
            return img;
        }
        public Bitmap GetImage(float ppm, Color BackColor)
        {
            return GetImage(ppm, BackColor, new StandardKarte(this, ppm));
        }
        public Bitmap GetImage(float ppm)
        {
            return GetImage(ppm, Color.FromArgb(0));
        }
        public Bitmap GetImageByHeight(float Height)
        {
            float ppm = Height / HintergrundDarstellung.Size.Height;
            return GetImage(ppm, Color.FromArgb(0));
        }
        public Image GetBackImageByHeight(float Height)
        {
            float ppm = Height / HintergrundDarstellung.Size.Height;
            return GetBackImage(ppm, Color.FromArgb(0));
        }

        public int CompareTo(Karte other)
        {
            return this.Gesinnung.Name.CompareTo(other.Gesinnung.Name) * 10000
                //+this.HintergrundDarstellung.Name.CompareTo(other.HintergrundDarstellung.Name) * 10000000
                + this.Name.CompareTo(other.Name);
        }
        public override void Rescue()
        {
            Universe.BildDarstellungen.Rescue(BildDarstellung);
            Universe.HintergrundDarstellungen.Rescue(HintergrundDarstellung);
            Universe.TextDarstellungen.Rescue(TextDarstellung);
            Universe.TitelDarstellungen.Rescue(TitelDarstellung);
            Universe.BildDarstellungen.Rescue(BildDarstellung);

            Universe.Fraktionen.Rescue(Fraktion);
            Universe.Gesinnungen.Rescue(Gesinnung);
            Universe.HauptBilder.Rescue(HauptBild);

            Aufgaben.Rescue();
        }
        public virtual WolfBox GetVorderSeite(float Ppm)
        {
            return new StandardKarte(this, Ppm);
        }
        public virtual WolfBox GetRuckSeite(float Ppm)
        {
            return new StandardRuckseite(this, Ppm);
        }
    }
}
