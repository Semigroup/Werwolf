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
using Designer.RahmenCreator;
using Assistment.Drawing;
using System.Drawing.Imaging;
using Assistment.Drawing.Style;
using Assistment.Drawing.Geometries;

using Assistment.Extensions;
using Assistment.Mathematik;
using Werwolf.Generating;

namespace Designer
{
    public class HintergrundTool : Form, ITool
    {
        private Button MWButton;
        private Button button1;
        private Button FragmentButton;

        public string ToolDescription => "Hintergrund-Designer";

        public Universe Universe { get; set; }

        public DialogResult EditUniverse(Universe Universe)
        {
            this.Universe = Universe;
            this.InitializeComponent();
            return this.ShowDialog();
        }

        private void InitializeComponent()
        {
            this.FragmentButton = new System.Windows.Forms.Button();
            this.MWButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // FragmentButton
            // 
            this.FragmentButton.Location = new System.Drawing.Point(12, 12);
            this.FragmentButton.Name = "FragmentButton";
            this.FragmentButton.Size = new System.Drawing.Size(128, 23);
            this.FragmentButton.TabIndex = 0;
            this.FragmentButton.Text = "Fragmente Testen";
            this.FragmentButton.UseVisualStyleBackColor = true;
            this.FragmentButton.Click += new System.EventHandler(this.FragmentButton_Click);
            // 
            // MWButton
            // 
            this.MWButton.Location = new System.Drawing.Point(12, 41);
            this.MWButton.Name = "MWButton";
            this.MWButton.Size = new System.Drawing.Size(128, 61);
            this.MWButton.TabIndex = 1;
            this.MWButton.Text = "ModernWolf Rahmen Erstellen";
            this.MWButton.UseVisualStyleBackColor = true;
            this.MWButton.Click += new System.EventHandler(this.ModernWolfButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(128, 33);
            this.button1.TabIndex = 2;
            this.button1.Text = "Hintergrund Erstellen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // HintergrundTool
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.MWButton);
            this.Controls.Add(this.FragmentButton);
            this.Name = "HintergrundTool";
            this.ResumeLayout(false);

        }

        private void FragmentButton_Click(object sender, EventArgs e)
        {
            new FragmentTest().Show();
        }

        private void ModernWolfButton_Click(object sender, EventArgs e)
        {
            MWRahmenCreator mwrc = new MWRahmenCreator();
            mwrc.Init(Universe);
            mwrc.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new HintergrundErstellerForm().Show();
        }
    }
}
