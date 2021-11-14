using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab1
{
    public static class HelpCLass
    {
        public static double PercentOfText(string input)
        {
            var amount = 0;

            foreach (var ch in input.ToCharArray())
            {
                if (Char.IsLetter(ch) || Char.IsPunctuation(ch) || Char.IsWhiteSpace(ch))
                    amount++;
            }

            var result = amount / input.Length * 100;
            return result;

        }

        public static string ToHexString(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.UTF8.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString();
        }


        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];

            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.UTF8.GetString(bytes);
        }
    }
}