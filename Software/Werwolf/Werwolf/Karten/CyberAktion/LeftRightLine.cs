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

        public override DrawBox Clone()
        {
            LeftRightLine clone = new LeftRightLine();
            clone.LeftText = LeftText.Clone();
            clone.RightText = RightText.Clone();
            return clone;
        }
        public override void Draw(DrawContext con)
        {
            if (LeftText != null)
                LeftText.Draw(con);
            if (RightText != null)
                RightText.Draw(con);
        }
        public override float Max => LeftText.Max + RightText.Max;
        public override float Min => LeftText.Min + RightText.Min;
        public override float Space => LeftText.Space + RightText.Space;
        public override void Setup(RectangleF box)
        {
            this.Box = box;
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
                this.Box.Height = 0;
        }
        private void setupBoth()
        {
            float leftWidth = Box.Width * LeftText.Space/ this.Space;
            float rightWidth = Box.Width - leftWidth;
            float leftRest = leftWidth - LeftText.Min;
            float rightRest = rightWidth - RightText.Min;
            if (leftRest < 0)
            {
                if (rightRest < 0)
                    leftWidth = Box.Width * LeftText.Min/ this.Min;
                else
                    leftWidth = LeftText.Min;
            }
            else if (rightRest < 0)
                leftWidth = Box.Width - RightText.Min;
            rightWidth = Box.Width - leftWidth;

            LeftText.Setup(Box.Location, leftWidth);
            RightText.Setup(Box.Location, rightWidth);
            RightText.Move(Box.Right - RightText.Right, 0);
            this.Box.Height = Math.Max(RightText.Box.Height, LeftText.Box.Height);
        }
        private void setupSingle()
        {
            if (LeftText != null)
            {
                LeftText.Setup(Box);
                this.Box.Height = LeftText.Box.Height;
            }
            else
            {
                RightText.Setup(Box);
                RightText.Move(Box.Right - RightText.Right, 0);
                this.Box.Height = RightText.Box.Height;
            }
        }
        public override void Update()
        {
            if (LeftText != null)
                LeftText.Update();
            if (RightText != null)
                RightText.Update();
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
