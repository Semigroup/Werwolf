using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Texts;
using Werwolf.Inhalt;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten.Modern
{
    public class ModernInfo : WolfBox
    {
        private ShadowBox LeftBox;
        private ShadowBox RightBox;

        public ModernInfo(Karte Karte, float PPm) : base(Karte, PPm)
        {
        }

        public override void Setup(RectangleF box)
        {
            this.Box = AussenBox;
            this.Box.Location = box.Location;

            RectangleF MovedInnenBox = InnenBox.move(Box.Location);
            RectangleF BottomBox = MovedInnenBox.move(0, InnenBox.Height - HintergrundDarstellung.MarginBottom * Faktor);
            LeftBox.Setup(BottomBox);
            RightBox.Setup(BottomBox);
        }

        public override void Update()
        {
        }

        public override bool Visible()
        {
            return base.Visible()
                && InfoDarstellung.Existiert
                && (LeftBox != null || RightBox != null);
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
                return;

            SizeF size = new SizeF(InnenBox.Width, HintergrundDarstellung.MarginBottom * Faktor);

            Text left = new Text(Karte.Fraktion.Schreibname, InfoDarstellung.FontMeasurer);
            FixedBox leftFB = new FixedBox(size, left);
            leftFB.Alignment = new SizeF(0.05f, 0.5f);
            LeftBox = new ShadowBox(leftFB,
                InfoDarstellung.TextFarbe.ToBrush(),
                InfoDarstellung.Farbe.ToBrush(),
                InfoDarstellung.Rand.mul(Faktor).ToPointF());

            Text right = Karte.Gesinnung.GetText(InfoDarstellung.FontMeasurer);
            FixedBox rightFB = new FixedBox(size, right);
            rightFB.Alignment = new SizeF(0.95f, 0.5f);
            RightBox = new ShadowBox(rightFB,
                InfoDarstellung.TextFarbe.ToBrush(),
                InfoDarstellung.Farbe.ToBrush(),
                InfoDarstellung.Rand.mul(Faktor).ToPointF());

            ColorMatrix shadowMatrix = GetSolidMatrix(InfoDarstellung.Farbe);
            foreach (var item in (RightBox.BackDrawBox as FixedBox).DrawBox as Text)
                if (item is WolfTextBild jtem)
                    jtem.ImageAttributes.SetColorMatrix(
                        shadowMatrix,
                        ColorMatrixFlag.Default,
                        ColorAdjustType.Bitmap);
        }

        private ColorMatrix GetSolidMatrix(Color Color)
        {
            ColorMatrix cm = new ColorMatrix();
            cm.Matrix00 = cm.Matrix11 = cm.Matrix22 = 0;
            cm.Matrix33 = cm.Matrix44 = 1;
            cm.Matrix30 = Color.R / 255f;
            cm.Matrix31 = Color.G / 255f;
            cm.Matrix32 = Color.B / 255f;
            return cm;
        }

        public override void Draw(DrawContext con)
        {
            LeftBox.Draw(con);
            RightBox.Draw(con);
        }
    }
}
