using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Assistment.Drawing.LinearAlgebra;
using Assistment.Texts;
using Assistment.form;
using Assistment.Extensions;

using Werwolf.Inhalt;
using Werwolf.Karten;

namespace Werwolf.Forms
{
    public class BildForm<T> : PreForm<T> where T : Bild, new()
    {
        protected BallPointFBox ball;
        protected ImageSelectBox image;
        protected ArtLibrarySelectionButton libImage;
        protected Point MouseDownPoint;

        protected Timer timer;

        public BildForm(Karte Karte, ViewBox ViewBox)
            : base(Karte, ViewBox)
        {
        }

        public override void BuildWerteListe()
        {
            UpdatingWerteListe = true;

            image = new ImageSelectBox();
            libImage = new ArtLibrarySelectionButton();
            ball = new BallPointFBox();

            WerteListe.AddStringBox("", "Name");

            image.ShowImage = false;
            image.ImageChanged += new EventHandler(image_ImageChanged);
            image.MaximumImageSize = Settings.MaximumImageArea;
            WerteListe.AddWertePaar<string>(image, "", "Datei");

            libImage.ImageChanged += LibImage_ImageChanged;
            WerteListe.AddWertePaar<string>(libImage, "", "Lib-Datei");

            WerteListe.AddStringBox("", "Artist");
            WerteListe.AddChainedSizeFBox(new SizeF(1, 1), "Größe in mm", true);
            WerteListe.AddWertePaar<PointF>(ball, new PointF(), "Point of Interest");

            timer = new Timer();
            timer.Interval = 1000;
            timer.Enabled = false;
            timer.Tick += Timer_Tick;
            ViewBox.MouseDown += ViewBox_MouseDown;
            ViewBox.MouseUp += ViewBox_MouseUp;

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }

  

        private void ViewBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                timer.Enabled = false;
        }
        private void ViewBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.MouseDownPoint = Cursor.Position;
                timer.Enabled = true;
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            Point newPos = Cursor.Position;
            PointF center =  newPos.sub(MouseDownPoint);
            center = center.div(ViewBox.GetCurrentPictureSize());
            center = ball.GetValue().sub(center);
            ball.SetValue(center);
            this.MouseDownPoint = newPos;
        }



        public virtual void image_ImageChanged(object sender, EventArgs e)
        {
            if (UpdatingWerteListe)
                return;
            ball.SetImage(image.ImagePath);
            WerteListe.SetValue("Größe in mm", element.StandardSize(ball.Image));
        }
        private void LibImage_ImageChanged(object sender, EventArgs e)
        {
            if (UpdatingWerteListe)
                return;
            image.ImagePath = libImage.ImagePath;
            ball.SetImage(libImage.ImagePath);
            WerteListe.SetValue("Größe in mm", element.StandardSize(ball.Image));
        }
        public override void UpdateWerteListe()
        {
            if (element == null)
                return;
            UpdatingWerteListe = true;
            this.Text = element.XmlName + " namens " + element.Schreibname + " bearbeiten...";
            WerteListe.SetValue("Name", element.Schreibname);
            WerteListe.SetValue("Datei", element.TotalFilePath);
            WerteListe.SetValue("Lib-Datei", element.TotalFilePath);
            WerteListe.SetValue("Artist", element.Artist);
            WerteListe.SetValue("Größe in mm", element.Size);
            WerteListe.SetValue("Point of Interest", element.Zentrum);
            ball.Image = element.Image;
            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            if (element == null || UpdatingWerteListe)
                return;
            element.Name = element.Schreibname = WerteListe.GetValue<string>("Name");
            element.SetFilePath(WerteListe.GetString("Datei"));
            element.Artist = WerteListe.GetValue<string>("Artist");
            element.Size = WerteListe.GetValue<SizeF>("Größe in mm");
            element.Zentrum = WerteListe.GetValue<PointF>("Point of Interest");
            element.TryNewIdentifier = true;
        }
    }

    public class HauptBildForm : BildForm<HauptBild>
    {
        public HauptBildForm(Karte Karte)
            : base(Karte, new ViewKarte())
        {
        }
    }
    public class HintergrundBildForm : BildForm<HintergrundBild>
    {
        public HintergrundBildForm(Karte Karte)
            : base(Karte, new ViewKarte())
        {
        }
    }
    public class RuckseitenBildForm : BildForm<RuckseitenBild>
    {
        public RuckseitenBildForm(Karte Karte)
            : base(Karte, new ViewRuckseitenBild())
        {
        }
    }
    public class TextBildForm : BildForm<TextBild>
    {
        public TextBildForm(Karte Karte)
            : base(Karte, new ViewKarte())
        {
        }
        public override void BuildWerteListe()
        {
            UpdatingWerteListe = true;

            image = new ImageSelectBox();

            WerteListe.AddWerteBox<string>(
                new WertPaar<string>("Name", new TextBildNameBox(Universe.TextBilder)), "Name");
            image.ShowImage = false;
            image.MaximumImageSize = Settings.MaximumImageArea;
            WerteListe.AddWertePaar<string>(image, "", "Datei");
            WerteListe.AddStringBox("", "Artist");
            WerteListe.AddChainedSizeFBox(new SizeF(1, 1), "Größe in mm", true);

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }

        public override void image_ImageChanged(object sender, EventArgs e)
        {
            if (UpdatingWerteListe)
                return;
        }
        public override void UpdateWerteListe()
        {
            if (element == null)
                return;
            UpdatingWerteListe = true;
            this.Text = element.XmlName + " namens " + element.Name + " bearbeiten...";
            WerteListe.SetValue("Name", element.Name);
            WerteListe.SetValue("Datei", element.TotalFilePath);
            WerteListe.SetValue("Artist", element.Artist);
            WerteListe.SetValue("Größe in mm", element.Size);
            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            if (element == null || UpdatingWerteListe)
                return;
            element.Name = WerteListe.GetValue<string>("Name");
            element.SetFilePath(WerteListe.GetString("Datei"));
            element.Artist = WerteListe.GetValue<string>("Artist");
            element.Size = WerteListe.GetValue<SizeF>("Größe in mm");
        }
    }
}
