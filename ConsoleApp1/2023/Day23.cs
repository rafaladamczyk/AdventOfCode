using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day23 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2023, 23);
            //var input = await IO.GetExampleInput();
            var grid = input.Select(x => x.ToCharArray()).ToArray();

            Point start = default;
            for (int i = 0; i < grid[0].Length; i++)
            {
                if (grid[0][i] == '.')
                {
                    start = new Point(0, i);
                }
            }
            
            var longest = Explore(grid, start, new HashSet<Point> { start }, slopeDefinitions);
            
            foreach (var p in longest)
            {
                grid[p.x][p.y] = 'o';
            }

            Misc.Print(grid);
            
            return longest.Count - 1; //we want steps, not tiles
        }

        public async Task<object> Part2()
        {
            //var input = await IO.GetInput(2023, 23);
            var input = await IO.GetExampleInput();
            var grid = input.Select(x => x.ToCharArray()).ToArray();

            Point start = default;
            for (int i = 0; i < grid[0].Length; i++)
            {
                if (grid[0][i] == '.')
                {
                    start = new Point(0, i);
                }
            }

            var longest = Explore(grid, start, new HashSet<Point> { start }, new List<char>(0));

            foreach (var p in longest)
            {
                grid[p.x][p.y] = 'o';
            }

            Misc.Print(grid);

            return longest.Count - 1; //we want steps, not tiles
        }

        HashSet<Point> Explore(char[][] grid, Point pos, HashSet<Point> pathSoFar, List<char> slopes)
        {
            if (pos.x == grid.Length - 1)
            {
                return pathSoFar;
            }

            if (slopes.Contains(grid[pos.x][pos.y]))
            {
                var nextPoint = pos + SlopeToDir(grid[pos.x][pos.y]);
                if (pathSoFar.Contains(nextPoint)) return null;
                return Explore(grid, nextPoint, pathSoFar.Concat(new[] { nextPoint }).ToHashSet(), slopes);
            }

            var candidatePaths = new List<HashSet<Point>>();
            foreach (var n in pos.GetNeighbors().Where(n => Misc.PointInGrid(n, grid)))
            {
                if (pathSoFar.Contains(n)) continue;
                if (grid[n.x][n.y] == '#') continue;
                candidatePaths.Add(Explore(grid, n, pathSoFar.Concat(new[] { n }).ToHashSet(), slopes));
            }

            return candidatePaths.Where(x => x != null).MaxBy(x => x.Count);
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
