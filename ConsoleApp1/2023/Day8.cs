using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day8 : IAocDay
{
    public async Task<object> Part1()
    {
        var ans = 0;
        var input = await IO.GetInput(2023, 8);
        //var input = await IO.GetExampleInput();

        await IO.SubmitAnswer(2023, 8, 1, ans);
        return ans;
    }

    public async Task<object> Part2()
    {
        var ans = 0;
        var input = await IO.GetInput(2023, 8);
        //var input = await IO.GetExampleInput();

        await IO.SubmitAnswer(2023, 8, 2, ans);
        return ans;
    }
}
