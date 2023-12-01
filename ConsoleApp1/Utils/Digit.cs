using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Utils
{
    internal static class Digit
    {
        public static readonly string[] Digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

        public static int? GetDigitFromStringStart(this string s)
        {
            for (int j = 0; j < Digits.Length; j++)
            {
                if (s.StartsWith(Digits[j]))
                {
                    return j;
                }
            }

            return null;
        }
    }
}
