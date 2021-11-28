using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GeneticAlgo
{
    public class Generation
    {
        public List<Pool> PoolList { get; set; }
        public string EncryptedText { get; set; }
        public int PoolSize { get; set; }

        public Generation(string encryptedText)
        {
            PoolList = new List<Pool>();
            EncryptedText = encryptedText;
        }

        public Generation(Pool pool1, Pool pool2)
        {
            PoolList = new List<Pool>(2) { pool1, pool2 };
        }

        public void CreateFirstGeneration(Pool pool1, Pool pool2)
        {
            PoolList.Add(pool1);
            PoolList.Add(pool2);
        }

        public void EnsureDistinctPools()
        {
            var isCommonKeys = true;
            while (isCommonKeys)
            {
                var pool1ForRand = PoolList[0];
                var pool2ForRand = PoolList[1];
                var pool1 = PoolList[0].PossibleKeysList.Select(x => x.KeyValue).ToList();
                var pool2 = PoolList[1].PossibleKeysList.Select(x => x.KeyValue).ToList();

                var intersection = pool1.Intersect(pool2);
                if (intersection.Count() != 0)
                {
                    foreach (var key in intersection)
                    {
                        var newKey = pool1ForRand.CreateRandomKey();
                        pool1ForRand.PossibleKeysList[pool1ForRand.PossibleKeysList.FindIndex(ind => ind.KeyValue.Equals(key))] = new PKey(newKey, EncryptedText);
                    }
                    PoolList[0] = pool1ForRand;
                }
                else isCommonKeys = false;
            }
           
        }

        public PKey GetBestKey()
        {
            return PoolList[0].PossibleKeysList[0].FitnessValue > PoolList[1].PossibleKeysList[0].FitnessValue ? PoolList[0].PossibleKeysList[0] : PoolList[1].PossibleKeysList[0];
        }

        public override string ToString()
        {
            int i = 1;
            StringBuilder res = new StringBuilder();
            foreach (var pool in PoolList)
            {
                res.Append($"Pool {i}:\n{pool.ToString()}").AppendLine();
                i++;
            }
            return res.ToString();
        }

    }
}
