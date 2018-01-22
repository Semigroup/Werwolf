using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Werwolf.Forms;
using Werwolf.Karten;
using Werwolf.Inhalt;
using Assistment.Extensions;
using Assistment.Texts;

namespace Translation
{
    public partial class TranslatingTool : Form, ITool, IComparer<Karte>
    {
        public struct Tupel : IComparable<Tupel>
        {
            public Karte Karte;
            public string Aufgabe;

            public Tupel(Karte Karte)
            {
                this.Karte = Karte;
                this.Aufgabe = Karte.Aufgaben.ToString();
            }

            public int CompareTo(Tupel other)
            {
                int d = Aufgabe.CompareTo(other.Aufgabe);
                if (d != 0) return d;
                return Karte.Name.CompareTo(other.Karte.Name);
            }
        }

        private Karte[] karten;
        private int index;
        private Universe universe;

        public TranslatingTool()
        {
            InitializeComponent();
            checkBox1.PreviewKeyDown += CheckBox1_PreviewKeyDown;
            textBox1.PreviewKeyDown += TextBox1_PreviewKeyDown;
            textBox4.PreviewKeyDown += TextBox4_PreviewKeyDown;
        }

        private void TextBox4_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                checkBox1.Checked = true;
            }
        }

        private void TextBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Shift)
            {
                PrevKarte();
                textBox2.Focus();
            }
        }

        private void CheckBox1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                checkBox1.Checked = !checkBox1.Checked;
            else if (e.KeyCode == Keys.Tab && !e.Shift)
            {
                if (checkBox1.Checked)
                    ApplyChanges();
                NextKarte();
                checkBox2.Focus();
            }
        }

        public string ToolDescription => "Übersetzen";

        private void SetIndex(int value)
        {
            index = value;
            prevButton.Enabled = index > 0;
            nextButton.Enabled = index < karten.Length - 1;
            //textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
            checkBox1.Checked = false;
            this.pictureBox1.Image = karten[index].GetImageByHeight(pictureBox1.Height, true);
            MakeLegende();
            InfoLabel.Text = "Kartennumer: " + (index + 1) + " / " + karten.Length;
            InfoLabel.Text += "\r\n" + karten.Count(x => x.Translatiert) + " Karten von " + karten.Length + " übersetzt";
            InfoLabel.Text += "\r\nWahrer Name: " + karten[index].Name;
            InfoLabel.Text += "\r\n" + karten[index].HintergrundDarstellung.Schreibname
                 + ", " + karten[index].Fraktion.Schreibname;
        }

        public DialogResult EditUniverse(Universe universe)
        {
            this.universe = universe;
            List<Tupel> list = new List<Tupel>(universe.Karten.Values.Map(x => new Tupel(x)));
            list.Sort();
            karten = list.Map(x => x.Karte).ToArray();
            if (karten.Length == 0)
            {
                MessageBox.Show("Dieses Spiel enthält keine Karten!");
                return DialogResult.Abort;
            }
            SetIndex(0);
            return this.ShowDialog();
        }
        private void NextKarte()
        {
            if (index >= karten.Length - 1) return;
            int value = index + 1;
            if (checkBox2.Checked)
                while (karten[value].Translatiert && value < karten.Length - 1)
                    value++;
            SetIndex(value);
        }
        private void PrevKarte()
        {
            if (index <= 0) return;
            int value = index - 1;
            if (checkBox2.Checked)
                while (karten[value].Translatiert && value > 0)
                    value--;
            SetIndex(value);
        }
        private void ApplyChanges()
        {
            List<string> lines = karten[index].Aufgaben.GetLines();
            string beschreibung = lines.Count > 0 ? lines[0] : "";
            string anekdote = lines.Count > 1 ? lines[1] : "";

            if (textBox1.Text.Length > 0)
                karten[index].Schreibname = textBox1.Text;
            string aufgabe;
            if (textBox2.Text.Length > 0)
                aufgabe = textBox2.Text + "\\+";
            else
                aufgabe = beschreibung + "\\+";
            if (textBox3.Text.Length > 0 || textBox4.Text.Length > 0)
                aufgabe += "\\i„" + textBox3.Text + "“" + "\r\n" + "—" + textBox4.Text;
            else
                aufgabe += anekdote;
            karten[index].Aufgaben = new Aufgabe(aufgabe, universe);
            karten[index].Translatiert = checkBox1.Checked;
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            NextKarte();
            textBox1.Focus();
        }
        private void PrevButton_Click(object sender, EventArgs e)
        {
            PrevKarte();
            textBox1.Focus();
        }
        public int Compare(Karte x, Karte y)
        {
            int a = x.HintergrundDarstellung.Name.CompareTo(y.HintergrundDarstellung.Name);
            if (a > 0) return 1;
            else if (a < 0) return -1;
            else a = x.Fraktion.Name.CompareTo(y.Fraktion.Name);
            if (a > 0) return 1;
            else if (a < 0) return -1;
            else return x.Name.CompareTo(y.Name);
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            ApplyChanges();
            this.pictureBox1.Image = karten[index].GetImageByHeight(pictureBox1.Height, true);
        }

        private void MakeLegende()
        {
            Text t = new Text();
            t.PreferedFont = new FontGraphicsMeasurer(new Font("Calibri", 11));
            foreach (var item in karten[index].Aufgaben.GetTextBilder())
            {
                t.Add(new WolfTextBild(item, t.PreferedFont));
                t.AddWort(" ::" + item.Name + "::");
                t.AddAbsatz();
            }
            Image img = new Bitmap(legendenBox.Width, legendenBox.Height);
            using (Graphics g = img.GetHighGraphics())
            using (DrawContext dc = new DrawContextGraphics(g))
            {
                g.Clear(Color.White);
                t.Setup(legendenBox.Width);
                t.Draw(dc);
            }
            legendenBox.Image = img;
        }
    }
}
