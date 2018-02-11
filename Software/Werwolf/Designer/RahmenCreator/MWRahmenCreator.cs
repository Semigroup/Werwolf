using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Werwolf;
using Werwolf.Inhalt;
using Werwolf.Forms;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Extensions;
using Assistment.Drawing;
using Assistment.Drawing.Geometries;
using Werwolf.Karten;

namespace Designer.RahmenCreator
{
    public partial class MWRahmenCreator : Form
    {
        public State State { get; set; }

        public Universe Universe { get; set; }
        public HintergrundBild HintergrundBild { get; set; }

        public Bitmap Bitmap { get; set; }
        public Graphics Graphics { get; set; }

        private HintergrundDarstellung HintergrundDarstellung
            => Universe.Karten.Standard.HintergrundDarstellung;

        public MWRahmenCreator()
        {
            InitializeComponent();
            State = new State();

            this.MarginBottomBox.UserValueChanged += Draw;
            this.MarginTopBox.UserValueChanged += Draw;
            this.MarginLeftBox.UserValueChanged += Draw;
            this.MarginRightBox.UserValueChanged += Draw;

            this.RadiusBox.UserValueChanged += Draw;

            this.FragmentDickeBox.UserValueChanged += Draw;
            this.FragmentNumberBox.UserValueChanged += Draw;
            this.enumBox1.UserValueChanged += Draw;

            this.ppmBox1.UserValueChanged += Draw;

            this.penBox1.PenChanged += Draw;

            this.SamplesBox.UserValueChanged += Draw;
            this.LeftInversion.CheckedChanged += Draw;
            this.RightInversion.CheckedChanged += Draw;
        }
        public void Init(Universe Universe)
        {
            this.Universe = Universe;
            this.HintergrundBild = Universe.HintergrundBilder.Standard;

            this.enumBox1.UserValue = Fragments.Style.Gerade;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ElementAuswahlForm<HintergrundBild> form = new ElementAuswahlForm<HintergrundBild>(
                Universe.Karten.Standard,
                Universe.HintergrundBilder,
                HintergrundBild,
                new ViewBild(),
                false);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                HintergrundBild = form.Element;
                Draw(sender, e);
            }
        }

        public void Draw(object sender, EventArgs e)
        {
            State.Size = Universe.HintergrundDarstellungen.Standard.Size.mul(ppmBox1.Ppm).ToSize();
            State.FragmentDicke = FragmentDickeBox.UserValue;
            State.FragmentStyle = (Fragments.Style)enumBox1.UserValue;
            State.FragmentZahl = FragmentNumberBox.UserValue;
            State.HintergrundBildName = HintergrundBild.Name;
            State.PPM = ppmBox1.Ppm;
            State.Radius = RadiusBox.UserValue;
            State.UniverseName = Universe.Name;
            State.HintergrundDarstellungName = HintergrundDarstellung.Name;
            State.MarginBottom = MarginBottomBox.UserValue;
            State.MarginLeft = MarginLeftBox.UserValue;
            State.MarginRight = MarginRightBox.UserValue;
            State.MarginTop = MarginTopBox.UserValue;
            State.Pen = penBox1.Pen;
            State.Samples = SamplesBox.UserValue;
            State.InvertLeft = LeftInversion.Checked;
            State.InvertRight = RightInversion.Checked;

            if (Bitmap == null || Bitmap.Size != State.Size)
            {
                Bitmap = new Bitmap(State.Size.Width, State.Size.Height);
                Graphics = Bitmap.GetHighGraphics(ppmBox1.Ppm);
                this.pictureBox1.Image = Bitmap;
            }
            Graphics.Clear(Color.FromArgb(0));

            if (HintergrundBild != null && HintergrundBild.Image != null)
            {
                RectangleF rect = HintergrundBild.Rectangle.div(WolfBox.Faktor);
                rect = rect.move(HintergrundDarstellung.Size.div(2).ToPointF());
                Graphics.DrawImage(HintergrundBild.Image, rect);
            }

            OrientierbarerWeg ow = GetWegMitRadius();
            PointF[] pts = ow.GetPolygon(State.Samples, 0, 1);

            Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
            Graphics.FillPolygon(new SolidBrush(Color.FromArgb(0)), pts);
            Graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            Graphics.DrawPolygon(State.Pen, pts);

            this.pictureBox1.Refresh();
        }

        public OrientierbarerWeg GetSingleFragment(bool Rechts, bool Invertiert, float fragBreite)
        {
            OrientierbarerWeg fragment = Fragments.GetFragment(State.FragmentStyle, fragBreite, State.FragmentDicke);
            if (Invertiert)
                fragment = fragment.Trim(1, 0).Spiegel(new Gerade(fragBreite / 2, 0, 0, 1));
            if (Rechts)
                fragment = fragment ^ (-Math.PI / 2);
            else
                fragment = fragment ^ (Math.PI / 2);
            return fragment;
        }

