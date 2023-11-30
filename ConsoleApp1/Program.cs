using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode;

namespace ConsoleApp1
{
    class Program
    {
        private static readonly List<IAocDay> DaysToRun = new()
        {
            new AoC2021.Day8(),
            new AoC2023.Day1()
        };

        static async Task Main(string[] args)
        {
            var result = await new AoC2022.Day13().Part1();
            Console.WriteLine(result);

            //foreach (var day in DaysToRun)
            //{
            //    Console.WriteLine($"{day.GetType()} \t part 1: {await day.Part1()} \t part 2: {await day.Part2()}");
            //}
        }
    }
}
