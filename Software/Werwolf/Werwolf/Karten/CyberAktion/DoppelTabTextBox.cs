
using System.Drawing;
using Assistment.Texts;
using Assistment.Drawing.Geometries.Extensions;
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
        public override void Draw(DrawContext con)
        {
            base.Draw(con);
            con.FillRectangle(Darstellung.Farbe.ToBrush(), Box);
            foreach (var item in Lines)
                item.Draw(con);
            con.DrawRectangle(Darstellung.RandFarbe.ToPen(Darstellung.Rand.Width), Box);
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            foreach (var item in Lines)
                item.Move(ToMove);
        }
        public override void Setup(RectangleF box)
        {
            this.Box = new RectangleF(box.Location, new SizeF());
            PointF pointOfProgress = box.Location;
            foreach (var item in Lines)
            {
                item.Setup(pointOfProgress, box.Width);
                pointOfProgress = pointOfProgress.add(0, item.Box.Height);
            }
            this.Box.Size = new SizeF(box.Width, pointOfProgress.Y - this.Box.Y);
        }
        public override void Update()
        {
            foreach (var item in Lines)
                item.Update();
        }
        public override void OnKarteChanged()
        {
            if (Lines == null) return;
            base.OnKarteChanged();
        }
    }
}
