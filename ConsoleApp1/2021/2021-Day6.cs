using System;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2021
{
    public class Day8 : IAocDay
    {
        public class Digit
        {
            public char[,] parts = new char[7, 6]
            {
                { ',', 'a', 'a', 'a', 'a', ',' },
                { 'b', ',', ',', ',', ',', 'c' },
                { 'b', ',', ',', ',', ',', 'c' },
                { ',', 'd', 'd', 'd', 'd', ',' },
                { 'e', ',', ',', ',', ',', 'f' },
                { 'e', ',', ',', ',', ',', 'f' },
                { ',', 'g', 'g', 'g', 'g', ',' },
            };

        }

        public async Task<object> Part1()
        {
            var input = await Input.GetInput(2021, 8);
            var lines = input.Select(x => x.Split('|')).Select(x => (x[0], x[1].Split(' ').Select(y => y.Trim()))).ToList();
            var outputDigits = lines.SelectMany(x => x.Item2).ToList();
            
            var ans = 0;
            foreach (var outputDigit in outputDigits)
            {
                var uniqueChars = outputDigit.Distinct().Count();
                if (uniqueChars == 2 || uniqueChars == 4 || uniqueChars == 3 || uniqueChars == 7)
                {
                    ans++;
                }
            }
            
            return ans.ToString();
        }

        public async Task<object> Part2()
        {
            return "";
        }
    }
}