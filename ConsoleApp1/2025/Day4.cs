using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2025
{
    public class Day4 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 4);
            var map = new Dictionary<Point, char>();
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    map.Add(new Point(x, y), input[y][x]);
                }
            }

            var reachable = 0;
            foreach (var point in map.Keys)
            {
                if (map[point] != '@')
                {
                    continue;
                }

                var adjacentRolls = 0;
                foreach (var neighbor in point.GetNeighbors(includeDiagonal: true))
                {
                    if (map.TryGetValue(neighbor, out var c) && c == '@')
                    {
                        adjacentRolls++;
                    }
                }

                if (adjacentRolls < 4)
                {
                    reachable++;
                }
            }

            return reachable;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 4);
            var map = new Dictionary<Point, char>();
            for (var y = 0; y < input.Count; y++)
            {
                for (var x = 0; x < input[y].Length; x++)
                {
                    map.Add(new Point(x, y), input[y][x]);
                }
            }
            
            var reachable = 0;

            while (true)
            {
                bool removedSomething = false;

                foreach (var point in map.Keys)
                {
                    if (map[point] != '@')
                    {
                        continue;
                    }

                    var adjacentRolls = 0;
                    foreach (var neighbor in point.GetNeighbors(includeDiagonal: true))
                    {
                        if (map.TryGetValue(neighbor, out var c) && c == '@')
                        {
                            adjacentRolls++;
                        }
                    }

                    if (adjacentRolls < 4)
                    {
                        reachable++;
                        map[point] = '.';
                        removedSomething = true;
                    }
                }

                if (!removedSomething)
                {
                    break;
                }
            }

            return reachable;
        }
    }
}
