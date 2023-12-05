﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2023;

namespace AdventOfCode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var answer = await new Day5().Part1();
            Console.WriteLine(answer);
            var answer2 = await new Day5().Part2();
            Console.WriteLine(answer2);
        }

        private static async Task PrintAnswers(IEnumerable<IAocDay> days)
        {
            foreach (var day in days)
            {
                Console.WriteLine($"{day.GetType()} Part 1: {await day.Part1()}");
                Console.WriteLine($"{day.GetType()} Part 2: {await day.Part2()}");
                Console.WriteLine(string.Join("", Enumerable.Range(1,80).Select(x => "*")));
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
            //new Day5(),
            //new Day6(),
            //new Day7(),
            //new Day8(),
            //new Day9(),
            //new Day10(),
        };
    }
}
