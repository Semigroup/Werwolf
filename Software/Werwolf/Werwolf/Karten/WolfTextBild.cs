using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten
{
    public class WolfTextBild : WolfBox
    {
        public TextBild TextBild { get; private set; }
        public bool Info { get; set; }

        private Size ImageSize;

        public WolfTextBild(TextBild TextBild, Karte Karte, bool Info)
            : base(Karte, 1)
        {
            this.TextBild = TextBild;
            this.Info = Info;
            this.update();
        }

        public override float getMax()
        {
            return box.Width;
        }
        public override float getMin()
        {
            return getMax();
        }
        public override float getSpace()
        {
            return box.Size.Inhalt();
        }
        public override void OnKarteChanged()
        {
            if (TextBild != null)
                update();
        }

        public override void update()
        {
            ImageSize = TextBild.GetImageSize();
            FontMeasurer fm = Info ? Karte.InfoDarstellung.FontMeasurer : Karte.TextDarstellung.FontMeasurer;
            this.box.Height = fm.yMass('_');
            this.box.Width = box.Height * ImageSize.Width / ImageSize.Height;
        }
        public override void setup(RectangleF box)
        {
            this.box.Location = box.Location;
        }
        public override void draw(DrawContext con)
        {
            Image img = TextBild.Image;
            if (img == null)
                return;
            con.drawImage(img, box);
        }
    }
}