        public OrientierbarerWeg MakeFragmente(RectangleF box, bool Rechts, bool Invertiert, float fragBreite)
        {
            PointF startingPoint = Rechts ? new PointF(box.Right, box.Top) : new PointF(box.Left, box.Bottom);
            if (State.Radius > 0)
                startingPoint = startingPoint.add(0, Rechts ? State.Radius : -State.Radius);

            OrientierbarerWeg fragment = GetSingleFragment(Rechts, Invertiert, fragBreite);

            OrientierbarerWeg vieleFragmente = fragment;
            for (int i = 1; i < State.FragmentZahl; i++)
                vieleFragmente = vieleFragmente 
                    + (Fragments.IsRandom(State.FragmentStyle) ? GetSingleFragment(Rechts,Invertiert, fragBreite) : fragment);
            return vieleFragmente + startingPoint;
        }
        private OrientierbarerWeg GetWegMitRadius()
        {
            RectangleF box = new RectangleF(
                State.MarginLeft,
                State.MarginTop,
                HintergrundDarstellung.Size.Width - State.MarginLeft - State.MarginRight,
                HintergrundDarstellung.Size.Height - State.MarginTop - State.MarginBottom);
            box = box.Inner(HintergrundDarstellung.Rand);

            float fragBreite = (box.Height - 2 * State.Radius) / State.FragmentZahl;
            FragmentHeight.Text = "Fragment Höhe: " + fragBreite.ToString("G2");
           

            OrientierbarerWeg ow = null;
            if (State.Radius > 0)
            {
                OrientierbarerWeg kreis = OrientierbarerWeg.Kreisbogen(State.Radius, 0, 1);

                ow = kreis.Trim(0.75f, 1) + box.Location + new PointF(box.Width - State.Radius, State.Radius);

                ow = ow + MakeFragmente(box, true, State.InvertRight, fragBreite);

                ow = ow + kreis.Trim(0, 0.25f);
                ow = ow * (kreis.Trim(0.25f, 0.5f) + new PointF(box.Left + State.Radius, box.Bottom - State.Radius));

                ow = ow + MakeFragmente(box, false, State.InvertLeft, fragBreite);
                ow = ow * (kreis.Trim(0.5f, 0.75f) + box.Location + new PointF(State.Radius, State.Radius));
            }
            else
                ow = MakeFragmente(box, true, State.InvertRight, fragBreite) * MakeFragmente(box, false, State.InvertLeft, fragBreite);
            return ow;
        }
        //private OrientierbarerWeg GetWegMitRadius()
        //{
        //    RectangleF box = new RectangleF(
        //        State.MarginLeft,
        //        State.MarginTop,
        //        HintergrundDarstellung.Size.Width - State.MarginLeft - State.MarginRight,
        //        HintergrundDarstellung.Size.Height - State.MarginTop - State.MarginBottom);
        //    box = box.Inner(HintergrundDarstellung.Rand);

        //    float fragBreite = (box.Height - 2 * State.Radius) / State.FragmentZahl;
        //    FragmentHeight.Text = "Fragment Höhe: " + fragBreite.ToString("G2");
        //    OrientierbarerWeg fragment = Fragments.GetFragment(State.FragmentStyle, fragBreite, State.FragmentDicke);
        //    OrientierbarerWeg fragmentInvertiert = fragment.Trim(1, 0).Spiegel(new Gerade(fragBreite / 2, 0, 0, 1));
        //    fragment = fragment ^ (-Math.PI / 2);
        //    fragmentInvertiert = fragmentInvertiert ^ (-Math.PI / 2);

        //    OrientierbarerWeg fragmenteRechts, fragmenteLinks;
        //    if (State.InvertRight)
        //    {
        //        fragmenteRechts = fragment;
        //        for (int i = 1; i < State.FragmentZahl; i++)
        //            fragmenteRechts = fragmenteRechts + fragment;
        //    }
        //    else
        //    {
        //        fragmenteRechts = fragmentInvertiert;
        //        for (int i = 1; i < State.FragmentZahl; i++)
        //            fragmenteRechts = fragmenteRechts + fragmentInvertiert;
        //    }

        //    fragment = (fragment.Trim(1, 0)).Spiegel(new Gerade(fragment.Weg(1), new PointF(0, 1))) - new PointF(0, fragBreite);
        //    fragmentInvertiert = (fragmentInvertiert.Trim(1, 0)).Spiegel(new Gerade(fragmentInvertiert.Weg(1), new PointF(0, 1))) - new PointF(0, fragBreite);

        //    if (State.InvertLeft)
        //    {
        //        fragmenteLinks = fragment;
        //        for (int i = 1; i < State.FragmentZahl; i++)
        //            fragmenteLinks = fragmenteLinks + fragment;
        //    }
        //    else
        //    {
        //        fragmenteLinks = fragmentInvertiert;
        //        for (int i = 1; i < State.FragmentZahl; i++)
        //            fragmenteLinks = fragmenteLinks + fragmentInvertiert;
        //    }

        //    OrientierbarerWeg ow = null;
        //    if (State.Radius > 0)
        //    {
        //        OrientierbarerWeg kreis = OrientierbarerWeg.Kreisbogen(State.Radius, 0, 1);

        //        ow = kreis.Trim(0.5f, 0.75f) + box.Location + new PointF(State.Radius, State.Radius);
        //        ow = ow * (kreis.Trim(0.75f, 1) + box.Location + new PointF(box.Width - State.Radius, State.Radius));

        //        ow = ow + fragmenteRechts;

        //        ow = ow + kreis.Trim(0, 0.25f);
        //        ow = ow * (kreis.Trim(0.25f, 0.5f) + new PointF(box.Left + State.Radius, box.Bottom - State.Radius));

        //        ow = ow + fragmenteLinks;
        //    }
        //    else
        //    {
        //        fragmenteRechts = fragmenteRechts + new PointF(box.Right, box.Top);
        //        fragmenteLinks = fragmenteLinks + new PointF(box.Left, box.Bottom);
        //        ow = fragmenteRechts * fragmenteLinks;
        //    }
        //    return ow;
        //}
    }
}
