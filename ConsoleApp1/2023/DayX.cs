using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day : IAocDay
    {
        public async Task<object> Part1()
        {
            var ans = 0;
            var input = await IO.GetInput(2023, 666);
            //var input = await IO.GetExampleInput();

            await IO.SubmitAnswer(2023, 666, 1, ans);
            return ans;
        }

        public async Task<object> Part2()
        {
            var ans = 0;
            var input = await IO.GetInput(2023, 666);
            //var input = await IO.GetExampleInput();

            await IO.SubmitAnswer(2023, 666, 2, ans);
            return ans;
        }
    }
}
