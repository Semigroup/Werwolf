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

        public override bool Visible()
        {
            return base.Visible() && Karte.InfoDarstellung.Existiert;
        }

        public override void OnKarteChanged()
        {
            if (!Visible())
                return;

            base.OnKarteChanged();

            SizeF Rand = InfoDarstellung.Rand.mul(Faktor);
            Color HintergrundFarbe = InfoDarstellung.Farbe;
            //System.Windows.Forms.MessageBox.Show(HintergrundFarbe + ", " + HintergrundFarbe.ToBrush().Color);

            Text[] gesinnungen = Karte.Gesinnung.Aufgabe.ProduceTexts(InfoDarstellung.FontMeasurer);
            if (gesinnungen.Length > 0)
                Gesinnung = gesinnungen[0].Colorize(HintergrundFarbe).Geometry(Rand);
            else
                Gesinnung = new Text();
            Artist = new Text(Karte.HauptBild.Artist, InfoDarstellung.FontMeasurer)
                .Colorize(HintergrundFarbe).Geometry(Rand);
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
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Kompositum.Move(ToMove);
        }
    }
}
