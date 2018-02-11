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
        public LayoutDarstellung LayoutDarstellung { get; set; }

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

        public Aufgabe AktionsName { get; set; }

        public string ZielSicherheiten { get; set; }

        public bool Translatiert { get; set; }

        public enum KartenModus
        {
            Werwolfkarte = 0x1,
            AktionsKarte = 0x2,
            WondersKarte = 0x4,
            WondersReichKarte = 0x8,
            WonderGlobalesProjekt = 0x10,
            WondersAuswahlKarte = 0x20,
            CyberWaffenKarte = 0x40,
            CyberSupportKarte = 0x80,
            CyberHackKarte = 0x100,
            ModernWolfKarte = 0x200
        }
        public KartenModus Modus { get; set; }
        public static KartenModus WondersIrgendwas
        {
            get
            {
                return KartenModus.WondersKarte | KartenModus.WondersReichKarte | KartenModus.WonderGlobalesProjekt
                    | KartenModus.WondersAuswahlKarte;
            }
        }
        public static KartenModus CyberIrgendwas
        {
            get
            {
                return KartenModus.CyberWaffenKarte | KartenModus.CyberSupportKarte | KartenModus.CyberHackKarte;
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
            LayoutDarstellung = Universe.LayoutDarstellungen.Standard;

            this.Kosten = new Aufgabe();
            this.Effekt = new Aufgabe();
            this.Basen = new Karte[0];
            this.Entwicklungen = new Karte[0];

            this.AktionsName = new Aufgabe();
            this.ZielSicherheiten = "";

            this.Modus = KartenModus.Werwolfkarte;
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
            LayoutDarstellung = Loader.GetLayoutDarstellung();

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
            Modus = Loader.XmlReader.getEnum<KartenModus>("Modus");
            //Abwärtskompatibilität mit Daten, die keinen Kartenmodus definieren
            if (Modus == 0)
               Modus = KartenModus.Werwolfkarte;

            AktionsName = Loader.GetAufgabe("AktionsName");
            ZielSicherheiten = Loader.XmlReader.getString("ZielSicherheiten");
            Translatiert = Loader.XmlReader.getBoolean("Translatiert");
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
            XmlWriter.writeAttribute("LayoutDarstellung", LayoutDarstellung.Name);

            XmlWriter.writeFloat("Initiative", Initiative);
            XmlWriter.writeInt("ReichweiteMin", ReichweiteMin);
            XmlWriter.writeInt("ReichweiteMax", ReichweiteMax);
            XmlWriter.writeInt("Felder", Felder);
            XmlWriter.writeInt("Storung", Storung);

            XmlWriter.writeAttribute("Kosten", Kosten.ToString());
            XmlWriter.writeAttribute("Effekt", Effekt.ToString());
            XmlWriter.writeAttribute("Basen", Basen.Map(b => b.Name).SumText(";"));
            XmlWriter.writeAttribute("Entwicklungen", Entwicklungen.Map(b => b.Name).SumText(";"));
            XmlWriter.writeEnum<KartenModus>("Modus", Modus);

            XmlWriter.writeAttribute("AktionsName", AktionsName.ToString());
            XmlWriter.writeAttribute("ZielSicherheiten", ZielSicherheiten);
            XmlWriter.writeBoolean("Translatiert", Translatiert);
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
            Karte.LayoutDarstellung = LayoutDarstellung;

            Karte.Initiative = Initiative;
            Karte.ReichweiteMin = ReichweiteMin;
            Karte.ReichweiteMax = ReichweiteMax;
            Karte.Storung = Storung;
            Karte.Felder = Felder;

            Karte.Kosten = Kosten;
            Karte.Effekt = Effekt;
            Karte.Entwicklungen = Entwicklungen.FlatClone();
            Karte.Basen = Basen.FlatClone();

            Karte.AktionsName = AktionsName;
            Karte.ZielSicherheiten = ZielSicherheiten;

            Karte.Modus = Modus;
            Karte.Translatiert = Translatiert;
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
        public Image GetBackImage(float ppm, Color BackColor, bool high)
        {
            switch (Fraktion.RuckArt)
            {
                case Fraktion.RuckseitenArt.Normal:
                    return GetImage(ppm, BackColor, new StandardRuckseite(this, ppm), high);
                case Fraktion.RuckseitenArt.Identisch:
                    return GetImage(ppm, BackColor, high);
                case Fraktion.RuckseitenArt.Deckungsgleich:
                    Image img = GetImage(ppm, BackColor, high);
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    return img;
                default:
                    throw new NotImplementedException();
            }
        }
        public Bitmap GetImage(float ppm, Color BackColor, WolfBox WolfBox, bool high)
        {
            Size s = GetPictureSize(ppm);
            Bitmap img = new Bitmap(s.Width, s.Height);
            using (Graphics g = img.GetGraphics(ppm / WolfBox.Faktor, BackColor, high))
            using (DrawContextGraphics dcg = new DrawContextGraphics(g))
            {
                WolfBox.Setup(0);
                WolfBox.Draw(dcg);
            }
            return img;
        }
        public Bitmap GetImage(float ppm, Color BackColor, bool high)
        {
            return GetImage(ppm, BackColor, new StandardKarte(this, ppm), high);
        }
        public Bitmap GetImage(float ppm, bool high)
        {
            return GetImage(ppm, Color.FromArgb(0), high);
        }
        public Bitmap GetImageByHeight(float Height, bool high)
        {
            float ppm = Height / HintergrundDarstellung.Size.Height;
            return GetImage(ppm, Color.FromArgb(0), high);
        }
        public Image GetBackImageByHeight(float Height, bool high)
        {
            float ppm = Height / HintergrundDarstellung.Size.Height;
            return GetBackImage(ppm, Color.FromArgb(0), high);
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
            Universe.LayoutDarstellungen.Rescue(LayoutDarstellung);

            Universe.Fraktionen.Rescue(Fraktion);
            Universe.Gesinnungen.Rescue(Gesinnung);
            Universe.HauptBilder.Rescue(HauptBild);

            Aufgaben.Rescue();

            Kosten.Rescue();
            Effekt.Rescue();

            AktionsName.Rescue();
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
