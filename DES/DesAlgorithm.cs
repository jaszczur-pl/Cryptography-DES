

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DES{
    public partial class DesAlgorithm
    {
        /* metoda odpowiedzialna za wykonanie i przygotowanie danych w odpowieniej kolejności do szyfrowania
         * czyli w skrócie, odpowidnia kolejnosć kluczy zostaje ustawiona
         */
        public byte[] Encrypt(byte[] text, byte[] byteKey) {
            //krok 1.1
            BitArray tmpBitKey = new BitArray(byteKey);

            BitArray permutedKey = performPermutation(RevertBits(ref tmpBitKey), pc1Table);//permutujemy klucz

            //krok 1.2.
            BitArray[] subKeys = MakeSubKeys(permutedKey);//generujemy nasz 16 kluczy używanych do operacji

            //krok 1.3
            BitArray[] permutedSubKeys = new BitArray[subKeys.Length];//permutacja kluczy, narzucenie właściwego porządku używania
            for (int i = 0; i < subKeys.Length; i++) {
                permutedSubKeys[i] = performPermutation(subKeys[i], pc2Table);
            }

            //krok 2.0
            //mamy już klucze, teraz można przystąpić do kodowania wiadomości
            double parts = Math.Ceiling(text.Length / (64.0 / 8.0));//wyznaczenie ilości 64 bitowych paczek 

            byte[] result = new byte[Convert.ToInt32(parts) * (64 / 8)];//w tym będziemy trzymać postać wyjściową :)

            int k = 0;
            for (int i = 0; i < Convert.ToInt32(parts); i++) {//każdą część obliczamy

                BitArray binaryText = new BitArray(CopyBits(text, i * (64 / 8), (64 / 8)));//zapisujemy tymczosowo nasze 64 bity któe będziemy kodować

                RevertBits(ref binaryText);//musimy zachować przyjęty wewnętrzny porządek

                BitArray permutedText = performPermutation(binaryText, ipTable);//wstępna permutacja

                BitArray codedText = CodeText(permutedText, permutedSubKeys);//zakodowanie, permutacja, podmiana z sboksami

                codedText = performPermutation(codedText, finalIPTable); //ostatnia permutacja macierzą

                byte[] encyrptedText = ConvertBitsToByteWithRevert(codedText);//przechowujemy dane w postaci byte[] zatem musimy przejść z postaci bitowej
                                                        //do postaci bajtowej, przywracamy przy okazji prawidłowe ułożenie bitów

                AddByteArray(ref result, encyrptedText, k);//dopisujemy naszą paczuszkę zaszyfrowaną do całości
                k += encyrptedText.Length;//zwiększamy indeks startowy, gdzie mają być dopisywane zakodowane dane do "całości wyjściowej" 
            }

            return result;//zwróć całość zaszyfrowaną.
        }

        /* metoda odpowiedzialna za przygotowanie danych i wykonanie w odpowiedniej kolejności metod w celu deszyfracji
        * de facto to ta sama metoda text szyfrowanie, tylko zamienia się kolejność kluczy jaką się używa,
        * używa ich się w odwrotnej kolejnosci niż przy szyfrowaniu
        */
        public byte[] Decrypt(byte[] text, byte[] key) {
            //krok 1.1
            BitArray tmpBitKey = new BitArray(key);

            BitArray permutedKey = performPermutation(RevertBits(ref tmpBitKey), pc1Table);

            //krok 1.2.
            BitArray[] subKeys = MakeSubKeys(permutedKey);

            //krok 1.3
            BitArray[] permutedSubKeys = new BitArray[subKeys.Length];
            for (int i = 0; i < subKeys.Length; i++) {
                permutedSubKeys[i] = performPermutation(subKeys[subKeys.Length - i - 1], pc2Table);//odwracamy kolejność kluczy, tylko ten krok jest różny, reszta linii taka sama jak w szyfrowaniu
            }

            //krok 2.0
            //mamy już klucze, teraz można przystąpić do kodowania wiadomości
            double parts = Math.Ceiling(text.Length / (64.0 / 8.0));

            byte[] result = new byte[Convert.ToInt32(parts) * (64 / 8)];//w tym będziemy trzymać postać wyjściową :)

            int k = 0;
            for (int i = 0; i < Convert.ToInt32(parts); i++) {//każdą część obliczamy

                BitArray binaryText = new BitArray(CopyBits(text, i * (64 / 8), (64 / 8)));

                RevertBits(ref binaryText);

                BitArray permutedText = performPermutation(binaryText, ipTable);

                BitArray encodedText = CodeText(permutedText, permutedSubKeys);

                encodedText = performPermutation(encodedText, finalIPTable);

                byte[] decryptedText = ConvertBitsToByteWithRevert(encodedText);
                AddByteArray(ref result, decryptedText, k);
                k += decryptedText.Length;

            }

            return result;
        }
        //---------------------------------------------------------------------------------------------------------------------------------------
        /* przesuniecie "bitowe" w lewo
		 * operacja bezpieczna tylko dla liczb dodatnich, poprawnosc w tym wypadku nie jest sprawdzana
		 * dane bierzemy do tablicy opisującej ilosć przesunieć,
		 * metoda prywatna wykorzystywana tylko wewnetrz klasy do tworzenia kluczy
		 */
        private BitArray MoveBitsLeft(BitArray array, int shift) {//niestety klasa wbudowana w C# nie zapewnia tego typu operacji
			BitArray result = new BitArray(array.Length, false);
			for (int i = 0; i < array.Length; i++) {
				if (i < array.Length - shift) {
					result[i] = array[i + shift];
				} else {
					result.Set(i, false);
				}
			}
			
			return result;
		}
		
		/* złcza 2 tablice bitów w jedną, konkatenacja tab1+tab2
		 * długość tablic dowolna, tablica wyjściowa to rozmiar 1 + rozmia 2
		 * operacja dość powolna, sporo pamięci idzie, 
		 * byś może jest wbudowana, ale ta działa w stopniu wystarczającym
		 */
		private BitArray MergeArrays(BitArray tab1, BitArray tab2) {
			BitArray result = new BitArray(tab1.Length + tab2.Length);
			for (int i = 0; i < tab1.Length; i++) {
				result[i] = tab1.Get(i);
			}
			for (int i = 0; i < tab2.Length; i++) {
				result[i+tab1.Length] = tab2.Get(i);
			}
			return result;
		}
//---------------------------------------------------------------------------------------------------------------------------------------
		/* dokonuje permutacji zawartości tablicy,
		 * czyli po ludzku, dokonuje zamiany bitów wedle określonego wzorca(mapy),
		 * w wyniku tej operacji powstajemy nową tablica bitów, która może mieć inny rozmiar od wejsciowego
		 * do tego bity w tablicy wyjściowej są uzyskane z tablicy wejściowej, w porzadku narzuconym przez mape
		 * zakładamy że właściwa map jest podłaczana do właściwej tablicy,
		 * w przypadku niewłaściwej mapy, może zostać zgłoszony wyjątek przekroczenia zakresu
		 */
		private BitArray performPermutation(BitArray array, int[] table) {
			BitArray result = new BitArray(table.Length, false);
			
			for (int i = 0; i < table.Length; i++) {
				result[i] = array[ table[i]-1 ];
			}

			return result;
		}

//---------------------------------------------------------------------------------------------------------------------------------------
		/* operacje z użyciem sboxów,
		 * dokonujemy zamiany wartości z tablicy na te odpowiadajace im z sboxa(oczywiście odpowiedniego)
		 * numer iteracji i służy do wyboru sboksa, wiersze i kolumny w tym wypadku są numerowane po bożemu czyli od zera
		 */
		private BitArray getSBoxValue(int row, int col, int i) {
			int newValue = 0;
			row *= 16;//w wierszu jest 16 liczb, my trzymamy wszystko w tablicy a nie macierzy, zatem trzeba dokonać odpowiedniego przesunięcia
			switch (i) {
				case 0:
					newValue = sBox1[row + col];
					break;
				case 1:
					newValue = sBox2[row + col];
					break;
				case 2:
					newValue = sBox3[row + col];
					break;
				case 3:
					newValue = sBox4[row + col];
					break;
				case 4:
					newValue = sBox5[row + col];
					break;
				case 5:
					newValue = sBox6[row + col];
					break;
				case 6:
					newValue = sBox7[row + col];
					break;
				case 7:
					newValue = sBox8[row + col];
					break;
			}
			
			byte[] bytes = BitConverter.GetBytes((char)newValue);//niestety dostajemy 2 bajty zamiast potrzebnego 1, trzeba go skrócić
			byte[] tmpB = new byte[1];
			tmpB[0] = bytes[0];
			//mamy 1 bajt włąściwy, ale to jest pełny bajt == 8 bitów, nam są potrzebne tylko 4 bity, reszta to i tak zera

			BitArray tmp = new BitArray(tmpB);
			BitArray result = new BitArray(4);
			result[0] = tmp[0];
			result[1] = tmp[1];
			result[2] = tmp[2];
			result[3] = tmp[3];

			return Revert4Bits(ref result);	//odwracamy kolejność zgodnie z przyjętym wewnętrznym porządkiem czytanai bitów od lewej do prawej
										//sposób ten ułatwia nam zrozumienie i łatwiejsze operowanie na bitach 
										//(szczególnie przydatne to było przy debugowaniu)
		}

		/* funkcja operująca na 32 bitach (połówka pakietu danych)
		 * wykonuje 4 niezbędne czynności do zakodowania wiadomości:
		 * 1. rozszerzenie (nazwa moze mylić, że permutacja, ale ta metoda się do tego nadaje)
		 * 2. miksowanie klucza
		 * 3. podstawienie wartości z Sboksa
		 * 4. permutacja
		 */
		private BitArray performFeistelFunction(BitArray leftKey, BitArray key) {
			BitArray tmp = performPermutation(leftKey, eBitTable);//robi też rozszerzenie.(krok 1)
			
			tmp.Xor(key);//(krok 2)
			//podział na grupy do sboksów
			int k = 0;
			BitArray result = new BitArray(32, false);
			for (int i = 0; i < tmp.Length; i+=6) {// osiem paczek po 6 bitów
				BitArray rowB = new BitArray(2);// bit 0 i 5 stanowią o wierszu z którego należy pobrać wartość
				rowB[0] = tmp[i + 5];
				rowB[1] = tmp[i];//stosujemy tutaj tradycyjne ułozenie bitów i liczenie, od prawej do lewej
				
				BitArray colB = new BitArray(4);//4 bity środkowe wyznaczają numer kolumny z której odczytujemy wartość podstawienia
				colB[0] = tmp[i + 4];
				colB[1] = tmp[i + 3];
				colB[2] = tmp[i + 2];
				colB[3] = tmp[i + 1];
				BitArray SBoxValue = getSBoxValue(Convert.ToInt32(ConvertBitsToByteWithoutReverse(rowB)[0]), Convert.ToInt32(ConvertBitsToByteWithoutReverse(colB)[0]), i / 6);
				
				CopyBits(ref result, SBoxValue, k);//dopisujemy do naszej tablicy bitów odczytane z podstawienia, musimy przecież na koniec mieć 32 bity :)
				k += SBoxValue.Length;
			}

			return performPermutation(result, pTable);//permutacja całej połówki
		}
//---------------------------------------------------------------------------------------------------------------------------------------
		/* kopiuje dane(bajty) z tablicy wejściowej od pozycji start, poprzez określoną ilość
		 * zwraca tak utworzoną tablice bajtów
		 * operacja bezpieczna, w razie "skończenia" się tablicy źródłowej wypełniamy reszte bajtów zerami
		 */
		private byte[] CopyBits(byte[] array, int start, int counter) {
			byte[] result = new byte[counter];
			for (int i = 0; i < counter; i++) {
				if (start + i >= array.Length) {
					result[i] = new byte();	//uzupełniamy zerami te pozycje, których brakuje 
											//(wykorzystywane, np by dopełniać brakujace 64 bitowe paczki danych do szyfrowania)
				} else {
					result[i] = array[start + i];
				}
			}
			return result;
		}

		/* dopisuje do głównej tablicy bajtów od pozycji startowej całą tablice drugą.
		 * tablice muszą się "mieścić" w sobie inaczej błąd, nie ma tego sprawdzania,
		 * tzn jest częsciowe, bajty które się nie mieszczą zostaną zignorowane
		 * zostanie zwrócone tylko tyle ile udało się zmieścić do tablicy docelowej
		 */
		private byte[] AddByteArray( ref byte[] target, byte[] source, int start) {
			for (int i = 0; i < source.Length; i++) {
				if (target.Length <= start + i)
					break;
				target[start + i] = source[i];
			}
			return target;
		}

		/*kopiuje bity z jednej tablicy do drugiej, przy czym kopiowana jest cała tablica źródłowa do docelowej,
		 * miejsce początkowe w tablicy docelowej wyznaczane jest poprzez "start"
		 * w przypadku przekroczenia zakresu, bity z tablicy źródłowej są pomijane
		 * metodo praktycznie jak ta powyżej ino dla bitów, w sumie też mogłą nazwyać się dołącz, ale powstała w innym czasie)
		 */
		private BitArray CopyBits(ref BitArray target, BitArray source, int start) {
			for (int i = 0; i < source.Length; i++) {
				if (target.Length <= start + i)
					break;
				target[start + i] = source[i];
			}
			return target;
		}

		/* przepisanie z tablicy bitów do tablicy bajtów (z nią można wiecej),
		 * "naprawiana" przy tym jest kolejność bitów, która w klasie zosałą zmieniona,
		 * czytaj w klasie uznajemy, że numerowanie bitów jest od lewej do prawej, czyli odwrotnie niż w rzeczywistości
		 */
		public byte[] ConvertBitsToByteWithRevert(BitArray bits) {
			int numBytes = bits.Count / 8;
			if (bits.Count % 8 != 0)
				numBytes++;

			byte[] bytes = new byte[numBytes];
			int byteIndex = 0, bitIndex = 0;

			for (int i = 0; i < bits.Count; i++) {
				if (bits[i])
					bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

				bitIndex++;
				if (bitIndex == 8) {
					bitIndex = 0;
					byteIndex++;
				}
			}

			return bytes;
		}

		/* przepisane z tablicy bitów do tablicy bajtów, kolejnosć bitów zachowana,
		 * czyli numeracja od prawej do lewej,
		 * jeśli chcemy używać potem tego gdzieś wewnątrz klasy, należy najpierw odwrócić odwrócić bity
		 */
		public byte[] ConvertBitsToByteWithoutReverse(BitArray bits) {
			byte[] bytes = new byte[bits.Length];
			bits.CopyTo(bytes, 0);
			return bytes;
		}
        //---------------------------------------------------------------------------------------------------------------------------------------		

        //*tworzy z naszej tablicy bajtów ciąg heksadecymalnych liczb, odpowiadajacy wartością bajtów
        // * przydatne do wypisywanie zakodowanego tekstu, bądź aktualnego stanu jakieś zmiennej, bądź do kontroli poszczególnych kroków
        // */
        public string ConvertBytesToHexString(byte[] bytes) {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++) {
                int b = bytes[i];
                chars[i * 2] = hexDigits[b >> 4];
                chars[i * 2 + 1] = hexDigits[b & 0xF];
            }
            return new string(chars);
        }

        /* odwracamy kolejność bitów w bajcie, rozmiar paczki to 8 bitów (tzn może być wiecej elementów, ale 8 bitowe paczki są odwracane)
		 * wykorzystywane do uproszczenia rozumowania, zamieniono kolejność oznaczenia bajtów - od lewej do prawej,
		 * zamiast prawidłowego od prawej do lewej)
		 */
        private BitArray RevertBits(ref BitArray tab) {
			for (int i = 0; i < tab.Length; i = i + 8) {
				bool tmp = false;
				for (int j = 0; j < 4; j++) {
					tmp = tab[i + j];
					tab[i + j] = tab[i + 7 - j];
					tab[i + 7 - j] = tmp;
				}

			}
			return tab;
		}

		/* to text wyżej, tylko że wielkość "bajtu" wynosi tutaj 4 bity
		 */
		private BitArray Revert4Bits(ref BitArray tab) {
			for (int i = 0; i < tab.Length; i = i + 4) {
				bool tmp = false;
				for (int j = 0; j < 2; j++) {
					tmp = tab[i + j];
					tab[i + j] = tab[i + 3 - j];
					tab[i + 3 - j] = tmp;
				}

			}
			return tab;
		}

		/* pobiera ciąg znaków (string) w którym jest zapisany w postaci heksadecymalnej klucz
		 * długość tego ciągu to 16 znaków (2 znaki na liczbe), czyli 8 "liczb", zatem prawidłowe 64 bity klucza
		 */
		public byte[] ConvertHexStringKeyToBytes(string klucz) {
			byte[] result = new byte[8];
			for (int i = 0; i < 16; i += 2) {
				string tmp = klucz.Substring(i, 2);
				int value = int.Parse(tmp, System.Globalization.NumberStyles.HexNumber, null);
				byte[] bytes = BitConverter.GetBytes((char)value);
				result[i / 2] = bytes[0];
			}
			return result;
		}

		/* pobiera najczęściej zakodowany tekst zapisany w postaci heksadecymalnej,
		 * taki tekst możemy bezpiecznie wprowadzać w okienku,
		 * sprowadzamy go do tablicy bajtów, tak jkabyśmy odczytali binarny plik
		 * nie sprawdzamy poprawności danych, czy są wszystkie wprowadzone czy nie
		 * zakładamy że użytkownik na tym etapie to nie idiota i umie wprowadzić wszystkie znaki (czyli 2 znaki na liczbe w hex, np 00, 01, 10, AB)
		 */
		public byte[] ConvertStringTextToBytes(string text) {
			if( text.Length % 2 == 1 )//sprawdzzmy czy parzyscie znaków, jak nie to szkoda naszej roboty
				throw new ArgumentException("Ilość znaków musi być parzysta");
			byte[] result = new byte[text.Length/2];
			for (int i = 0; i < text.Length; i += 2) {
				string tmp = text.Substring(i, 2);
				int value = int.Parse(tmp, System.Globalization.NumberStyles.HexNumber, null);
				byte[] bytes = BitConverter.GetBytes((char)value);
				result[i / 2] = bytes[0];
			}
			return result;
		}
//---------------------------------------------------------------------------------------------------------------------------------------
		/* metoda tworząca na postawie klucza podanego przez użytkownia,
		 * 16 kluczy wykorzystywanych do szyfrowania i deszyfrowania
		 */
		private BitArray[] MakeSubKeys(BitArray wholeKey) {
			BitArray[] result = new BitArray[16];

			int keyLength = wholeKey.Length;

			BitArray tempKey = new BitArray(2, false);//bo maxymalny left shift to 2 pola, więc tyle musimy zrobić machniom z początku

			BitArray leftKey = new BitArray(keyLength / 2);//podział klucza na dwie równe połówki po 32 bity
			BitArray rightKey = new BitArray(keyLength / 2);

			//dzielimy klucz na 2 równe częsci
			for (int i = 0; i < keyLength / 2; i++) {
				leftKey[i] = wholeKey.Get(i);
			}
			for (int i = (keyLength / 2); i < keyLength; i++) {
				rightKey[i - (keyLength / 2)] = wholeKey.Get(i);
			}
			
			//dokonujemy "left shifts" dla 0 kroku, którego nie można uruchomić w pętli
			
			//zapamiętujemy bity które mają być przesuniete na koniec 
			tempKey[0] = leftKey[0];//w pierwszym przesunieciu jest tylko o 1 w lewo, będzie to na stałe wpisane, trudno

			leftKey = MoveBitsLeft(leftKey, leftShifts[0]);//przesuniecie
			
			leftKey[leftKey.Length-1] = tempKey[0];//wpisanie bitów z początka

			tempKey[0] = rightKey[0];
			rightKey = MoveBitsLeft(rightKey, leftShifts[0]);//przesuniecie

			rightKey[rightKey.Length-1] = tempKey[0];//wpisanie bitów z początka


			result[0] = MergeArrays(leftKey,rightKey);//mamy nasz 1 klucz z 16
			
			
			for (int i = 1; i < 16; i++) {
				
				//dzielimy klucz na 2 równe częsci
				for (int k = 0; k < keyLength / 2; k++) {
					leftKey[k] = result[i-1].Get(k);
				}
				for (int k = (keyLength / 2); k < keyLength; k++) {
					rightKey[k - (keyLength / 2)] = result[i-1].Get(k);
				}
				//zapamiętanie bajtów do przesuniecia na koniec
				for (int k = 0; k < leftShifts[i]; k++) {
					tempKey[k] = leftKey.Get(k);
				}
				leftKey = MoveBitsLeft(leftKey, leftShifts[i]);//przesuniecie
				
				//wpisanie na koniec zapamietanych bitów
				for (int k = 0; k < leftShifts[i]; k++) {
					leftKey[leftKey.Length-leftShifts[i]+k] = tempKey.Get(k);
				}

				for (int k = 0; k < leftShifts[i]; k++) {
					tempKey[k] = rightKey.Get(k);
				}
				rightKey = MoveBitsLeft(rightKey, leftShifts[i]);//przesuniecie

				for (int k = 0; k < leftShifts[i]; k++) {
					rightKey[rightKey.Length - leftShifts[i] + k] = tempKey.Get(k);
				}

				result[i] = MergeArrays(leftKey, rightKey);

			}
			return result;
		}

		/* szyfrujemy/kodujemy naszą paczke 64 bitów danych według odpowiedniego układu kluczy
		 * by odszyfrować dane wystarczy wykonać te same czynności text do szyfrowania, tylko należy te 16 kluczy text wygenerowaliśmy
		 * użyć w przeciwnej kolejności (od 16 .. 1) [szyfrowanie zaś 1..16]
		 */
		private BitArray CodeText(BitArray text, BitArray[] keys) {
			int textLength = text.Length;

			//dzielimy na 2 części
			BitArray leftKey = new BitArray(textLength / 2);
			BitArray rightKey = new BitArray(textLength / 2);

			//dzielimy klucz na 2 równe częsci
			for (int i = 0; i < textLength / 2; i++) {
				leftKey[i] = text.Get(i);
			}
			for (int i = (textLength / 2); i < textLength; i++) {
				rightKey[i - (textLength / 2)] = text.Get(i);
			}

			BitArray tempKey = new BitArray(textLength / 2);
			//tu musi być 16 inaczej dupa blada :D
			for (int i = 0; i < 16; i++) {
				tempKey = leftKey;
				leftKey = rightKey;
				rightKey = tempKey.Xor(performFeistelFunction(leftKey, keys[i]));//xor wywołanie metody "4 kroków"
											//(głównie to miksacja z kluczem, podstawienie sboksa i permutacja w skrócie)
			}

			return MergeArrays(rightKey,leftKey);//zamieniamy kolejność połówek
		}





    }
}
