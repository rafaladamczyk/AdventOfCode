using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day11 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 11);
            var stones = input.Single()
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

            for (int i = 0; i < 25; i++)
            {
                stones = stones.SelectMany(SplitValue);
            }

            return stones.Count();

        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 11);
            var stones = input.Single()
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse);

            var memo = new Dictionary<(long value, int blink), long>(); 

            long acc = 0;
            foreach (var stone in stones)
            {
                acc += CountStones(stone, 0, 75);
            }

            return acc;

            long CountStones(long value, int currentBlink, int targetBlink)
            {
                if (currentBlink == targetBlink)
                {
                    return 1;
                }

                if (memo.TryGetValue((value, currentBlink), out var precomputed))
                {
                    return precomputed;
                }

                if (value == 0)
                {
                    var result = CountStones(1, currentBlink + 1, targetBlink);
                    memo[(value, currentBlink)] = result;
                    return result;
                }

                var valString = value.ToString();
                if (valString.Length % 2 == 0)
                {
                    var first = valString.Take(valString.Length / 2).ToArray();
                    var second = valString.Skip(valString.Length / 2).ToArray();

                    var left = CountStones(long.Parse(first), currentBlink + 1, targetBlink);
                    var right = CountStones(long.Parse(second), currentBlink + 1, targetBlink);

                    memo[(value, currentBlink)] = left + right;
                    return left + right;
                }
                else
                {
                    var result = CountStones(value * 2024, currentBlink + 1, targetBlink);
                    memo[(value, currentBlink)] = result;
                    return result;
                }
            }

        }

        private static IEnumerable<long> SplitValue(long value)
        {
            if (value == 0)
            {
                yield return 1;
                yield break;
            }

            var old = value.ToString();
            if (old.Length % 2 == 0)
            {
                var first = old.Take(old.Length / 2).ToArray();
                var second = old.Skip(old.Length / 2).ToArray();

                yield return long.Parse(first);
                yield return long.Parse(second);
            }
            else
            {
                yield return value * 2024;
            }
        }
    }



}
