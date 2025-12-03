using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;
using Range = AdventOfCode.Utils.Range;

namespace AoC2025
{
    public class Day2 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 2);
            var ranges = new List<Range>();
            foreach (var line in input)
            {
                var rangeTexts = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var text in rangeTexts)
                {
                    var split = text.Split('-');
                    var range = new Range(ulong.Parse(split[0]), ulong.Parse(split[1]));
                    ranges.Add(range);
                }
            }

            ulong acc = 0;
            foreach (var range in ranges)
            {
                var current = range.s;

                while (current <= range.e)
                {
                    if (!IsValidId(current.ToString()))
                    {
                        acc += current;
                    }

                    current++;
                }
            }

            return acc;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 2);
            var ranges = new List<Range>();
            foreach (var line in input)
            {
                var rangeTexts = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var text in rangeTexts)
                {
                    var split = text.Split('-');
                    var range = new Range(ulong.Parse(split[0]), ulong.Parse(split[1]));
                    ranges.Add(range);
                }
            }

            ulong acc = 0;
            foreach (var range in ranges)
            {
                var current = range.s;

                while (current <= range.e)
                {
                    if (!IsValidId2(current.ToString()))
                    {
                        acc += current;
                    }

                    current++;
                }
            }

            return acc;

        }

        private bool IsValidId(string id)
        {
            if (id.Length % 2 != 0)
            {
                return true;
            }

            return id.Substring(0, id.Length / 2) != id.Substring(id.Length / 2);
        }

        private bool IsValidId2(string id)
        {
            var patternLength = 1;
            while (patternLength <= id.Length / 2)
            {
                var pattern = id.Substring(0, patternLength);
                var test = id.Replace(pattern, string.Empty);
                if (test.Length == 0)
                {
                    return false;
                }

                patternLength++;
            }

            return true;
        }
    }
}
