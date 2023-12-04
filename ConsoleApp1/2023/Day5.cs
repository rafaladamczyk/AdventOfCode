using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day5 : IAocDay
{
    public async Task<object> Part1()
    {
        var input = await IO.GetInput(2023, 6);
        var ans = 0;

        await IO.SubmitAnswer(2023, 6, 1, ans);
        return ans;
    }

    public async Task<object> Part2()
    {
        var input = await IO.GetInput(2023, 6);
        var ans = 0;

        await IO.SubmitAnswer(2023, 6, 2, ans);
        return ans;
    }
}