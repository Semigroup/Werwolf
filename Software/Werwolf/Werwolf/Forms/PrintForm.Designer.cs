namespace Werwolf.Forms
{
    partial class PrintForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.DeckButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.Drucken = new System.Windows.Forms.Button();
            this.Printer = new System.ComponentModel.BackgroundWorker();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.ppmBox1 = new Assistment.form.PpmBox();
            this.colorBox2 = new Assistment.form.ColorBox();
            this.colorBox1 = new Assistment.form.ColorBox();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(852, 59);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(186, 21);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Eine Rückseite pro Karte";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(852, 140);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(277, 21);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Eine gemeinsame Rückseite pro Papier";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Visible = false;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(852, 86);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(139, 21);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Keine Rückseiten";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.Control;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(830, 814);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // DeckButton
            // 
            this.DeckButton.Location = new System.Drawing.Point(852, 12);
            this.DeckButton.Name = "DeckButton";
            this.DeckButton.Size = new System.Drawing.Size(139, 41);
            this.DeckButton.TabIndex = 4;
            this.DeckButton.Text = "Deck Auswählen";
            this.DeckButton.UseVisualStyleBackColor = true;
            this.DeckButton.Click += new System.EventHandler(this.DeckButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(997, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Ausgewähltes Deck";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(849, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Hintergrundfarbe";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(849, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "Farbe Trennlinien";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(852, 113);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(126, 21);
            this.radioButton4.TabIndex = 10;
            this.radioButton4.Text = "Nur Rückseiten";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // Drucken
            // 
            this.Drucken.Location = new System.Drawing.Point(1035, 469);
            this.Drucken.Name = "Drucken";
            this.Drucken.Size = new System.Drawing.Size(152, 43);
            this.Drucken.TabIndex = 11;
            this.Drucken.Text = "PDF erstellen";
            this.Drucken.UseVisualStyleBackColor = true;
            this.Drucken.Click += new System.EventHandler(this.Drucken_Click);
            // 
            // Printer
            // 
            this.Printer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Printer_DoWork);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(852, 440);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(335, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 16;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(852, 309);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(176, 21);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Platz zwischen Karten?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "pdf";
            this.saveFileDialog1.Filter = "PDF (*.pdf)|*.pdf";
            // 
            // ppmBox1
            // 
            this.ppmBox1.Location = new System.Drawing.Point(852, 363);
            this.ppmBox1.Name = "ppmBox1";
            this.ppmBox1.Ppm = 11.81102F;
            this.ppmBox1.PpmMaximum = 1000F;
            this.ppmBox1.PpmMinimum = 0.001F;
            this.ppmBox1.Size = new System.Drawing.Size(177, 44);
            this.ppmBox1.TabIndex = 15;
            // 
            // colorBox2
            // 
            this.colorBox2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorBox2.Location = new System.Drawing.Point(971, 226);
            this.colorBox2.Name = "colorBox2";
            this.colorBox2.Size = new System.Drawing.Size(217, 23);
            this.colorBox2.TabIndex = 8;
            // 
            // colorBox1
            // 
            this.colorBox1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorBox1.Location = new System.Drawing.Point(971, 197);
            this.colorBox1.Name = "colorBox1";
            this.colorBox1.Size = new System.Drawing.Size(217, 23);
            this.colorBox1.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(853, 469);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(176, 43);
            this.button1.TabIndex = 18;
            this.button1.Text = "Vorschau: Erste Seite";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(852, 336);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(91, 21);
            this.checkBox2.TabIndex = 19;
            this.checkBox2.Text = "FixedFont";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(852, 282);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(191, 21);
            this.checkBox3.TabIndex = 20;
            this.checkBox3.Text = "Trennlinie auf Rückseite?";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(852, 255);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(202, 21);
            this.checkBox4.TabIndex = 21;
            this.checkBox4.Text = "Trennlinie auf Vorderseite?";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 814);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ppmBox1);
            this.Controls.Add(this.Drucken);
            this.Controls.Add(this.radioButton4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.colorBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.colorBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DeckButton);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.radioButton3);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Name = "PrintForm";
            this.Text = "PrintForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button DeckButton;
        private System.Windows.Forms.Label label1;
        private Assistment.form.ColorBox colorBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private Assistment.form.ColorBox colorBox2;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Button Drucken;
        private Assistment.form.PpmBox ppmBox1;
        private System.ComponentModel.BackgroundWorker Printer;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
    }
}