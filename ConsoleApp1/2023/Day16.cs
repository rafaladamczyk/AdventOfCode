using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day16 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2023, 16);
            var grid = input.Select(x => x.ToCharArray()).ToArray();
            
            var ans = Energize(new Beam(new Point(0, -1), new Point(0, 1)), grid);
            return ans;
        }

        public async Task<object> Part2()
        {
            var sync = new object();
            var ans = 0;
            var input = await IO.GetInput(2023, 16);
            var grid = input.Select(x => x.ToCharArray()).ToArray();

            void UpdateMax(int result)
            {
                lock (sync)
                {
                    ans = Math.Max(ans, result);
                }
            }

            Parallel.For(0, grid.Length, r =>
            {
                var beam = new Beam(new Point(r, -1), new Point(0, 1));
                UpdateMax(Energize(beam, grid));
            });

            Parallel.For(0, grid.Length, r =>
            {
                var beam = new Beam(new Point(r, grid.Length), new Point(0, -1));
                UpdateMax(Energize(beam, grid));
            });

            Parallel.For(0, grid[0].Length, c =>
            {
                var beam = new Beam(new Point( -1, c), new Point(1, 0));
                UpdateMax(Energize(beam, grid));
            });

            Parallel.For(0, grid[0].Length, c =>
            {
                var beam = new Beam(new Point(grid[0].Length, c), new Point(-1, 0));
                UpdateMax(Energize(beam, grid));
            });

            return ans;
        }

        private static int Energize(Beam startingBeam, char[][] grid)
        {
            var gridPoints = new Dictionary<Point, char>();
            for (var r = 0; r < grid.Length; r++)
            {
                for (var c = 0; c < grid.Length; c++)
                {
                    gridPoints.Add(new Point(r, c), grid[r][c]);
                }
            }

            var spawnPoints = new HashSet<(Point start, Point dir)>();
            var energized = new HashSet<Point>();
            var Q = new Queue<Beam>();

            void EnqueueIfNotAlreadySpawned(Beam b)
            {
                if (!spawnPoints.Contains((b.points.Single(), b.dir)))
                {
                    Q.Enqueue(b);
                    spawnPoints.Add((b.points.Single(), b.dir));
                }
            }

            Q.Enqueue(startingBeam);
            while (Q.Count > 0)
            {
                var beam = Q.Dequeue();
                var done = false;

                while (!done)
                {
                    var currentPos = beam.points.Last();
                    energized.Add(currentPos);

                    var newPos = currentPos + beam.dir;
                    if (!gridPoints.TryGetValue(newPos, out var gridPoint))
                    {
                        break; // outside the grid
                    }

                    beam.points.Add(newPos);

                    switch (gridPoint)
                    {
                        case '|':
                            if (beam.dir.y != 0)
                            {
                                EnqueueIfNotAlreadySpawned(new Beam(newPos, new Point(-1, 0)));
                                EnqueueIfNotAlreadySpawned(new Beam(newPos, new Point(1, 0)));
                                done = true;
                            }

                            break;

                        case '-':
                            if (beam.dir.x != 0)
                            {
                                EnqueueIfNotAlreadySpawned(new Beam(newPos, new Point(0, -1)));
                                EnqueueIfNotAlreadySpawned(new Beam(newPos, new Point(0, 1)));
                                done = true;
                            }

                            break;

                        case '/':
                            if (beam.dir.y != 0)
                            {
                                beam.dir.x = -beam.dir.y;
                                beam.dir.y = 0;
                            }
                            else
                            {
                                beam.dir.y = -beam.dir.x;
                                beam.dir.x = 0;
                            }

                            break;

                        case '\\':
                            if (beam.dir.y != 0)
                            {
                                beam.dir.x = beam.dir.y;
                                beam.dir.y = 0;
                            }
                            else
                            {
                                beam.dir.y = beam.dir.x;
                                beam.dir.x = 0;
                            }

                            break;

                        case '.':
                        default:
                            break;
                    }
                }
            }

            var pointsInGrid = energized.Where(p => p.x < grid.Length && p.x >= 0 && p.y >= 0 && p.y < grid[0].Length);
            return pointsInGrid.Count();
        }

        public class Beam
        {
            public Beam(Point startingPoint, Point startingDir)
            {
                points = new List<Point> { startingPoint };
                dir = startingDir;
            }

            public List<Point> points;
            public Point dir;
        }
    }
}
