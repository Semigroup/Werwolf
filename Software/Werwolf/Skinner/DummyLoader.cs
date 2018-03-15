using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Werwolf.Inhalt;

namespace Skinner
{
    public class DummyLoader : Loader
    {
        public DummyLoader(string Pfad) : base(Pfad)
        {

        }

        public override Aufgabe GetAufgabe(string AttributeName)
        {
            return new Aufgabe();
        }
        public override BildDarstellung GetBildDarstellung()
        {
            return new BildDarstellung();
        }
        public override Deck GetDeck()
        {
            return new Deck();
        }
        public override Fraktion GetFraktion()
        {
            return new Fraktion();
        }
        public override Gesinnung GetGesinnung()
        {
            return new Gesinnung();
        }
        public override HauptBild GetHauptBild()
        {
            return new HauptBild();
        }
        public override HintergrundBild GetHintergrundBild()
        {
            return new HintergrundBild();
        }
        public override HintergrundBild GetHintergrundBildQuer()
        {
            return GetHintergrundBild();
        }
        public override HintergrundDarstellung GetHintergrundDarstellung()
        {
            return new HintergrundDarstellung();
        }
        public override InfoDarstellung GetInfoDarstellung()
        {
            return new InfoDarstellung();
        }
        public override LayoutDarstellung GetLayoutDarstellung()
        {
            return new LayoutDarstellung();
        }
        public override RuckseitenBild GetRuckseitenBild()
        {
            return new RuckseitenBild();
        }
        public override TextBild GetTextBild()
        {
            return new TextBild();
        }
        public override TextDarstellung GetTextDarstellung()
        {
            return new TextDarstellung();
        }
        public override TitelDarstellung GetTitelDarstellung()
        {
            return new TitelDarstellung();
        }
    }
}
