using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day13 : IAocDay
    {
        public async Task<object> Part1()
        {
            var ans = 0;
            var input = await IO.GetInputString(2023, 13);

            var grids = input
                .Split($"\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                    x.Split("\n",
                        StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x.Select(y => y.ToCharArray()).ToArray()).ToList();

            foreach (var grid in grids)
            {
                var s = GetSymmetry(grid);
                if (s.col != null)
                {
                    ans += s.col.Value + 1;
                }
                else if (s.row != null)
                {
                    ans += 100 * (s.row.Value + 1);
                }
            }
            
            return ans;
        }

        public async Task<object> Part2()
        {
            var ans = 0;
            var input = await IO.GetInputString(2023, 13);

            var grids = input
                .Split($"\n\n", StringSplitOptions.RemoveEmptyEntries)
                .Select(x =>
                    x.Split("\n",
                        StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x.Select(y => y.ToCharArray()).ToArray()).ToList();

            foreach (var baseGrid in grids)
            {
                var oldSyms = GetSymmetry(baseGrid);

                foreach (var grid in GenerateGrids(baseGrid))
                {
                    int? colSymmetry = null;
                    int? rowSymmetry = null;

                    for (int c = 0; c < grid[0].Length - 1; c++)
                    {
                        if (oldSyms.col != null && c == oldSyms.col.Value)
                        {
                            continue;
                        }

                        if (ColSymmetry(grid, c, c + 1))
                        {
                            colSymmetry = c + 1;
                            break;
                        }
                    }

                    for (int r = 0; r < grid.Length - 1; r++)
                    {
                        if (oldSyms.row != null && r == oldSyms.row.Value)
                        {
                            continue;
                        }

                        if (RowSymmetry(grid, r, r + 1))
                        {
                            rowSymmetry = 100 * (r + 1);
                            break;
                        }
                    }

                    var score = colSymmetry ?? (rowSymmetry ?? 0);
                    ans += score;
                    if (score > 0)
                    {
                        break; // exactly 1 smudge
                    }
                }
            }

            return ans;
        }

        public bool ColSymmetry(char[][] grid, int c1, int c2)
        {
            if (!SameColumns(grid, c1, c2))
            {
                return false;
            }

            for (int c = c1 - 1; c >= 0; c--)
            {
                var other = c1 - c + c1 + 1;
                if (other < grid[0].Length)
                {
                    if (!SameColumns(grid, c, other))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool SameColumns(char[][] grid, int c1, int c2)
        {
            for (int r = 0; r < grid.Length; r++)
            {
                if (grid[r][c1] != grid[r][c2])
                {
                    return false;
                }
            }

            return true;
        }

        public bool RowSymmetry(char[][] grid, int r1, int r2)
        {
            if (!SameRows(grid, r1, r2))
            {
                return false;
            }

            for (int c = r1 - 1; c >= 0; c--)
            {
                var other = r1 - c + r1 + 1;
                if (other < grid.Length)
                {
                    if (!SameRows(grid, c, other))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public bool SameRows(char[][] grid, int r1, int r2)
        {
            for (int c = 0; c < grid[0].Length; c++)
            {
                if (grid[r1][c] != grid[r2][c])
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerable<char[][]> GenerateGrids(char[][] baseGrid)
        {
            for (int r = 0; r < baseGrid.Length; r++)
            {
                for (int c = 0; c < baseGrid[0].Length; c++)
                {
                    var newGrid = baseGrid.ToArray();
                    for (int x = 0; x < newGrid.Length; x++)
                    {
                        newGrid[x] = baseGrid[x].ToArray();
                    }

                    var oldChar = baseGrid[r][c];
                    newGrid[r][c] = oldChar == '.' ? '#' : '.';
                    yield return newGrid;
                }
            }
        }

        private (int? row, int? col) GetSymmetry(char[][] grid)
        {
            for (int c = 0; c < grid[0].Length - 1; c++)
            {
                if (ColSymmetry(grid, c, c + 1))
                {
                    return (null, c);
                }
            }

            for (int r = 0; r < grid.Length - 1; r++)
            {
                if (RowSymmetry(grid, r, r + 1))
                {
                    return (r, null);
                }
            }

            return (null, null);
        }
    }
}
