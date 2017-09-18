using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using Werwolf.Inhalt;
using Werwolf.Inhalt.Data;

namespace Werwolf.Forms.Data
{
    public partial class KonfliktFeld : UserControl
    {
        public KonfliktForm Form { get; private set; }
        public Konflikt Konflikt { get; private set; }
        public KonfliktFeld()
        {
            InitializeComponent();
        }

        public void SetKonflikt(KonfliktForm Form, Konflikt Konflikt)
        {
            this.Form = Form;
            this.Konflikt = Konflikt;
            pictureBoxD.ImageLocation = Konflikt.DestinyFile;
            pictureBoxS.ImageLocation = Konflikt.SourceFile;
            FileInfo fileInfo = new FileInfo(Konflikt.DestinyFile);
            labelD.Text = "Zieladresse:\r\n"
                + fileInfo.FullName
                + "\r\nZuletzt geändert am:" + fileInfo.LastWriteTime
                + "\r\nGröße: " + fileInfo.Length + " Bytes";

            fileInfo = new FileInfo(Konflikt.SourceFile);
            labelS.Text = "Quelladresse:\r\n"
                + fileInfo.FullName
                  + "\r\nZuletzt geändert am:" + fileInfo.LastWriteTime
                  + "\r\nGröße: " + fileInfo.Length + " Bytes";

            //labelExplain.Text = "Das Bild rechts von der Adresse "
            //    + Konflikt.SourceFile + " soll zur Adresse "
            //    + Konflikt.DestinyFile + " kopiert werden, wo sich aber bereits das linke Bild befindet.";
        }

        private void buttonD_Click(object sender, EventArgs e)
        {
            Konflikt.LosungArt = Konflikt.Losung.NichtErsetzen;
            Form.RemoveKonfliktFeld(this, checkBox1.Checked);
        }
        private void buttonDS_Click(object sender, EventArgs e)
        {
            Konflikt.LosungArt = Konflikt.Losung.Umbennen;
            Form.RemoveKonfliktFeld(this, checkBox1.Checked);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Konflikt.LosungArt = Konflikt.Losung.Ersetzen;
            Form.RemoveKonfliktFeld(this, checkBox1.Checked);
        }
    }
}
