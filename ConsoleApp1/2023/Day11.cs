using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day11 : IAocDay
    {
        public async Task<object> Part1()
        {
            var ans = 0L;
            var input = await IO.GetInput(2023, 11);

            var galaxies = ExpandUniverse(input,2);
            foreach (var g in galaxies)
            {
                foreach (var g2 in galaxies)
                {
                    ans += Math.Abs(g.x - g2.x) + Math.Abs(g.y - g2.y);
                }
            }

            ans /= 2;
            return ans;
        }

        public async Task<object> Part2()
        {
            var ans = 0L;
            var input = await IO.GetInput(2023, 11);

            var galaxies = ExpandUniverse(input, 1000000);
            foreach (var g in galaxies)
            {
                foreach (var g2 in galaxies)
                {
                    ans += Math.Abs(g.x - g2.x) + Math.Abs(g.y - g2.y);
                }
            }

            ans /= 2;
            return ans;
        }

        private static List<Point> ExpandUniverse(List<string> input, int factor)
        {
            var emptyColumns = new List<int>();
            var emptyRows = new List<int>();
            var galaxies = new List<Point>();

            var grid = input.Select(x => x.ToCharArray()).ToArray();
            for (var r = 0; r < grid.Length; r++)
            {
                bool emptyRow = true;
                for (var c = 0; c < grid[0].Length; c++)
                {
                    if (grid[r][c] == '#')
                    {
                        galaxies.Add(new Point(r, c));
                        emptyRow = false;
                    }
                }

                if (emptyRow)
                {
                    emptyRows.Add(r);
                }
            }

            for (var c = 0; c < grid[0].Length; c++)
            {
                bool emptyCol = true;
                for (var r = 0; r < grid.Length; r++)
                {
                    if (grid[r][c] == '#')
                    {
                        emptyCol = false;
                    }
                }

                if (emptyCol)
                {
                    emptyColumns.Add(c);
                }
            }

            galaxies = galaxies.Select(g =>
            {
                var shiftRows = emptyRows.Count(r => r < g.x) * (factor - 1);
                var shiftCols = emptyColumns.Count(c => c < g.y) * (factor - 1);
                return new Point(g.x + shiftRows, g.y + shiftCols);
            }).ToList();

            return galaxies;
        }
    }
}
