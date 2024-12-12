using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day2 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 2);
            var safe = 0;

            foreach (var report in input)
            {
                var levels = report.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse).ToArray();

                if (AreIncreasing(levels) || AreDecreasing(levels))
                {
                    var diffs = MinMaxDifference(levels);
                    if (diffs.min >= 1 && diffs.max <= 3)
                    {
                        safe++;
                    }
                }

            }

            return safe;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 2);
            var safe = 0;

            foreach (var report in input)
            {
                var levels = report.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .Select(int.Parse).ToArray();

                if (IsSafe(levels))
                {
                    safe++;
                }
                else
                {
                    var dampened = GenerateNewLevels(levels);
                    foreach (var d in dampened)
                    {
                        if (IsSafe(d))
                        {
                            safe++;
                            break;
                        }
                    }
                }

            }

            return safe;
        }

        private bool IsSafe(int[] levels)
        {
            if (AreIncreasing(levels) || AreDecreasing(levels))
            {
                var diffs = MinMaxDifference(levels);
                if (diffs.min >= 1 && diffs.max <= 3)
                {
                    return true;
                }
            }

            return false; }

        private IEnumerable<int[]> GenerateNewLevels(int[] levels)
        {
            for (int i = 0; i < levels.Length; i++)
            {
                yield return levels.Where((x, idx) => idx != i).ToArray();
            }
        }

        private bool AreIncreasing(int[] levels)
        {
            for (var i = 0; i < levels.Length - 1; i++)
            {
                if (levels[i] > levels[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        private bool AreDecreasing(int[] levels)
        {
            for (var i = 0; i < levels.Length - 1; i++)
            {
                if (levels[i] < levels[i + 1])
                {
                    return false;
                }
            }

            return true;
        }

        private (int min, int max) MinMaxDifference(int[] levels)
        {
            var minDiff = 100000;
            var maxDiff = 0;
            for (var i = 0; i < levels.Length - 1; i++)
            {
                var diff = Math.Abs(levels[i] - levels[i + 1]);
                maxDiff = Math.Max(maxDiff, diff);
                minDiff = Math.Min(minDiff, diff);
            }

            return (minDiff, maxDiff);
        }
    }
}
