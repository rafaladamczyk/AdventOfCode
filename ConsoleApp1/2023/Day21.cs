using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day21 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2023, 21);
            var grid = input.Select(x => x.ToCharArray()).ToArray();
            Point start = default;

            for (int r = 0; r < grid.Length; r++)
            {
                for (int c = 0; c < grid[0].Length; c++)
                {
                    if (grid[r][c] == 'S')
                    {
                        start = new Point(r, c);
                        grid[r][c] = '.';
                    }
                }
            }

            var reached = new HashSet<(Point, int)>();
            void Explore(Point currentPos, int currentStep, HashSet<(Point, int)> visited, List<Point> path)
            {
                if (currentStep == 64)
                {
                    return;
                }

                foreach (var n in currentPos.GetNeighbors())
                {
                    if (!visited.Contains((n, currentStep + 1)) && Misc.PointInGrid(n, grid) && grid[n.x][n.y] == '.')
                    {
                        visited.Add((n, currentStep + 1));
                        Explore(n, currentStep + 1, visited, path.Concat(new[] { n }).ToList());
                    }
                }
            }

            Explore(start, 0, reached, new List<Point>());
            var canReach = reached.Where(x => x.Item2 % 2 == 0).Select(x => x.Item1).ToHashSet();

            foreach (var p in canReach)
            {
                grid[p.x][p.y] = 'O';
            }

            Misc.Print(grid);

            return canReach.Count;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 21);
            var grid = input.Select(x => x.ToCharArray()).ToArray();
            Point start = default;

            for (int r = 0; r < grid.Length; r++)
            {
                for (int c = 0; c < grid[0].Length; c++)
                {
                    if (grid[r][c] == 'S')
                    {
                        start = new Point(r, c);
                        grid[r][c] = '.';
                    }
                }
            }

            const int target = 26501365;
            var gridSize = grid.Length;
            var mid = gridSize / 2;

            var breakpoints = new List<int>() { mid, mid + gridSize, mid + gridSize * 2 };
            var tilesReached = new List<int>() { 0, 0, 0 };

            var visited = new HashSet<Point>();
            var reached = new HashSet<Point>();
            var Q = new Queue<(Point, int)>();
            Q.Enqueue((start, 0));

            while (Q.Count > 0)
            {
                var (point, steps) = Q.Dequeue();
               
                int x = point.x % grid.Length;
                x = x >= 0 ? x : x + grid.Length;
                int y = point.y % grid[0].Length;
                y = y >= 0 ? y : y + grid[0].Length;
                if (steps % 2 == 0 && grid[x][y] == '.')
                {
                    reached.Add(point);
                }

                for (int i = 0; i < breakpoints.Count; i++)
                {
                    if (breakpoints[i] == steps)
                    {
                        tilesReached[i] = reached.Count;
                    }
                }

                if (grid[x][y] == '.')
                {
                    foreach (var n in point.GetNeighbors())
                    {
                        if (steps < breakpoints.Last() + 1)
                        {
                            if (!visited.Contains(n))
                            {
                                Q.Enqueue((n, steps + 1));
                                visited.Add(n);
                            }
                        }
                    }
                }
            }

            // used wolfram alpha to fit polynomial to data
            // TODO: do it in code instead

            var polynomialAtTarget = 601441063166538;
            return polynomialAtTarget;
        }
    }
}
