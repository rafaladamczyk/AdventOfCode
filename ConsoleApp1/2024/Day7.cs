using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day7 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 7);
            
            var ops = input
                .Select(x => x.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Select(x => new
                {
                    result = long.Parse(x[0]), nums = x[1]
                        .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        .Select(long.Parse).ToList()
                }).ToList();

            long acc = 0;
            foreach (var op in ops)
            {
                var possibleResults = GetPossibleResults(op.nums, op.nums.Count);
                if (possibleResults.Any(r => r == op.result))
                {
                    acc += op.result;
                }
            }

            return acc;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 7);

            var ops = input
                .Select(x => x.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Select(x => new
                {
                    result = long.Parse(x[0]),
                    nums = x[1]
                        .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                        .Select(long.Parse).ToList()
                }).ToList();

            long acc = 0;
            foreach (var op in ops)
            {
                var possibleResults = GetPossibleResults2(op.nums, op.nums.Count);
                if (possibleResults.Any(r => r == op.result))
                {
                    acc += op.result;
                }
            }

            return acc;
        }

        private IEnumerable<long> GetPossibleResults(List<long> nums, int count)
        {
            if (count == 1)
            {
                yield return nums[0];
            }
            else
            {
                foreach (var result in GetPossibleResults(nums, count - 1))
                {
                    yield return nums[count - 1] + result;
                    yield return nums[count - 1] * result;
                }
            }
        }

        private IEnumerable<long> GetPossibleResults2(List<long> nums, int count)
        {
            if (count == 1)
            {
                yield return nums[0];
            }
            else
            {
                foreach (var result in GetPossibleResults2(nums, count - 1))
                {
                    yield return nums[count - 1] + result;
                    yield return nums[count - 1] * result;
                    yield return Concatenate(result, nums[count - 1]);
                }
            }
        }

        private static long Concatenate(long first, long second)
        {
            return long.Parse($"{first}{second}");
        }
    }
}
