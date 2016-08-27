using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Texts;

using Werwolf.Inhalt;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten
{
    public class WolfInfo : WolfBox
    {
        private DrawBox Gesinnung;
        private DrawBox Artist;
        public CString Kompositum;

        public WolfInfo(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();

            SizeF Rand = InfoDarstellung.Rand.mul(Faktor);

            Gesinnung = new Text(Karte.Gesinnung.Schreibname, InfoDarstellung.FontMeasurer).Geometry(Rand);
            Artist = new Text(Karte.Bild.Artist, InfoDarstellung.FontMeasurer).Geometry(Rand);
            Kompositum = new CString(Gesinnung, Artist);
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
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
