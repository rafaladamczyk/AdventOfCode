using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var result = await new AoC2022.Day19().Part2();
            Console.WriteLine(result);
        }
    }
}
