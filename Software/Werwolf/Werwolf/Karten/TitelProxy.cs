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
        private xFont LastFont;

        public Text Text { get { return (Inhalt as GeometryBox).DrawBox as Text; } }

        public TitelProxy(Titel.Art Art, string regex, xFont Font, float RandHohe, Pen RandFarbe, Brush HintergrundFarbe)
            : base(new Text(regex, Font), RandHohe, RandFarbe, HintergrundFarbe)
        {
            Proxied = Titel.GetTitel(Art, Inhalt, RandHohe, RandFarbe, HintergrundFarbe);
            InhaltChanged = true;
            LastRegex = regex;
            LastFont = Font;
            Text.alignment = 0.5f;
        }

        private void DrawImage()
        {
            Size Size = box.Size.mul(Scaling).Max(1, 1).ToSize();
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
                RectangleF pseudoBox = new RectangleF(RandHohe, RandHohe, Inhalt.box.Width, Inhalt.box.Height);
                OrientierbarerWeg ow = OrientierbarerWeg.RundesRechteck(pseudoBox);
                int samples = 10000;
                Weg y = GetVerlauf(ow.L / RandHohe);
                Weg z = t => y(t);
                Pen RandFarbe = (Pen)this.RandFarbe.Clone();
                g.FillDrawWegAufOrientierbarerWeg(HintergrundFarbe, RandFarbe, z, ow, samples);
            }
        }
        public override void draw(DrawContext con)
        {
            DrawImage();
            con.drawImage(Image, box);
            Inhalt.draw(con);
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
        public void SetText(string regex, xFont Font)
        {
            if ((regex != LastRegex) || (LastFont != Font))
            {
                LastRegex = regex;
                LastFont = Font;
                InhaltChanged = true;
                (this.Inhalt as GeometryBox).DrawBox = new Text(regex, Font);
                Text.alignment = 0.5f;
            }
        }
        public void SetArt(Titel.Art Art)
        {
            if (Art != Proxied.GetArt())
            {
                Proxied = Titel.GetTitel(Art, Inhalt, RandHohe, RandFarbe, HintergrundFarbe);
                InhaltChanged = true;
            }
        }
        public bool Empty()
        {
            return Text.empty();
        }

        public override DrawBox clone()
        {
            throw new NotImplementedException();
        }
    }
}
