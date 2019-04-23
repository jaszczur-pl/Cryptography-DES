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

		private static byte[] StrToByteArray(string str) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			return encoding.GetBytes(str);
		}


		private void btnSzyfruj_Click(object sender, EventArgs e) {
			DesAlgorithm des = new DesAlgorithm();

            byte[] key = des.GetStringHexKey(txtKlucz.Text);
            byte[] result = des.Encrypt(StrToByteArray(txtZrodlo.Text), key);

            txtWynik.Text = des.ToHexString(result);

            MessageBox.Show("Szyfrowanie zakończone sukcesem!");


        }

        private void btnDeszyfruj_Click(object sender, EventArgs e)
        {
            DesAlgorithm des = new DesAlgorithm();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();

            byte[] encyrptedText = des.GetStringHexText(txtZrodlo.Text);
            byte[] key = des.GetStringHexKey(txtKlucz.Text);
            byte[] result = des.Decrypt(encyrptedText, key);

            txtWynik.Text = enc.GetString(result);

            MessageBox.Show("Deszyfrowanie zakończone sukcesem!");

        }


        private void btnZrodlowy_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dialog.FilterIndex = 1;
            dialog.RestoreDirectory = true;

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtZrodlo.Text = File.ReadAllText(dialog.FileName);

            }
        }

        private void btnWyjsciowy_Click(object sender, EventArgs e)
        {
			SaveFileDialog dlg = new SaveFileDialog();

            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(dlg.FileName, txtWynik.Text);

            }

        }
   
		private void txtKlucz_KeyPress(object sender, KeyPressEventArgs e) {
			if (e.KeyChar < '0' || e.KeyChar > '9') {
				if (e.KeyChar <'A' || e.KeyChar > 'F') {
					if (e.KeyChar != 8) //backspace dopuszczony
						e.Handled = true;//powinno zatrzymać obsługe tego :/
				}
			}
			//sprawdzamy długość
			if (sender.Equals(txtKlucz)) {
				if (txtKlucz.Text.Length == 16 && e.KeyChar != 8)
					e.Handled = true;
			}
		}

    }
}
