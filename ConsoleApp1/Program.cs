using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AoC2023;

namespace AdventOfCode
{
    class Program
    {
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
            var reversed = Days.ToList();
            reversed.Reverse();

            foreach (var day in reversed)
            {
                Console.WriteLine($"{day.GetType()}\t1: {await day.Part1()}\t2: {await day.Part2()}");
            }
        }
    }
}
