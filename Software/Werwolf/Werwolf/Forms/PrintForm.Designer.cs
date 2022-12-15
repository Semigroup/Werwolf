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
            this.DruckenBilder = new System.Windows.Forms.Button();
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
            this.floatBox1 = new Assistment.form.FloatBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dinA4ForcedBox = new System.Windows.Forms.CheckBox();
            this.consoleBox = new System.Windows.Forms.CheckBox();
            this.DruckenJob = new System.Windows.Forms.Button();
            this.PrintButtonAtlas = new System.Windows.Forms.Button();
            this.PrintButtonTTS = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(852, 59);
            this.radioButton1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(175, 20);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Eine Rückseite pro Karte";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(852, 140);
            this.radioButton2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(266, 20);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Eine gemeinsame Rückseite pro Papier";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.Visible = false;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(852, 86);
            this.radioButton3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(132, 20);
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
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(829, 814);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // DeckButton
            // 
            this.DeckButton.Location = new System.Drawing.Point(852, 12);
            this.DeckButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.label1.Location = new System.Drawing.Point(997, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Ausgewähltes Deck";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(849, 197);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Hintergrundfarbe";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(849, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Farbe Trennlinien";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(852, 113);
            this.radioButton4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(119, 20);
            this.radioButton4.TabIndex = 10;
            this.radioButton4.Text = "Nur Rückseiten";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // Drucken
            // 
            this.Drucken.Location = new System.Drawing.Point(1033, 651);
            this.Drucken.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Drucken.Name = "Drucken";
            this.Drucken.Size = new System.Drawing.Size(152, 43);
            this.Drucken.TabIndex = 11;
            this.Drucken.Text = "PDF Erstellen";
            this.Drucken.UseVisualStyleBackColor = true;
            this.Drucken.Click += new System.EventHandler(this.Drucken_Click);
            // 
            // DruckenBilder
            // 
            this.DruckenBilder.Location = new System.Drawing.Point(1033, 694);
            this.DruckenBilder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.DruckenBilder.Name = "DruckenBilder";
            this.DruckenBilder.Size = new System.Drawing.Size(152, 43);
            this.DruckenBilder.TabIndex = 102;
            this.DruckenBilder.Text = "Bilder Erstellen";
            this.DruckenBilder.UseVisualStyleBackColor = true;
            this.DruckenBilder.Click += new System.EventHandler(this.DruckenBilder_Click);
            // 
            // Printer
            // 
            this.Printer.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Printer_DoWork);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(853, 622);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(332, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.TabIndex = 16;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(852, 309);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(163, 20);
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
            this.ppmBox1.Location = new System.Drawing.Point(851, 491);
            this.ppmBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ppmBox1.Name = "ppmBox1";
            this.ppmBox1.Ppm = 24F;
            this.ppmBox1.PpmMaximum = 1000F;
            this.ppmBox1.PpmMinimum = 0.001F;
            this.ppmBox1.Size = new System.Drawing.Size(177, 44);
            this.ppmBox1.TabIndex = 15;
            // 
            // colorBox2
            // 
            this.colorBox2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorBox2.Location = new System.Drawing.Point(971, 226);
            this.colorBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.colorBox2.Name = "colorBox2";
            this.colorBox2.Size = new System.Drawing.Size(217, 23);
            this.colorBox2.TabIndex = 8;
            // 
            // colorBox1
            // 
            this.colorBox1.Color = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.colorBox1.Location = new System.Drawing.Point(971, 197);
            this.colorBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.colorBox1.Name = "colorBox1";
            this.colorBox1.Size = new System.Drawing.Size(217, 23);
            this.colorBox1.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(853, 651);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 43);
            this.button1.TabIndex = 18;
            this.button1.Text = "Vorschau: Erste Seite";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(852, 439);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(88, 20);
            this.checkBox2.TabIndex = 19;
            this.checkBox2.Text = "FixedFont";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Visible = false;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(852, 282);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(179, 20);
            this.checkBox3.TabIndex = 20;
            this.checkBox3.Text = "Trennlinie auf Rückseite?";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(852, 255);
            this.checkBox4.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(189, 20);
            this.checkBox4.TabIndex = 21;
            this.checkBox4.Text = "Trennlinie auf Vorderseite?";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // floatBox1
            // 
            this.floatBox1.Location = new System.Drawing.Point(1004, 553);
            this.floatBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.floatBox1.Name = "floatBox1";
            this.floatBox1.Size = new System.Drawing.Size(51, 22);
            this.floatBox1.TabIndex = 22;
            this.floatBox1.UserValue = 300F;
            this.floatBox1.UserValueMaximum = 3.402823E+38F;
            this.floatBox1.UserValueMinimum = 0F;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(848, 553);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(142, 16);
            this.label4.TabIndex = 23;
            this.label4.Text = "Maximale Größe in MB";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dinA4ForcedBox
            // 
            this.dinA4ForcedBox.AutoSize = true;
            this.dinA4ForcedBox.Location = new System.Drawing.Point(852, 335);
            this.dinA4ForcedBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dinA4ForcedBox.Name = "dinA4ForcedBox";
            this.dinA4ForcedBox.Size = new System.Drawing.Size(164, 20);
            this.dinA4ForcedBox.TabIndex = 103;
            this.dinA4ForcedBox.Text = "Karten um 90° rotieren?";
            this.dinA4ForcedBox.UseVisualStyleBackColor = true;
            // 
            // consoleBox
            // 
            this.consoleBox.AutoSize = true;
            this.consoleBox.Location = new System.Drawing.Point(852, 362);
            this.consoleBox.Margin = new System.Windows.Forms.Padding(4);
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.Size = new System.Drawing.Size(150, 20);
            this.consoleBox.TabIndex = 104;
            this.consoleBox.Text = "Konsolen anzeigen?";
            this.consoleBox.UseVisualStyleBackColor = true;
            // 
            // DruckenJob
            // 
            this.DruckenJob.Location = new System.Drawing.Point(853, 694);
            this.DruckenJob.Margin = new System.Windows.Forms.Padding(4);
            this.DruckenJob.Name = "DruckenJob";
            this.DruckenJob.Size = new System.Drawing.Size(175, 43);
            this.DruckenJob.TabIndex = 105;
            this.DruckenJob.Text = "Nur Job Erstellen";
            this.DruckenJob.UseVisualStyleBackColor = true;
            this.DruckenJob.Click += new System.EventHandler(this.DruckenJob_Click);
            // 
            // PrintButtonAtlas
            // 
            this.PrintButtonAtlas.Location = new System.Drawing.Point(1033, 741);
            this.PrintButtonAtlas.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PrintButtonAtlas.Name = "PrintButtonAtlas";
            this.PrintButtonAtlas.Size = new System.Drawing.Size(152, 43);
            this.PrintButtonAtlas.TabIndex = 106;
            this.PrintButtonAtlas.Text = "Atlanten Erstellen";
            this.PrintButtonAtlas.UseVisualStyleBackColor = true;
            this.PrintButtonAtlas.Click += new System.EventHandler(this.PrintButtonAtlas_Click);
            // 
            // PrintButtonTTS
            // 
            this.PrintButtonTTS.Location = new System.Drawing.Point(853, 741);
            this.PrintButtonTTS.Margin = new System.Windows.Forms.Padding(4);
            this.PrintButtonTTS.Name = "PrintButtonTTS";
            this.PrintButtonTTS.Size = new System.Drawing.Size(175, 43);
            this.PrintButtonTTS.TabIndex = 107;
            this.PrintButtonTTS.Text = "TTSDaten Erstellen";
            this.PrintButtonTTS.UseVisualStyleBackColor = true;
            this.PrintButtonTTS.Click += new System.EventHandler(this.PrintButtonTTS_Click);
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 814);
            this.Controls.Add(this.PrintButtonTTS);
            this.Controls.Add(this.PrintButtonAtlas);
            this.Controls.Add(this.DruckenJob);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.dinA4ForcedBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.floatBox1);
            this.Controls.Add(this.checkBox4);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ppmBox1);
            this.Controls.Add(this.Drucken);
            this.Controls.Add(this.DruckenBilder);
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
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
        private System.Windows.Forms.Button DruckenBilder;
        private Assistment.form.PpmBox ppmBox1;
        private System.ComponentModel.BackgroundWorker Printer;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox4;
        private Assistment.form.FloatBox floatBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox dinA4ForcedBox;
        private System.Windows.Forms.CheckBox consoleBox;
        private System.Windows.Forms.Button DruckenJob;
        private System.Windows.Forms.Button PrintButtonAtlas;
        private System.Windows.Forms.Button PrintButtonTTS;
    }
}