using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Werwolf.Inhalt;
using Werwolf.Karten;

using Assistment.Texts;
using Assistment.Extensions;
using Assistment.Drawing.Geometries.Extensions;

namespace Werwolf.Forms
{
    public class ViewDeck : ViewBox
    {
        public override void ChangeKarte(XmlElement ChangedElement)
        {
            ((StandardDeck)WolfBox).Deck = ChangedElement as Deck;
            OnKarteChanged();
        }
        protected override WolfBox GetWolfBox(Karte Karte, float Ppm)
        {
            return new StandardDeck(Karte, Ppm);
        }
        protected override bool ChangeSize()
        {
            SizeF size = PictureBox.Size;
            Size Size = size.Max(1, 1).ToSize();
            if (Size.Equals(LastSize))
                return false;
            LastSize = Size;
            PictureBox.Image = new Bitmap(Size.Width, Size.Height);
            g = PictureBox.Image.GetHighGraphics(WolfBox.Faktor / ppm);
            DrawContext = new DrawContextGraphics(g, Brushes.White);
            return true;
        }
    }
}
