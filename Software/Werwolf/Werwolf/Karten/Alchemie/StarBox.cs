using System;
using Assistment.Texts;
using System.Drawing;
using Assistment.Drawing.Geometries.Extensions;

namespace Werwolf.Karten.Alchemie
{
    public class StarBox : WrappingBox
    {
        public DrawBox ForeDrawBox;
        public DrawBox[] BackDrawBoxs;

        public Brush ForeColor;
        public Brush BackColor;
        public PointF[] OffSets;

        /// <summary>
        /// OffSet in DrawBox Koordinaten, also mm * Faktor
        /// </summary>
        /// <param name="DrawBox"></param>
        /// <param name="ForeColor"></param>
        /// <param name="BackColor"></param>
        /// <param name="OffSet"></param>
        public StarBox(DrawBox DrawBox, Brush ForeColor, Brush BackColor, PointF OffSet, int stars) : base(DrawBox)
        {
            this.ForeColor = ForeColor;
            this.BackColor = BackColor;
            this.OffSets = new PointF[stars];
            for (int i = 0; i < stars; i++)
                this.OffSets[i] = OffSet.rot(i * 2 * Math.PI / stars);
            this.BackDrawBoxs = new DrawBox[stars];
            this.Update();
        }

        public override void Update()
        {
            base.Update();
            this.ForeDrawBox = DrawBox.Clone();
            this.ForeDrawBox.ForceWordStyle(ForeColor);
            for (int i = 0; i < BackDrawBoxs.Length; i++)
            {
                this.BackDrawBoxs[i] = DrawBox.Clone();
                this.BackDrawBoxs[i].ForceWordStyle(BackColor);
            }
        }

        public override DrawBox Clone()
            =>new StarBox(DrawBox, ForeColor, BackColor, OffSets[0], BackDrawBoxs.Length);

        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            ForeDrawBox.Move(ToMove);
            foreach (var item in BackDrawBoxs)
                item.Move(ToMove);
        }
        public override void Setup(RectangleF box)
        {
            ForeDrawBox.Setup(box);
            for (int i = 0; i < BackDrawBoxs.Length; i++)
                this.BackDrawBoxs[i].Setup(box.move(OffSets[i]));
        }
        public override void Draw(DrawContext con)
        {
            for (int i = 0; i < BackDrawBoxs.Length; i++)
                this.BackDrawBoxs[i].Draw(con);
            ForeDrawBox.Draw(con);
        }
    }
}
