using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assistment.Texts;

namespace Werwolf.Karten.CyberAktion
{
    public class LeftRightLine : DrawBox
    {
        public DrawBox LeftText { get; set; }
        public DrawBox RightText { get; set; }

        public override DrawBox clone()
        {
            LeftRightLine clone = new LeftRightLine();
            clone.LeftText = LeftText.clone();
            clone.RightText = RightText.clone();
            return clone;
        }
        public override void draw(DrawContext con)
        {
            if (LeftText != null)
                LeftText.draw(con);
            if (RightText != null)
                RightText.draw(con);
        }
        public override float getMax()
        {
            return LeftText.getMax() + RightText.getMax();
        }
        public override float getMin()
        {
            return LeftText.getMin() + RightText.getMin();
        }
        public override float getSpace()
        {
            return LeftText.getSpace() + RightText.getSpace();
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            if (LeftText != null)
            {
                if (RightText != null)
                    setupBoth();
                else
                    setupSingle();
            }
            else if (RightText != null)
                setupSingle();
            else
                this.box.Height = 0;
        }
        private void setupBoth()
        {
            float leftWidth = box.Width * LeftText.getSpace() / this.getSpace();
            float rightWidth = box.Width - leftWidth;
            float leftRest = leftWidth - LeftText.getMin();
            float rightRest = rightWidth - RightText.getMin();
            if (leftRest < 0)
            {
                if (rightRest < 0)
                    leftWidth = box.Width * LeftText.getMin() / this.getMin();
                else
                    leftWidth = LeftText.getMin();
            }
            else if (rightRest < 0)
                leftWidth = box.Width - RightText.getMin();
            rightWidth = box.Width - leftWidth;

            LeftText.setup(box.Location, leftWidth);
            RightText.setup(box.Location, rightWidth);
            RightText.Move(box.Right - RightText.Right, 0);
            this.box.Height = Math.Max(RightText.box.Height, LeftText.box.Height);
        }
        private void setupSingle()
        {
            if (LeftText != null)
            {
                LeftText.setup(box);
                this.box.Height = LeftText.box.Height;
            }
            else
            {
                RightText.setup(box);
                RightText.Move(box.Right - RightText.Right, 0);
                this.box.Height = RightText.box.Height;
            }
        }
        public override void update()
        {
            if (LeftText != null)
                LeftText.update();
            if (RightText != null)
                RightText.update();
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            if (LeftText != null)
                LeftText.Move(ToMove);
            if (RightText != null)
                RightText.Move(ToMove);
        }
    }
}
