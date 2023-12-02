using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day3 : IAocDay
{
    public async Task<object> Part1()
    {
        var input = await IO.GetInput(2023, 2);
        var answer = "";
        
        await IO.SubmitAnswer(2023, 3, level: 1, answer);
        return answer;
    }

    public async Task<object> Part2()
    {
        var input = await IO.GetInput(2023, 2);
        var answer = "";

        await IO.SubmitAnswer(2023, 3, level: 2, answer);
        return answer;
    }
}