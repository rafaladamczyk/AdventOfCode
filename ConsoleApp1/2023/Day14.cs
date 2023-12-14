using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day14 : IAocDay
    {
        public async Task<object> Part1()
        {
            var ans = 0;
            var input = await IO.GetInput(2023, 14);
            var grid = input.Select(x => x.ToCharArray()).ToArray();

            Cycle(grid);

            for (var r = 0; r < grid.Length; r++)
            {
                for (var c = 0; c < grid[0].Length; c++)
                {
                    if (grid[r][c] == 'O')
                    {
                        ans += grid.Length - r;
                    }
                }
            }

            return ans;
        }

        public async Task<object> Part2()
        {
            var ans = 0;
            var input = await IO.GetInput(2023, 14);
            var grid = input.Select(x => x.ToCharArray()).ToArray();

            var known = new List<char[][]>();
            var cycleStart = -1;
            var cycleLength = -1;

            for (int i = 0; i < 1000000000; i++)
            {
                if (cycleStart > 0)
                {
                    break;
                }

                Cycle4(grid);

                var gridCopy = grid.ToArray();
                for (int x = 0; x < gridCopy.Length; x++)
                {
                    gridCopy[x] = grid[x].ToArray();
                }

                for (int x = 0; x < known.Count; x++)
                {
                    if (SameGrid(known[x], gridCopy))
                    {
                        cycleStart = x;
                        cycleLength = i - x;
                    }
                }

                if (cycleStart == -1)
                {
                    known.Add(gridCopy);
                }
            }

            var gridIndex = (1000000000 - cycleStart - 1) % cycleLength + cycleStart;
            var theGrid = known[gridIndex];

            for (var r = 0; r < theGrid.Length; r++)
            {
                for (var c = 0; c < theGrid[0].Length; c++)
                {
                    if (theGrid[r][c] == 'O')
                    {
                        ans += theGrid.Length - r;
                    }
                }
            }

            return ans;
        }

        private bool SameGrid(char[][] g1, char[][] g2)
        {
            for (int r = 0; r < g1.Length; r++)
            {
                for (int c = 0; c < g1.Length; c++)
                {
                    if (g1[r][c] != g2[r][c])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private static void Cycle(char[][] grid)
        {
            for (var rr = 0; rr < grid.Length; rr++)
            {
                for (var r = 1; r < grid.Length; r++)
                {
                    for (var c = 0; c < grid[0].Length; c++)
                    {
                        var item = grid[r][c];
                        if (item == 'O' && grid[r - 1][c] == '.')
                        {
                            grid[r][c] = '.';
                            grid[r - 1][c] = 'O';
                        }
                    }
                }
            }
        }

        private static void Cycle4(char[][] grid)
        {
            //north
            for (var rr = 0; rr < grid.Length; rr++)
            {
                for (var r = 1; r < grid.Length; r++)
                {
                    for (var c = 0; c < grid[0].Length; c++)
                    {
                        var item = grid[r][c];
                        if (item == 'O' && grid[r - 1][c] == '.')
                        {
                            grid[r][c] = '.';
                            grid[r - 1][c] = 'O';
                        }
                    }
                }
            }

            //west
            for (var cc = 0; cc < grid[0].Length; cc++)
            {
                for (var r = 0; r < grid.Length; r++)
                {
                    for (var c = 1; c < grid[0].Length; c++)
                    {
                        var item = grid[r][c];
                        if (item == 'O' && grid[r][c - 1] == '.')
                        {
                            grid[r][c] = '.';
                            grid[r][c - 1] = 'O';
                        }
                    }
                }
            }

            //south
            for (var rr = 0; rr < grid.Length; rr++)
            {
                for (var r = grid.Length - 2; r >=0; r--)
                {
                    for (var c = 0; c < grid[0].Length; c++)
                    {
                        var item = grid[r][c];
                        if (item == 'O' && grid[r + 1][c] == '.')
                        {
                            grid[r][c] = '.';
                            grid[r+1][c] = 'O';
                        }
                    }
                }
            }

            //east
            for (var cc = 0; cc < grid[0].Length; cc++)
            {
                for (var r = 0; r < grid.Length; r++)
                {
                    for (var c = grid[0].Length - 2; c >= 0; c--)
                    {
                        var item = grid[r][c];
                        if (item == 'O' && grid[r][c + 1] == '.')
                        {
                            grid[r][c] = '.';
                            grid[r][c+1] = 'O';
                        }
                    }
                }
            }
        }
    }
}
