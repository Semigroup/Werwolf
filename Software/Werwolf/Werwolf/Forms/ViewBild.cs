using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Werwolf.Inhalt;
using Werwolf.Karten;

using Assistment.Texts;
using Assistment.Extensions;


namespace Werwolf.Forms
{
    public class ViewBild : ViewBox
    {
        public override void ChangeKarte(XmlElement ChangedElement)
        {
            ((StandardBild)WolfBox).Bild = ChangedElement as Bild;
            OnKarteChanged();
        }
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            return new StandardBild(Karte, Ppm);
        }
        //protected override bool ChangeSize()
        //{
        //    SizeF size = PictureBox.Size;
        //    Size Size = size.Max(1, 1).ToSize();
        //    if (Size.Equals(LastSize))
        //        return false;
        //    LastSize = Size;
        //    PictureBox.Image = new Bitmap(Size.Width, Size.Height);
        //    g = PictureBox.Image.GetHighGraphics(WolfBox.Faktor / ppm);
        //    DrawContext = new DrawContextGraphics(g, Brushes.White);
        //    return true;
        //}
    }
}
