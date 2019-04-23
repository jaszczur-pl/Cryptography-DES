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
            byte[] wynik = des.Encrypt(StrToByteArray(txtZrodlo.Text), klucz);
            txtWynik.Text = des.ToHexString(wynik);
            MessageBox.Show("Szyfrowanie zakończone sukcesem!");


        }

        private void btnDeszyfruj_Click(object sender, EventArgs e)
        {
            DesAlgorithm des = new DesAlgorithm();
            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            byte[] zaszyfrowane = des.GetStringHexText(txtZrodlo.Text);
            byte[] klucz = des.GetStringHexKey(txtKlucz.Text);
            byte[] wynik = des.Decrypt(zaszyfrowane, klucz);
            txtWynik.Text = enc.GetString(wynik);
            MessageBox.Show("Deszyfrowanie zakończone sukcesem!");

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
