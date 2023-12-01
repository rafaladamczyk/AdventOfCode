using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2023;

namespace AdventOfCode
{
    class Program
    {
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

        private static readonly List<IAocDay> Days = new List<IAocDay>()
        {
            new Day1(),
            //new Day2(),
            //new Day3(),
            //new Day4(),
            //new Day5(),
            //new Day6(),
            //new Day7(),
            //new Day8(),
            //new Day9(),
            //new Day10(),
        };

        static async Task Main(string[] args)
        {
            var reversed = Days2022.ToList();
            reversed.Reverse();

            foreach (var day in reversed)
            {
                Console.WriteLine($"{day.GetType()}\t1: {await day.Part1()}\t2: {await day.Part2()}");
            }
        }
    }
}
