using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day5 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 5);
            var orders = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x));
            var sequences = input.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(1);

            var rules = new Dictionary<int, HashSet<int>>();
            foreach (var order in orders)
            {
                var split = order.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var pre = int.Parse(split[0]);
                var post = int.Parse(split[1]);

                var n = rules.TryGetValue(pre, out var nums) ? nums : new HashSet<int>();
                n.Add(post);

                rules[pre] = n;
            }

            var acc = 0;
            var comparer = new PageComparer(rules);
            foreach (var seq in sequences)
            {
                var pages = seq.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToList();

                var sorted = pages.ToList();
                BubbleSort(sorted, comparer);

                if (sorted.SequenceEqual(pages))
                {
                    var middle = (pages.Count - 1) / 2;
                    acc += pages[middle];
                }
            }

            return acc;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 5);
            var orders = input.TakeWhile(x => !string.IsNullOrWhiteSpace(x));
            var sequences = input.SkipWhile(x => !string.IsNullOrWhiteSpace(x)).Skip(1);

            var rules = new Dictionary<int, HashSet<int>>();
            foreach (var order in orders)
            {
                var split = order.Split('|', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var pre = int.Parse(split[0]);
                var post = int.Parse(split[1]);

                var n = rules.TryGetValue(pre, out var nums) ? nums : new HashSet<int>();
                n.Add(post);

                rules[pre] = n;
            }

            var acc = 0;
            var comparer = new PageComparer(rules);
            foreach (var seq in sequences)
            {
                var pages = seq.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                    .Select(int.Parse).ToList();

                var sorted = pages.ToList();
                BubbleSort(sorted, comparer);

                if (!sorted.SequenceEqual(pages))
                {
                    var middle = (sorted.Count - 1) / 2;
                    acc += sorted[middle];
                }
            }

            return acc;
        }

        public List<int> BubbleSort(List<int> toSort, IComparer<int> comparer)
        {
            for (int i = 0; i < toSort.Count - 1; i++)
            {
                for (int j = i + 1; j < toSort.Count; j++)
                {
                    if (comparer.Compare(toSort[i], toSort[j]) > 0)
                    {
                        (toSort[i], toSort[j]) = (toSort[j], toSort[i]);
                    }
                }
            }

            return toSort;
        }

        public class PageComparer : IComparer<int>
        {
            private readonly Dictionary<int, HashSet<int>> rules;

            public PageComparer(Dictionary<int, HashSet<int>> rules)
            {
                this.rules = rules;
            }

            public int Compare(int first, int second)
            {
                if (first == second)
                {
                    return 0;
                }

                if (rules.TryGetValue(first, out var firstRule))
                {
                    if (firstRule.Contains(second))
                    {
                        return -1;
                    }
                }
                if (rules.TryGetValue(second, out var secondRule))
                {
                    if (secondRule.Contains(first))
                    {
                        return 1;
                    }
                }

                return 0;
            }
        }
    }
}
