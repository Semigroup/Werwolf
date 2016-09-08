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
    //public class StandardTextBild : WolfBox
    //{
    //    private Text Text = new Text();
    //    public TextBild TextBild { get; set; }

    //    public StandardTextBild(Karte Karte, float ppm)
    //        : base(Karte, ppm)
    //    {

    //    }

    //    public override float getSpace()
    //    {
    //        return AussenBox.Size.Inhalt();
    //    }
    //    public override float getMin()
    //    {
    //        return AussenBox.Size.Width;
    //    }
    //    public override float getMax()
    //    {
    //        return getMin();
    //    }

    //    public override void OnKarteChanged()
    //    {
    //        base.OnKarteChanged();
    //        update();
    //    }

    //    public override void update()
    //    {
    //        Text = new Text();
    //        Text.alignment = 0f;
    //        if (Karte != null && TextBild != null)
    //        {
    //            Text.preferedFont = Karte.TextDarstellung.FontMeasurer;
    //            Text.addRegex("Text: " + Karte.TextDarstellung.Font.Name + ", " + Karte.TextDarstellung.Font.Size + "");
    //            Text.addAbsatz();
    //            Text.addRegex("this is evolution");
    //            Text.add(new WolfTextBild(TextBild, Karte, false));
    //            Text.addRegex(" the monkey");

    //            Text.addAbsatz();
    //            Text.addWort(" ");
    //            Text.addAbsatz();

    //            Text.preferedFont = Karte.InfoDarstellung.FontMeasurer;
    //            Text.addRegex("Info: " + Karte.InfoDarstellung.Font.Name + ", " + Karte.InfoDarstellung.Font.Size + "");
    //            Text.addAbsatz();
    //            Text.addRegex("the man");
    //            Text.add(new WolfTextBild(TextBild, Karte, false));
    //            Text.addRegex("and then the gun");
    //        }
    //        else if (TextBild != null)
    //        {
    //            Text.preferedFont = new FontMeasurer("Calibri", 12);
    //            Text.addRegex("Karte == null");
    //        }
    //        else
    //        {
    //            Text.preferedFont = new FontMeasurer("Calibri", 12);
    //            Text.addRegex("TextBild == null");
    //        }
    //    }
    //    public override void Move(PointF ToMove)
    //    {
    //        base.Move(ToMove);
    //        Text.Move(ToMove);
    //    }
    //    public override void setup(RectangleF box)
    //    {
    //        this.box = box;
    //        this.box.Size = AussenBox.Size;

    //        Text.setup(this.box);
    //    }
    //    public override void draw(DrawContext con)
    //    {
    //        RectangleF MovedAussenBox = AussenBox.move(box.Location);
    //        PointF MovedAussenBoxCenter = MovedAussenBox.Center();

    //        Text.draw(con);
    //    }
    //}
}
