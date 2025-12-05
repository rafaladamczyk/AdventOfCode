using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AoC2023;

namespace AdventOfCode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await PrintAnswers(new IAocDay[]
            {
                new AoC2025.Day5(),
                new AoC2025.Day4(),
                new AoC2025.Day3(),
                new AoC2025.Day2(),
                new AoC2025.Day1()
            });
        }

        private static async Task PrintAnswers(IEnumerable<IAocDay> days)
        {
            var sw = new Stopwatch();

            foreach (var day in days)
            {
                sw.Restart();
                Console.WriteLine($"{day.GetType()} Part 1: {await day.Part1()} \t [{sw.Elapsed}]");

                sw.Restart();
                Console.WriteLine($"{day.GetType()} Part 2: {await day.Part2()} \t [{sw.Elapsed}]");

                Console.WriteLine(string.Join("", Enumerable.Range(1, 80).Select(x => "*")));
            }
        }

        private static readonly List<IAocDay> Days2021 = new()
        {
            new AoC2021.Day8()
        };

        private static readonly List<IAocDay> Days2022 = new List<IAocDay>()
        {
            new AoC2022.Day13(),
            new AoC2022.Day14(),
            new AoC2022.Day15(),
            new AoC2022.Day16(),
            new AoC2022.Day17(),
            new AoC2022.Day18(),
            new AoC2022.Day19(),
            new AoC2022.Day20(),
            new AoC2022.Day21(),
            new AoC2022.Day22(),
            new AoC2022.Day23(),
            new AoC2022.Day24(),
            new AoC2022.Day25(),
        };

        private static readonly List<IAocDay> Days2023 = new List<IAocDay>()
        {
            new Day1(),
            new Day2(),
            new Day3(),
            new Day4(),
            new Day5(),
            new Day6(),
            new Day7(),
            new Day8(),
            new Day9(),
            new Day10(),
            new Day11(),
            new Day12(),
            new Day13(),
            new Day14(),
            new Day15(),
            new Day16(),
            new Day17(),
            new Day18(),
            new Day19(),
            new Day20(),
            new Day21(),
            new Day22(),
            new Day23(),
            new Day24(),
            new Day25(),
        };
    }
}
