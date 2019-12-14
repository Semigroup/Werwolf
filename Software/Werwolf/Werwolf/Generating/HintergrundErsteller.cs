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
using Assistment.form;

namespace Werwolf.Generating
{
    public class HintergrundSchema
    {
        public enum Art
        {
            ChaosRechteck,
            OldSchool,
            Kreis,
            Stetig,
            StetigeRaute,
            StetigerKreis
        }

        public Art MeineArt { get; set; }
        /// <summary>
        /// in Millimeter
        /// </summary>
        public SizeF Size { get; set; }

        public FlachenSchema Schema { get; set; }

        public Schema ToSchema(float burst)
        {
            Schema s = new Schema("Aus HintergrundSchema");
            s.background = Schema.BackColor.HasValue ? Schema.BackColor.Value : Color.Black;
            s.burst = burst * 500;
            s.farben = new Brush[2 * Schema.Boxes.Y];
            for (int i = 0; i < 2 * Schema.Boxes.Y; i++)
                s.farben[i] = Schema.Pinsel(0.5f, i / (2 * Schema.Boxes.Y - 1f));
            s.sampleRate = Schema.Samples.X;
            s.scale = 1;
            if (Schema.Stift != null)
                s.stift = Schema.Stift(0.5f, 0.5f);
            s.strings = Schema.Samples.X;
            return s;
        }
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
            hs.Schema = new FlachenSchema();
            hs.Schema.BackColor = Color.White;
            hs.Schema.Boxes = new Point(1, 50);
            hs.Schema.Flache = (u, v) => hs.Size.mul(u, v + burst * d.NextCenterd()).ToPointF();
            hs.Schema.Pinsel = (u, v) => new SolidBrush(Color.Blue.tween(Color.Yellow, v).flat(100));
            hs.Schema.Samples = new Point(20, 50).mul(1);
            hs.Schema.Thumb = new Point(1, 2);

            //Make(hs);
        }

        public void Make(HintergrundSchema Schema, float ppm)
        {
            Bitmap b = new Bitmap((Schema.Size.Width).Ceil(), (Schema.Size.Height).Ceil());
            Graphics g = b.GetHighGraphics();
            if (Schema.Schema.BackColor.HasValue)
                g.Clear(Schema.Schema.BackColor.Value);

            Shadex.ChaosFlache(g, Schema.Schema);

            b.Save("Hintergrund.png");
        }

    }
}
