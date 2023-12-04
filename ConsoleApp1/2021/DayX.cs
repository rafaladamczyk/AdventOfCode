using System;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2021
{
    public class Day16 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2021, 16);
            //var input = await Input.GetExampleInput();
            var ans = 0;
            foreach (var line in input)
            {
                var s = line.Split('|');
                var winning = s[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
                var mine = s[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();

                var sub = 0;
                foreach (var win in mine)
                {
                    if (winning.Contains(win))
                    {
                        sub = sub > 0 ? sub *= 2 : 1;
                    }
                }

                ans += sub;
            }

            await IO.SubmitAnswer(2021, 16, 1, ans);
            return ans;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2021, 16);
            //var input = await Input.GetExampleInput();
            return "";
        }
    }
}
