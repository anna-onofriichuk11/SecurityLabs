using System;
using System.Collections.Generic;
using System.Text;

namespace GeneticAlgo
{
    public class PKey
    {
        public string KeyValue { get; set; }
        public double FitnessValue { get; set; }

        public PKey(string key, string text)
        {
            KeyValue = key;
            FitnessValue = GetFitnessValue(text);
        }

        public double GetFitnessValue(string text)
        {
            double fitness = 0;
            
            string decrypted = Cypher.Decrypt(text, KeyValue);
            //var trigramCounts = new FrequenciesCalculation();

            var decrTrigramCounts = FrequencyCalculation.getTrigramCounts(decrypted);
            foreach (var trigram in decrTrigramCounts.Keys)
            {
                double defTrigramCount = FrequencyCalculation.getCount(trigram);
                if (defTrigramCount != 0)
                    fitness += decrTrigramCounts[trigram] * Math.Log(defTrigramCount, 2.0);
            }

            return fitness;
        }

        public string DecodeByKeyValue(string text)
        {
            
            return Cypher.Decrypt(text, KeyValue);
        }
    }
}
