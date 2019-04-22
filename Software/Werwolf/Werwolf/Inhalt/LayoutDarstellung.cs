using System.Linq;
using System.Xml;
using System.Drawing;
using Assistment.Extensions;
using Assistment.Xml;

namespace Werwolf.Inhalt
{
    /// <summary>
    /// Beinhaltet alle Textbilder, die fürs Layout verwendet werden.
    /// </summary>
    public class LayoutDarstellung : Darstellung
    {
        private TextBild grossesNamenfeld;
        public TextBild GrossesNamenfeld
        {
            get { return grossesNamenfeld; }
            set
            {
                grossesNamenfeld = value;
                GrossesNamenfeldTransponiert = new TrafoTextBild(RotateFlipType.RotateNoneFlipY, value);
            }
        }
        public TextBild GrossesNamenfeldTransponiert { get; private set; }

        private TextBild kleinesNamenfeld;
        public TextBild KleinesNamenfeld
        {
            get { return kleinesNamenfeld; }
            set
            {
                kleinesNamenfeld = value;
                KleinesNamenfeldTransponiert = new TrafoTextBild(RotateFlipType.RotateNoneFlipY, value);
            }
        }
        public TextBild KleinesNamenfeldTransponiert { get; private set; }

        public TextBild Storung { get; set; }
        public TextBild Initiative { get; set; }
        public TextBild Felder { get; set; }
        public TextBild Reichweite { get; set; }

        /// <summary>
        /// ZielSicherheiten[x+9] lieftert ZielSicherheit x
        /// <para> x geht von -9 bis 9, beide Grenzen eingeschlossen </para>
        /// </summary>
        public TextBild[] ZielSicherheiten { get; set; } = new TextBild[19];
        public TextBild ZielSicherheitenSchutze { get; set; }

        public LayoutDarstellung()
            : base("LayoutDarstellung")
        {
        }
        public override void Init(Universe Universe)
        {
            base.Init(Universe);
            GrossesNamenfeld = Universe.TextBilder.Standard;
            KleinesNamenfeld = Universe.TextBilder.Standard;

            Storung = Initiative = Felder = Reichweite
                = Universe.TextBilder.Standard;

            for (int i = 0; i < ZielSicherheiten.Length; i++)
                ZielSicherheiten[i] = Universe.TextBilder.Standard;
            ZielSicherheitenSchutze = Universe.TextBilder.Standard;
        }
        protected override void ReadIntern(Loader Loader)
        {
            base.ReadIntern(Loader);
            GrossesNamenfeld = Universe.TextBilder[Loader.XmlReader.GetString("GrossesNamenfeld")];
            KleinesNamenfeld = Universe.TextBilder[Loader.XmlReader.GetString("KleinesNamenfeld")];

            Storung = Universe.TextBilder[Loader.XmlReader.GetString("Storung")];
            Initiative = Universe.TextBilder[Loader.XmlReader.GetString("Initiative")];
            Felder = Universe.TextBilder[Loader.XmlReader.GetString("Felder")];
            Reichweite = Universe.TextBilder[Loader.XmlReader.GetString("Reichweite")];

            string[] strings = Loader.XmlReader.GetStrings("ZielSicherheiten", ", ");

            for (int i = 0; i < strings.Length; i++)
                ZielSicherheiten[i] = Universe.TextBilder[strings[i]];
            ZielSicherheitenSchutze = Universe.TextBilder[Loader.XmlReader.GetString("Schütze")];
        }
        protected override void WriteIntern(XmlWriter XmlWriter)
        {
            base.WriteIntern(XmlWriter);
            XmlWriter.WriteAttribute("GrossesNamenfeld", GrossesNamenfeld.Name);
            XmlWriter.WriteAttribute("KleinesNamenfeld", KleinesNamenfeld.Name);

            XmlWriter.WriteAttribute("Storung", Storung.Name);
            XmlWriter.WriteAttribute("Initiative", Initiative.Name);
            XmlWriter.WriteAttribute("Felder", Felder.Name);
            XmlWriter.WriteAttribute("Reichweite", Reichweite.Name);

            XmlWriter.WriteAttribute("ZielSicherheiten",ZielSicherheiten.SumText(","));
            XmlWriter.WriteAttribute("Schütze", ZielSicherheitenSchutze.Name);
        }

        public override void AdaptToCard(Karte Karte)
        {
            Karte.LayoutDarstellung = this;
        }
        public override void Assimilate(XmlElement Element)
        {
            base.Assimilate(Element);
            LayoutDarstellung ld = Element as LayoutDarstellung;
            ld.GrossesNamenfeld = GrossesNamenfeld;
            ld.KleinesNamenfeld = KleinesNamenfeld;
            ld.Initiative = Initiative;
            ld.Storung = Storung;
            ld.Reichweite = Reichweite;
            ld.Felder = Felder;
            for (int i = 0; i < ld.ZielSicherheiten.Length; i++)
                ld.ZielSicherheiten[i] = ZielSicherheiten[i];
            ld.ZielSicherheitenSchutze = ZielSicherheitenSchutze;
        }
        public override object Clone()
        {
            LayoutDarstellung hg = new LayoutDarstellung();
            Assimilate(hg);
            return hg;
        }
        public override void Rescue()
        {
            base.Rescue();
            Universe.TextBilder.Rescue(GrossesNamenfeld);
            Universe.TextBilder.Rescue(KleinesNamenfeld);
            Universe.TextBilder.Rescue(Storung);
            Universe.TextBilder.Rescue(Initiative);
            Universe.TextBilder.Rescue(Felder);
            Universe.TextBilder.Rescue(Reichweite);
            for (int i = 0; i < ZielSicherheiten.Length; i++)
                Universe.TextBilder.Rescue(ZielSicherheiten[i]);
            Universe.TextBilder.Rescue(ZielSicherheitenSchutze);
        }

        public TextBild GetGrossesNamenfeld(bool AufDemKopf)
        {
            if (AufDemKopf)
                return GrossesNamenfeldTransponiert;
            else
                return GrossesNamenfeld;
        }
        public TextBild GetKleinesNamenfeld(bool AufDemKopf)
        {
            if (AufDemKopf)
                return KleinesNamenfeldTransponiert;
            else
                return KleinesNamenfeld;
        }
    }
}
