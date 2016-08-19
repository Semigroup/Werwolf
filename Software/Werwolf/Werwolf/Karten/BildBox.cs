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

        public BildBox(Karte Karte, Bild Bild, Modus MyModus)
            : base(null)
        {
            this.Karte = Karte;
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
            //if (MyModus == Modus.Hauptbild)
            //    this.box = new RectangleF(Darstellung.Bild.Point.mul(Faktor), Darstellung.Bild.Size.mul(Faktor));
        }
        public override void OnPpmChanged()
        {
            this.update();
        }

        public override void setup(RectangleF box)
        {
            switch (MyModus)
            {
                case Modus.Hintergrund:
                    this.box = box;
                    break;
                case Modus.Hauptbild:
                    //this.box = new RectangleF(Darstellung.Bild.Point.mul(Faktor), Darstellung.Bild.Size.mul(Faktor));
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        public override void draw(DrawContext con)
        {
            if (Bild != null)
                con.drawImage(Bild.Image, box);
        }

        public override DrawBox clone()
        {
            BildBox bb = new BildBox(Karte, Bild, MyModus);
            bb.box = box;
            return bb;
        }
    }
}
