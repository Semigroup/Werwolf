using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using Assistment.Drawing;
using Assistment.Texts;
using Assistment.Drawing.Geometries;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Style;
using Assistment.Extensions;
using Assistment.Mathematik;

namespace Werwolf.Karten
{
    public class Titel : DrawBox
    {
        public Brush HintergrundFarbe { get; private set; }
        public Pen RandFarbe { get; private set; }
        public Weg RandVerlauf { get; private set; }
        public float RandHohe { get; private set; }
        public DrawBox Inhalt { get; private set; }

        public Titel(DrawBox Inhalt, float RandHohe, Weg RandVerlauf, Pen RandFarbe, Brush HintergrundFarbe)
        {
            this.Inhalt = Inhalt;
            this.RandHohe = RandHohe;
            this.RandVerlauf = RandVerlauf;
            this.RandFarbe = RandFarbe;
            this.HintergrundFarbe = HintergrundFarbe;
        }

        public override float getSpace()
        {
            float ab = RandHohe * 2;
            float breite = (Inhalt.getMax() + Inhalt.getMin()) / 2;
            float hohe = Inhalt.getSpace() / breite;

            return (breite + ab) * (hohe + ab);
        }

        public override float getMin()
        {
            return Inhalt.getMin() + 2 * RandHohe;
        }

        public override float getMax()
        {
            return Inhalt.getMax() + 2 * RandHohe;
        }

        public override void update()
        {
            Inhalt.update();
        }

        public override void setup(RectangleF box)
        {
            float ab = RandHohe * 2;
            this.box = box;
            box.X += RandHohe;
            box.Y += RandHohe;
            box.Width -= ab;
            box.Height -= ab;
            this.Inhalt.setup(box);
            this.box.Height = Inhalt.box.Height + ab;
            this.box.Width = this.Inhalt.box.Width + ab;
        }

        public override void draw(DrawContext con)
        {
            Bitmap b = new Bitmap(box.Width.Ceil(), box.Height.Ceil());
            Graphics g = b.GetHighGraphics();
            RectangleF pseudoBox = new RectangleF(RandHohe, RandHohe, Inhalt.box.Width, Inhalt.box.Height);
            OrientierbarerWeg ow = OrientierbarerWeg.RundesRechteck(pseudoBox);
            int samples = 10000;
            g.FillDrawWegAufOrientierbarerWeg(HintergrundFarbe, RandFarbe, RandVerlauf, ow, samples);
            g.Dispose();
            con.drawImage(b, box.Location);
            Inhalt.draw(con);
        }

        public override DrawBox clone()
        {
            return new Titel(Inhalt.clone(), RandHohe, RandVerlauf, RandFarbe, HintergrundFarbe);
        }

        public override void InStringBuilder(StringBuilder sb, string tabs)
        {
            throw new NotImplementedException();
        }
    }
}
