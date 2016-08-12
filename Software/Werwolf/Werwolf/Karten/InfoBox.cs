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
    public class InfoBox : WolfBox
    {
        public Text Text = new Text();

        public InfoBox(Karte Karte)
            : base(Karte)
        {

        }

        public override float getSpace()
        {
            return Text.getSpace();
        }
        public override float getMin()
        {
            return Text.getMin();
        }
        public override float getMax()
        {
            return Text.getMax();
        }

        public override void update()
        {
            Text.update();
        }
        public override void setup(RectangleF box)
        {
            Text.setup(box);
        }

        public override void draw(DrawContext con)
        {
            Text.draw(con);
        }

        public override Assistment.Texts.DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
}
