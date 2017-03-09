using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using Werwolf.Karten;
using Werwolf.Inhalt;

using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten
{
    public class WolfHeader : WolfBox
    {
        //private static Image FelderBild = Image.FromFile("./Ressourcen/Felder.png");
        //private static Image InitiativeBild = Image.FromFile("./Ressourcen/Initiative.png");
        //private static Image ReichweiteBild = Image.FromFile("./Ressourcen/Reichweite.png");
        //private static Image StorungBild = Image.FromFile("./Ressourcen/Storung.png");

        private Pen Rand = Pens.Black;
        CString Kompositum;
        Text Links;
        Text Rechts;

        public WolfHeader(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {

        }

        public override void update()
        {
        }

        public void Build()
        {
            xFont font = Karte.TitelDarstellung.FontMeasurer;
            float h = font.getZeilenabstand();

            Links = new Text(Karte.Fraktion.Schreibname + "\n" + Karte.Schreibname, font);
            Rechts = new Text("", font);
            Rechts.addWort(Karte.Initiative);
            Rechts.add(new WolfTextBild(Karte.LayoutDarstellung.Initiative, font));
            //Rechts.addZoomedImage(InitiativeBild);
            Rechts.addWhitespace(1);
            if (Karte.Felder > 0)
            {
                Rechts.addWort(Karte.Felder);
                Rechts.add(new WolfTextBild(Karte.LayoutDarstellung.Felder, font));
                //Rechts.addZoomedImage(FelderBild);
                Rechts.addWhitespace(1);
            }
            Rechts.addAbsatz();

            if (Karte.ReichweiteMax > 0)
            {
                if (Karte.ReichweiteMin == 0)
                    Rechts.addWort(Karte.ReichweiteMax + "m");
                else
                    Rechts.addWort(Karte.ReichweiteMin + "-" + Karte.ReichweiteMax + "m");
                Rechts.add(new WolfTextBild(Karte.LayoutDarstellung.Reichweite, font));
                //Rechts.addZoomedImage(ReichweiteBild);
                Rechts.addWhitespace(1);
            }
          
            if (Karte.Storung > 0)
            {
                Rechts.addWort(Karte.Storung);
                Rechts.add(new WolfTextBild(Karte.LayoutDarstellung.Storung, font));
                //Rechts.addZoomedImage(StorungBild);
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
