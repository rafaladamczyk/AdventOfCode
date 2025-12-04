using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2025
{
    public class Day3 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 3);
            var banks = new List<List<uint>>();
            var joltages = new List<uint>();

            foreach (var line in input)
            {
                var bank = line.Select(x => uint.Parse($"{x}")).ToList();
                banks.Add(bank);
            }

            foreach (var bank in banks)
            {
                var max = bank.Max();
                var index = bank.IndexOf(max);
                uint max2;

                if (index < bank.Count - 1)
                {
                    max2 = bank.Skip(index + 1).Max();
                }
                else
                {
                    max2 = max;
                    max = bank.Take(bank.Count - 1).Max();
                }

                joltages.Add(uint.Parse($"{max}{max2}"));
            }

            return joltages.Sum(x => x);
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 3);
            var banks = new List<List<ulong>>();
            var joltages = new List<ulong>();

            foreach (var line in input)
            {
                var bank = line.Select(x => ulong.Parse($"{x}")).ToList();
                banks.Add(bank);
            }

            foreach (var bank in banks)
            {
                var joltage = "";
                var lastIndex= -1;

                for (var i = 0; i < 12; i++)
                {
                    lastIndex = MaxIndex(bank, lastIndex + 1, 12 - i - 1);
                    joltage += $"{bank[lastIndex]}";
                }

                joltages.Add(ulong.Parse(joltage));
            }

            return joltages.Aggregate(0UL, (acc, j) => acc + j);
        }

        private int MaxIndex(List<ulong> digits, int startIndex, int followedBy)
        {
            ulong currentMax = 0;
            var currentMaxIndex = -1;

            for (int i = startIndex; i < digits.Count - followedBy; i++)
            {
                if (digits[i] > currentMax)
                {
                    currentMax = digits[i];
                    currentMaxIndex = i;
                }
            }

            return currentMaxIndex;
        }

    }
}
