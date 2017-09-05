using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Werwolf.Inhalt;

using Assistment.Texts;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;
using Assistment.Drawing.Geometries;
using Assistment.Drawing;

namespace Werwolf.Karten.CyberAktion
{
    public class SimpleMeineAufgabenBox : WolfBox
    {
        public virtual Aufgabe Aufgabe => Karte.MeineAufgaben;
        public virtual Darstellung Darstellung => Karte.TextDarstellung;
        public virtual float InnenRadius => Darstellung is TextDarstellung ? (Darstellung as TextDarstellung).InnenRadius :  0;

        private Bitmap backGroundImage;
        private string LastAufgabe;
        private float LastPpm;
        private Color LastRandFarbe;
        private SizeF LastRand;
        private Color LastFarbe;
        private Font LastFont;
        private float LastInnRad;

        private DrawBox[] boxs;
        private bool changed;

        public SimpleMeineAufgabenBox(Karte Karte, float Ppm)
            : base(Karte, Ppm)
        {
        }

        public override void setup(RectangleF box)
        {
            if (Karte == null) return;

            string aufgabe = Aufgabe.ToString();
            if (boxs != null
                && aufgabe.Equals(LastAufgabe)
                && Darstellung.Rand.Equal(LastRand)
                && box.Equals(this.box)
                && Darstellung.Font.Equals(LastFont)
                && InnenRadius.Equals(LastInnRad)) return;

            this.box = box;
            LastAufgabe = aufgabe;
            LastRand = Darstellung.Rand;
            LastFont = Darstellung.Font;
            LastInnRad = InnenRadius;
            changed = true;

            SizeF rand = Darstellung.Rand.mul(Faktor);
            RectangleF subBox = box.Inner(rand);
             boxs = Aufgabe.ProduceTexts(Darstellung.FontMeasurer).Map(x => x.Geometry(InnenRadius * Faktor)).ToArray();
            for (int i = 0; i < boxs.Length; i++)
            {
                boxs[i].setup(subBox);
                subBox = subBox.move(0, boxs[i].box.Height + rand.Height);
            }
            this.box.Height = subBox.Y - rand.Height - this.box.Y;
        }

        public override void update()
        {
        }
        public override void Move(PointF ToMove)
        {
            base.Move(ToMove);
            foreach (var item in boxs)
                item.Move(ToMove);
        }
        public override void draw(DrawContext con)
        {
            base.draw(con);
            con.drawImage(backGroundImage, box);
            foreach (var item in boxs)
                item.draw(con);
        }
        public override void DrawRessources()
        {
            base.DrawRessources();

            if (backGroundImage != null
                && Darstellung.Farbe.Equals(LastFarbe)
                && Ppm.Equals(LastPpm)
                && Darstellung.RandFarbe.Equals(LastRandFarbe)
                && !changed) return;

            LastFarbe = Darstellung.Farbe;
            LastPpm = ppm;
            LastRandFarbe = Darstellung.RandFarbe;
            changed = false;

            Size Size = box.Size.mul(ppm / Faktor).Max(new SizeF(1, 1)).ToPointF().Ceil().ToSize().ToSize();
            backGroundImage = new Bitmap(Size.Width, Size.Height);
            Region region = new Region();
            region.MakeEmpty();
            foreach (var item in boxs)
            {
                RectangleF subBox = item.box.move(box.Location.mul(-1)).div(Faktor);
                subBox.Width = box.Width / Faktor - 2 * LastRand.Width;
                region.Union(subBox);
            }
            using (Graphics g = backGroundImage.GetHighGraphics(ppm))
            {
                g.Clear(LastFarbe);
                region.Complement(g.Clip);
                g.Clip = region;
                g.Clear(LastRandFarbe);
            }
        }
    }
}
