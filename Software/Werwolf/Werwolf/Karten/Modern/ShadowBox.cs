using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Texts;
using System.Drawing;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten.Modern
{
    public class ShadowBox : WrappingBox
    {
        public DrawBox ForeDrawBox;
        public DrawBox BackDrawBox;

        public Brush ForeColor;
        public Brush BackColor;
        public PointF OffSet;

        /// <summary>
        /// OffSet in DrawBox Koordinaten, also mm * Faktor
        /// </summary>
        /// <param name="DrawBox"></param>
        /// <param name="ForeColor"></param>
        /// <param name="BackColor"></param>
        /// <param name="OffSet"></param>
        public ShadowBox(DrawBox DrawBox, Brush ForeColor, Brush BackColor, PointF OffSet) : base(DrawBox)
        {
            this.ForeColor = ForeColor;
            this.BackColor = BackColor;
            this.OffSet = OffSet;
            this.Update();
        }

        public override void Update()
        {
            base.Update();
            this.ForeDrawBox = DrawBox.Clone();
            this.BackDrawBox = DrawBox.Clone();
            this.ForeDrawBox.ForceWordStyle(ForeColor);
            this.BackDrawBox.ForceWordStyle(BackColor);
        }

        public override DrawBox Clone()
        {
            return new ShadowBox(DrawBox, ForeColor, BackColor, OffSet);
        }

        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            ForeDrawBox.Move(ToMove);
            BackDrawBox.Move(ToMove);
        }
        public override void Setup(RectangleF box)
        {
            ForeDrawBox.Setup(box);
            BackDrawBox.Setup(box.move(OffSet));
        }
        public override void Draw(DrawContext con)
        {
            BackDrawBox.Draw(con);
            ForeDrawBox.Draw(con);
        }
    }
}
