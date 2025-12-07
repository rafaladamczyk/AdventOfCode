using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2025
{
    public class Day7 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 7);
            var grid = input.Select(x => x.ToCharArray()).ToArray();

            return FindAllSplits(grid);
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 7);
            var grid = input.Select(x => x.ToCharArray()).ToArray();


            return FindAllTimelines(grid);
        }

        ulong CountTimelines(char[][] grid, Point start, Dictionary<Point, ulong> solved)
        {
            var split = FindNextSplit(grid, start);
            if (split == null)
            {
                return 1;
            }
            else
            {
                var left = new Point(split.Value.x, split.Value.y - 1);
                if (!solved.ContainsKey(left))
                {
                    solved[left] = CountTimelines(grid, left, solved);
                }

                var right = new Point(split.Value.x, split.Value.y + 1);
                if (!solved.ContainsKey(right))
                {
                    solved[right] = CountTimelines(grid, right, solved);
                }

                return solved[left] + solved[right];
            }
        }

        private ulong FindAllTimelines(char[][] grid)
        {
            for (int r = 0; r < grid.Length; r++)
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (grid[r][c] == 'S')
                {
                    var solved = new Dictionary<Point, ulong>();
                    return CountTimelines(grid, new Point(r, c), solved);
                }
            }

            throw new Exception("Start not found");
        }

        private int FindAllSplits(char[][] grid)
        {
            var q = new Queue<Point>();
            for (int r = 0; r < grid.Length; r++)
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (grid[r][c] == 'S')
                {
                    q.Enqueue(new Point(r, c));
                    break;
                }
            }

            var splits = new HashSet<Point>();
            while (q.Count > 0)
            {
                var start = q.Dequeue();
                var split = FindNextSplit(grid, start);
                if (split != null)
                {
                    splits.Add(split.Value);

                    var left = new Point(split.Value.x, split.Value.y - 1);
                    var right = new Point(split.Value.x, split.Value.y + 1);
                    if (!q.Contains(left))
                    {
                        q.Enqueue(left);
                    }

                    if (!q.Contains(right))
                    {
                        q.Enqueue(right);
                    }
                }
            }

            return splits.Count;
        }

        private Point? FindNextSplit(char[][] grid, Point start)
        {
            var r = start.x + 1;
            var c = start.y;

            while (r < grid.Length)
            {

                if (grid[r][c] == '^')
                {
                    return new Point(r, c);
                }

                r++;
            }

            return null;
        }
    }
}
