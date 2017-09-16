namespace Werwolf.Forms.Data
{
    partial class KonfliktFeld
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

        #region Vom Komponenten-Designer generierter Code

        /// <summary> 
        /// Erforderliche Methode für die Designerunterstützung. 
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.buttonD = new System.Windows.Forms.Button();
            this.buttonDS = new System.Windows.Forms.Button();
            this.pictureBoxD = new System.Windows.Forms.PictureBox();
            this.pictureBoxS = new System.Windows.Forms.PictureBox();
            this.labelD = new System.Windows.Forms.Label();
            this.labelS = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxS)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(554, 390);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(146, 49);
            this.button1.TabIndex = 0;
            this.button1.Text = "Dieses Bild Verwenden und die Datei im Ziel Ersetzen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonD
            // 
            this.buttonD.Location = new System.Drawing.Point(6, 390);
            this.buttonD.Name = "buttonD";
            this.buttonD.Size = new System.Drawing.Size(138, 49);
            this.buttonD.TabIndex = 1;
            this.buttonD.Text = "Dieses Bild Verwenden, und die Quelldatei Nicht Rüberkopieren";
            this.buttonD.UseVisualStyleBackColor = true;
            this.buttonD.Click += new System.EventHandler(this.buttonD_Click);
            // 
            // buttonDS
            // 
            this.buttonDS.Location = new System.Drawing.Point(408, 390);
            this.buttonDS.Name = "buttonDS";
            this.buttonDS.Size = new System.Drawing.Size(140, 49);
            this.buttonDS.TabIndex = 2;
            this.buttonDS.Text = "Dieses Bild Verwenden, aber die Datei im Ziel Nicht Ersetzen";
            this.buttonDS.UseVisualStyleBackColor = true;
            this.buttonDS.Click += new System.EventHandler(this.buttonDS_Click);
            // 
            // pictureBoxD
            // 
            this.pictureBoxD.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxD.Name = "pictureBoxD";
            this.pictureBoxD.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxD.TabIndex = 3;
            this.pictureBoxD.TabStop = false;
            // 
            // pictureBoxS
            // 
            this.pictureBoxS.Location = new System.Drawing.Point(408, 0);
            this.pictureBoxS.Name = "pictureBoxS";
            this.pictureBoxS.Size = new System.Drawing.Size(300, 300);
            this.pictureBoxS.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxS.TabIndex = 4;
            this.pictureBoxS.TabStop = false;
            // 
            // labelD
            // 
            this.labelD.AutoSize = true;
            this.labelD.Location = new System.Drawing.Point(3, 303);
            this.labelD.Name = "labelD";
            this.labelD.Size = new System.Drawing.Size(53, 13);
            this.labelD.TabIndex = 5;
            this.labelD.Text = "AdresseD";
            // 
            // labelS
            // 
            this.labelS.AutoSize = true;
            this.labelS.Location = new System.Drawing.Point(405, 303);
            this.labelS.Name = "labelS";
            this.labelS.Size = new System.Drawing.Size(52, 13);
            this.labelS.TabIndex = 6;
            this.labelS.Text = "AdresseS";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoEllipsis = true;
            this.checkBox1.Location = new System.Drawing.Point(227, 390);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(175, 55);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Diese Lösung für alle anderen Konflikte auch übernehmen";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // KonfliktFeld
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.labelS);
            this.Controls.Add(this.labelD);
            this.Controls.Add(this.pictureBoxS);
            this.Controls.Add(this.pictureBoxD);
            this.Controls.Add(this.buttonDS);
            this.Controls.Add(this.buttonD);
            this.Controls.Add(this.button1);
            this.Name = "KonfliktFeld";
            this.Size = new System.Drawing.Size(957, 445);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxS)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonD;
        private System.Windows.Forms.Button buttonDS;
        private System.Windows.Forms.PictureBox pictureBoxD;
        private System.Windows.Forms.PictureBox pictureBoxS;
        private System.Windows.Forms.Label labelD;
        private System.Windows.Forms.Label labelS;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}
