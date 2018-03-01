using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistment.Extensions;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Drawing.Geometries;

namespace Werwolf.Karten.Modern
{
    public class ModernText : WolfBox
    {
        private GeometryBox[] Texts;

        private FontGraphicsMeasurer LastFont;
        private FontGraphicsMeasurer LastFlavourFont;
        private string LastAufgabe;

        private RectangleF MovedInnenBox;
        private RectangleF TextRegion;

        private Pen Pen;

        public ModernText(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override bool Visible()
        {
            return base.Visible()
                && Karte != null
                && Karte.MeineAufgaben.Anzahl > 0
                && !HintergrundDarstellung.Quer;
        }
        public override void Update()
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
                return;
            string aufgabe = Karte.MeineAufgaben.ToString();
            if (!(LastAufgabe == aufgabe && LastFont == TextDarstellung.FontMeasurer))
            {
                LastAufgabe = aufgabe;
                LastFont = TextDarstellung.FontMeasurer as FontGraphicsMeasurer;
                LastFlavourFont = LastFont * 0.5f;
                DrawContainer[] raws = GetTexts();
                Texts = new GeometryBox[raws.Length];
                for (int i = 0; i < raws.Length; i++)
                    Texts[i] = raws[i].Geometry(TextDarstellung.Rand.mul(Faktor * 2));
            }
            Pen = new Pen(TextDarstellung.RandFarbe, TextDarstellung.BalkenDicke * Faktor);
        }

        public DrawContainer[] GetTexts()
        {
            DrawContainer[] raws = Karte.MeineAufgaben.ProduceDrawContainers(LastFont, Karte.Fraktion.IstKomplex);
            if (Karte.Modus == Karte.KartenModus.ModernWolfEreignisKarte
                && raws.Length > 1)
            {
                DrawContainer[] italics = Karte.MeineAufgaben
                    .ProduceDrawContainers(LastFlavourFont, Karte.Fraktion.IstKomplex);
                italics[0].ForceWordStyle(style: Word.FONTSTYLE_ITALIC);
                italics[0].Alignment = 0.5f;
                raws[0] = italics[0];
            }
            return raws;
        }

        public override void Setup(RectangleF box)
        {
            this.Box = box;
            this.Box.Size = AussenBox.Size;


            MovedInnenBox = InnenBox.move(box.Location);
            TextRegion = MovedInnenBox;
            TextRegion.X += HintergrundDarstellung.MarginLeft * Faktor;
            TextRegion.Width -= Faktor * (HintergrundDarstellung.MarginLeft + HintergrundDarstellung.MarginRight);
            TextRegion.Height = 0;

            for (int i = 0; i < Texts.Length; i++)
            {
                Texts[i].Setup(TextRegion);
                TextRegion.Y += Texts[i].Box.Height;
                TextRegion.Height += Texts[i].Box.Height;
            }
            float verschieben = MovedInnenBox.Height - TextRegion.Height - HintergrundDarstellung.MarginBottom * Faktor;
            TextRegion.Y += verschieben - TextRegion.Height;
            for (int i = 0; i < Texts.Length; i++)
                Texts[i].Move(0, verschieben);
            TextRegion.X -= HintergrundDarstellung.MarginLeft * Faktor;
            TextRegion.Width += Faktor * (HintergrundDarstellung.MarginLeft + HintergrundDarstellung.MarginRight);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            for (int i = 0; i < Texts.Length; i++)
                Texts[i].Move(ToMove);
        }
        public void DrawBack(DrawContext con)
        {
            if (TextDarstellung.InnenRadius > 0)
            {
                OrientierbarerWeg ow1 = OrientierbarerWeg.Kreisbogen(
                    TextDarstellung.InnenRadius * Faktor, 0.5f, 0.25f);
                OrientierbarerWeg ow2 = OrientierbarerWeg.Kreisbogen(
                    TextDarstellung.InnenRadius * Faktor, 0.25f, 0f);
                PointF center = new PointF();
                center.X = HintergrundDarstellung.MarginLeft
                    + HintergrundDarstellung.Rand.Width
                    + TextDarstellung.InnenRadius;
                // + TextDarstellung.Rand.Width;
                center.X *= Faktor;
                center.Y = TextRegion.Y - Faktor * TextDarstellung.InnenRadius;
                ow1 += center;
                center.X = AussenBox.Width - Faktor *
                    (HintergrundDarstellung.MarginRight
                    + HintergrundDarstellung.Rand.Width
                    + TextDarstellung.InnenRadius);//+TextDarstellung.Rand.Width
                ow2 += center;

                PointF[] polygon = new PointF[10006];
                polygon[0] = new PointF(TextRegion.X + TextRegion.Width * 0.5f, TextRegion.Bottom);
                polygon[1] = new PointF(TextRegion.Left, TextRegion.Bottom);
                polygon[2] = new PointF(TextRegion.Left, center.Y);
                (ow1 * ow2).GetPolygon(polygon, 3, 10000, 0, 1);
                polygon[10003] = new PointF(TextRegion.Right, center.Y);
                polygon[10004] = new PointF(TextRegion.Right, TextRegion.Bottom);
                polygon[10005] = polygon[0];
                con.FillPolygon(TextDarstellung.Farbe.ToBrush(), polygon);
                //con.DrawPolygon(Pens.Red, polygon);
            }
            else
                con.FillRectangle(TextDarstellung.Farbe.ToBrush(), TextRegion);
        }
        public override void Draw(DrawContext con)
        {
            DrawBack(con);
            for (int i = 0; i < Texts.Length; i++)
                Texts[i].Draw(con);
            //for (int i = 1; i < Texts.Length; i++)
            //    con.DrawLine(Pen, TextRegion.Left, Texts[i].Box.Top, TextRegion.Right, Texts[i].Box.Top);
        }
    }
}
