using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GeneticAlgo
{
    public class FrequencyCalculation
    {
         public static Dictionary<string, int> DefTrigramsCounts = new Dictionary<string, int>();


         public static void PopulateDefTrigramCounts()
        {

            string singleFreq = "";
            var trigramsCounts = File.ReadAllText("C:/Users/Anna_Onofriichuk/source/repos/lab1/eng_trigrams.txt");
            var trigramsReader = new StringReader(trigramsCounts);
            while (true)
            {
                singleFreq = trigramsReader.ReadLine();
                if (singleFreq != null)
                {
                    var line = singleFreq.Split(' ');
                    string trigram = line[0].ToUpper();
                    int count = Convert.ToInt32(line[1]);
                    DefTrigramsCounts.Add(trigram, count);
                }
                else
                    break;
            }
        }

         public static Dictionary<string, int> getTrigramCounts(string text)
        {
            var gram = 3;
            var resDict = new Dictionary<string, int>();
            for (int i = 0; i < text.Length; i++)
            {
                if (i + gram >= text.Length)
                    break;
                int currentCount;
                var singleGram = text.Substring(i, gram);
                if (singleGram.Length == gram)
                {
                    if (resDict.TryGetValue(singleGram, out currentCount))
                        resDict[singleGram] = currentCount + 1;
                    else
                        resDict[singleGram] = 1;
                }
                else break;
            }

            return resDict;
        }

         public static int getCount(string trigram)
        {
            int count;
            if (DefTrigramsCounts.TryGetValue(trigram, out count))
                return count;
            else
                return 0;
        }
    }
}

