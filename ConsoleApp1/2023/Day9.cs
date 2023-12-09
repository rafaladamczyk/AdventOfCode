using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day9 : IAocDay
{
    public async Task<object> Part1()
    {
        var ans = 0;
        var input = await IO.GetInput(2023, 9);
        var lines = input.Select(x => x.Split().Select(int.Parse).ToList()).ToList();
        foreach (var line in lines)
        {
            ans += PredictNextNumber(line);
        }

        return ans;
    }

    public async Task<object> Part2()
    {
        var ans = 0;
        var input = await IO.GetInput(2023, 9);

        var lines = input.Select(x => x.Split().Select(int.Parse).ToList()).ToList();
        foreach (var line in lines)
        {
            ans += PredictPrevNumber(line);
        }

        return ans;
    }

    private int PredictNextNumber(List<int> line)
    {
        if (line.All(x => x == 0))
        {
            return 0;
        }

        var newLine = line.Take(line.Count - 1).Select((x, i) => line[i + 1] - line[i]).ToList();
        return PredictNextNumber(newLine) + line.Last();
    }

    private int PredictPrevNumber(List<int> line)
    {
        if (line.All(x => x == 0))
        {
            return 0;
        }

        var newLine = line.Take(line.Count - 1).Select((x, i) => line[i + 1] - line[i]).ToList();
        return line.First() - PredictPrevNumber(newLine);
    }

}
