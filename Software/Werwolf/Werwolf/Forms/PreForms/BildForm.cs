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
    public class BildForm : PreForm<Bild>
    {
        private BallPointFBox ball;
        private ImageSelectBox image;

        public BildForm(Karte Karte)
            : base(Karte)
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
            ball.Image = image.Image;
            WerteListe.SetValue("Größe in mm", element.StandardSize(image.Image));
        }
        public override void UpdateWerteListe()
        {
            if (element == null)
                return;
            UpdatingWerteListe = true;
            this.Text = element.XmlName + " namens " + element.Schreibname + " bearbeiten...";
            WerteListe.SetValue("Name", element.Name);
            WerteListe.SetValue("Datei", element.TotalFilePath);
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
            element.Schreibname = element.Name = WerteListe.GetValue<string>("Name");
            element.FilePath = WerteListe.GetValue<string>("Datei");
            element.Artist = WerteListe.GetValue<string>("Artist");
            element.Size = WerteListe.GetValue<SizeF>("Größe in mm");
            element.Zentrum = WerteListe.GetValue<PointF>("Point of Interest");
        }
    }
}
