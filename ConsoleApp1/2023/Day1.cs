using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day1 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2023, 1);
            var numbers = input.Select(x => (x.First(char.IsDigit), x.Last(char.IsDigit)))
                .Select(x => int.Parse($"{x.Item1}{x.Item2}"));

            return numbers.Sum();
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 1);
            var numbers = new List<int>();
            foreach (var line in input)
            {
                int first = 0;
                int last = 0;

                for (int i = 0; i < line.Length; i++)
                {
                    var digit = GetDigitAt(line, i);
                    if (digit != null)
                    {
                        if (first == 0)
                        {
                            first = digit.Value;
                        }

                        last = digit.Value;
                    }
                }

                var number = int.Parse($"{first}{last}");
                numbers.Add(number);
            }

            return numbers.Sum();

            int? GetDigitAt(string s, int i)
            {
                if (char.IsDigit(s[i]))
                {
                    return int.Parse($"{s[i]}");
                }

                return s.Substring(i).GetDigitFromStringStart();
            }
        }
    }
}
