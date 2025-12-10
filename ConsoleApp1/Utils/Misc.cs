using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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

        public static ulong LCM(params ulong[] xs)
        {
            var result = xs.Last();
            for (int i = 0; i < xs.Length - 1; i++)
            {
                result = LCM(result, xs[i]);
            }

            return result;
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

        public readonly struct ListOfIntsKey : IEquatable<ListOfIntsKey>
        {
            private readonly ulong[] ulongs;

            public ListOfIntsKey(IReadOnlyCollection<int> integers)
            {
                var max = integers.Max();
                ulongs = new ulong[(max / 64) + 1];

                foreach (int v in integers)
                {
                    ulongs[v / 64] |= 1UL << (v % 64);
                }
            }

            public bool Equals(ListOfIntsKey other)
            {
                if (ulongs.Length != other.ulongs.Length)
                {
                    return false;
                }

                for (int i = 0; i < ulongs.Length; i++)
                {
                    if (ulongs[i] != other.ulongs[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            public override bool Equals(object? obj) => obj is ListOfIntsKey other && Equals(other);

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    foreach (ulong w in ulongs)
                    {
                        hash = hash * 31 + w.GetHashCode();
                    }

                    return hash;
                }
            }

            public IEnumerable<int> ToIntegers()
            {
                for (int i = 0; i < ulongs.Length; i++)
                {
                    ulong buckets = ulongs[i];

                    while (buckets != 0)
                    {
                        // Find index of lowest set bit
                        int bit = BitOperations.TrailingZeroCount(buckets);

                        yield return i * 64 + bit;

                        // Clear the lowest set bit
                        buckets &= buckets - 1;
                    }
                }
            }
        }

    }
}
