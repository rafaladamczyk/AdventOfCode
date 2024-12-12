using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day1 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 1);
            var list1 = new List<int>();
            var list2 = new List<int>();

            foreach (var line in input)
            {
                var numbers = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                list1.Add(int.Parse(numbers[0]));
                list2.Add(int.Parse(numbers[1]));
            }
            
            list1.Sort();
            list2.Sort();

            var zipped = list1.Zip(list2);
            var sum = 0;
            foreach (var pair in zipped)
            {
                sum += Math.Abs(pair.First - pair.Second);
            }

            return sum;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 1);
            var list1 = new List<int>();
            var list2 = new List<int>();

            foreach (var line in input)
            {
                var numbers = line.Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                list1.Add(int.Parse(numbers[0]));
                list2.Add(int.Parse(numbers[1]));
            }

            var sum = 0;
            foreach (var number in list1)
            {
                var count = 0;
                foreach (var number2 in list2)
                {
                    if (number2 == number) count++;
                }

                sum += number * count;
            }

            return sum;
        }
    }
}
