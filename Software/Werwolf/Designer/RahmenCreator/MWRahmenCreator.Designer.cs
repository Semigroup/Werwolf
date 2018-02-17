namespace Designer.RahmenCreator
{
    partial class MWRahmenCreator
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.FragmentHeight = new System.Windows.Forms.Label();
            this.RightInversion = new System.Windows.Forms.CheckBox();
            this.LeftInversion = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.SaveButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.ShiftBox = new Assistment.form.FloatBox();
            this.SamplesBox = new Assistment.form.IntBox();
            this.penBox1 = new Assistment.form.PenBox();
            this.enumBox1 = new Assistment.form.EnumBox();
            this.ppmBox1 = new Assistment.form.PpmBox();
            this.FragmentNumberBox = new Assistment.form.IntBox();
            this.FragmentDickeBox = new Assistment.form.FloatBox();
            this.RadiusBox = new Assistment.form.FloatBox();
            this.MarginBottomBox = new Assistment.form.FloatBox();
            this.MarginTopBox = new Assistment.form.FloatBox();
            this.MarginRightBox = new Assistment.form.FloatBox();
            this.MarginLeftBox = new Assistment.form.FloatBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(762, 669);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(797, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Hintergrundbild Auswählen";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(863, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Margin Left";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(863, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Margin Right";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(863, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Margin Top";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(863, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Margin Bottom";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(883, 133);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Radius";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(827, 155);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Fragment Dicke";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(823, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Fragment Anzahl";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(826, 225);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Fragment Style";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(838, 497);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Anzahl Samples";
            // 
            // FragmentHeight
            // 
            this.FragmentHeight.AutoSize = true;
            this.FragmentHeight.Location = new System.Drawing.Point(823, 201);
            this.FragmentHeight.Name = "FragmentHeight";
            this.FragmentHeight.Size = new System.Drawing.Size(86, 13);
            this.FragmentHeight.TabIndex = 22;
            this.FragmentHeight.Text = "Fragment Höhe: ";
            // 
            // RightInversion
            // 
            this.RightInversion.AutoSize = true;
            this.RightInversion.Checked = true;
            this.RightInversion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RightInversion.Location = new System.Drawing.Point(860, 312);
            this.RightInversion.Name = "RightInversion";
            this.RightInversion.Size = new System.Drawing.Size(119, 17);
            this.RightInversion.TabIndex = 23;
            this.RightInversion.Text = "Rechts Invertieren?";
            this.RightInversion.UseVisualStyleBackColor = true;
            // 
            // LeftInversion
            // 
            this.LeftInversion.AutoSize = true;
            this.LeftInversion.Location = new System.Drawing.Point(860, 335);
            this.LeftInversion.Name = "LeftInversion";
            this.LeftInversion.Size = new System.Drawing.Size(110, 17);
            this.LeftInversion.TabIndex = 24;
            this.LeftInversion.Text = "Links Invertieren?";
            this.LeftInversion.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(838, 528);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(131, 17);
            this.checkBox1.TabIndex = 25;
            this.checkBox1.Text = "Kartenrand Anzeigen?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(820, 630);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(155, 49);
            this.SaveButton.TabIndex = 26;
            this.SaveButton.Text = "Speichern";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(820, 604);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(155, 20);
            this.textBox1.TabIndex = 27;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(820, 585);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(35, 13);
            this.label10.TabIndex = 28;
            this.label10.Text = "Name";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(827, 269);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 13);
            this.label11.TabIndex = 30;
            this.label11.Text = "Fragment Shift";
            // 
            // ShiftBox
            // 
            this.ShiftBox.Location = new System.Drawing.Point(914, 264);
            this.ShiftBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ShiftBox.Name = "ShiftBox";
            this.ShiftBox.Size = new System.Drawing.Size(38, 18);
            this.ShiftBox.TabIndex = 29;
            this.ShiftBox.UserValue = 0F;
            this.ShiftBox.UserValueMaximum = 1F;
            this.ShiftBox.UserValueMinimum = 0F;
            // 
            // SamplesBox
            // 
            this.SamplesBox.Location = new System.Drawing.Point(930, 497);
            this.SamplesBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.SamplesBox.Name = "SamplesBox";
            this.SamplesBox.Size = new System.Drawing.Size(37, 16);
            this.SamplesBox.TabIndex = 21;
            this.SamplesBox.UserValue = 10000;
            this.SamplesBox.UserValueMaximum = 10000000;
            this.SamplesBox.UserValueMinimum = 2;
            // 
            // penBox1
            // 
            this.penBox1.Location = new System.Drawing.Point(820, 357);
            this.penBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.penBox1.Name = "penBox1";
            this.penBox1.Size = new System.Drawing.Size(197, 42);
            this.penBox1.TabIndex = 19;
            // 
            // enumBox1
            // 
            this.enumBox1.EnumType = null;
            this.enumBox1.Location = new System.Drawing.Point(829, 240);
            this.enumBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.enumBox1.Name = "enumBox1";
            this.enumBox1.Size = new System.Drawing.Size(154, 20);
            this.enumBox1.TabIndex = 18;
            this.enumBox1.UserValue = null;
            // 
            // ppmBox1
            // 
            this.ppmBox1.Location = new System.Drawing.Point(834, 429);
            this.ppmBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.ppmBox1.Name = "ppmBox1";
            this.ppmBox1.Ppm = 23F;
            this.ppmBox1.PpmMaximum = 1000F;
            this.ppmBox1.PpmMinimum = 0.001F;
            this.ppmBox1.Size = new System.Drawing.Size(133, 36);
            this.ppmBox1.TabIndex = 16;
            // 
            // FragmentNumberBox
            // 
            this.FragmentNumberBox.Location = new System.Drawing.Point(915, 177);
            this.FragmentNumberBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FragmentNumberBox.Name = "FragmentNumberBox";
            this.FragmentNumberBox.Size = new System.Drawing.Size(37, 16);
            this.FragmentNumberBox.TabIndex = 15;
            this.FragmentNumberBox.UserValue = 10;
            this.FragmentNumberBox.UserValueMaximum = 2147483647;
            this.FragmentNumberBox.UserValueMinimum = 1;
            // 
            // FragmentDickeBox
            // 
            this.FragmentDickeBox.Location = new System.Drawing.Point(914, 150);
            this.FragmentDickeBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.FragmentDickeBox.Name = "FragmentDickeBox";
            this.FragmentDickeBox.Size = new System.Drawing.Size(38, 18);
            this.FragmentDickeBox.TabIndex = 12;
            this.FragmentDickeBox.UserValue = 5F;
            this.FragmentDickeBox.UserValueMaximum = 3.402823E+38F;
            this.FragmentDickeBox.UserValueMinimum = -3.402823E+38F;
            // 
            // RadiusBox
            // 
            this.RadiusBox.Location = new System.Drawing.Point(941, 128);
            this.RadiusBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.RadiusBox.Name = "RadiusBox";
            this.RadiusBox.Size = new System.Drawing.Size(38, 18);
            this.RadiusBox.TabIndex = 10;
            this.RadiusBox.UserValue = 2F;
            this.RadiusBox.UserValueMaximum = 3.402823E+38F;
            this.RadiusBox.UserValueMinimum = 0F;
            // 
            // MarginBottomBox
            // 
            this.MarginBottomBox.Location = new System.Drawing.Point(941, 106);
            this.MarginBottomBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MarginBottomBox.Name = "MarginBottomBox";
            this.MarginBottomBox.Size = new System.Drawing.Size(38, 18);
            this.MarginBottomBox.TabIndex = 8;
            this.MarginBottomBox.UserValue = 5F;
            this.MarginBottomBox.UserValueMaximum = 3.402823E+38F;
            this.MarginBottomBox.UserValueMinimum = -3.402823E+38F;
            // 
            // MarginTopBox
            // 
            this.MarginTopBox.Location = new System.Drawing.Point(941, 84);
            this.MarginTopBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MarginTopBox.Name = "MarginTopBox";
            this.MarginTopBox.Size = new System.Drawing.Size(38, 18);
            this.MarginTopBox.TabIndex = 6;
            this.MarginTopBox.UserValue = 10F;
            this.MarginTopBox.UserValueMaximum = 3.402823E+38F;
            this.MarginTopBox.UserValueMinimum = -3.402823E+38F;
            // 
            // MarginRightBox
            // 
            this.MarginRightBox.Location = new System.Drawing.Point(941, 62);
            this.MarginRightBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MarginRightBox.Name = "MarginRightBox";
            this.MarginRightBox.Size = new System.Drawing.Size(38, 18);
            this.MarginRightBox.TabIndex = 4;
            this.MarginRightBox.UserValue = 3F;
            this.MarginRightBox.UserValueMaximum = 3.402823E+38F;
            this.MarginRightBox.UserValueMinimum = -3.402823E+38F;
            // 
            // MarginLeftBox
            // 
            this.MarginLeftBox.Location = new System.Drawing.Point(941, 40);
            this.MarginLeftBox.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MarginLeftBox.Name = "MarginLeftBox";
            this.MarginLeftBox.Size = new System.Drawing.Size(38, 18);
            this.MarginLeftBox.TabIndex = 2;
            this.MarginLeftBox.UserValue = 3F;
            this.MarginLeftBox.UserValueMaximum = 3.402823E+38F;
            this.MarginLeftBox.UserValueMinimum = -3.402823E+38F;
            // 
            // MWRahmenCreator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1060, 693);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.ShiftBox);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.LeftInversion);
            this.Controls.Add(this.RightInversion);
            this.Controls.Add(this.FragmentHeight);
            this.Controls.Add(this.SamplesBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.penBox1);
            this.Controls.Add(this.enumBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ppmBox1);
            this.Controls.Add(this.FragmentNumberBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.FragmentDickeBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.RadiusBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.MarginBottomBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MarginTopBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MarginRightBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MarginLeftBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "MWRahmenCreator";
            this.Text = "MWRahmenCreator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private Assistment.form.FloatBox MarginLeftBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Assistment.form.FloatBox MarginRightBox;
        private System.Windows.Forms.Label label3;
        private Assistment.form.FloatBox MarginTopBox;
        private System.Windows.Forms.Label label4;
        private Assistment.form.FloatBox MarginBottomBox;
        private System.Windows.Forms.Label label5;
        private Assistment.form.FloatBox RadiusBox;
        private System.Windows.Forms.Label label6;
        private Assistment.form.FloatBox FragmentDickeBox;
        private System.Windows.Forms.Label label7;
        private Assistment.form.IntBox FragmentNumberBox;
        private Assistment.form.PpmBox ppmBox1;
        private System.Windows.Forms.Label label8;
        private Assistment.form.EnumBox enumBox1;
        private Assistment.form.PenBox penBox1;
        private Assistment.form.IntBox SamplesBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label FragmentHeight;
        private System.Windows.Forms.CheckBox RightInversion;
        private System.Windows.Forms.CheckBox LeftInversion;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private Assistment.form.FloatBox ShiftBox;
    }
}