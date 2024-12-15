using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day8 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 8);
            var rows = input.Count;
            var cols = input[0].Length;
            var map = new Dictionary<char, List<Point>>();

            var acc = 0;
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var symbol = input[r][c];
                    if (symbol != '.')
                    {
                        if (!map.TryGetValue(symbol, out var list))
                        {
                            list = new List<Point>();
                            map[symbol] = list;
                        }

                        list.Add(new Point(r, c));
                    }
                }
            }

            var antinodes = new HashSet<Point>();

            foreach (var kvp in map)
            {
                var antennas = kvp.Value;

                for (int i = 0; i < antennas.Count; i++)
                {
                    for (int j = 0; j < antennas.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        var delta = antennas[j] - antennas[i];

                        var candidates = new List<Point> { antennas[i] - delta, antennas[j] + delta };

                        foreach (var c in candidates)
                        {
                            if (c.x >= 0 && c.y >= 0 && c.x < rows && c.y < cols)
                            {
                                antinodes.Add(c);
                            }
                        }
                    }
                }
            }

            return antinodes.Count;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 8);

            var rows = input.Count;
            var cols = input[0].Length;
            var map = new Dictionary<char, List<Point>>();

            var acc = 0;
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var symbol = input[r][c];
                    if (symbol != '.')
                    {
                        if (!map.TryGetValue(symbol, out var list))
                        {
                            list = new List<Point>();
                            map[symbol] = list;
                        }

                        list.Add(new Point(r, c));
                    }
                }
            }

            var antinodes = new HashSet<Point>();

            foreach (var kvp in map)
            {
                var antennas = kvp.Value;

                for (int i = 0; i < antennas.Count; i++)
                {
                    for (int j = 0; j < antennas.Count; j++)
                    {
                        if (i == j)
                        {
                            continue;
                        }

                        foreach (var c in GenerateAntinodes(antennas[i], antennas[j], rows, cols))
                        {
                            antinodes.Add(c);
                        }
                    }
                }
            }

            return antinodes.Count;
        }

        private static IEnumerable<Point> GenerateAntinodes(Point a, Point b, int maxX, int maxY)
        {
            var delta = b - a;

            yield return a;
            yield return b;

            var current = a;
            while (true)
            { 
                current -= delta;
                if (current.IsWithinGrid(maxX, maxY))
                {
                    yield return current;
                }
                else
                {
                    break;
                }
            }

            current = b;
            while (true)
            {
                current += delta; 
                if (current.IsWithinGrid(maxX, maxY))
                {
                    yield return current;
                }
                else
                {
                    break;
                }
            }
        }
    }
}
