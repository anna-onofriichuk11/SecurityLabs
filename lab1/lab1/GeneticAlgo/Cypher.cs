using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgo
{
    public static class Cypher
    {
        public static string Decrypt(string str, string key)
        {
            string text = str.Trim().ToUpper();
            string decryptedText = "";

            Dictionary<char, int> indexInKey = new Dictionary<char, int>();
            for (int i = 0; i < key.Length; i++)
                indexInKey[key[i]] = i;

            foreach (char c in text)
            {
                decryptedText += (char)('A' + indexInKey[c]);
            }

            return decryptedText;
        }
    }
}
