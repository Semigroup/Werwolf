using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Texts;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class WolfInfo : WolfBox
    {
        private Text Gesinnung;
        private Text Artist;
        private CString Kompositum;

        public WolfInfo(Karte Karte)
            : base(Karte)
        {

        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            update();
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            update();
        }

        public override float getSpace()
        {
            throw new NotImplementedException();
        }
        public override float getMin()
        {
            throw new NotImplementedException();
        }
        public override float getMax()
        {
            throw new NotImplementedException();
        }

        public override void update()
        {
            Gesinnung = new Text(Karte.Gesinnung.Schreibname, InfoDarstellung.FontMeasurer);
            Artist = new Text(Karte.Bild.Artist, InfoDarstellung.FontMeasurer);
            Kompositum = new CString(Gesinnung, Artist);
        }
        public override void setup(RectangleF box)
        {
            Kompositum.setup(InnenBox);
            Kompositum.Bottom = InnenBox.Bottom;
            Artist.Right = InnenBox.Right;
            Kompositum.Move(box.Location);
        }
        public override void draw(DrawContext con)
        {
            Kompositum.draw(con);
        }
    }
}
