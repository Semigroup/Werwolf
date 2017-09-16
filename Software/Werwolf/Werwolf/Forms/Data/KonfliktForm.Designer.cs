namespace Werwolf.Forms.Data
{
    partial class KonfliktForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.scrollList1 = new Assistment.form.ScrollList();
            this.AbbrechenButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scrollList1
            // 
            this.scrollList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scrollList1.Location = new System.Drawing.Point(0, 0);
            this.scrollList1.Name = "scrollList1";
            this.scrollList1.Size = new System.Drawing.Size(1003, 761);
            this.scrollList1.TabIndex = 0;
            this.scrollList1.Load += new System.EventHandler(this.scrollList1_Load);
            // 
            // AbbrechenButton
            // 
            this.AbbrechenButton.Location = new System.Drawing.Point(12, 12);
            this.AbbrechenButton.Name = "AbbrechenButton";
            this.AbbrechenButton.Size = new System.Drawing.Size(75, 23);
            this.AbbrechenButton.TabIndex = 1;
            this.AbbrechenButton.Text = "Abbrechen";
            this.AbbrechenButton.UseVisualStyleBackColor = true;
            this.AbbrechenButton.Visible = false;
            this.AbbrechenButton.Click += new System.EventHandler(this.AbbrechenButton_Click);
            // 
            // KonfliktForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 761);
            this.Controls.Add(this.AbbrechenButton);
            this.Controls.Add(this.scrollList1);
            this.Name = "KonfliktForm";
            this.Text = "Im Ziel existieren bereits Bilder mit denselben Namen. Bitte lösen sie alle Konfl" +
    "ikte auf!";
            this.ResumeLayout(false);

        }

        #endregion

        private Assistment.form.ScrollList scrollList1;
        private System.Windows.Forms.Button AbbrechenButton;
    }
}