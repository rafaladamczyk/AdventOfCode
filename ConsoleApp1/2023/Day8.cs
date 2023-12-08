using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day8 : IAocDay
{
    public async Task<object> Part1()
    {
        var input = await IO.GetInput(2023, 8);
        var instructions = input[0].ToCharArray();
        var pairs = input.Skip(2).ToDictionary(
            z => z.Split('=', StringSplitOptions.TrimEntries)[0],
            z => z.Split('=', StringSplitOptions.TrimEntries)[1].Trim('(').Trim(')')
                .Split(',', StringSplitOptions.TrimEntries));

        bool finished = false;
        int i = 0;
        var current = "AAA";
        while (!finished)
        {
            var ins = instructions[i % instructions.Length];
            var next = ins == 'L' ? pairs[current][0] : pairs[current][1];
            i++;
            current = next;
            if (current == "ZZZ")
            {
                finished = true;
            }
        }

        return i;
    }

    public async Task<object> Part2()
    {
        var input = await IO.GetInput(2023, 8);

        var instructions = input[0].ToCharArray();
        var pairs = input.Skip(2).ToDictionary(
            z => z.Split('=', StringSplitOptions.TrimEntries)[0],
            z => z.Split('=', StringSplitOptions.TrimEntries)[1].Trim('(').Trim(')')
                .Split(',', StringSplitOptions.TrimEntries));

        var startingPairs = pairs.Where(x => x.Key.EndsWith('A')).ToList();
        var toReachZ = new ConcurrentDictionary<string, ulong>();
        Parallel.ForEach(startingPairs, pair =>
        {
            bool done = false;
            int j = 0;
            var current = pair.Key;
            while (!done)
            {
                var ins = instructions[j % instructions.Length];
                var next = ins == 'L' ? pairs[current][0] : pairs[current][1];
                j++;

                if (current == next)
                {
                    toReachZ.TryAdd(pair.Key, 0);
                    done = true;
                }

                current = next;
                if (current.EndsWith("Z"))
                {
                    toReachZ.TryAdd(pair.Key, (ulong)j);
                    done = true;
                }
            }
        });

        var stepCounts = startingPairs.Select(x => toReachZ[x.Key]).ToList();
        var lcm = stepCounts.Aggregate(1UL, (a, b) => a.LCM(b));
        return lcm; 
    }
}
