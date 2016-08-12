using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class RandBox : WolfBox
    {
        public RandBox(Karte Karte)
            : base(Karte)
        {

        }

        public override void OnKarteChanged()
        {
            update();
        }
        public override void OnPpmChanged()
        {
            update();
        }

        public override float getSpace()
        {
            return Darstellung.Size.Inhalt() * Faktor * Faktor;
        }

        public override float getMin()
        {
            return Darstellung.Size.Width * Faktor;
        }

        public override float getMax()
        {
            return getMin();
        }

        public override void update()
        {
            Darstellung.Hintergrund.MakeRandBild(ppm, Darstellung, Faktor);
        }

        public override void setup(RectangleF box)
        {
            this.box = box;
        }

        public override void draw(DrawContext con)
        {
            con.drawImage(Darstellung.Hintergrund.RandBild, box);
        }

        public override Assistment.Texts.DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
}
