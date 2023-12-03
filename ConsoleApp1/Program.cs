﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AoC2023;

namespace AdventOfCode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var answer = await new Day3().Part1();
            var answer2 = await new Day3().Part2();
            Console.WriteLine($"ANSWER: {answer}");
            Console.WriteLine($"ANSWER: {answer2}");
        }

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
    }
}
