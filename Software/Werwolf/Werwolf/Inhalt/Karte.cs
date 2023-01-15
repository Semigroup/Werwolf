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

        public int Leben { get; set; }
        public int Rustung { get; set; }
        public string Geldkosten { get; set; }

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
            /// <summary>
            /// old werwolf style
            /// </summary>
            Werwolfkarte = 0x1,
            /// <summary>
            /// Dreamers-and-Wargers-Agency style
            /// </summary>
            AktionsKarte = 0x2,
            /// <summary>
            /// card for 7Wonders mod
            /// </summary>
            WondersKarte = 0x4,
            /// <summary>
            /// domain card for 7Wonders mod
            /// </summary>
            WondersReichKarte = 0x8,
            /// <summary>
            /// global project card for 7Wonders mod
            /// </summary>
            WonderGlobalesProjekt = 0x10,
            /// <summary>
            /// select domain card for 7Wonders mod
            /// </summary>
            WondersAuswahlKarte = 0x20,
            /// <summary>
            /// item card for cyber sage
            /// </summary>
            CyberWaffenKarte = 0x40,
            /// <summary>
            /// action card for cyber sage
            /// </summary>
            CyberSupportKarte = 0x80,
            /// <summary>
            /// spell card for cyber sage
            /// </summary>
            CyberHackKarte = 0x100,
            /// <summary>
            /// modern werwolf style role card
            /// </summary>
            ModernWolfKarte = 0x200,
            /// <summary>
            /// modern werwolf style event card
            /// </summary>
            ModernWolfEreignisKarte = 0x400,
            /// <summary>
            /// modern werwolf style sign card
            /// </summary>
            ModernWolfZeichenKarte = 0x800,
            /// <summary>
            /// simple mode for making TT-RPG figures
            /// </summary>
            RollenspielFigur = 0x1000,
            /// <summary>
            /// Action card for TT-RPG
            /// </summary>
            AlchemieKarte = 0x2000,
        }
        public KartenModus Modus { get; set; }
        public static KartenModus WondersIrgendwas
            => KartenModus.WondersKarte | KartenModus.WondersReichKarte | KartenModus.WonderGlobalesProjekt | KartenModus.WondersAuswahlKarte;
        public static KartenModus CyberIrgendwas
            => KartenModus.CyberWaffenKarte | KartenModus.CyberSupportKarte | KartenModus.CyberHackKarte;
        public static KartenModus ModernIrgendwas
            => KartenModus.ModernWolfKarte | KartenModus.ModernWolfEreignisKarte | KartenModus.ModernWolfZeichenKarte;
        public static KartenModus AlchemieIrgendwas
            => KartenModus.AlchemieKarte;

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

            this.Leben = 0;
            this.Rustung = 0;
            this.Geldkosten = "";
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

            Initiative = Loader.XmlReader.GetFloat("Initiative");
            ReichweiteMin = Loader.XmlReader.GetInt("ReichweiteMin");
            ReichweiteMax = Loader.XmlReader.GetInt("ReichweiteMax");
            Felder = Loader.XmlReader.GetInt("Felder");
            Storung = Loader.XmlReader.GetInt("Storung");

            Leben = Loader.XmlReader.GetInt("Leben");
            Rustung = Loader.XmlReader.GetInt("Rustung");
            Geldkosten = Loader.XmlReader.GetString("Geldkosten");

            Kosten = Loader.GetAufgabe("Kosten");
            Effekt = Loader.GetAufgabe("Effekt");
            basen = Loader.XmlReader.GetString("Basen")
                .Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            entwicklungen = Loader.XmlReader.GetString("Entwicklungen")
               .Split(";".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            Modus = Loader.XmlReader.GetEnum<KartenModus>("Modus");
            //Abwärtskompatibilität mit Daten, die keinen Kartenmodus definieren
            if (Modus == 0)
                Modus = KartenModus.Werwolfkarte;

            AktionsName = Loader.GetAufgabe("AktionsName");
            ZielSicherheiten = Loader.XmlReader.GetString("ZielSicherheiten");
            Translatiert = Loader.XmlReader.GetBoolean("Translatiert");
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);

            XmlWriter.WriteAttribute("Fraktion", Fraktion.Name);
            XmlWriter.WriteAttribute("Gesinnung", Gesinnung.Name);
            XmlWriter.WriteAttribute("Aufgaben", Aufgaben.ToString());
            XmlWriter.WriteAttribute("HauptBild", HauptBild.Name);

            XmlWriter.WriteAttribute("HintergrundDarstellung", HintergrundDarstellung.Name);
            XmlWriter.WriteAttribute("TextDarstellung", TextDarstellung.Name);
            XmlWriter.WriteAttribute("TitelDarstellung", TitelDarstellung.Name);
            XmlWriter.WriteAttribute("InfoDarstellung", InfoDarstellung.Name);
            XmlWriter.WriteAttribute("BildDarstellung", BildDarstellung.Name);
            XmlWriter.WriteAttribute("LayoutDarstellung", LayoutDarstellung.Name);

            XmlWriter.WriteFloat("Initiative", Initiative);
            XmlWriter.WriteInt("ReichweiteMin", ReichweiteMin);
            XmlWriter.WriteInt("ReichweiteMax", ReichweiteMax);
            XmlWriter.WriteInt("Felder", Felder);
            XmlWriter.WriteInt("Storung", Storung);

            XmlWriter.WriteInt("Leben", Leben);
            XmlWriter.WriteInt("Rustung", Rustung);
            XmlWriter.WriteAttribute("Geldkosten", Geldkosten);

            XmlWriter.WriteAttribute("Kosten", Kosten.ToString());
            XmlWriter.WriteAttribute("Effekt", Effekt.ToString());
            XmlWriter.WriteAttribute("Basen", Basen.Map(b => b.Name).SumText(";"));
            XmlWriter.WriteAttribute("Entwicklungen", Entwicklungen.Map(b => b.Name).SumText(";"));
            XmlWriter.WriteEnum<KartenModus>("Modus", Modus);

            XmlWriter.WriteAttribute("AktionsName", AktionsName.ToString());
            XmlWriter.WriteAttribute("ZielSicherheiten", ZielSicherheiten);
            XmlWriter.WriteBoolean("Translatiert", Translatiert);
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

            Karte.Leben = Leben;
            Karte.Rustung = Rustung;
            Karte.Geldkosten = Geldkosten;

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
                    return GetImage(ppm, BackColor, new StandardRuckseite(this, ppm), high, false);
                case Fraktion.RuckseitenArt.Identisch:
                    return GetImage(ppm, BackColor, high);
                case Fraktion.RuckseitenArt.Deckungsgleich:
                    Image img = GetImage(ppm, BackColor, high);
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    return img;
                case Fraktion.RuckseitenArt.Figur:
                    return GetImage(ppm, BackColor, new StandardKarte(this, ppm), high, true);
                default:
                    throw new NotImplementedException();
            }
        }

        public void DrawOnGraphics(Graphics g, float ppm, WolfBox WolfBox, bool flipX)
        {
            using (DrawContextGraphicsFlip dcg = new DrawContextGraphicsFlip(g,
                new RectangleF(new PointF(), HintergrundDarstellung.Size.mul(WolfBox.Faktor)),
                ppm / WolfBox.Faktor))
            {
                if (flipX)
                    dcg.FlipX();
                WolfBox.Setup(0);
                WolfBox.Draw(dcg);
            }
        }
        public Bitmap GetImage(float ppm, Color BackColor, WolfBox WolfBox, bool high, bool flipX)
        {
            Size s = GetPictureSize(ppm);
            Bitmap img = new Bitmap(s.Width, s.Height);
            img.CleanResolution();
            using (Graphics g = img.GetGraphics(ppm / WolfBox.Faktor, BackColor, high))
                DrawOnGraphics(g, ppm, WolfBox, flipX);

            return img;
        }
        public Bitmap GetImage(float ppm, Color BackColor, bool high)
            => GetImage(ppm, BackColor, new StandardKarte(this, ppm), high, false);
        public Bitmap GetImage(float ppm, bool high)
            => GetImage(ppm, Color.FromArgb(0), high);
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
            => this.ID - other.ID;
        public int OldCompareTo(Karte other)
        {
            int val = this.HintergrundDarstellung.Name.CompareTo(other.HintergrundDarstellung.Name) * 100
                + this.Fraktion.Name.CompareTo(other.Fraktion.Name) * 10
                + this.Name.CompareTo(other.Name);
            if (val > 0)
                return 1;
            else if (val < 0)
                return -1;
            else
                return 0;
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
