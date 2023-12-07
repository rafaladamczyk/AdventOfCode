using System;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day6 : IAocDay
{
    public async Task<object> Part1()
    {
        var input = await IO.GetInput(2023, 6);
        var times = input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        var distances = input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);

        var races = times.Zip(distances).Select(i => new { time = i.First, dist = i.Second }).ToList();
        var ans = 1;
        foreach (var race in races)
        {
            var ways = 0;
            for (int i = 1; i < race.time; i++)
            {
                var timePressed = i;
                var timeRemaining = race.time - i;
                var distance = timeRemaining * timePressed;
                if (distance > race.dist)
                {
                    ways += 1;
                }
            }

            ans *= ways;
        }
        
        return ans;
    }

    public async Task<object> Part2()
    {
        var input = await IO.GetInput(2023, 6);
        var time = ulong.Parse(
            string.Join("", input[0].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)));
        var distance =
            ulong.Parse(string.Join("", input[1].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)));

        var ans = 0;
        for (ulong i = 1; i < time; i++)
        {
            var timePressed = i;
            var timeRemaining = time - i;
            var result = timeRemaining * timePressed;
            if (result > distance)
            {
                ans++;
            }
        }

        return ans;
    }
}