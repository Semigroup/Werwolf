using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

using Assistment.Drawing;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;

using Werwolf.Inhalt;

namespace Werwolf.Karten
{
    public class StandardKarte : WolfBox
    {
        public WolfHeader Header { get; set; }

        public WolfTitel Titel { get; set; }
        public WolfText Text { get; set; }
        public WolfInfo Info { get; set; }
        public WolfHauptBild HauptBild { get; set; }

        public WonderBalken Balken { get; set; }
        public WonderInfos WonderInfos { get; set; }
        public WonderEffekt WonderEffekt { get; set; }
        public WonderText WonderText { get; set; }
        public WonderReich WonderReich { get; set; }
        public WonderGlobalesReich WonderGlobalesReich { get; set; }
        public WondersDoppelBild WondersDoppelBild { get; set; }
        public WonderDoppelName WonderDoppelName { get; set; }

        private WolfBox[] WolfBoxs
        {
            get
            {
                if (Karte != null)
                {
                    switch (Karte.Modus)
                    {
                        case Karte.KartenModus.Werwolfkarte:
                            return new WolfBox[] { HauptBild, Titel, Text, Info };
                        case Karte.KartenModus.AktionsKarte:
                            return new WolfBox[] { HauptBild, Header, Text };
                        case Karte.KartenModus.WondersKarte:
                            return new WolfBox[] { HauptBild, WonderText, Balken, WonderEffekt, WonderInfos };
                        case Karte.KartenModus.WondersReichKarte:
                            return new WolfBox[] { HauptBild, WonderReich };
                        case Karte.KartenModus.WonderGlobalesProjekt:
                            return new WolfBox[] { HauptBild, WonderGlobalesReich };
                        case Karte.KartenModus.WondersAuswahlKarte:
                            return new WolfBox[] { WondersDoppelBild, WonderDoppelName };
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                    return new WolfBox[] { };
            }
        }

        public override float getSpace()
        {
            return AussenBox.Size.Inhalt();
        }
        public override float getMin()
        {
            return AussenBox.Size.Width;
        }
        public override float getMax()
        {
            return getMin();
        }

        public StandardKarte(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
            BuildUp();
        }
        public virtual void BuildUp()
        {
            Header = new WolfHeader(Karte, ppm);
            Titel = new WolfTitel(Karte, ppm);
            Text = new WolfText(Karte, ppm);
            Info = new WolfInfo(Karte, ppm);
            Balken = new WonderBalken(Karte, ppm);
            WonderInfos = new WonderInfos(Karte, ppm);
            WonderEffekt = new WonderEffekt(Karte, ppm);
            HauptBild = new WolfHauptBild(Karte, ppm);
            WonderText = new WonderText(Karte, ppm);
            WonderReich = new WonderReich(Karte, ppm);
            WonderGlobalesReich = new WonderGlobalesReich(Karte, ppm);
            WondersDoppelBild = new WondersDoppelBild(Karte, ppm);
            WonderDoppelName = new WonderDoppelName(Karte, ppm);
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            foreach (var item in WolfBoxs)
                if (item != null)
                    item.Karte = Karte;
            update();
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            foreach (var item in WolfBoxs)
                if (item != null && item.Visible())
                    item.OnPpmChanged();
            update();
        }

        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.Move(ToMove);
        }
        public override void update()
        {
            foreach (var item in WolfBoxs)
                if (item != null && item.Visible())
                    item.update();
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Size = AussenBox.Size;

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.setup(box);
            switch (Karte.Modus)
            {
                case Karte.KartenModus.Werwolfkarte:
                    if (Text.Visible() && Info.Visible())
                        Text.KorrigierUmInfo(Info.Kompositum.box.Height);
                    break;
                case Karte.KartenModus.AktionsKarte:
                    break;
                case Karte.KartenModus.WondersKarte:
                    if (WonderText.Visible() && WonderInfos.Visible())
                    {
                        WonderText.SetEntwicklungsBreite(WonderInfos.EntwicklungsBreite);
                        WonderText.setup(box);
                    }
                    break;
                default:
                    break;
            }
        }
        public void drawNormal(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(-1, -1);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            float top = Titel.Visible() ? Titel.Titel.Bottom : MovedInnenBox.Top;
            float bottom = MovedInnenBox.Bottom;
            if (Text.Visible())
                bottom = Text.OuterBox.Top + box.Location.Y;
            else if (Info.Visible())
                bottom = Info.Kompositum.Top;

            HauptBild.CenterTop = top;
            HauptBild.CenterBottom = bottom;
            HauptBild.setup(HauptBild.box);

            if (HintergrundDarstellung.Existiert)
            {
                con.fillRectangle(HintergrundDarstellung.Farbe.ToBrush(), MovedInnenBox);
                con.DrawCenteredImage(Karte.Fraktion.HintergrundBild, MovedAussenBoxCenter, MovedInnenBox);
            }

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.draw(con);
        }
        public void drawAction(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(-1, -1);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            float top = Header.Visible() ? Header.Bottom : MovedInnenBox.Top;
            float bottom = MovedInnenBox.Bottom;
            if (Text.Visible())
                bottom = Text.OuterBox.Top + box.Location.Y;
            else if (Info.Visible())
                bottom = Info.Kompositum.Top;

            HauptBild.CenterTop = top;
            HauptBild.CenterBottom = bottom;
            HauptBild.setup(HauptBild.box);

            if (HintergrundDarstellung.Existiert)
            {
                con.fillRectangle(HintergrundDarstellung.Farbe.ToBrush(), MovedInnenBox);
                con.DrawCenteredImage(Karte.Fraktion.HintergrundBild, MovedAussenBoxCenter, MovedInnenBox);
            }

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.draw(con);
        }
        public void drawWonders(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(0, 0);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            HauptBild.CenterTop = MovedInnenBox.Top;
            HauptBild.CenterBottom = MovedInnenBox.Bottom;
            if (HauptBild.Visible())
                HauptBild.setup(HauptBild.box);

            con.fillRectangle(Karte.TitelDarstellung.Farbe.ToBrush(), MovedInnenBox);

            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.draw(con);
        }
        public void drawWondersReich(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(0, 0);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            HauptBild.CenterTop = MovedInnenBox.Top;
            HauptBild.CenterBottom = MovedInnenBox.Bottom;
            if (HauptBild.Visible())
                HauptBild.setup(HauptBild.box);

            con.fillRectangle(Color.White.ToBrush(), MovedInnenBox);
            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.draw(con);
        }
        public void drawWondersAuswahl(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            RectangleF MovedInnenBox = InnenBox.move(box.Location).Inner(0, 0);
            PointF MovedAussenBoxCenter = MovedAussenBox.Center();

            con.fillRectangle(Color.White.ToBrush(), MovedInnenBox);
            foreach (var item in WolfBoxs)
                if (item.Visible())
                    item.draw(con);
        }
        public override void draw(DrawContext con)
        {
            switch (Karte.Modus)
            {
                case Karte.KartenModus.Werwolfkarte:
                    drawNormal(con);
                    break;
                case Karte.KartenModus.AktionsKarte:
                    drawAction(con);
                    break;
                case Karte.KartenModus.WondersKarte:
                    drawWonders(con);
                    break;
                case Karte.KartenModus.WondersReichKarte:
                case Karte.KartenModus.WonderGlobalesProjekt:
                    drawWondersReich(con);
                    break;
                case Karte.KartenModus.WondersAuswahlKarte:
                    drawWondersAuswahl(con);
                    break;
                default:
                    throw new NotImplementedException();
            }
            drawRand(con);
        }
        public void drawRand(DrawContext con)
        {
            RectangleF MovedAussenBox = AussenBox.move(box.Location);
            if (HintergrundDarstellung.Rand.Inhalt() > 0)
            {
                HintergrundDarstellung.MakeRandBild(ppm);
                con.drawImage(HintergrundDarstellung.RandBild, MovedAussenBox);
            }
        }
    }
}
