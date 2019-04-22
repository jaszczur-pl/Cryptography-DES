namespace DES
{
    partial class MainWindow
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
            this.btnSzyfruj = new System.Windows.Forms.Button();
            this.btnDeszyfruj = new System.Windows.Forms.Button();
            this.txtZrodlo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtKlucz = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnWyjscie = new System.Windows.Forms.Button();
            this.btnZrodlo = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCreateKey = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWynik = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSzyfruj
            // 
            this.btnSzyfruj.Location = new System.Drawing.Point(79, 446);
            this.btnSzyfruj.Name = "btnSzyfruj";
            this.btnSzyfruj.Size = new System.Drawing.Size(103, 23);
            this.btnSzyfruj.TabIndex = 0;
            this.btnSzyfruj.Text = "Encode";
            this.btnSzyfruj.UseVisualStyleBackColor = true;
            this.btnSzyfruj.Click += new System.EventHandler(this.btnSzyfruj_Click);
            // 
            // btnDeszyfruj
            // 
            this.btnDeszyfruj.Location = new System.Drawing.Point(188, 446);
            this.btnDeszyfruj.Name = "btnDeszyfruj";
            this.btnDeszyfruj.Size = new System.Drawing.Size(103, 23);
            this.btnDeszyfruj.TabIndex = 1;
            this.btnDeszyfruj.Text = "Decode";
            this.btnDeszyfruj.UseVisualStyleBackColor = true;
            this.btnDeszyfruj.Click += new System.EventHandler(this.btnDeszyfruj_Click);
            // 
            // txtZrodlo
            // 
            this.txtZrodlo.Location = new System.Drawing.Point(71, 16);
            this.txtZrodlo.Multiline = true;
            this.txtZrodlo.Name = "txtZrodlo";
            this.txtZrodlo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtZrodlo.Size = new System.Drawing.Size(408, 124);
            this.txtZrodlo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 103);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Key:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Input Text:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtZrodlo);
            this.groupBox1.Location = new System.Drawing.Point(12, 129);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(556, 311);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // txtKlucz
            // 
            this.txtKlucz.Location = new System.Drawing.Point(58, 103);
            this.txtKlucz.Name = "txtKlucz";
            this.txtKlucz.Size = new System.Drawing.Size(124, 20);
            this.txtKlucz.TabIndex = 12;
            this.txtKlucz.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKlucz_KeyPress);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnWyjscie);
            this.groupBox2.Controls.Add(this.btnZrodlo);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(338, 78);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Files:";
            // 
            // btnWyjscie
            // 
            this.btnWyjscie.Location = new System.Drawing.Point(40, 43);
            this.btnWyjscie.Name = "btnWyjscie";
            this.btnWyjscie.Size = new System.Drawing.Size(120, 24);
            this.btnWyjscie.TabIndex = 13;
            this.btnWyjscie.Text = "Save File";
            this.btnWyjscie.UseVisualStyleBackColor = true;
            this.btnWyjscie.Click += new System.EventHandler(this.btnWyjsciowy_Click);
            // 
            // btnZrodlo
            // 
            this.btnZrodlo.Location = new System.Drawing.Point(40, 14);
            this.btnZrodlo.Name = "btnZrodlo";
            this.btnZrodlo.Size = new System.Drawing.Size(120, 24);
            this.btnZrodlo.TabIndex = 12;
            this.btnZrodlo.Text = "Read File";
            this.btnZrodlo.UseVisualStyleBackColor = true;
            this.btnZrodlo.Click += new System.EventHandler(this.btnZrodlowy_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Output";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Input";
            // 
            // btnCreateKey
            // 
            this.btnCreateKey.Location = new System.Drawing.Point(188, 98);
            this.btnCreateKey.Name = "btnCreateKey";
            this.btnCreateKey.Size = new System.Drawing.Size(103, 23);
            this.btnCreateKey.TabIndex = 13;
            this.btnCreateKey.Text = "Create a key";
            this.btnCreateKey.UseVisualStyleBackColor = true;
            this.btnCreateKey.Click += new System.EventHandler(this.btnCreateKey_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(580, 445);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(58, 24);
            this.btnExit.TabIndex = 14;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtWynik);
            this.groupBox3.Location = new System.Drawing.Point(12, 278);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(556, 162);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Output Text:";
            // 
            // txtWynik
            // 
            this.txtWynik.Location = new System.Drawing.Point(70, 16);
            this.txtWynik.Multiline = true;
            this.txtWynik.Name = "txtWynik";
            this.txtWynik.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtWynik.Size = new System.Drawing.Size(409, 124);
            this.txtWynik.TabIndex = 2;
            // 
            // DESForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 481);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCreateKey);
            this.Controls.Add(this.txtKlucz);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDeszyfruj);
            this.Controls.Add(this.btnSzyfruj);
            this.Name = "DESForm";
            this.Text = "DesAlgorithm";
            this.Load += new System.EventHandler(this.DESForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSzyfruj;
        private System.Windows.Forms.Button btnDeszyfruj;
        private System.Windows.Forms.TextBox txtZrodlo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnWyjscie;
        private System.Windows.Forms.Button btnZrodlo;
		private System.Windows.Forms.TextBox txtKlucz;
        private System.Windows.Forms.Button btnCreateKey;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWynik;
    }
}

