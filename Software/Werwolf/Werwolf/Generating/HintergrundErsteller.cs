using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.Style;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;
using Assistment.Drawing;
using Assistment.Mathematik;

namespace Werwolf.Generating
{
    public class HintergrundSchema
    {
        public enum Art
        {
            ChaosRechteck
        }

        public Art MeineArt { get; set; }
        /// <summary>
        /// in Millimeter
        /// </summary>
        public SizeF Size { get; set; }
        /// <summary>
        /// Pixel pro Millimeter
        /// </summary>
        public float ppm { get; set; }

        public FlachenSchema Schema { get; set; }
    }

    public class HintergrundErsteller
    {
        public HintergrundErsteller()
        {
            Random d = new Random();
            float burst = 0.1f;

            HintergrundSchema hs = new HintergrundSchema();
            hs.MeineArt = HintergrundSchema.Art.ChaosRechteck;
            hs.Size = new SizeF(100, 141);
            hs.ppm = 10;
            hs.Schema = new FlachenSchema();
            hs.Schema.BackColor = Color.White;
            hs.Schema.Boxes = new Point(1, 50);
            hs.Schema.Flache = (u, v) => hs.Size.mul(hs.ppm).mul(u, v + burst * d.NextCenterd()).ToPointF();
            hs.Schema.Pinsel = (u, v) => new SolidBrush(Color.Blue.tween(Color.Yellow, v).flat(100));
            hs.Schema.Samples = new Point(20, 50).mul(1);
            hs.Schema.Thumb = new Point(1, 2);

            Make(hs);
        }

        public void Make(HintergrundSchema Schema)
        {
            Bitmap b = new Bitmap((Schema.Size.Width * Schema.ppm).Ceil(), (Schema.Size.Height * Schema.ppm).Ceil());
            Graphics g = b.GetHighGraphics();
            if (Schema.Schema.BackColor.HasValue)
                g.Clear(Schema.Schema.BackColor.Value);

            Shadex.ChaosFlache(g, Schema.Schema);

            b.Save("Hintergrund.png");
        }
    }
}
