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

        public BildForm(Karte Karte, ViewBox ViewBox)
            : base(Karte, ViewBox)
        {
        }

        public override void BuildWerteListe()
        {
            UpdatingWerteListe = true;

            image = new ImageSelectBox();
            ball = new BallPointFBox();

            WerteListe.AddStringBox("", "Name");
            image.ShowImage = false;
            image.ImageChanged += new EventHandler(image_ImageChanged);
            image.MaximumImageSize = Settings.MaximumImageArea;
            WerteListe.AddWertePaar<string>(image, "", "Datei");
            WerteListe.AddStringBox("", "Artist");
            WerteListe.AddChainedSizeFBox(new SizeF(1, 1), "Größe in mm", true);
            WerteListe.AddWertePaar<PointF>(ball, new PointF(), "Point of Interest");

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }

        void image_ImageChanged(object sender, EventArgs e)
        {
            if (UpdatingWerteListe)
                return;
            ball.SetImage(image.ImagePath);
            WerteListe.SetValue("Größe in mm", element.StandardSize(ball.Image));
        }
        public override void UpdateWerteListe()
        {
            if (element == null)
                return;
            UpdatingWerteListe = true;
            this.Text = element.XmlName + " namens " + element.Schreibname + " bearbeiten...";
            WerteListe.SetValue("Name", element.Schreibname);
            WerteListe.SetValue("Datei", element.HiddenFilePath);
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
            element.Schreibname = WerteListe.GetValue<string>("Name");
            string s = WerteListe.GetValue<string>("Datei");
            element.FilePath = (s == null) ? "" : s;
            element.Artist = WerteListe.GetValue<string>("Artist");
            element.Size = WerteListe.GetValue<SizeF>("Größe in mm");
            element.Zentrum = WerteListe.GetValue<PointF>("Point of Interest");
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

            UpdatingWerteListe = false;
            WerteListe.Setup();
        }


        public override void UpdateWerteListe()
        {
            if (element == null)
                return;
            UpdatingWerteListe = true;
            this.Text = element.XmlName + " namens " + element.Name + " bearbeiten...";
            WerteListe.SetValue("Name", element.Name);
            WerteListe.SetValue("Datei", element.HiddenFilePath);
            WerteListe.SetValue("Artist", element.Artist);
            UpdatingWerteListe = false;
        }
        public override void UpdateElement()
        {
            if (element == null || UpdatingWerteListe)
                return;
            element.Name = WerteListe.GetValue<string>("Name");
            element.FilePath = WerteListe.GetValue<string>("Datei");
            element.Artist = WerteListe.GetValue<string>("Artist");
        }
    }
}
