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
        private Size ImageSize;
        private xFont Font;

        public WolfTextBild(TextBild TextBild, xFont Font)
            : base(null, 1)
        {
            this.TextBild = TextBild;
            this.Font = Font.getFont().GetMeasurer();
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
            this.box.Height = Font.yMass('_');
            this.box.Width = ImageSize.Width * box.Height / ImageSize.Height;
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
