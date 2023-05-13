using System.Drawing;
using Assistment.Drawing.Geometries.Extensions;

using Assistment.Texts;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public static class DrawContextExtensions
    {
        public static void DrawCenteredImage(this DrawContext Context, Bild Bild, PointF Zentrum, RectangleF ClippedRegion)
        {
            Context.DrawClippedImage(ClippedRegion, Bild.Image, Bild.Rectangle.move(Zentrum));
        }
        public static void DrawCenteredImage(this DrawContext Context, Bild Bild, Image Image, PointF Zentrum, RectangleF ClippedRegion)
        {
            Context.DrawClippedImage(ClippedRegion, Image, Bild.Rectangle.move(Zentrum));
        }
    }
}
