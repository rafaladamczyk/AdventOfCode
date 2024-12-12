using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day4 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 4);
            var rows = input.Count;
            var cols = input[0].Length;

            var acc = 0;
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var p1 = new Point(r, c);
                    if (input[p1.x][p1.y] == 'X')
                    {
                        var lines = new Point(r, c).GetNeighbors(true).Select(n => new List<Point> { n, p1 + (n - p1) * 2, p1 + (n - p1) * 3 });

                        foreach (var line in lines.Where(l => l.All(p => PointInGrid(p, rows, cols))))
                        {
                            if (input[line[0].x][line[0].y] == 'M')
                            {
                                if (input[line[1].x][line[1].y] == 'A')
                                {
                                    if (input[line[2].x][line[2].y] == 'S')
                                    {
                                        acc++;
                                    }
                                }
                            }
                        }
                    }

                }
            }

            return acc;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 4);
            var rows = input.Count;
            var cols = input[0].Length;

            var acc = 0;
            for (var r = 0; r < rows; r++)
            {
                for (var c = 0; c < cols; c++)
                {
                    var p1 = new Point(r, c);
                    if (input[p1.x][p1.y] == 'A')
                    {
                        var d1 = new List<Point> { p1 + new Point(1, 1), p1 + new Point(-1, -1) };
                        var d2 = new List<Point> { p1 + new Point(1, -1), p1 + new Point(-1, 1) };

                        if (d1.Concat(d2).Any(p => !PointInGrid(p, rows, cols)))
                        {
                            continue;
                        }

                        var lettersD1 = d1.Select(p => input[p.x][p.y]);
                        var lettersD2 = d2.Select(p => input[p.x][p.y]);

                        if (lettersD1.Contains('M') && lettersD1.Contains('S'))
                        {
                            if (lettersD2.Contains('M') && lettersD2.Contains('S'))
                            {
                                acc++;
                            }
                        }
                    }

                }
            }

            return acc;
        }

        private bool PointInGrid(Point p, int rows, int cols)
        {
            return p.x < rows && p.y < cols && p.x >= 0 && p.y >= 0;
        }
    }
}
