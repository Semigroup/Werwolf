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
        public WonderTextFeld Name;
        public WonderTextFeld Kosten;
        public WonderTextFeld[] Basen;
        public WonderTextFeld[] Entwicklungen;

        private bool Initialized = false;

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
                    TextBild tb = Karte.Universe.TextBilder["KleinesNamenfeld"];
                    Name = new WonderTextFeld(Karte, Ppm, false, Karte.Universe.TextBilder["GroßesNamenfeld"], Karte.Schreibname);
                }
                else
                {
                    Name.Karte = Karte;
                    Name.Ppm = Ppm;
                    Name.Text = Karte.Schreibname;
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
                Name.SetLot(new PointF(MovedInnenBox.Left + 3 * Faktor, MovedInnenBox.Bottom));
            }

        }

        public override void draw(DrawContext con)
        {
            if (Name.Visible())
                Name.draw(con);
        }
    }
}
