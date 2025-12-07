using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2025
{
    public class Day6 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 6);
            var problems = new List<Tuple<List<int>, char>>();

            var numbers = new List<List<int>>();

            for (var r =0; r<input.Count; r++)
            {
                var nums = input[r].Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                for (var i=0; i<nums.Length; i++)
                {
                    if (numbers.Count < i + 1)
                    {
                        numbers.Add(new List<int>());
                    }

                    if (int.TryParse(nums[i], out var n))
                    {
                        numbers[i].Add(n);
                    }
                    else
                    {
                        char op = nums[i].Single();
                        problems.Add(new Tuple<List<int>, char>(numbers[i], op));
                    }
                }
            }

            ulong acc = 0;
            foreach (var problem in problems)
            {
                var result = Solve(problem.Item1, problem.Item2);
                acc += result;
            }

            return acc;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 6);
            var grid = input.Select(x => x.ToCharArray()).ToArray();
            var opColumns= new List<int>();
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (grid[^1][c] != ' ')
                {
                    opColumns.Add(c);
                }
            }

            var acc = 0UL;
            for (var o = 0; o < opColumns.Count; o++)
            {
                var opIndex = opColumns[o];
                var next = o + 1 < opColumns.Count ? opColumns[o + 1] : grid[0].Length;

                var numbers = new List<int>();
                for (int c = opIndex; c < next; c++)
                {
                    var number = "";
                    for (int r = 0; r < grid.Length; r++)
                    {
                        if (char.IsDigit(grid[r][c]))
                        {
                            number += grid[r][c];
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(number))
                    {
                        numbers.Add(int.Parse(number));
                    }
                }

                char opCode = grid[^1][opIndex];
                acc += Solve(numbers, opCode);
            }

            return acc;
        }

        ulong Solve(List<int> numbers, char op)
        {
            switch (op)
            {
                case '*':
                    return numbers.Aggregate(1UL, (acc, i) => acc * (ulong)i);
                case '+':
                    return numbers.Aggregate(0UL, (acc, i) => acc + (ulong)i);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
