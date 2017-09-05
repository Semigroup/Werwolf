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
namespace Werwolf.Karten.CyberAktion
{
    public class DoppelTabTextBox : WolfBox
    {
        public LeftRightLine[] Lines { get; set; }

        public virtual Darstellung Darstellung => Karte.TextDarstellung;

        public DoppelTabTextBox(Karte Karte, float Ppm, int lines)
            : base(Karte, Ppm)
        {
            Lines = new LeftRightLine[lines];
            for (int i = 0; i < lines; i++)
                Lines[i] = new LeftRightLine();
        }
        /// <summary>
        /// line x {left = 0, right = 1}
        /// </summary>
        /// <param name="line"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public DrawBox this[int line, int column]
        {
            get => column == 0 ? Lines[line].LeftText : Lines[line].RightText;
            set
            {
                if (column == 0)
                    Lines[line].LeftText = value;
                else
                    Lines[line].RightText = value;
            }
        }
        public override void draw(DrawContext con)
        {
            base.draw(con);
            con.fillRectangle(Darstellung.Farbe.ToBrush(), box);
            foreach (var item in Lines)
                item.draw(con);
            con.drawRectangle(Darstellung.RandFarbe.ToPen(Darstellung.Rand.Width), box);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            foreach (var item in Lines)
                item.Move(ToMove);
        }
        public override void setup(RectangleF box)
        {
            this.box = new RectangleF(box.Location, new SizeF());
            PointF pointOfProgress = box.Location;
            foreach (var item in Lines)
            {
                item.setup(pointOfProgress, box.Width);
                pointOfProgress = pointOfProgress.add(0, item.box.Height);
            }
            this.box.Size = new SizeF(box.Width, pointOfProgress.Y - this.box.Y);
        }
        public override void update()
        {
            foreach (var item in Lines)
                item.update();
        }
        public override void OnKarteChanged()
        {
            if (Lines == null) return;
            base.OnKarteChanged();
        }
    }
}
