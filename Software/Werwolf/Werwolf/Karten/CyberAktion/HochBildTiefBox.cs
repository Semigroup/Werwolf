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

        public HochBildTiefBox(Karte Karte, float Ppm)
            :base(Karte, Ppm)
        {
         
        }

        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Tief.Move(ToMove);
            Hoch.Move(ToMove);
            Mitte.Move(ToMove);
        }
        public override void setup(RectangleF box)
        {
            RectangleF movedInnenBox = InnenBox.move(box.Location);
            Hoch.setup(movedInnenBox);
            Tief.setup(movedInnenBox);
            Tief.Move(new PointF(0, InnenBox.Height - Tief.box.Height));

            Mitte.CenterTop = Hoch.Bottom;
            Mitte.CenterBottom = Tief.Top;
            Mitte.setup(box);
        }
        public override void update()
        {
            Mitte.update();
            Hoch.update();
            Tief.update();
        }
        public override void draw(DrawContext con)
        {
            base.draw(con);
            Mitte.draw(con);
            Hoch.draw(con);
            Tief.box = Tief.box.Inner(-1, -1);
            Tief.draw(con);
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
