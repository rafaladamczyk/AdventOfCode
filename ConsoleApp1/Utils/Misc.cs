using System;
using System.Collections.Generic;

namespace AdventOfCode.Utils
{
    internal static class Misc
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

        public static ulong GDC(this ulong a, ulong b)
        {
            while (b != 0)
            {
                var x = b;
                b = a % b;
                a = x;
            }

            return a;
        }

        public static ulong LCM(this ulong a, ulong b)
        {
            return a / a.GDC(b) * b;
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


        public static bool PointInGrid(Point point, int[][] grid)
        {
            return point.x >= 0 && point.x < grid.Length && point.y >= 0 && point.y < grid[0].Length;
        }

        public static bool PointInGrid(Point point, char[][] grid)
        {
            return point.x >= 0 && point.x < grid.Length && point.y >= 0 && point.y < grid[0].Length;
        }

        public static void Print(char[][] grid)
        {
            for (int r = 0; r < grid.Length; r++)
            {
                for (int c = 0; c < grid[0].Length; c++)
                {
                    Console.Write(grid[r][c]);
                }

                Console.WriteLine();
            }
        }
    }
}
