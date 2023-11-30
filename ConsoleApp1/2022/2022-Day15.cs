using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using ConsoleApp1.Utils;

namespace AoC2022
{
    public class Day15 : IAocDay
    {
        public async Task<object> Part1()
        {
            const int limit = 4000000;

            var input = await Input.GetInput(2022, 15);
            var sensors = input.Select(inputLine => inputLine
                    .Split(' ').Select(x => x.Trim()).ToList())
                .Select(line => new Sensor
                {
                    x = int.Parse(line[2].Split('=')[1].Trim(',')),
                    y = int.Parse(line[3].Split('=')[1].Trim(':')),
                    closestBeaconX = int.Parse(line[8].Split('=')[1].Trim(',')),
                    closestBeaconY = int.Parse(line[9].Split('=')[1].Trim())
                }).ToList();

            var timer = new Stopwatch();
            timer.Start();

            Parallel.For(0, limit, i =>
            {
                var ranges = sensors.Select(s => s.GetNoBeaconsRange(limit, i));
                var combinedRanges = CombineRanges(ranges.Where(r => r != null).Cast<(int, int)>());
                if (combinedRanges.Count > 1)
                {
                    Debug.Assert(combinedRanges.Count == 2);

                    var ordered = combinedRanges.OrderBy(r => r.start);
                    var jackpot = ordered.First().end + 1;
                    Console.WriteLine(
                        $"x:{jackpot}, y:{i} - frequency: {(ulong)jackpot * 4000000UL + (ulong)i} - elapsed: {timer.Elapsed}");
                }
            });

            return string.Empty;
        }

        class Sensor
        {
            public int x, y;
            public int closestBeaconX, closestBeaconY;

            public (int start, int end)? GetNoBeaconsRange(int limit, int atY)
            {
                var beaconToSensorDistance = Math.Abs(closestBeaconX - x) + Math.Abs(closestBeaconY - y);
                var fromCurrentPositionToSensorDistanceY = Math.Abs(atY - y);
                if (fromCurrentPositionToSensorDistanceY > beaconToSensorDistance)
                {
                    return null;
                }

                var leeway = Math.Abs(beaconToSensorDistance - fromCurrentPositionToSensorDistanceY);
                var noBeaconForSureStart = Math.Max(0, x - leeway);
                var noBeaconForSureEnd = Math.Min(x + leeway, limit);

                return (noBeaconForSureStart, noBeaconForSureEnd);
            }
        }

        private static ISet<(int start, int end)> CombineRanges(IEnumerable<(int, int)> ranges)
        {
            var toConsider = new Queue<(int, int)>(ranges);
            var doesntCombineWithAnything = new HashSet<(int, int)>();

            while (toConsider.Any())
            {
                var combinedSomething = false;
                var r1 = toConsider.Dequeue();
                var candidatesCount = toConsider.Count;

                for (int i = 0; i < candidatesCount; i++)
                {
                    var r2 = toConsider.Dequeue();
                    var combineResult = Combine(r1, r2);
                    if (combineResult.combined)
                    {
                        toConsider.Enqueue(combineResult.range);
                        combinedSomething = true;
                    }
                    else
                    {
                        toConsider.Enqueue(r2);
                    }
                }

                if (!combinedSomething)
                {
                    doesntCombineWithAnything.Add(r1);
                }
            }

            return doesntCombineWithAnything;

            (bool combined, (int start, int end) range) Combine((int start, int end) r1, (int start, int end) r2)
            {
                if (      (r1.end >= r2.start - 1 && r1.start <= r2.end - 1)
                       || (r2.end >= r1.start - 1 && r2.start <= r1.end - 1)
                       || (r1.start >= r2.start && r1.end <= r2.end)
                       || (r2.start >= r1.start && r2.end <= r1.end))

                    return (true, (Math.Min(r1.start, r2.start), Math.Max(r1.end, r2.end)));

                return (false, (-1, -1));
            }
        }

        public async Task<object> Part2()
        {
            return await Part1(); //part1 is gone, only part2 remains
        }
    }
}
