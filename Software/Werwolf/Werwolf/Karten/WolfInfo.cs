using System;
using System.Drawing;

using Assistment.Texts;

using Werwolf.Inhalt;
using Assistment.Drawing.Geometries.Extensions;

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

        public override float Space => throw new NotImplementedException();
        public override float Min => throw new NotImplementedException();
        public override float Max => throw new NotImplementedException();

        public override void Update()
        {

        }
        public override void Setup(RectangleF box)
        {
            Kompositum.Setup(InnenBox);
            Kompositum.Bottom = InnenBox.Bottom;
            Artist.Right = InnenBox.Right;
            Kompositum.Move(box.Location);
        }
        public override void Draw(DrawContext con)
        {
            Kompositum.Draw(con);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Kompositum.Move(ToMove);
        }
    }
}
