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
    public class BildBox : WolfBox
    {
        public enum Modus
        {
            Hintergrund,
            Hauptbild
        }

        public Modus MyModus { get; private set; }

        public Bild Bild { get; private set; }

        public BildBox(Bild Bild, Modus MyModus) :base(null)
        {
            this.Bild = Bild;
            this.MyModus = MyModus;
            this.update();
        }

        public override float getSpace()
        {
            return box.Size.Inhalt();
        }
        public override float getMin()
        {
            return box.Width;
        }
        public override float getMax()
        {
            return box.Width;
        }

        public override void update()
        {
            if (MyModus == Modus.Hauptbild)
                this.box.Size = Bild.Size.mul(ppm);
        }
        public override void OnPpmChanged()
        {
            this.update();
        }

        public override void setup(RectangleF box)
        {
            this.box.Location = box.Location;
            if (MyModus == Modus.Hintergrund)
                this.box.Size = box.Size;
        }
        public override void draw(DrawContext con)
        {
            con.drawImage(Bild.Image, box);
        }

        public override DrawBox clone()
        {
            BildBox bb = new BildBox(Bild, MyModus);
            bb.box = box;
            return bb;
        }
    }
}
