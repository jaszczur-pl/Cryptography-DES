

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DES{
    public partial class DesAlgorithm
    {

//---------------------------------------------------------------------------------------------------------------------------------------
		/* przesuniecie "bitowe" w lewo
		 * operacja bezpieczna tylko dla liczb dodatnich, poprawnosc w tym wypadku nie jest sprawdzana
		 * dane bierzemy do tablicy opisującej ilosć przesunieć,
		 * metoda prywatna wykorzystywana tylko wewnetrz klasy do tworzenia kluczy
		 */
		private BitArray moveBitsLeft(BitArray array, int shift) {//niestety klasa wbudowana w C# nie zapewnia tego typu operacji
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
		private BitArray zlacz(BitArray tab1, BitArray tab2) {
			BitArray wynik = new BitArray(tab1.Length + tab2.Length);
			for (int i = 0; i < tab1.Length; i++) {
				wynik[i] = tab1.Get(i);
			}
			for (int i = 0; i < tab2.Length; i++) {
				wynik[i+tab1.Length] = tab2.Get(i);
			}
			return wynik;
		}
//---------------------------------------------------------------------------------------------------------------------------------------
		/* dokonuje permutacji zawartości tablicy,
		 * czyli po ludzku, dokonuje zamiany bitów wedle określonego wzorca(mapy),
		 * w wyniku tej operacji powstajemy nową tablica bitów, która może mieć inny rozmiar od wejsciowego
		 * do tego bity w tablicy wyjściowej są uzyskane z tablicy wejściowej, w porzadku narzuconym przez mape
		 * zakładamy że właściwa map jest podłaczana do właściwej tablicy,
		 * w przypadku niewłaściwej mapy, może zostać zgłoszony wyjątek przekroczenia zakresu
		 */
		private BitArray performPermutation(BitArray array, int[] map) {
			BitArray result = new BitArray(map.Length, false);
			
			for (int i = 0; i < map.Length; i++) {
				result[i] = array[ map[i]-1 ];
			}

			return result;
		}

//---------------------------------------------------------------------------------------------------------------------------------------
		/* operacje z użyciem sboxów,
		 * dokonujemy zamiany wartości z tablicy na te odpowiadajace im z sboxa(oczywiście odpowiedniego)
		 * numer iteracji i służy do wyboru sboksa, wiersze i kolumny w tym wypadku są numerowane po bożemu czyli od zera
		 */
		private BitArray sbox(int row, int col, int i) {
			int zamiennik = 0;
			row *= 16;//w wierszu jest 16 liczb, my trzymamy wszystko w tablicy a nie macierzy, zatem trzeba dokonać odpowiedniego przesunięcia
			switch (i) {
				case 0:
					zamiennik = sBox1[row + col];
					break;
				case 1:
					zamiennik = sBox2[row + col];
					break;
				case 2:
					zamiennik = sBox3[row + col];
					break;
				case 3:
					zamiennik = sBox4[row + col];
					break;
				case 4:
					zamiennik = sBox5[row + col];
					break;
				case 5:
					zamiennik = sBox6[row + col];
					break;
				case 6:
					zamiennik = sBox7[row + col];
					break;
				case 7:
					zamiennik = sBox8[row + col];
					break;
			}
			
			byte[] bytes = BitConverter.GetBytes((char)zamiennik);//niestety dostajemy 2 bajty zamiast potrzebnego 1, trzeba go skrócić
			byte[] tmpB = new byte[1];
			tmpB[0] = bytes[0];
			//mamy 1 bajt włąściwy, ale to jest pełny bajt == 8 bitów, nam są potrzebne tylko 4 bity, reszta to i tak zera

			BitArray tmp = new BitArray(tmpB);
			BitArray wynik = new BitArray(4);
			wynik[0] = tmp[0];
			wynik[1] = tmp[1];
			wynik[2] = tmp[2];
			wynik[3] = tmp[3];

			return Revert4Bits(ref wynik);	//odwracamy kolejność zgodnie z przyjętym wewnętrznym porządkiem czytanai bitów od lewej do prawej
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
		private BitArray fun(BitArray l, BitArray klucz) {
			BitArray tmp = performPermutation(l, eTable);//robi też rozszerzenie.(krok 1)
			
			tmp.Xor(klucz);//(krok 2)
			//podział na grupy do sboksów
			int k = 0;
			BitArray wynik = new BitArray(32, false);
			for (int i = 0; i < tmp.Length; i+=6) {// osiem paczek po 6 bitów
				BitArray rowB = new BitArray(2);// bit 0 i 5 stanowią o wierszu z którego należy pobrać wartość
				rowB[0] = tmp[i + 5];
				rowB[1] = tmp[i];//stosujemy tutaj tradycyjne ułozenie bitów i liczenie, od prawej do lewej
				
				BitArray colB = new BitArray(4);//4 bity środkowe wyznaczają numer kolumny z której odczytujemy wartość podstawienia
				colB[0] = tmp[i + 4];
				colB[1] = tmp[i + 3];
				colB[2] = tmp[i + 2];
				colB[3] = tmp[i + 1];
				BitArray zamiennik = sbox(Convert.ToInt32(ToByteArrayCopy(rowB)[0]), Convert.ToInt32(ToByteArrayCopy(colB)[0]), i / 6);
				
				CopyBits(ref wynik, zamiennik, k);//dopisujemy do naszej tablicy bitów odczytane z podstawienia, musimy przecież na koniec mieć 32 bity :)
				k += zamiennik.Length;
			}

			return performPermutation(wynik, p);//permutacja całej połówki
		}
//---------------------------------------------------------------------------------------------------------------------------------------
		/* kopiuje dane(bajty) z tablicy wejściowej od pozycji start, poprzez określoną ilość
		 * zwraca tak utworzoną tablice bajtów
		 * operacja bezpieczna, w razie "skończenia" się tablicy źródłowej wypełniamy reszte bajtów zerami
		 */
		private byte[] CopyBits(byte[] tablica, int start, int ile) {
			byte[] wynik = new byte[ile];
			for (int i = 0; i < ile; i++) {
				if (start + i >= tablica.Length) {
					wynik[i] = new byte();	//uzupełniamy zerami te pozycje, których brakuje 
											//(wykorzystywane, np by dopełniać brakujace 64 bitowe paczki danych do szyfrowania)
				} else {
					wynik[i] = tablica[start + i];
				}
			}
			return wynik;
		}

		/* dopisuje do głównej tablicy bajtów od pozycji startowej całą tablice drugą.
		 * tablice muszą się "mieścić" w sobie inaczej błąd, nie ma tego sprawdzania,
		 * tzn jest częsciowe, bajty które się nie mieszczą zostaną zignorowane
		 * zostanie zwrócone tylko tyle ile udało się zmieścić do tablicy docelowej
		 */
		private byte[] dopisz( ref byte[] target, byte[] source, int start) {
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
		public byte[] ToByteArray(BitArray bits) {
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
		public byte[] ToByteArrayCopy(BitArray bits) {
			byte[] bytes = new byte[bits.Length];
			bits.CopyTo(bytes, 0);
			return bytes;
		}
//---------------------------------------------------------------------------------------------------------------------------------------		
		//tablica wykorzystywana do wypluwania hexymalnych ciagów.
		public char[] hexDigits = {
        '0', '1', '2', '3', '4', '5', '6', '7',
        '8', '9', 'A', 'B', 'C', 'D', 'E', 'F'};

		
		/*tworzy z naszej tablicy bajtów ciąg heksadecymalnych liczb, odpowiadajacy wartością bajtów
		 * przydatne do wypisywanie zakodowanego tekstu, bądź aktualnego stanu jakieś zmiennej, bądź do kontroli poszczególnych kroków
		 */
		public string ToHexString(byte[] bytes) {
			char[] chars = new char[bytes.Length * 2];
			for (int i = 0; i < bytes.Length; i++) {
				int b = bytes[i];
				chars[i * 2] = hexDigits[b >> 4];
				chars[i * 2 + 1] = hexDigits[b & 0xF];
			}
			return new string(chars);
		}

		/* tworzy string (ciąg znaków) w postaci 0 i 1, będący reprezentacją bitową tego co jest w tablicy
		 * wykorzystuje przy tym przyjętą kolejność bitów, także np na ekranie widzimy je w odpowiedniej kolejności
		 * i możemy czytać i od prawej do lewej, jak i od lewej do prawej
		 */
		public string ToBitString(BitArray tab){
			char[] chars = new char[tab.Length];
			for (int i = 0; i < tab.Length; i++) {
				if (tab[i]) {
					chars[i] = '1';
				} else {
					chars[i] = '0';
				}
			}
			return new string(chars);
		}

		/* konwersja ze stringa do tablicy bajtów,
		 * przydatna np do tekstów wprowadzanych z palca, wczytywanie z pliku powinno od razu odbywać się do byte[]
		 * gdyż w ten sposób możemy obsłyżyć pliki binarne.
		 * za pomocą tej metody można pobierać np tekst do szyfrowania i tylko, bo w sumie w innych przypadkach, może ona coś zgubić
		 */
		public byte[] stringToByteArray(string napis){
			byte[] wynik = new byte[napis.Length];
			for (int i = 0; i < napis.Length; i++) {
				wynik[i] = Convert.ToByte(napis[i]);
			}
			return wynik;
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

		/* to co wyżej, tylko że wielkość "bajtu" wynosi tutaj 4 bity
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
		public byte[] GetStringHexKey(string klucz) {
			byte[] wynik = new byte[8];
			for (int i = 0; i < 16; i += 2) {
				string tmp = klucz.Substring(i, 2);
				int hexWart = int.Parse(tmp, System.Globalization.NumberStyles.HexNumber, null);
				byte[] bytes = BitConverter.GetBytes((char)hexWart);
				wynik[i / 2] = bytes[0];
			}
			return wynik;
		}

		/* pobiera najczęściej zakodowany tekst zapisany w postaci heksadecymalnej,
		 * taki tekst możemy bezpiecznie wprowadzać w okienku,
		 * sprowadzamy go do tablicy bajtów, tak jkabyśmy odczytali binarny plik
		 * nie sprawdzamy poprawności danych, czy są wszystkie wprowadzone czy nie
		 * zakładamy że użytkownik na tym etapie to nie idiota i umie wprowadzić wszystkie znaki (czyli 2 znaki na liczbe w hex, np 00, 01, 10, AB)
		 */
		public byte[] GetStringHexText(string text) {
			if( text.Length % 2 == 1 )//sprawdzzmy czy parzyscie znaków, jak nie to szkoda naszej roboty
				throw new ArgumentException("Sprawdź ilość znaków, musi być ona parzysta");
			byte[] wynik = new byte[text.Length/2];
			for (int i = 0; i < text.Length; i += 2) {
				string tmp = text.Substring(i, 2);
				int hexWart = int.Parse(tmp, System.Globalization.NumberStyles.HexNumber, null);
				byte[] bytes = BitConverter.GetBytes((char)hexWart);
				wynik[i / 2] = bytes[0];
			}
			return wynik;
		}
//---------------------------------------------------------------------------------------------------------------------------------------
		/* metoda tworząca na postawie klucza podanego przez użytkownia,
		 * 16 kluczy wykorzystywanych do szyfrowania i deszyfrowania
		 */
		private BitArray[] makeSubKey(BitArray kPlus) {
			BitArray[] wynik = new BitArray[16];

			int keyLength = kPlus.Length;

			BitArray tmp = new BitArray(2, false);//bo maxymalny left shift to 2 pola, więc tyle musimy zrobić machniom z początku

			BitArray c = new BitArray(keyLength / 2);//podział klucza na dwie równe połówki po 32 bity
			BitArray d = new BitArray(keyLength / 2);

			//dzielimy klucz na 2 równe częsci
			for (int i = 0; i < keyLength / 2; i++) {
				c[i] = kPlus.Get(i);
			}
			for (int i = (keyLength / 2); i < keyLength; i++) {
				d[i - (keyLength / 2)] = kPlus.Get(i);
			}
			
			//dokonujemy "left shifts" dla 0 kroku, którego nie można uruchomić w pętli
			
			//zapamiętujemy bity które mają być przesuniete na koniec 
			tmp[0] = c[0];//w pierwszym przesunieciu jest tylko o 1 w lewo, będzie to na stałe wpisane, trudno

			c = moveBitsLeft(c, leftShifts[0]);//przesuniecie
			
			c[c.Length-1] = tmp[0];//wpisanie bitów z początka

			tmp[0] = d[0];
			d = moveBitsLeft(d, leftShifts[0]);//przesuniecie

			d[d.Length-1] = tmp[0];//wpisanie bitów z początka


			wynik[0] = zlacz(c,d);//mamy nasz 1 klucz z 16
			
			
			for (int i = 1; i < 16; i++) {
				
				//dzielimy klucz na 2 równe częsci
				for (int k = 0; k < keyLength / 2; k++) {
					c[k] = wynik[i-1].Get(k);
				}
				for (int k = (keyLength / 2); k < keyLength; k++) {
					d[k - (keyLength / 2)] = wynik[i-1].Get(k);
				}
				//zapamiętanie bajtów do przesuniecia na koniec
				for (int k = 0; k < leftShifts[i]; k++) {
					tmp[k] = c.Get(k);
				}
				c = moveBitsLeft(c, leftShifts[i]);//przesuniecie
				
				//wpisanie na koniec zapamietanych bitów
				for (int k = 0; k < leftShifts[i]; k++) {
					c[c.Length-leftShifts[i]+k] = tmp.Get(k);
				}

				for (int k = 0; k < leftShifts[i]; k++) {
					tmp[k] = d.Get(k);
				}
				d = moveBitsLeft(d, leftShifts[i]);//przesuniecie

				for (int k = 0; k < leftShifts[i]; k++) {
					d[d.Length - leftShifts[i] + k] = tmp.Get(k);
				}

				wynik[i] = zlacz(c, d);

			}
			return wynik;
		}

		/* szyfrujemy/kodujemy naszą paczke 64 bitów danych według odpowiedniego układu kluczy
		 * by odszyfrować dane wystarczy wykonać te same czynności co do szyfrowania, tylko należy te 16 kluczy co wygenerowaliśmy
		 * użyć w przeciwnej kolejności (od 16 .. 1) [szyfrowanie zaś 1..16]
		 */
		private BitArray koduj(BitArray paczka, BitArray[] klucze) {
			int packageLength = paczka.Length;

			//dzielimy na 2 części
			BitArray l = new BitArray(packageLength / 2);
			BitArray r = new BitArray(packageLength / 2);

			//dzielimy klucz na 2 równe częsci
			for (int i = 0; i < packageLength / 2; i++) {
				l[i] = paczka.Get(i);
			}
			for (int i = (packageLength / 2); i < packageLength; i++) {
				r[i - (packageLength / 2)] = paczka.Get(i);
			}

			BitArray t = new BitArray(packageLength / 2);
			//tu musi być 16 inaczej dupa blada :D
			for (int i = 0; i < 16; i++) {
				t = l;
				l = r;
				r = t.Xor(fun(l,klucze[i]));//xor wywołanie metody "4 kroków"
											//(głównie to miksacja z kluczem, podstawienie sboksa i permutacja w skrócie)
			}

			return zlacz(r,l);//zamieniamy kolejność połówek
		}

		/* metoda odpowiedzialna za wykonanie i przygotowanie danych w odpowieniej kolejności do szyfrowania
		 * czyli w skrócie, odpowidnia kolejnosć kluczy zostaje ustawiona
		 */
        public byte[] Encrypt(byte[] co, byte[] kluczB){
			//krok 1.1
			BitArray klucz = new BitArray(kluczB);

			BitArray kPlus = performPermutation(RevertBits(ref klucz), pc1);//permutujemy klucz
			
			//krok 1.2.
			BitArray[] klucze = makeSubKey(kPlus);//generujemy nasz 16 kluczy używanych do operacji
			
			//krok 1.3
			BitArray[] kluczePlus = new BitArray[klucze.Length];//permutacja kluczy, narzucenie właściwego porządku używania
			for (int i = 0; i < klucze.Length; i++) {
				kluczePlus[i] = performPermutation(klucze[i], pc2);
			}

			//krok 2.0
			//mamy już klucze, teraz można przystąpić do kodowania wiadomości
			double parts = Math.Ceiling(co.Length/(64.0/8.0));//wyznaczenie ilości 64 bitowych paczek 

			byte[] wynik = new byte[Convert.ToInt32(parts) * (64/8)];//w tym będziemy trzymać postać wyjściową :)

			int k = 0;
			for (int i = 0; i < Convert.ToInt32(parts); i++) {//każdą część obliczamy
				
				BitArray tmp0 = new BitArray(CopyBits(co, i * (64/8), (64/8)));//zapisujemy tymczosowo nasze 64 bity któe będziemy kodować

				RevertBits(ref tmp0);//musimy zachować przyjęty wewnętrzny porządek

				BitArray tmp = performPermutation(tmp0,ip);//wstępna permutacja

				BitArray tmp2 = koduj(tmp, kluczePlus);//zakodowanie, permutacja, podmiana z sboksami

				tmp2 = performPermutation(tmp2, ipr); //ostatnia permutacja macierzą

				byte[] zaszyfrowane = ToByteArray(tmp2);//przechowujemy dane w postaci byte[] zatem musimy przejść z postaci bitowej
														//do postaci bajtowej, przywracamy przy okazji prawidłowe ułożenie bitów

				dopisz(ref wynik, zaszyfrowane, k);//dopisujemy naszą paczuszkę zaszyfrowaną do całości
				k += zaszyfrowane.Length;//zwiększamy indeks startowy, gdzie mają być dopisywane zakodowane dane do "całości wyjściowej" 
			}

			return wynik;//zwróć całość zaszyfrowaną.
        }

		/* metoda odpowiedzialna za przygotowanie danych i wykonanie w odpowiedniej kolejności metod w celu deszyfracji
		 * de facto to ta sama metoda co szyfrowanie, tylko zamienia się kolejność kluczy jaką się używa,
		 * używa ich się w odwrotnej kolejnosci niż przy szyfrowaniu
		 */
		public byte[] Decrypt(byte[] co, byte[] kluczB) {
			//krok 1.1
			BitArray klucz = new BitArray(kluczB);

			BitArray kPlus = performPermutation(RevertBits(ref klucz), pc1);

			//krok 1.2.
			BitArray[] klucze = makeSubKey(kPlus);

			//krok 1.3
			BitArray[] kluczePlus = new BitArray[klucze.Length];
			for (int i = 0; i < klucze.Length; i++) {
				kluczePlus[i] = performPermutation(klucze[klucze.Length-i-1], pc2);//odwracamy kolejność kluczy, tylko ten krok jest różny, reszta linii taka sama jak w szyfrowaniu
			}

			//krok 2.0
			//mamy już klucze, teraz można przystąpić do kodowania wiadomości
			double parts = Math.Ceiling(co.Length / (64.0 / 8.0));

			byte[] wynik = new byte[Convert.ToInt32(parts) * (64 / 8)];//w tym będziemy trzymać postać wyjściową :)

			int k = 0;
			for (int i = 0; i < Convert.ToInt32(parts); i++) {//każdą część obliczamy

				BitArray tmp0 = new BitArray(CopyBits(co, i * (64 / 8), (64 / 8)));

				RevertBits(ref tmp0);

				BitArray tmp = performPermutation(tmp0, ip);

				BitArray tmp2 = koduj(tmp, kluczePlus);

				tmp2 = performPermutation(tmp2, ipr);

				byte[] odkodowane = ToByteArray(tmp2);
				dopisz(ref wynik, odkodowane, k);
				k += odkodowane.Length;

			}

			return wynik;
		}
    }
}
