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
            this.txtKlucz = new System.Windows.Forms.TextBox();
            this.btnWyjscie = new System.Windows.Forms.Button();
            this.btnZrodlo = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtWynik = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSzyfruj
            // 
            this.btnSzyfruj.Location = new System.Drawing.Point(356, 83);
            this.btnSzyfruj.Name = "btnSzyfruj";
            this.btnSzyfruj.Size = new System.Drawing.Size(103, 23);
            this.btnSzyfruj.TabIndex = 0;
            this.btnSzyfruj.Text = "Szyfruj";
            this.btnSzyfruj.UseVisualStyleBackColor = true;
            this.btnSzyfruj.Click += new System.EventHandler(this.btnSzyfruj_Click);
            // 
            // btnDeszyfruj
            // 
            this.btnDeszyfruj.Location = new System.Drawing.Point(465, 83);
            this.btnDeszyfruj.Name = "btnDeszyfruj";
            this.btnDeszyfruj.Size = new System.Drawing.Size(103, 23);
            this.btnDeszyfruj.TabIndex = 1;
            this.btnDeszyfruj.Text = "Deszyfruj";
            this.btnDeszyfruj.UseVisualStyleBackColor = true;
            this.btnDeszyfruj.Click += new System.EventHandler(this.btnDeszyfruj_Click);
            // 
            // txtZrodlo
            // 
            this.txtZrodlo.Location = new System.Drawing.Point(53, 137);
            this.txtZrodlo.Multiline = true;
            this.txtZrodlo.Name = "txtZrodlo";
            this.txtZrodlo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtZrodlo.Size = new System.Drawing.Size(515, 167);
            this.txtZrodlo.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Klucz heksadecymalny";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tekst wejściowy";
            // 
            // txtKlucz
            // 
            this.txtKlucz.Location = new System.Drawing.Point(53, 31);
            this.txtKlucz.Name = "txtKlucz";
            this.txtKlucz.Size = new System.Drawing.Size(124, 20);
            this.txtKlucz.TabIndex = 12;
            this.txtKlucz.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKlucz_KeyPress);
            // 
            // btnWyjscie
            // 
            this.btnWyjscie.Location = new System.Drawing.Point(53, 543);
            this.btnWyjscie.Name = "btnWyjscie";
            this.btnWyjscie.Size = new System.Drawing.Size(95, 24);
            this.btnWyjscie.TabIndex = 13;
            this.btnWyjscie.Text = "Zapisz";
            this.btnWyjscie.UseVisualStyleBackColor = true;
            this.btnWyjscie.Click += new System.EventHandler(this.btnWyjsciowy_Click);
            // 
            // btnZrodlo
            // 
            this.btnZrodlo.Location = new System.Drawing.Point(53, 82);
            this.btnZrodlo.Name = "btnZrodlo";
            this.btnZrodlo.Size = new System.Drawing.Size(95, 24);
            this.btnZrodlo.TabIndex = 12;
            this.btnZrodlo.Text = "Wybierz plik";
            this.btnZrodlo.UseVisualStyleBackColor = true;
            this.btnZrodlo.Click += new System.EventHandler(this.btnZrodlowy_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 319);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Tekst Wyjściowy";
            // 
            // txtWynik
            // 
            this.txtWynik.Location = new System.Drawing.Point(53, 335);
            this.txtWynik.Multiline = true;
            this.txtWynik.Name = "txtWynik";
            this.txtWynik.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtWynik.Size = new System.Drawing.Size(515, 181);
            this.txtWynik.TabIndex = 2;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 584);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnWyjscie);
            this.Controls.Add(this.txtWynik);
            this.Controls.Add(this.txtZrodlo);
            this.Controls.Add(this.btnZrodlo);
            this.Controls.Add(this.txtKlucz);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDeszyfruj);
            this.Controls.Add(this.btnSzyfruj);
            this.Name = "MainWindow";
            this.Text = "DesAlgorithm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSzyfruj;
        private System.Windows.Forms.Button btnDeszyfruj;
        private System.Windows.Forms.TextBox txtZrodlo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnWyjscie;
        private System.Windows.Forms.Button btnZrodlo;
		private System.Windows.Forms.TextBox txtKlucz;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtWynik;
    }
}

