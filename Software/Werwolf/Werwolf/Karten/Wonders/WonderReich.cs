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
    public class WonderReich : WolfBox
    {
        Karte SubKarte = new Karte();
        WonderBalken Balken;
        WonderEffekt Effekt;
        DrawBox Text;
        WonderAusbauStufe[] Stufen;

        public WonderReich(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }
        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte != null)
            {
                if (Karte.Basen.Length > 0)
                {
                    Karte.Basen[0].Assimilate(SubKarte);
                    SubKarte.Schreibname = "";
                    SubKarte.HintergrundDarstellung = SubKarte.HintergrundDarstellung.Clone() as HintergrundDarstellung;
                    SubKarte.HintergrundDarstellung.Rand = new SizeF();

                    if (Balken == null)
                    {
                        Balken = new WonderBalken(SubKarte, Ppm);
                        Effekt = new WonderEffekt(SubKarte, Ppm);
                    }
                    else
                    {
                        Balken.Karte = SubKarte;
                        Effekt.Karte = SubKarte;
                    }
                    Text Text = new Text(Karte.Schreibname, Karte.TitelDarstellung.FontMeasurer);
                    Text.addWhitespace(Text.getMax());
                    Text.alignment = 1;
                    CString cs = new CString(Text.Colorize(TitelDarstellung.Farbe).Geometry(TitelDarstellung.Rand.mul(Faktor)));
                    cs.addAbsatz();
                    cs.addWhitespace(Karte.HintergrundDarstellung.Size.Width * Faktor);
                    cs.alignment = 1;
                    this.Text = cs;
                }
            }
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            Balken.Ppm = ppm;
            Effekt.Ppm = ppm;
        }
        public override void update()
        {
            Balken.update();
            Effekt.update();
        }

        public override bool Visible()
        {
            return base.Visible();
        }

        public override void setup(RectangleF box)
        {
            if (SubKarte.HintergrundDarstellung == null)
                return;

            this.box = AussenBox;
            this.box.Location = box.Location;

            RectangleF SmallBox = new RectangleF(box.Location, SubKarte.HintergrundDarstellung.Size).mul(Faktor);
            SmallBox = SmallBox.move(HintergrundDarstellung.Anker.mul(Faktor));
            Balken.setup(SmallBox);
            Effekt.setup(SmallBox);
            Stufen = new WonderAusbauStufe[Karte.Entwicklungen.Length];
            if (Stufen.Length > 0)
            {
                Stufen.CountMap(i => new WonderAusbauStufe(Karte.Entwicklungen[i], ppm));
                float breite = Karte.Entwicklungen.Map(x => x.HintergrundDarstellung.Size.Width).Sum() * Faktor;
                float rest = this.HintergrundDarstellung.Size.Width * Faktor - breite;
                float part = rest / (Stufen.Length + 1);
                float hohe = (this.HintergrundDarstellung.Size.Height - 21) * Faktor;
                PointF loc = new PointF(part, hohe);
                foreach (var item in Stufen)
                {
                    item.setup(loc, 0);
                    loc = loc.add(item.HintergrundDarstellung.Size.Width * Faktor + part, 0);
                }
            }
            Text.setup(this.box);
        }
        public override void Move(PointF ToMove)
        {
            if (SubKarte.HintergrundDarstellung == null)
                return;

            base.Move(ToMove);
            Balken.Move(ToMove);
            Effekt.Move(ToMove);
            foreach (var item in Stufen)
                item.Move(ToMove);
            Text.Move(ToMove);
        }

        public override void draw(DrawContext con)
        {
            if (SubKarte.HintergrundDarstellung == null)
                return;

            Balken.draw(con);
            Effekt.draw(con);
            foreach (var item in Stufen)
                item.draw(con);
            Text.draw(con);
        }
    }
}