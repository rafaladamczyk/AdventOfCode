using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;
using Range = AdventOfCode.Utils.Range;

namespace AoC2025
{
    public class Day5 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 5);
            var ranges = new List<Range>();
            var ids = new List<ulong>();
            var middle = input.IndexOf(string.Empty);
            for (int i = 0; i < middle; i++)
            {
                var r = input[i].Split('-');
                ranges.Add(new Range(ulong.Parse(r[0]), ulong.Parse(r[1]) + 1));
            }

            for (int i = middle + 1; i < input.Count; i++)
            {
                ids.Add(ulong.Parse(input[i]));
            }

            return ids.Count(id => ranges.Any(r => r.Contains(id)));
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 5);
            var ranges = new HashSet<Range>();
            var middle = input.IndexOf(string.Empty);
            for (int i = middle - 1; i >= 0; i--)
            {
                var r = input[i].Split('-');
                ranges.Add(new Range(ulong.Parse(r[0]), ulong.Parse(r[1]) + 1));
            }

            Start:
            foreach (var r in ranges)
            {
                foreach (var r2 in ranges)
                {
                    if (r.Equals(r2))
                    {
                        continue;
                    }

                    var merged = r + r2;
                    if (merged != null)
                    {
                        ranges.Remove(r);
                        ranges.Remove(r2);
                        ranges.Add(merged.Value);
                        goto Start;
                    }
                }
            }

            return ranges.Aggregate(0UL, (acc, r) => acc + r.Length);
        }
    }
}

