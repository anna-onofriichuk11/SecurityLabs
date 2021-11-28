using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticAlgo
{
    public class Pool
    {
        public List<PKey> PossibleKeysList { get; set; }
        public int PoolSize { get; set; }
        const string ENG_ALPHABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        static Random rand = new Random();
        public string EncryptedText { get; set; }

        public Pool(int poolSize, string encryptedText)
        {
            PoolSize = poolSize;
            EncryptedText = encryptedText;
            PossibleKeysList = new List<PKey>();
        }

        public string CreateRandomKey()
        {
            char[] key = ENG_ALPHABET.ToCharArray();
            for (int i = 0; i < key.Length; i++)
            {
                char c = key[i];
                int shuffleIndex = rand.Next(key.Length - i) + i;
                key[i] = key[shuffleIndex];
                key[shuffleIndex] = c;
            }
            return new string(key);
        }

        public void CreatePool()
        {
            for (int i = 0; i < PoolSize; i++)
            {
                PossibleKeysList.Add(new PKey(CreateRandomKey(), EncryptedText));
            }
        }

        public void EnsureDistinctPool()
        {
            var isDistinct = false;
            while (!isDistinct)
            {
                var listOfKeys = PossibleKeysList;
                var duplicateKeys = listOfKeys.GroupBy(x => x.KeyValue)
                                                .Where(group => group.Count() > 1)
                                                .Select(group => group.Key);
                if (duplicateKeys.Count() != 0)
                {
                    foreach (var key in duplicateKeys)
                    {
                        var newKey = CreateRandomKey();
                        listOfKeys[listOfKeys.FindIndex(ind => ind.KeyValue.Equals(key))] = new PKey(newKey, EncryptedText);
                    }
                    PossibleKeysList = listOfKeys;
                }
                else isDistinct = true;
            }
           
        }

        public override string ToString()
        {
            StringBuilder res = new StringBuilder();
            foreach (var key in PossibleKeysList)
            {
                res.Append(key.KeyValue).AppendLine();
            }
            return res.ToString();
        }
    }
}
