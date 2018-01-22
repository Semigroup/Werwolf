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

namespace Werwolf.Karten
{
    public class WonderProduktionsFeld : WolfBox
    {
        public DrawBox Produktion;

        public WonderProduktionsFeld(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte != null && Karte.Effekt.Anzahl > 0)
            {
                Text Text = Karte.Effekt.ProduceTexts(Karte.TextDarstellung.EffektFontMeasurer)[0];
                SizeF Size = Karte.HintergrundDarstellung.Size.sub(Karte.HintergrundDarstellung.Rand);
                Produktion = new FixedBox(Size.mul(Faktor), Text);
                (Produktion as FixedBox).Alignment = new SizeF(0.5f, 0.5f);
            }
            else
                Produktion = null;
        }

        public override bool Visible()
        {
            return base.Visible() && Produktion != null;
        }
        public override void Update()
        {
        }

        public override void Setup(RectangleF box)
        {
            this.Box = box;
            this.Box.Size = Karte.HintergrundDarstellung.Size.mul(Faktor);
            Produktion.Setup(box);
        }

        public override void Draw(DrawContext con)
        {
            con.fillRectangle(Karte.HintergrundDarstellung.Farbe.ToBrush(), Box);
            using (Image Image = Karte.Fraktion.HintergrundBild.Image)
                con.drawImage(Image, Box);
            Produktion.Draw(con);
        }
    }
}
