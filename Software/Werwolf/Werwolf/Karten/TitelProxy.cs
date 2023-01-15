using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;
using Assistment.Extensions;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class TitelProxy : Titel
    {
        private Titel Proxied;
        private Image Image;

        private Size LastSize;
        private float LastRandHohe;

        private bool InhaltChanged;

        private string LastRegex;
        private IFontMeasurer LastFont;

        public Text Text { get { return (Inhalt as GeometryBox).DrawBox as Text; } }

        public TitelProxy(Titel.Art Art, string regex, IFontMeasurer Font, float RandHohe, float TextHorizontalMargin, Pen RandFarbe, Brush HintergrundFarbe)
            : base(new Text(regex, Font), RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe)
        {
            Proxied = Titel.GetTitel(Art, Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
            InhaltChanged = true;
            LastRegex = regex;
            LastFont = Font;
            Text.Alignment = 0.5f;
        }

        private void DrawImage()
        {
            Size Size = Box.Size.mul(Scaling).Max(1, 1).ToSize();
            if (!InhaltChanged
                && Size.Equals(LastSize)
                && RandHohe.Equals(LastRandHohe))
                return;

            LastSize = Size;
            LastRandHohe = RandHohe;
            InhaltChanged = false;

            Image = new Bitmap(Size.Width + 1, Size.Height + 1);
            using (Graphics g = Image.GetHighGraphics(Scaling))
            {
                RectangleF pseudoBox = new RectangleF(RandHohe, RandHohe, Inhalt.Box.Width, Inhalt.Box.Height);
                OrientierbarerWeg ow = OrientierbarerWeg.RundesRechteck(pseudoBox);
                int samples = 10000;
                Weg y = GetVerlauf(ow.L / RandHohe);
                Weg z = t => y(t);
                Pen RandFarbe = (Pen)this.RandFarbe.Clone();
                g.FillDrawWegAufOrientierbarerWeg(HintergrundFarbe, RandFarbe, z, ow, samples);
            }
        }
        public override void Draw(DrawContext con)
        {
            DrawImage();
            con.DrawImage(Image, Box);
            Inhalt.Draw(con);
        }

        public override Weg GetVerlauf(float units)
        {
            Proxied.RandHohe = this.RandHohe;
            Proxied.Scaling = this.Scaling;
            return Proxied.GetVerlauf(units);
        }
        public override Titel.Art GetArt()
        {
            return Proxied.GetArt();
        }
        public void SetText(string regex, IFontMeasurer Font)
        {
            if ((regex != LastRegex) || (LastFont != Font))
            {
                LastRegex = regex;
                LastFont = Font;
                InhaltChanged = true;
                (this.Inhalt as GeometryBox).DrawBox = new Text(regex, Font);
                (this.Inhalt as GeometryBox).LeftSpace = RandHohe + TextHorizontalMargin;
                (this.Inhalt as GeometryBox).RightSpace = RandHohe;
                Text.Alignment = 0.5f;
            }
        }
        public void SetArt(Titel.Art Art)
        {
            if (Art != Proxied.GetArt())
            {
                Proxied = Titel.GetTitel(Art, Inhalt, RandHohe, TextHorizontalMargin, RandFarbe, HintergrundFarbe);
                InhaltChanged = true;
            }
        }
        public bool Empty()
        {
            return Text.Empty();
        }

        public override DrawBox Clone()
        {
            throw new NotImplementedException();
        }
    }
}
