using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2021
{
    public class Day8 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2021, 8);
            var lines = input.Select(x => x.Split('|')).Select(x => (x[0], x[1].Split(' ').Select(y => y.Trim())))
                .ToList();
            var outputDigits = lines.SelectMany(x => x.Item2).ToList();

            var ans = 0;
            foreach (var outputDigit in outputDigits)
            {
                var uniqueChars = outputDigit.Distinct().Count();
                if (uniqueChars is 2 or 4 or 3 or 7)
                {
                    ans++;
                }
            }

            return ans.ToString();
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2021, 8);

            var ans = 0;
            foreach (var line in input)
            {
                var patterns = line.Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(n => n.Trim()).ToHashSet();
                var outputNumbers = line.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

                var map = new Dictionary<int, HashSet<char>>
                {
                    { 1, patterns.Single(x => x.Length == 2).Select(x => x).ToHashSet() },
                    { 4, patterns.Single(x => x.Length == 4).Select(x => x).ToHashSet() },
                    { 7, patterns.Single(x => x.Length == 3).Select(x => x).ToHashSet() },
                    { 8, patterns.Single(x => x.Length == 7).Select(x => x).ToHashSet() }
                };

                var sixLength = patterns.Where(x => x.Length == 6);
                var nine= sixLength
                    .Single(x => AreEquivalent(new HashSet<char>(x.Select(c => c).Intersect(map[4])), map[4]));
                map.Add(9, new HashSet<char>(nine));

                sixLength = sixLength.Except(new[] { nine });
                var zero = sixLength
                    .Single(x =>
                        AreEquivalent(map[7], new HashSet<char>(x).Intersect(map[7]).ToHashSet()));
                map.Add(0, new HashSet<char>(zero));
                var six = sixLength.Except(new[] { zero }).Single();
                map.Add(6, new HashSet<char>(six));

                var fiveLength = patterns.Where(x => x.Length == 5);
                var three = fiveLength.Single(x =>
                    AreEquivalent(new HashSet<char>(x).Intersect(map[7]).ToHashSet(), map[7]));
                map.Add(3, three.ToHashSet());

                fiveLength = fiveLength.Except(new[] { three });
                var five = fiveLength.Single(x => AreEquivalent(x.ToHashSet().Intersect(map[6]).ToHashSet(), x.ToHashSet()));
                map.Add(5, five.ToHashSet());

                map.Add(2, fiveLength.Except(new[] { five }).Single().ToHashSet());

                var digits = outputNumbers.Select(n => map.Single(kvp => AreEquivalent(kvp.Value, n.ToHashSet()))).Select(kvp => kvp.Key);
                var numberString = string.Join("", digits);
                var number = int.Parse(numberString);
                ans += number;
            }

            return ans;
        }

        public bool AreEquivalent(ISet<char> first, ISet<char> second)
        {
            if (first.Count != second.Count)
            {
                return false;
            }

            foreach (var key in first)
            {
                if (!second.Contains(key))
                {
                    return false;
                }
            }

            return true;
        }
    }
}