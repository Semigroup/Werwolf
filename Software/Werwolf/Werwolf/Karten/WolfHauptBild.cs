using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistment.Extensions;
using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class WolfHauptBild : WolfBox
    {
        public float CenterTop { get; set; }
        public float CenterBottom { get; set; }

        public PointF PointOfInterest;

        private Bitmap GefiltertesBild;

        private Color LastErsteFilterFarbe;
        private Color LastZweiteFilterFarbe;
        private BildDarstellung.Filter LastFilter;
        private string LastFilePath;

        public WolfHauptBild(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override bool Visible()
        {
            return base.Visible() && BildDarstellung.Existiert && Karte.HauptBild.FilePath.Length > 0;
        }

        public override void update()
        {
        }

        public override void setup(RectangleF box)
        {
            this.box = box;
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();
            PointOfInterest = new PointF(MovedAussenBoxCenter.X, (3 * CenterTop + CenterBottom) / 4);

            if (BildDarstellung.MyFilter != Inhalt.BildDarstellung.Filter.Keiner)
            {
                if (!(LastErsteFilterFarbe == BildDarstellung.ErsteFilterFarbe
                    && LastZweiteFilterFarbe == BildDarstellung.ZweiteFilterFarbe
                    && LastFilter == BildDarstellung.MyFilter
                    && LastFilePath == Karte.HauptBild.FilePath
                    ))
                {
                    LastErsteFilterFarbe = BildDarstellung.ErsteFilterFarbe;
                    LastZweiteFilterFarbe = BildDarstellung.ZweiteFilterFarbe;
                    LastFilter = BildDarstellung.MyFilter;
                    LastFilePath = Karte.HauptBild.FilePath;
                    Filter();
                }
            }

        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            PointOfInterest = PointOfInterest.add(ToMove);
        }

        public override void draw(DrawContext con)
        {
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(0, 0);
            if (BildDarstellung.MyFilter == Inhalt.BildDarstellung.Filter.Keiner)
                con.DrawCenteredImage(Karte.HauptBild, PointOfInterest, MovedInnenBox);
            else
                con.DrawCenteredImage(Karte.HauptBild, GefiltertesBild, PointOfInterest, MovedInnenBox);
        }
        private void Filter()
        {
            if (GefiltertesBild != null)
                GefiltertesBild.Dispose();

            Bitmap Bitmap = new Bitmap(Karte.HauptBild.TotalFilePath);
            BitmapData Data = Bitmap.LockBits(
                new Rectangle(new Point(), Bitmap.Size),
                ImageLockMode.ReadWrite,
                Bitmap.PixelFormat);
            int bufferSize = Data.Height * Data.Stride;
            byte[] bytes = new byte[bufferSize]; //BGRA
            Marshal.Copy(Data.Scan0, bytes, 0, bufferSize);
            BildDarstellung.FilterBytes(bytes, Bitmap.PixelFormat);
            Marshal.Copy(bytes, 0, Data.Scan0, bufferSize);
            Bitmap.UnlockBits(Data);
            GefiltertesBild = Bitmap;
        }
    }
}
