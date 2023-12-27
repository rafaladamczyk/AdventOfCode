using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day23 : IAocDay
    {
        private static int globalMax = 0;

        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2023, 23);
            var grid = input.Select(x => x.ToCharArray()).ToArray();

            Point start = default;
            for (int i = 0; i < grid[0].Length; i++)
            {
                if (grid[0][i] == '.')
                {
                    start = new Point(0, i);
                }
            }

            globalMax = 0;
            Explore(grid, start, new HashSet<Point> { start }, slopeDefinitions);
            return globalMax;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 23);
            var grid = input.Select(x => x.ToCharArray()).ToArray();

            Point start = default;
            for (int i = 0; i < grid[0].Length; i++)
            {
                if (grid[0][i] == '.')
                {
                    start = new Point(0, i);
                }
            }

            globalMax = 0;
            var thread = new Thread(() =>
            {
                Explore(grid, start, new HashSet<Point> { start }, null);
            }, 10000000); // we're gonna need a bigger stack! (should we use Stack<T> instead? no, this is way more funny)

            thread.Start();
            thread.Join();

            return globalMax;
        }

        void Explore(char[][] grid, Point pos, HashSet<Point> pathSoFar, List<char> slopes)
        {
            if (pos.x == grid.Length - 1)
            {
                globalMax = Math.Max(globalMax, pathSoFar.Count - 1);
                return;
            }

            if (slopes != null && slopes.Contains(grid[pos.x][pos.y]))
            {
                var nextPoint = pos + SlopeToDir(grid[pos.x][pos.y]);
                if (pathSoFar.Contains(nextPoint)) return;

                pathSoFar.Add(nextPoint);
                Explore(grid, nextPoint, pathSoFar, slopes);
                pathSoFar.Remove(nextPoint);
                return;
            }

            foreach (var n in pos.GetNeighbors().Where(x => x.x >= 0))
            {
                if (grid[n.x][n.y] == '#') continue;
                if (pathSoFar.Contains(n)) continue;

                pathSoFar.Add(n);
                Explore(grid, n, pathSoFar, slopes);
                pathSoFar.Remove(n);
            }
        }

        public static List<char> slopeDefinitions = new() { '^', '>', 'v', '<' };

        Point SlopeToDir(char c)
        {
            switch (c)
            {
                case '^':
                    return new Point(-1, 0);
                case '>':
                    return new Point(0, 1);
                case 'v':
                    return new Point(1, 0);
                case '<':
                    return new Point(0, -1);
                default:
                    throw new Exception();
            }
        }

    }
}
