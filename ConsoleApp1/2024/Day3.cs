using System;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day3 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 3);
            var sum = 0;

            foreach (var line in input)
            {
                int pos = 0;
                while (pos < line.Length - 4)
                {
                    if (line.Substring(pos, 4).Equals("mul(", StringComparison.OrdinalIgnoreCase))
                    {
                        var closingBracketIndex = line.IndexOf(')', pos);
                        if (closingBracketIndex != -1)
                        {
                            var diff = closingBracketIndex - pos - 4;
                            var numbersString = line.Substring(pos + 4, diff);
                            var numbers = numbersString.Split(',')
                                .Select(x => int.TryParse(x, out var n) ? (int?)n : null).ToList();
                            if (numbers.All(n => n != null))
                            {
                                var mul = numbers.Aggregate(1, (s, n) => (int)(s * n));
                                sum += mul;
                            }
                        }
                    }

                    pos++;
                }
            }

            return sum;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 3);
            var sum = 0;
            var active = true;

            foreach (var line in input)
            {
                int pos = 0;
                while (pos < line.Length - 4)
                {
                    if (line.Substring(pos, 4).Equals("do()"))
                    {
                        active = true;
                    }

                    if (line.Length - pos >= 7 && line.Substring(pos, 7).Equals("don't()"))
                    {
                        active = false;
                    }

                    if (active)
                    {
                        if (line.Substring(pos, 4).Equals("mul(", StringComparison.OrdinalIgnoreCase))
                        {
                            var closingBracketIndex = line.IndexOf(')', pos);
                            if (closingBracketIndex != -1)
                            {
                                var diff = closingBracketIndex - pos - 4;
                                var numbersString = line.Substring(pos + 4, diff);
                                var numbers = numbersString.Split(',')
                                    .Select(x => int.TryParse(x, out var n) ? (int?)n : null).ToList();
                                if (numbers.All(n => n != null))
                                {
                                    var mul = numbers.Aggregate(1, (s, n) => (int)(s * n));
                                    sum += mul;
                                }
                            }
                        }
                    }

                    pos++;
                }
            }

            return sum;
        }
    }
}
