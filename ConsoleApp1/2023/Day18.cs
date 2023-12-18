using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

public class Day18 : IAocDay
{
    public async Task<object> Part1()
    {
        var input = await IO.GetInput(2023, 18);
        var points = new List<Point>() { new(0, 0) };

        foreach (var line in input)
        {
            var parts = line.Split();
            var dirChar = parts[0].Single();
            var amount = int.Parse(parts[1]);

            var dir = dirChar == 'U' ? new Point(-1, 0) :
                dirChar == 'R' ? new Point(0, 1) :
                dirChar == 'D' ? new Point(1, 0) : new Point(0, -1);

            for (int i = 1; i <= amount; i++)
            {
                var newPoint = points.Last() + dir;
                if (!points[0].Equals(newPoint))
                {
                    points.Add(newPoint);
                }
            }
        }

        var grid = Point.GetSparseGrid(points);

        var outside = FloodFill(grid);
        var insideCount = 0;
        for (int r = 0; r < grid.Length; r++)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (outside.Contains(new Point(r, c)))
                {
                    grid[r][c] = '.';
                }
                else
                {
                    insideCount++;
                    grid[r][c] = '#';
                }
            }
        }

        return insideCount;
    }

    public async Task<object> Part2()
    {
        var input = await IO.GetInput(2023, 18);
        var points = new List<Point> { new(0, 0) };

        foreach (var line in input)
        {
            var color = line.Split()[2].Trim('(', ')', '#');
            var amountString = $"{string.Join("", color.Take(5))}";
            var amount = int.Parse(amountString, System.Globalization.NumberStyles.HexNumber);
            var dirChar = color.Skip(5).Single();

            var dir = dirChar == '3' ? new Point(-1, 0) :
                dirChar == '0' ? new Point(0, 1) :
                dirChar == '1' ? new Point(1, 0) : new Point(0, -1);

            for (int i = 1; i <= amount; i++)
            {
                var newPoint = points.Last() + dir;
                points.Add(newPoint);
            }
        }

        var shoe = Shoelace(points);
        return shoe + points.Count / 2 + 1;
    }

    static long Shoelace(List<Point> points)
    {
        int n = points.Count;
        long sum = 0L;

        for (int i = 0; i < n - 1; i++)
        {
            sum += points[i].x * points[i + 1].y - points[i + 1].x * points[i].y;
        }

        return Math.Abs(sum + points[n - 1].x * points[0].y - points[0].x * points[n - 1].y) / 2L;
    }

    private HashSet<Point> FloodFill(char[][] grid)
    {
        var outside = new HashSet<Point>();

        var Q = new Queue<Point>();
        Q.Enqueue(new Point(0, 0));
        Q.Enqueue(new Point(0, grid[0].Length-1));
        Q.Enqueue(new Point(grid.Length - 1, 0));
        Q.Enqueue(new Point(grid.Length - 1, grid[0].Length-1));

        while (Q.Count > 0)
        {
            var p = Q.Dequeue();

            if (outside.Contains(p))
            {
                continue;
            }
            
            outside.Add(p);
            
            foreach (var dir in Point.GetDirs())
            {
                var n = p + dir;
                if (Misc.PointInGrid(n, grid) && grid[n.x][n.y] == '.')
                {
                    Q.Enqueue(n);
                }
            }
        }

        return outside;
    }
}