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
        public WolfTitel Titel { get; set; }
        public WolfText Text { get; set; }
        public WolfInfo Info { get; set; }

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

        public StandardKarte(Karte Karte)
            : base(Karte)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            update();
        }

        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            update();
        }

        public override void update()
        {
            this.Titel = new WolfTitel(Karte);
            this.Text = new WolfText(Karte);
            this.Info = new WolfInfo(Karte);
        }
        public override void setup(RectangleF box)
        {
            this.box = box;
            this.box.Size = AussenBox.Size;

            Titel.setup(box);
            Text.setup(box);
            Info.setup(box);
        }
        public override void draw(DrawContext con)
        {
        }
    }
}
