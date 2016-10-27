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
    public class WonderInfos : WolfBox
    {
        public WonderNamenFeld Name;
        public WonderKostenFeld Kosten;
        public WonderTextFeld[] Basen;
        public WonderTextFeld[] Entwicklungen;

        private bool Initialized = false;

        private IEnumerable<WonderTextFeld> Felder
        {
            get
            {
                List<WonderTextFeld> l = new List<WonderTextFeld>();
                if (Name != null)
                    l.Add(Name);
                if (Kosten != null)
                    l.Add(Kosten);
                IEnumerable<WonderTextFeld> e = l;
                if (Basen != null)
                    e = e.Concat(Basen);
                if (Entwicklungen != null)
                    e = e.Concat(Entwicklungen);
                return e;
            }
        }

        public WonderInfos(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }

        public override void OnKarteChanged()
        {
            base.OnKarteChanged();
            if (Karte != null)
                if (!Initialized)
                {
                    Initialized = true;
                    Name = new WonderNamenFeld(Karte, Ppm);
                    Kosten = new WonderKostenFeld(Karte, Ppm);
                }
                else
                {
                    Name.Karte = Karte;
                    Name.Ppm = Ppm;
                    Name.Text = Karte.Schreibname;

                    Kosten.Karte = Karte;
                    Kosten.Ppm = Ppm;
                    Kosten.Text = Karte.Kosten.ToString();
                }
        }

        public override void update()
        {
        }

        public override bool Visible()
        {
            return base.Visible();
        }

        public override void setup(RectangleF box)
        {
            RectangleF MovedInnenBox = InnenBox.move(box.Location);
            this.box = AussenBox;
            this.box.Location = box.Location;
            if (Name.Visible())
            {
                Name.setup(box);
                float rest = Faktor * HintergrundDarstellung.Anker.X - Name.Size.Width;
                Name.SetLot(new PointF(MovedInnenBox.Left + rest / 2, MovedInnenBox.Bottom));
            }
            if (Kosten.Visible())
            {
                Kosten.setup(box);
                float rest = Faktor * HintergrundDarstellung.Anker.X - Kosten.Size.Width;
                Kosten.SetLot(new PointF(MovedInnenBox.Left + rest / 2, MovedInnenBox.Top));
            }

        }

        public override void draw(DrawContext con)
        {
            foreach (var item in Felder)
                if (item.Visible())
                    item.draw(con);
        }
    }
}
