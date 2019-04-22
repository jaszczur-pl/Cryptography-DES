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

        string dane;

		private static byte[] StrToByteArray(string str) {
			System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
			return encoding.GetBytes(str);
		}


		private void btnSzyfruj_Click(object sender, EventArgs e) {
			DesAlgorithm des = new DesAlgorithm();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] klucz = des.GetStringHexKey(txtKlucz.Text);
            byte[] wynik = des.szyfruj(StrToByteArray(txtZrodlo.Text), klucz);
            txtWynik.Text = des.ToHexString(wynik);
            MessageBox.Show("Encryption completed!");


        }

        private void btnDeszyfruj_Click(object sender, EventArgs e)
        {
            DesAlgorithm des = new DesAlgorithm();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] zaszyfrowane = des.GetStringHexText(txtZrodlo.Text);
            byte[] klucz = des.GetStringHexKey(txtKlucz.Text);
            byte[] wynik = des.deszyfruj(zaszyfrowane, klucz);
            txtWynik.Text = enc.GetString(wynik);
            MessageBox.Show("Decryption completed");

        }


        private void btnZrodlowy_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dlg.FilterIndex = 1;
            dlg.RestoreDirectory = true;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                txtZrodlo.Text = File.ReadAllText(dlg.FileName);

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
				if (e.KeyChar <'a' || e.KeyChar > 'f') {
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

        private void btnCreateKey_Click(object sender, EventArgs e)
        {
            Random random = new Random();
            byte[] buffer = new byte[16 / 2];
            random.NextBytes(buffer);
            string result = String.Concat(buffer.Select(x => x.ToString("X2")).ToArray());
            if (16 % 2 == 0)
                txtKlucz.Text = result;
            else
                txtKlucz.Text = result + random.Next(16);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();

        }
    }
}
