

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace DES{
    public partial class DesAlgorithm
    {
        public byte[] Encrypt(byte[] text, byte[] byteKey) {

            //key operations - creating 16 subkeys, each of which is 48-bits long
            //1. 64-bit key is permuted according to the table PC-1 and becomes 56-bit permutation
            //2. Key is splitted into left and right halves, where each half has 28 bits
            //3. We're creating 16 blocks C and D. Each pair of blocks is formed from the previous pair. Bits in blocks are shifted left according to "leftShifts" table
            //4. We're merging each pair. Concatenated pairs are permuted again using PC-2 table. Keys on output have 48 bits
            BitArray tmpBitKey = new BitArray(byteKey);

            BitArray permutedKey = performPermutation(RevertBits(tmpBitKey), pc1Table);
            BitArray[] subKeys = MakeSubKeys(permutedKey);
            BitArray[] permutedSubKeys = new BitArray[subKeys.Length];

            for (int i = 0; i < subKeys.Length; i++) {
                permutedSubKeys[i] = performPermutation(subKeys[i], pc2Table);
            }

            //operacje tekstowe - kodowanie 64-bitowych bloków danych
            //1. Poczatkowa permutacja 64-bitowego bloku kodu
            //2. Podzielenie bloku kodu z kroku 1 na lewa i prawa część
            //3. Przejście przez 16 iteracji, używajac zaszyfrowanego, 48-bitowego klucza oraz połówek bloku danych (po 32 bity), wg wzoru L- lewa połówka, R- prawa, K - klucz
            //   Ln = R(n-1)
            //   Rn = L(n-1) + feistel(R(n-1),Kn)
            //
            //   Funkcja Feistela dla prawej połówki: 
            //   3.1 Permutacja prawej połówki używajac tablicy eBit - prawa połówka ma teraz 48 bitów
            //   3.2 Wykonujemy operację XOR na kluczu 48-bitowym i prawej połówce blocku danych
            //   3.3 Dzielimy blok z punktu 3.2 na 8 6-bitowych podbloków, na każdym z podbloków wykonujemy obliczenia Sboxów.
            //      3.3.1 Na wejściu mamy 6 bitów - pierwszy i ostatni bit w systemie binarnym wyznacza numer wiersza Sboxa, zaś 4 środkowe bity wyznaczaja numer kolumny Sboxa,
            //            na tej podstawie pobieramy wartości z każdego Sboxa (czyli na wyjściu Sboxa mamy 4 bity)
            //   3.4 Po obliczeniu Sboxow mamy 8x4 = 32 bity, po ich otrzymaniu wykonujemy permutację za pomoca tabeli pTable.
            //4. Ostatnia wyznaczon parę połówek bloków zamieniamy miejscami i łaczymy (tj. zamiars LR będzie RL) co daje nam 64 bity
            //5. Wykonujemy ostateczna permutacje na wyniku z 4 punktu przy użyci tabeli finalIPTable - to jest zaszyfrowany tekst naszego 64-bitowego bloku

            double parts = Math.Ceiling(text.Length / (64.0 / 8.0));
            byte[] result = new byte[Convert.ToInt32(parts) * (64 / 8)];

            int k = 0;

            for (int i = 0; i < Convert.ToInt32(parts); i++) {

                BitArray binaryText = new BitArray(CopyBits(text, i * (64 / 8), (64 / 8)));
                RevertBits(binaryText);

                BitArray permutedText = performPermutation(binaryText, ipTable);
                BitArray codedText = CodeText(permutedText, permutedSubKeys);
                codedText = performPermutation(codedText, finalIPTable);

                byte[] encyrptedText = ConvertBitsToByteWithRevert(codedText);

                AddByteArray(ref result, encyrptedText, k);
                k += encyrptedText.Length;
            }

            return result;
        }

        public byte[] Decrypt(byte[] text, byte[] key) {

            //key operations
            BitArray tmpBitKey = new BitArray(key);

            BitArray permutedKey = performPermutation(RevertBits(tmpBitKey), pc1Table);
            BitArray[] subKeys = MakeSubKeys(permutedKey);
            BitArray[] permutedSubKeys = new BitArray[subKeys.Length];
            for (int i = 0; i < subKeys.Length; i++) {
                permutedSubKeys[i] = performPermutation(subKeys[subKeys.Length - i - 1], pc2Table);
            }

            //text operations
            double parts = Math.Ceiling(text.Length / (64.0 / 8.0));

            byte[] result = new byte[Convert.ToInt32(parts) * (64 / 8)];

            int k = 0;
            for (int i = 0; i < Convert.ToInt32(parts); i++) {

                BitArray binaryText = new BitArray(CopyBits(text, i * (64 / 8), (64 / 8)));

                RevertBits(binaryText);

                BitArray permutedText = performPermutation(binaryText, ipTable);

                BitArray encodedText = CodeText(permutedText, permutedSubKeys);

                encodedText = performPermutation(encodedText, finalIPTable);

                byte[] decryptedText = ConvertBitsToByteWithRevert(encodedText);
                AddByteArray(ref result, decryptedText, k);
                k += decryptedText.Length;

            }

            return result;
        }

        private BitArray MoveBitsLeft(BitArray array, int shift) {
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
		
		private BitArray MergeArrays(BitArray array1, BitArray array2) {
			BitArray mergedArray = new BitArray(array1.Length + array2.Length);
			for (int i = 0; i < array1.Length; i++) {
				mergedArray[i] = array1.Get(i);
			}
			for (int i = 0; i < array2.Length; i++) {
				mergedArray[i+array1.Length] = array2.Get(i);
			}
			return mergedArray;
		}

		private BitArray performPermutation(BitArray array, int[] table) {
			BitArray permutedArray = new BitArray(table.Length, false);
			
			for (int i = 0; i < table.Length; i++) {
				permutedArray[i] = array[ table[i]-1 ];
			}

			return permutedArray;
		}

		private BitArray getSBoxValue(int row, int col, int i) {
			int newValue = 0;
			row *= 16;
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
			
			byte[] bytes = BitConverter.GetBytes((char)newValue);
			byte[] tmpB = new byte[1];
			tmpB[0] = bytes[0];

			BitArray tmp = new BitArray(tmpB);
			BitArray result = new BitArray(4);
			result[0] = tmp[0];
			result[1] = tmp[1];
			result[2] = tmp[2];
			result[3] = tmp[3];

			return Revert4Bits(ref result);
		}

		private BitArray performFeistelFunction(BitArray rightText, BitArray key) {

			BitArray permutedRightText = performPermutation(rightText, eBitTable);
			
			permutedRightText.Xor(key);

			int bitCounter = 0;
			BitArray result = new BitArray(32, false);
			for (int i = 0; i < permutedRightText.Length; i+=6) {
				BitArray row = new BitArray(2);
				row[0] = permutedRightText[i + 5];
				row[1] = permutedRightText[i];
				
				BitArray col = new BitArray(4);
				col[0] = permutedRightText[i + 4];
				col[1] = permutedRightText[i + 3];
				col[2] = permutedRightText[i + 2];
				col[3] = permutedRightText[i + 1];
				BitArray SBoxValue = getSBoxValue(Convert.ToInt32(ConvertBitsToByteWithoutReverse(row)[0]), Convert.ToInt32(ConvertBitsToByteWithoutReverse(col)[0]), i / 6);
				
				CopyBits(ref result, SBoxValue, bitCounter);
				bitCounter += SBoxValue.Length;
			}

			return performPermutation(result, pTable);
		}

		private byte[] CopyBits(byte[] array, int start, int counter) {

			byte[] result = new byte[counter];

			for (int i = 0; i < counter; i++) {
				if (start + i >= array.Length) {
					result[i] = new byte();	
				} else {
					result[i] = array[start + i];
				}
			}

			return result;
		}

		private byte[] AddByteArray( ref byte[] target, byte[] source, int start) {

			for (int i = 0; i < source.Length; i++) {
				if (target.Length <= start + i)
					break;
				target[start + i] = source[i];
			}

			return target;
		}

		private BitArray CopyBits(ref BitArray target, BitArray source, int start) {

			for (int i = 0; i < source.Length; i++) {
				if (target.Length <= start + i)
					break;
				target[start + i] = source[i];
			}

			return target;
		}

		public byte[] ConvertBitsToByteWithRevert(BitArray bits) {
			int numBytes = bits.Count / 8;

			if (bits.Count % 8 != 0) {
                numBytes++;
            }

			byte[] bytes = new byte[numBytes];
			int byteIndex = 0, bitIndex = 0;

			for (int i = 0; i < bits.Count; i++) {

				if (bits[i]) {
                    bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));
                }

				bitIndex++;

				if (bitIndex == 8) {
					bitIndex = 0;
					byteIndex++;
				}
			}

			return bytes;
		}

		public byte[] ConvertBitsToByteWithoutReverse(BitArray bits) {
			byte[] bytes = new byte[bits.Length];
			bits.CopyTo(bytes, 0);
			return bytes;
		}

        public string ConvertBytesToHexString(byte[] bytes) {
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++) {
                int b = bytes[i];
                chars[i * 2] = hexChars[b >> 4];
                chars[i * 2 + 1] = hexChars[b & 0xF];
            }
            return new string(chars);
        }

        private BitArray RevertBits(BitArray tab) {
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

		public byte[] ConvertStringTextToBytes(string text) {

            if ( text.Length % 2 == 1) {
                throw new ArgumentException("Ilość znaków musi być parzysta");
            }

			byte[] result = new byte[text.Length/2];

			for (int i = 0; i < text.Length; i += 2) {
				string tmp = text.Substring(i, 2);
				int value = int.Parse(tmp, System.Globalization.NumberStyles.HexNumber, null);
				byte[] bytes = BitConverter.GetBytes((char)value);
				result[i / 2] = bytes[0];
			}

			return result;
		}

		private BitArray[] MakeSubKeys(BitArray wholeKey) {

            BitArray[] mergedKeys = new BitArray[16];
			int keyLength = wholeKey.Length;

			BitArray tempKey = new BitArray(2, false);
			BitArray leftKey = new BitArray(keyLength / 2);
			BitArray rightKey = new BitArray(keyLength / 2);

			for (int i = 0; i < keyLength / 2; i++) {
				leftKey[i] = wholeKey.Get(i);
			}

			for (int i = (keyLength / 2); i < keyLength; i++) {
				rightKey[i - (keyLength / 2)] = wholeKey.Get(i);
			}
			
			tempKey[0] = leftKey[0];
			leftKey = MoveBitsLeft(leftKey, leftShifts[0]);
			leftKey[leftKey.Length-1] = tempKey[0];

			tempKey[0] = rightKey[0];
			rightKey = MoveBitsLeft(rightKey, leftShifts[0]);
			rightKey[rightKey.Length-1] = tempKey[0];

			mergedKeys[0] = MergeArrays(leftKey,rightKey);
			
			for (int i = 1; i < 16; i++) {
				
				for (int k = 0; k < keyLength / 2; k++) {
					leftKey[k] = mergedKeys[i-1].Get(k);
				}

				for (int k = (keyLength / 2); k < keyLength; k++) {
					rightKey[k - (keyLength / 2)] = mergedKeys[i-1].Get(k);
				}

				for (int k = 0; k < leftShifts[i]; k++) {
					tempKey[k] = leftKey.Get(k);
				}

				leftKey = MoveBitsLeft(leftKey, leftShifts[i]);

				for (int k = 0; k < leftShifts[i]; k++) {
					leftKey[leftKey.Length-leftShifts[i]+k] = tempKey.Get(k);
				}

				for (int k = 0; k < leftShifts[i]; k++) {
					tempKey[k] = rightKey.Get(k);
				}

				rightKey = MoveBitsLeft(rightKey, leftShifts[i]);

				for (int k = 0; k < leftShifts[i]; k++) {
					rightKey[rightKey.Length - leftShifts[i] + k] = tempKey.Get(k);
				}

				mergedKeys[i] = MergeArrays(leftKey, rightKey);

			}

			return mergedKeys;
		}

		private BitArray CodeText(BitArray text, BitArray[] keys) {

			int textLength = text.Length;

			BitArray leftKey = new BitArray(textLength / 2);
			BitArray rightKey = new BitArray(textLength / 2);

			for (int i = 0; i < textLength / 2; i++) {
				leftKey[i] = text.Get(i);
			}

			for (int i = (textLength / 2); i < textLength; i++) {
				rightKey[i - (textLength / 2)] = text.Get(i);
			}

			BitArray tempKey = new BitArray(textLength / 2);

			for (int i = 0; i < 16; i++) {
				tempKey = leftKey;
				leftKey = rightKey;
				rightKey = tempKey.Xor(performFeistelFunction(leftKey, keys[i]));
											
			}

			return MergeArrays(rightKey,leftKey);
		}





    }
}
