using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using Assistment.Drawing.LinearAlgebra;

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
