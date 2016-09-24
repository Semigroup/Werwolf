using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Werwolf.Karten;
using Werwolf.Inhalt;

using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace ActionCardDesigner
{
    public class AktionsHeader : WolfBox
    {
        private static Image FelderBild = Image.FromFile("./Ressourcen/Felder.png");
        private static Image InitiativeBild = Image.FromFile("./Ressourcen/Initiative.png");
        private static Image ReichweiteBild = Image.FromFile("./Ressourcen/Reichweite.png");
        private static Image StorungBild = Image.FromFile("./Ressourcen/Storung.png");

        private Pen Rand = Pens.Black;
        CString Kompositum;
        Text Links;
        Text Rechts;

        public AktionsHeader(AktionsKarte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override void update()
        {
        }

        public void Build()
        {
            AktionsKarte ak = Karte as AktionsKarte;
            xFont font = Karte.TitelDarstellung.FontMeasurer;
            float h = font.getZeilenabstand();

            Links = new Text(ak.Fraktion.Schreibname + "\n" + ak.Schreibname, font);
            Rechts = new Text("", font);
            Rechts.addWort(ak.Initiative);
            Rechts.addZoomedImage(InitiativeBild);
            Rechts.addWhitespace(1);
            if (ak.Felder > 0)
            {
                Rechts.addWort(ak.Felder);
                Rechts.addZoomedImage(FelderBild);
                Rechts.addWhitespace(1);
            }
            Rechts.addAbsatz();
            if (ak.ReichweiteMin == 0)
                Rechts.addWort(ak.ReichweiteMax + "m");
            else
                Rechts.addWort(ak.ReichweiteMin + "-" + ak.ReichweiteMax + "m");
            Rechts.addZoomedImage(ReichweiteBild);
            Rechts.addWhitespace(1);
            if (ak.Storung < 100)
            {
                Rechts.addWort(ak.Storung);
                Rechts.addZoomedImage(StorungBild);
                Rechts.addWhitespace(1);
            }

            Kompositum = new CString(Links, Rechts);
        }

        public override void setup(RectangleF box)
        {
            if (Karte != null)
            {
                Build();

                this.box = InnenBox;
                Kompositum.setup(this.box);
                Rechts.Move(InnenBox.Right - Rechts.Right, 0);
                this.Move(box.Location);
                this.box.Height = Kompositum.box.Height;
            }
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            Kompositum.Move(ToMove);
        }
        public override void draw(DrawContext con)
        {
            con.fillRectangle(TitelDarstellung.Farbe.ToBrush(), box);
            Kompositum.draw(con);
        }

        public static PointF[] Dreieck(RectangleF r)
        {
            float l = (float)(Math.Sqrt(4 / 3) * r.Height);
            return new PointF[] {
            new PointF(r.Left, r.Bottom),
            new PointF(r.Left + l/ 2, r.Top),
            new PointF(r.Left + l, r.Bottom),
            new PointF(r.Left, r.Bottom)
            };
        }
        public static PointF[] Sechseck(RectangleF r)
        {
            float l = (float)(Math.Sqrt(4 / 3) * r.Height);
            float m = (r.Bottom + r.Top) / 2;
            return new PointF[] {
            new PointF(r.Left, m),
            new PointF(r.Left + l /4, r.Top),
            new PointF(r.Left + l*3 /4, r.Top),
            new PointF(r.Right, m),
            new PointF(r.Left + l*3 /4, r.Bottom),
            new PointF(r.Left + l /4, r.Bottom),
            new PointF(r.Left, m),
            };
        }
    }
}
