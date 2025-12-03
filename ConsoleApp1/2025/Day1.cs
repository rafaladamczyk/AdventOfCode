using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2025
{
    public class Day1 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 1);
            //var input = await IO.GetExampleInput();
            var moves = new List<int>();
            foreach (var line in input)
            {
                var number = int.Parse(line.Substring(1));
                if (line.StartsWith('L'))
                {
                    number = number * -1;
                }

                moves.Add(number);
            }

            var current = 50;
            var acc = 0;
            foreach (var move in moves)
            {
                current = current + move;
                current = current % 100;
                if (current < 0)
                {
                    current = 100 + current;
                }

                if (current == 0)
                {
                    acc++;
                }
            }

            return acc;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 1);
            var moves = new List<int>();
            foreach (var line in input)
            {
                var number = int.Parse(line.Substring(1));
                if (line.StartsWith('L'))
                {
                    number = number * -1;
                }

                moves.Add(number);
            }

            var current = 50;
            var acc = 0;
            foreach (var move in moves)
            {
                var old = current;
                current += move;

                var a = Math.Abs(current / 100);
                current %= 100;
                acc += a;

                if (current < 0)
                {
                    current = 100 + current;
                    if (old != 0)
                    {
                        acc++;
                    }
                }

                if (current == 0 && move < 0)
                {
                    acc++;
                }
            }

            return acc;
        }
    }
}
