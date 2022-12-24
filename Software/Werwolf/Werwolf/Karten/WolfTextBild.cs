using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten
{
    public class WolfTextBild : WolfBox
    {
        public TextBild TextBild { get; private set; }
        private Size ImageSize;
        private IFontMeasurer Font;
        public ImageAttributes ImageAttributes { get; set; }

        public WolfTextBild(TextBild TextBild, IFontMeasurer Font)
            : base(null, 1)
        {
            this.TextBild = TextBild;
            this.ImageAttributes = new ImageAttributes();
            this.Font = Font.GetFont().GetMeasurer();
            this.Update();
        }

        public override float Max => Box.Width;
        public override float Min => Max;
        public override float Space => Box.Size.Inhalt();
        public override void OnKarteChanged()
        {
            if (TextBild != null)
                Update();
        }

        public override void Update()
        {
            ImageSize = TextBild.GetImageSize();
            this.Box.Height = Font.YMass('_');
            this.Box.Width = ImageSize.Width * Box.Height / ImageSize.Height;
        }
        public override void Setup(RectangleF box)
        {
            this.Box.Location = box.Location;
        }
        public override void Draw(DrawContext con)
        {
            Image img = TextBild.Image;
            if (img == null)
                return;
            con.DrawImage(img, Box, ImageAttributes);
        }

        public override DrawBox Clone()
        {
            return new WolfTextBild(TextBild, Font);
        }
    }
}
