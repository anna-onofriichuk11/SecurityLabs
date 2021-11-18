using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static lab1.HelpCLass;
using ConsoleTables;

namespace lab1
{
  public static class Vigenere
    {
      
        public static Dictionary<int, Tuple<double, double>> HammingDistance(byte[] bytearr)
        {
            int maxKeySize = 20;
            int currKeySize = 2;
            var hammingDistance = new Dictionary<int, Tuple<double, double>>();
            while (currKeySize < maxKeySize)
            {
                int bytesCount = 0;
                var distancesList = new List<double>();
                for (int i = 0; i < bytearr.Length; i += currKeySize)
                {
                    if (i + (2 * currKeySize) > bytearr.Length)
                        continue;
                    var bytes1 = new List<byte>();
                    var bytes2 = new List<byte>();

                    for (int j = i; j < i + currKeySize; j++)
                    {
                        bytes1.Add(bytearr[j]);
                    }
                    for (int n = i + currKeySize; n < i + currKeySize + currKeySize; n++)
                    {
                        bytes2.Add(bytearr[n]);
                    }
                    distancesList.Add(VigenereHelpClass.BitwiseHamming(bytes1, bytes2));
                    bytesCount++;
                }
                hammingDistance.Add(currKeySize, new Tuple<double, double>(distancesList.Sum() / bytesCount, (distancesList.Sum() / bytesCount) / currKeySize));
                currKeySize++;
            }

            return hammingDistance;
        }

        public static string Decrypt(string keyLength, string input)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < input.Length; i++)
            {
                sb.Append((char)(input[i] ^ keyLength[i % keyLength.Length]));
            }
            return sb.ToString();
        }

        public static byte DecodeRepeatingXOR(List<byte> bytearr)
        {
            var topSymbols = bytearr.Take(6).ToList();
            var letters = new char[] { 'a', 'e', 'i', 'n', 'o', 't' };
            var letterBytes = letters.Select(c => (byte)c).ToList();

            var listCounts = new List<byte>();
            for (int i = 0; i < letterBytes.Count; i++)
            {
               // Console.WriteLine($"letter: {(char)letterBytes[i]}");
                for (int j = 0; j < topSymbols.Count; j++)
                {
                    var xor = topSymbols[j] ^ letterBytes[i];
                    listCounts.Add(Convert.ToByte(xor));
             //       Console.Write($"\nSymbol: {topSymbols[j]}\t\tXOR: {xor}");
                }
              //  Console.WriteLine();
            }
            var probableKeyLetters = GetByteFrequency(listCounts);
            var table = new ConsoleTable("Symbol", "Count");
            foreach (var dict in probableKeyLetters)
                table.AddRow(Convert.ToChar(dict.Key), dict.Value);
            table.Write();
            return probableKeyLetters.First().Key;
        }

        public static Dictionary<byte, int> GetByteFrequency(List<byte> bytearr)
        {
            var fr = bytearr.GroupBy(c => c)
                        .Select(g => new { Symbol = g.Key, Count = g.Count() })
                        .OrderByDescending(b => b.Count)
                        .ToDictionary(c => c.Symbol, c => c.Count);
            //foreach (var item in freqs)
            //{
            //    Console.WriteLine($"Symbol: {item.Key}, Count: {item.Value}");
            //}
            return fr;
        }

        public static string GetBestKey(byte[] arr, int keyLength)
        {
            var firstPart = VigenereHelpClass.BytePartition(arr.ToList(), 0, keyLength);
            var secondPart = VigenereHelpClass.BytePartition(arr.ToList(), 1, keyLength);
            var thirdPart = VigenereHelpClass.BytePartition(arr.ToList(), 2, keyLength);

            var firstSymbols = Vigenere.GetByteFrequency(firstPart).Keys.ToList();
            var secondSymbols = Vigenere.GetByteFrequency(secondPart).Keys.ToList();
            var thirdSymbols = Vigenere.GetByteFrequency(thirdPart).Keys.ToList();

           // Console.WriteLine("---------------------------------------------first:---------------------------------------------");
            var firstLetter = Vigenere.DecodeRepeatingXOR(firstSymbols);
          //  Console.WriteLine("---------------------------------------------second:---------------------------------------------");
            var secondLetter = Vigenere.DecodeRepeatingXOR(secondSymbols);
          //  Console.WriteLine("---------------------------------------------third:---------------------------------------------");
            var thirdLetter = Vigenere.DecodeRepeatingXOR(thirdSymbols);

            var keyWord = string.Format($"{(char)firstLetter}{(char)secondLetter}{(char)thirdLetter}");
            Console.WriteLine( "KEY WORD " + keyWord);

            return keyWord;
        }
       
    }
}
