﻿using System;
using System.Drawing;
using System.Windows.Forms;
using Werwolf.Inhalt;
using Werwolf.Forms;
using Assistment.Extensions;
using Assistment.Drawing.Geometries;
using Werwolf.Karten;
using System.Drawing.Drawing2D;
using Assistment.Drawing.Geometries.Extensions;

using System.Xml.Serialization;
using System.IO;

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

        private bool suppressKartenRand = false;

        private XmlSerializer XmlSerializer;

        public MWRahmenCreator()
        {
            InitializeComponent();
            State = new State();
            XmlSerializer = new XmlSerializer(typeof(State));

            this.SaveButton.Enabled = false;
            this.penBox1.Pen = new Pen(Color.Black, 0.3f);

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
            this.checkBox1.CheckedChanged += Draw;

            this.ShiftBox.UserValueChanged += Draw;

            this.TextBoxActive.CheckedChanged += Draw;
            this.TextLocBox.PointChanged += Draw;
            this.TextSizeBox.PointChanged += Draw;
            this.TextRadiusBox.UserValueChanged += Draw;
            this.TextBackColorBox.ColorChanged += Draw;
            this.TextShadowColorBox.ColorChanged += Draw;
            this.TextShadowOffsetBox.PointChanged += Draw;
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
            State.Size = HintergrundDarstellung.Size.mul(ppmBox1.Ppm).ToSize();
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
            State.PenWidth = penBox1.Pen.Width;
            State.PenColor = penBox1.Pen.Color;
            State.Samples = SamplesBox.UserValue;
            State.InvertLeft = LeftInversion.Checked;
            State.InvertRight = RightInversion.Checked;
            State.Shift = ShiftBox.UserValue;

            State.TextBoxActive = TextBoxActive.Checked;
            State.TextBox.Location = TextLocBox.UserPoint;
            State.TextBox.Size = TextSizeBox.UserSize;
            State.TextColor = TextBackColorBox.Color;
            State.TextShadowColor = TextShadowColorBox.Color;
            State.TextBoxShadowOffset = TextShadowOffsetBox.UserPoint;
            State.TextBoxRadius = TextRadiusBox.UserValue;

            LabelSize.Text = "Kartengröße: " + HintergrundDarstellung.Size.Width + " mm x " + HintergrundDarstellung.Size.Height + " mm";
            LabelRandSize.Text = "Kartenranddicke in mm: " + HintergrundDarstellung.Rand.Width + " mm x " + HintergrundDarstellung.Rand.Height + " mm";

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
            Graphics.DrawPolygon(penBox1.Pen, pts);

            if (State.TextBoxActive)
                MakeTextBox();

            if (checkBox1.Checked && !suppressKartenRand)
            {
                HintergrundDarstellung.MakeRandBild(State.PPM);
                Graphics.DrawImage(HintergrundDarstellung.RandBild, 0, 0, HintergrundDarstellung.Size.Width, HintergrundDarstellung.Size.Height);
            }

            this.pictureBox1.Refresh();
        }

        public PointF[] GetExtraShadow()
        {
            PointF v = State.TextBoxShadowOffset;
            PointF[] pts = new PointF[4];
            if (v.X * v.Y > 0)
            {
                pts[0] = new PointF(State.TextBox.Left, State.TextBox.Bottom);
                pts[1] = new PointF(State.TextBox.Right, State.TextBox.Top);
            }
            else
            {
                pts[0] = new PointF(State.TextBox.Left, State.TextBox.Top);
                pts[1] = new PointF(State.TextBox.Right, State.TextBox.Bottom);
            }
            if (State.TextBoxRadius > 0)
            {
                float redux = (float)(State.TextBoxRadius / Math.Sqrt(2));
                pts[0] = pts[0].add(redux, -redux);
                pts[1] = pts[1].add(-redux, redux);
            }
            pts[2] = pts[1].add(v);
            pts[3] = pts[0].add(v);

            return pts;
        }
        public void MakeTextBox()
        {
            OrientierbarerWeg weg = State.TextBoxRadius > 0 ?
                OrientierbarerWeg.RundesRechteck(State.TextBox, State.TextBoxRadius)
                : OrientierbarerWeg.Rechteck(State.TextBox);
            PointF[] pts = weg.GetPolygon(State.Samples, 0, 1);
            Graphics.FillPolygon(State.TextColor.ToBrush(), pts);
            GraphicsPath path = new GraphicsPath();
            path.AddPolygon(pts);
            Graphics.ExcludeClip(new Region(path));
            weg += State.TextBoxShadowOffset;
            pts = weg.GetPolygon(State.Samples, 0, 1);
            Graphics.FillPolygon(State.TextShadowColor.ToBrush(), pts);
            //weg -= State.TextBoxShadowOffset.mul(2);
            //pts = weg.GetPolygon(State.Samples, 0, 1);
            //Graphics.FillPolygon(Brushes.White, pts);
            //weg += new PointF(0, -State.TextBoxShadowOffset.Y);
            //pts = weg.GetPolygon(State.Samples, 0, 1);
            //Graphics.FillPolygon(State.TextShadowColor.ToBrush(), pts);
            //weg += new PointF(-State.TextBoxShadowOffset.X, State.TextBoxShadowOffset.Y);
            //pts = weg.GetPolygon(State.Samples, 0, 1);
            //Graphics.FillPolygon(State.TextShadowColor.ToBrush(), pts);
            Graphics.ResetClip();
        }
        public OrientierbarerWeg GetSingleFragment(bool Rechts, bool Invertiert, float fragBreite)
        {
            OrientierbarerWeg fragment = Fragments.GetFragment(State.FragmentStyle, fragBreite, State.FragmentDicke);
            if (0 < State.Shift && State.Shift < 1)
            {
                fragment = fragment.Trim(State.Shift, 1) + fragment.Trim(0, State.Shift);
                fragment.SetPosition(new PointF(0, fragment.Weg(0).Y));
            }
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
                    + (Fragments.IsRandom(State.FragmentStyle) ? GetSingleFragment(Rechts, Invertiert, fragBreite) : fragment);
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

        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.suppressKartenRand = true;
            Draw(sender, e);
            State.Name = textBox1.Text.ToFileName();
            string FilePath = Path.GetTempPath() + "\\" + State.Name + ".png";
            Bitmap.Save(FilePath);

            HintergrundBild bild = new HintergrundBild();
            bild.Init(Universe);
            bild.SetFilePath(FilePath);

            bild.SetAutoSize();
            bild.Name = bild.Schreibname = Path.GetFileNameWithoutExtension(State.Name);
            Universe.HintergrundBilder.AddPolymorph(bild);

            string directory = Universe.DirectoryName + "\\CreationData\\MWRahmenCreator\\";
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            XmlSerializer.Serialize(new FileStream(directory + "\\" + State.Name + ".xml", FileMode.Create), State);

            this.suppressKartenRand = false;

            Bitmap = null;
            Graphics = null;
            Draw(sender, e);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.SaveButton.Enabled = textBox1.Text.Length > 0;
        }
    }
}
