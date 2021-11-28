using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab1
{
   public static class CaesarAlgorithm
    {
        public static string DecryptXOR(int key, string input)
        {
            return new string(input.ToCharArray().Select(c => (char)(c ^ key)).ToArray());

        }

        public static string DecryptWithBruteforce(string input)
        {
            Dictionary<int, string> outputs = new Dictionary<int, string>();

            for (int i = 0; i < 255; i++)
            {
                outputs.Add(i, DecryptXOR(i, input));
            }
            double maxPercent = 0;
            int key = 0;
            var r = outputs.Values.ToArray();

            for (int i = 0; i < outputs.Count; i++)
            {
                var ptext = HelpCLass.PercentOfText(r[i]);

                if (ptext > maxPercent)
                {
                    key = i;
                    maxPercent = ptext;
                }
            }


            return r[key];
        }
    }
}