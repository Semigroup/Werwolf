﻿using System;
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

namespace Werwolf.Karten.Modern
{
    public class ModernTextQuer : WolfBox
    {
        private GeometryBox[] Texts;

        private FontGraphicsMeasurer LastFont;
        private string LastAufgabe;

        private RectangleF MovedInnenBox;
        private RectangleF TextRegion;

        private Pen Pen;

        public ModernTextQuer(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override bool Visible()
        {
            return base.Visible()
                && Karte != null
                && Karte.MeineAufgaben.Anzahl > 0
                && HintergrundDarstellung.Quer;
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
                Text[] raws = Karte.MeineAufgaben.ProduceTexts(LastFont);
                Texts = new GeometryBox[raws.Length];
                for (int i = 0; i < raws.Length; i++)
                    Texts[i] = raws[i].Geometry(TextDarstellung.Rand.mul(Faktor * 2));
            }
            Pen = new Pen(TextDarstellung.RandFarbe, TextDarstellung.BalkenDicke * Faktor);
        }

        public override void Setup(RectangleF box)
        {
            this.Box = box;
            this.Box.Size = AussenBox.Size;

            MovedInnenBox = InnenBox.move(box.Location);
            TextRegion = TextDarstellung.TextRectangle.mul(Faktor);
            float usedHeight = 0;

            for (int i = 0; i < Texts.Length; i++)
            {
                Texts[i].Setup(TextRegion);
                TextRegion.Y += Texts[i].Box.Height;
                usedHeight += Texts[i].Box.Height;
            }
            if (usedHeight < TextRegion.Height)
            {
                float remainder = (TextRegion.Height - usedHeight) / (Texts.Length + 1f);
                if (Texts.Length > 1)
                    for (int i = 0; i < Texts.Length; i++)
                        Texts[i].Move(0, remainder * (i + 1));
                else
                    Texts[0].Move(0, remainder / 2f);
            }

        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            for (int i = 0; i < Texts.Length; i++)
                Texts[i].Move(ToMove);
        }
        public override void Draw(DrawContext con)
        {
            //con.FillRectangle(TextDarstellung.Farbe.ToBrush(), TextRegion);
            for (int i = 0; i < Texts.Length; i++)
                Texts[i].Draw(con);
            //for (int i = 1; i < Texts.Length; i++)
            //    con.DrawLine(Pen, TextRegion.Left, Texts[i].Box.Top, TextRegion.Right, Texts[i].Box.Top);
        }
    }
}
