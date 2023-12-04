using System;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day4 : IAocDay
{
    public async Task<object> Part1()
    {
        var input = await IO.GetInput(2023, 4);
        var ans = 0;
        foreach (var line in input)
        {
            var s = line.Substring(line.IndexOf(":") + 1).Split('|');
            var winning = s[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
            var mine = s[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();

            var wins = mine.Count(n => winning.Contains(n));
            ans += (int)Math.Pow(2, wins - 1);
        }

        return ans;
    }

    public async Task<object> Part2()
    {
        var input = await IO.GetInput(2023, 4);
        var cards = Enumerable.Range(1, input.Count).ToDictionary(x => x, x => 0);

        foreach (var line in input)
        {
            var x = line.IndexOf(":");
            var cardNumberString =
                line.Substring(0, x + 1).Split(" ", StringSplitOptions.RemoveEmptyEntries)[1].TrimEnd(':');

            var cardNumber = int.Parse(cardNumberString);
            cards[cardNumber]++;

            var s = line.Substring(x + 1).Split('|');
            var winning = s[0].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim()))
                .ToHashSet();
            var mine = s[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(x => int.Parse(x.Trim())).ToList();

            var wins = mine.Count(n => winning.Contains(n));
            for (var w = 0; w < wins; w++)
            {
                cards[cardNumber + w + 1] += cards[cardNumber];
            }
        }

        var ans = cards.Select(x => x.Value).Sum();
        return ans;
    }
}