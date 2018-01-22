using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Texts;
using Werwolf.Inhalt;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;

namespace Werwolf.Karten.CyberAktion
{
    /// <summary>
    /// setuppt zuerst Hoch, dann Tief und sagt dann Mitte, wie viel Platz er aht
    /// </summary>
    public class HochBildTiefBox : WolfBox
    {
        public virtual WolfBox Hoch { get; set; }
        public virtual WolfBox Tief { get; set; }
        public virtual WolfHauptBild Mitte { get; set; }
        public virtual DrawBox FormattedImpressum { get; set; }
        //public virtual DrawBox ZielBox { get; set; }


    public HochBildTiefBox(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Tief.Move(ToMove);
            Hoch.Move(ToMove);
            Mitte.Move(ToMove);
            FormattedImpressum.Move(ToMove);
            //ZielBox.Move(ToMove);
        }
        public override void Setup(RectangleF box)
        {
            RectangleF movedInnenBox = InnenBox.move(box.Location);
            Hoch.Setup(movedInnenBox);
            Tief.Setup(movedInnenBox);
            //ZielBox.setup(movedInnenBox);
            //ZielBox.Move((InnenBox.Width -ZielBox.box.Width)/2, Hoch.box.Height);
            Tief.Move(0, InnenBox.Height - Tief.Box.Height);

            Mitte.CenterTop = Hoch.Bottom;
            Mitte.CenterBottom = Tief.Top;
            Mitte.Setup(box);

            FormattedImpressum.Setup(movedInnenBox);
            FormattedImpressum.Move(movedInnenBox.Right - FormattedImpressum.Right,
                Tief.Box.Top - FormattedImpressum.Box.Bottom);
        }
        public override void Update()
        {
            Mitte.Update();
            Hoch.Update();
            Tief.Update();
            FormattedImpressum.Update();
            //ZielBox.update();
        }
        public override void Draw(DrawContext con)
        {
            base.Draw(con);
            Mitte.Draw(con);
            Hoch.Draw(con);
            if (Karte.Modus != Karte.KartenModus.CyberSupportKarte)
            FormattedImpressum.Draw(con);
            //ZielBox.draw(con);
            Tief.Box = Tief.Box.Inner(-1, -1);
            Tief.Draw(con);
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Hoch == null)
            {
                Hoch = new CyberWaffenHeader(Karte, Ppm);
                Mitte = new WolfHauptBild(Karte, Ppm);
                Tief = new SimpleMeineAufgabenBox(Karte, ppm);
            }
            else
            {
                Hoch.Karte = Karte;
                Tief.Karte = Karte;
                Mitte.Karte = Karte;
            }
            if (karte != null)
            {
                Text impressum = new Text();
                impressum.Add(new WolfTextBild(Karte.Universe.TextBilder["Copyright"], InfoDarstellung.FontMeasurer));
                impressum.Add(new WolfTextBild(Karte.Fraktion.Symbol, InfoDarstellung.FontMeasurer));
                FormattedImpressum =
                impressum.Geometry(InfoDarstellung.Rand.mul(Faktor)).Colorize(InfoDarstellung.Farbe);

                //ZielBox = (Hoch as CyberWaffenHeader).GetZielBox();
            }
            else
                FormattedImpressum = new Text(); //ZielBox = 
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            Hoch.Ppm = ppm;
            Tief.Ppm = ppm;
            Mitte.Ppm = ppm;
        }
    }
}
