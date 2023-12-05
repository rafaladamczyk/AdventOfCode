using System.Collections.Generic;

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

        public static Dictionary<int, int> ReverseDict(this Dictionary<int, int> dict)
        {
            var result = new Dictionary<int, int>();
            foreach (var kvp in dict)
            {
                result[kvp.Value] = kvp.Key;
            }

            return result;
        }
    }
}
