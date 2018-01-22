using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Assistment.Extensions;
using Werwolf.Inhalt;
using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;

namespace Werwolf.Karten
{
    public class WonderDoppelName : WolfBox
    {
        public WonderNamenFeld Name1, Name2;

        public float EntwicklungsBreite { get; set; }

        private IEnumerable<WonderTextFeld> Felder
        {
            get
            {
                List<WonderTextFeld> l = new List<WonderTextFeld>();
                if (Name1 != null)
                    l.Add(Name1);
                if (Name2 != null)
                    l.Add(Name2);
                return l;
            }
        }

        public WonderDoppelName(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte == null)
                return;

            xFont font = Karte.TitelDarstellung.FontMeasurer;

            if (Karte.Entwicklungen.Length > 0)
            {
                Name1 = new WonderNamenFeld(Karte.Entwicklungen[0], true,true, ppm);
                Name1.Font = font;
                Name1.UpdateDrawBox();
            }
            else
                Name1 = null;
            if (Karte.Entwicklungen.Length > 1)
            {
                Name2 = new WonderNamenFeld(Karte.Entwicklungen[1],  ppm);
                Name2.Font = font;
                Name2.UpdateDrawBox();
            }
            else
                Name2 = null;
        }
        public override void OnPpmChanged()
        {
            base.OnPpmChanged();
            foreach (var item in Felder)
                item.Ppm = ppm;
        }

        public override void Update()
        {
        }

        public override bool Visible()
        {
            return base.Visible();
        }

        public override void Setup(RectangleF box)
        {
            RectangleF MovedInnenBox = InnenBox.move(box.Location);
            this.Box = AussenBox;
            this.Box.Location = box.Location;
            if (Name1 != null && Name1.Visible())
            {
                Name1.Setup(box);
                float rest = Faktor * HintergrundDarstellung.Anker.X - Name1.Size.Width;
                Name1.SetLot(new PointF(MovedInnenBox.Left + rest / 2, MovedInnenBox.Top));
            }
            if (Name2 != null && Name2.Visible())
            {
                Name2.Setup(box);
                float rest = Faktor * HintergrundDarstellung.Anker.X - Name2.Size.Width;
                Name2.SetLot(new PointF(MovedInnenBox.Right - Name2.Size.Width - rest / 2, MovedInnenBox.Bottom));
            }
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            foreach (var item in Felder)
                if (item != null)
                    item.Move(ToMove);
        }

        public override void Draw(DrawContext con)
        {
            foreach (var item in Felder)
                if (item != null && item.Visible())
                    item.Draw(con);
        }
    }
}
