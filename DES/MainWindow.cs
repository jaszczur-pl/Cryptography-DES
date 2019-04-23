using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace DES{
    public partial class MainWindow : Form{
        public MainWindow(){
            InitializeComponent();

        }

		private void btnEncrypt_Click(object sender, EventArgs e) {
			DesAlgorithm des = new DesAlgorithm();
            UTF8Encoding encoding = new UTF8Encoding();

            byte[] convertedText = encoding.GetBytes(textInput.Text);
            byte[] key = des.ConvertHexStringKeyToBytes(textKey.Text);
            byte[] result = des.Encrypt(convertedText, key);

            textOutput.Text = des.ConvertBytesToHexString(result);

            MessageBox.Show("Szyfrowanie zakończone sukcesem!");
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            DesAlgorithm des = new DesAlgorithm();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();

            byte[] encyrptedText = des.ConvertStringTextToBytes(textInput.Text);
            byte[] key = des.ConvertHexStringKeyToBytes(textKey.Text);
            byte[] result = des.Decrypt(encyrptedText, key);

            textOutput.Text = enc.GetString(result);

            MessageBox.Show("Deszyfrowanie zakończone sukcesem!");
        }


        private void btnGetFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                textInput.Text = File.ReadAllText(dialog.FileName);

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
			SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dialog.FileName, textOutput.Text);

            }

        }
   
		private void textKey_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar < '0' || e.KeyChar > '9') {
				if (e.KeyChar <'A' || e.KeyChar > 'F') {
					if (e.KeyChar != 8) //backspace dopuszczony
						e.Handled = true;//powinno zatrzymać obsługe tego :/
				}
			}
			//sprawdzamy długość
			if (sender.Equals(textKey)) {
				if (textKey.Text.Length == 16 && e.KeyChar != 8)
					e.Handled = true;
			}
		}

    }
}
