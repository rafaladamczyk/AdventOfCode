using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day12 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 12);

            var map = ParseInput(input).ToDictionary(x => x.p, x => x.symbol);
            var regions = new List<Region>();

            foreach (var kvp in map)
            {
                if (regions.Any(r => r.Points.Contains(kvp.Key)))
                {
                    continue;
                }

                var region = ConstructRegion(map, kvp.Key, kvp.Value);
                regions.Add(region);
            }

            return regions.Sum(r => r.Area * r.Perimeter);
        }

        private static Region ConstructRegion(Dictionary<Point, char> map, Point point, char symbol)
        {
            var region = new Region(symbol);
            var s = new Stack<Point>();
            s.Push(point);
            region.Points.Add(point);

            while (s.Count > 0)
            {
                var current = s.Pop();
                var neighbors = current.GetNeighbors().Where(n => !region.Points.Contains(n));

                foreach (var n in neighbors)
                {
                    if (map.ContainsKey(n) && map[n] == symbol)
                    {
                        region.Points.Add(n);
                        s.Push(n);
                    }
                    else
                    {
                        region.Perimeter++;
                    }
                }
            }

            return region;
        }

        public class Region
        {
            public Region(char symbol)
            {
                Symbol = symbol;
            }

            public char Symbol { get; }
            public HashSet<Point> Points { get; set; } = new HashSet<Point>();
            public int Area => Points.Count;
            public int Perimeter { get; set; }
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 12);
            return 6;
        }

        static IEnumerable<(Point p, char symbol)> ParseInput(IList<string> input)
        {
            var rows = input.Count;
            var cols = input[0].Length;

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    yield return (new Point(r, c), input[r][c]);
                }
            }
        }
    }
}
