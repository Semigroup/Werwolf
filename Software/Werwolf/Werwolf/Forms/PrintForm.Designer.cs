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
            this.colorBox2 = new Assistment.form.ColorBox();
            this.colorBox1 = new Assistment.form.ColorBox();
            this.floatBox1 = new Assistment.form.FloatBox();
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
            this.radioButton2.Location = new System.Drawing.Point(852, 86);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(277, 21);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "Eine gemeinsame Rückseite pro Papier";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(852, 113);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(139, 21);
            this.radioButton3.TabIndex = 2;
            this.radioButton3.Text = "Keine Rückseiten";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(830, 814);
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
            this.radioButton4.Location = new System.Drawing.Point(852, 140);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(126, 21);
            this.radioButton4.TabIndex = 10;
            this.radioButton4.Text = "Nur Rückseiten";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // Drucken
            // 
            this.Drucken.Location = new System.Drawing.Point(852, 344);
            this.Drucken.Name = "Drucken";
            this.Drucken.Size = new System.Drawing.Size(93, 43);
            this.Drucken.TabIndex = 11;
            this.Drucken.Text = "Drucken";
            this.Drucken.UseVisualStyleBackColor = true;
            this.Drucken.Click += new System.EventHandler(this.Drucken_Click);
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
            // floatBox1
            // 
            this.floatBox1.Location = new System.Drawing.Point(875, 272);
            this.floatBox1.Name = "floatBox1";
            this.floatBox1.Size = new System.Drawing.Size(51, 22);
            this.floatBox1.TabIndex = 12;
            this.floatBox1.UserValue = 10F;
            this.floatBox1.UserValueMaximum = 3.402823E+38F;
            this.floatBox1.UserValueMinimum = 1E-06F;
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 814);
            this.Controls.Add(this.floatBox1);
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
        private Assistment.form.FloatBox floatBox1;
    }
}