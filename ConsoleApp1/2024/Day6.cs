using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day6 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 6);
            var blockades = GetBlockades(input, out var start);
            var visited = Walk(start, blockades, null, input[0].Length, input.Count, out _);

            return visited.Count;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 6);

            var blockades = GetBlockades(input, out var start);

            var candidates = Walk(start, blockades, null, input[0].Length, input.Count, out _);
            candidates.Remove(start.p);
            
            var solves = new ConcurrentBag<Point>();

            var options = new ParallelOptions() { MaxDegreeOfParallelism = -1 };
            Parallel.ForEach(candidates, options, candidate =>
            {
                Walk(start, blockades, candidate, input.Count, input[0].Length, out var
                    looped);

                if (looped)
                {
                    solves.Add(candidate);
                }
            });

            return solves.Count;
        }

        private static HashSet<Point> GetBlockades(List<string> input, out (Point p, Point dir) start)
        {
            start = (default, default);
            var rows = input.Count;
            var cols = input[0].Length;
            var blockades = new HashSet<Point>(rows*cols);

            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    if (input[r][c] == '#')
                    {
                        blockades.Add(new Point(c, r));
                    }

                    if (input[r][c] == '^')
                    {
                        start = (new Point(c, r), new Point(0, -1));
                    }
                }
            }

            return blockades;
        }

        private static HashSet<Point> Walk((Point p, Point dir) start, HashSet<Point> blockades, Point? candidate, int maxX, int maxY, out bool looped)
        {
            var visited = new HashSet<(Point p, Point dir)>();
            var current = start;
            while (true)
            {
                visited.Add(current);

                if (blockades.Contains(current.p + current.dir))
                {
                    current.dir = current.dir.TurnLeft(); // maybe I should rename it, but it's called Left because it assumes different coordinate system.
                }
                else if (candidate != null && candidate.Value.Equals(current.p + current.dir))
                {
                    current.dir = current.dir.TurnLeft();
                }
                else
                {
                    current.p += current.dir;
                }

                if (visited.Contains(current))
                {
                    looped = true;
                    break;
                }

                if (current.p.x >= maxX || current.p.y >= maxY || current.p.x < 0 || current.p.y < 0)
                {
                    looped = false;
                    break;
                }
            }

            return visited.Select(x => x.p).ToHashSet();
        }
    }
}
