﻿namespace DES
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
            this.btnEncrypt = new System.Windows.Forms.Button();
            this.btnDecrypt = new System.Windows.Forms.Button();
            this.textInput = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textKey = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnGetFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textOutput = new System.Windows.Forms.TextBox();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.btnLoadImage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnEncrypt
            // 
            this.btnEncrypt.Location = new System.Drawing.Point(589, 32);
            this.btnEncrypt.Name = "btnEncrypt";
            this.btnEncrypt.Size = new System.Drawing.Size(103, 23);
            this.btnEncrypt.TabIndex = 0;
            this.btnEncrypt.Text = "Szyfruj";
            this.btnEncrypt.UseVisualStyleBackColor = true;
            this.btnEncrypt.Click += new System.EventHandler(this.btnEncrypt_Click);
            // 
            // btnDecrypt
            // 
            this.btnDecrypt.Location = new System.Drawing.Point(589, 61);
            this.btnDecrypt.Name = "btnDecrypt";
            this.btnDecrypt.Size = new System.Drawing.Size(103, 23);
            this.btnDecrypt.TabIndex = 1;
            this.btnDecrypt.Text = "Deszyfruj";
            this.btnDecrypt.UseVisualStyleBackColor = true;
            this.btnDecrypt.Click += new System.EventHandler(this.btnDecrypt_Click);
            // 
            // textInput
            // 
            this.textInput.Location = new System.Drawing.Point(53, 137);
            this.textInput.MaxLength = 1000000;
            this.textInput.Multiline = true;
            this.textInput.Name = "textInput";
            this.textInput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textInput.Size = new System.Drawing.Size(515, 167);
            this.textInput.TabIndex = 2;
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
            // textKey
            // 
            this.textKey.Location = new System.Drawing.Point(53, 31);
            this.textKey.Name = "textKey";
            this.textKey.Size = new System.Drawing.Size(124, 20);
            this.textKey.TabIndex = 12;
            this.textKey.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textKey_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(218, 61);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 24);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Zapisz plik tekstowy";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnGetFile
            // 
            this.btnGetFile.Location = new System.Drawing.Point(218, 31);
            this.btnGetFile.Name = "btnGetFile";
            this.btnGetFile.Size = new System.Drawing.Size(120, 24);
            this.btnGetFile.TabIndex = 12;
            this.btnGetFile.Text = "Wybierz plik tekstowy";
            this.btnGetFile.UseVisualStyleBackColor = true;
            this.btnGetFile.Click += new System.EventHandler(this.btnGetFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(586, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Tekst Wyjściowy";
            // 
            // textOutput
            // 
            this.textOutput.Location = new System.Drawing.Point(589, 137);
            this.textOutput.Multiline = true;
            this.textOutput.Name = "textOutput";
            this.textOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textOutput.Size = new System.Drawing.Size(515, 167);
            this.textOutput.TabIndex = 2;
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Location = new System.Drawing.Point(360, 61);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(120, 24);
            this.btnSaveImage.TabIndex = 15;
            this.btnSaveImage.Text = "Zapisz obrazek";
            this.btnSaveImage.UseVisualStyleBackColor = true;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // btnLoadImage
            // 
            this.btnLoadImage.Location = new System.Drawing.Point(360, 31);
            this.btnLoadImage.Name = "btnLoadImage";
            this.btnLoadImage.Size = new System.Drawing.Size(120, 24);
            this.btnLoadImage.TabIndex = 14;
            this.btnLoadImage.Text = "Wybierz obrazek";
            this.btnLoadImage.UseVisualStyleBackColor = true;
            this.btnLoadImage.Click += new System.EventHandler(this.btnLoadImage_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1128, 338);
            this.Controls.Add(this.btnSaveImage);
            this.Controls.Add(this.btnLoadImage);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.textOutput);
            this.Controls.Add(this.textInput);
            this.Controls.Add(this.btnGetFile);
            this.Controls.Add(this.textKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnDecrypt);
            this.Controls.Add(this.btnEncrypt);
            this.Name = "MainWindow";
            this.Text = "DesAlgorithm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.TextBox textInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnGetFile;
		private System.Windows.Forms.TextBox textKey;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textOutput;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.Button btnLoadImage;
    }
}

