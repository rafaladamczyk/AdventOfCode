using System;
using System.Threading.Tasks;

namespace AdventOfCode
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await new AoC2023.Day1().Part1();
            Console.WriteLine($"1: {result}");

            var result2 = await new AoC2023.Day1().Part2();
            Console.WriteLine($"2: {result2}");
        }
    }
}
