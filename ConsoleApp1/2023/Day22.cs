using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day22 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2023, 22);

            var bricks = new List<Brick>();
            var occupiedSpace = new HashSet<Point3d>();
            ParseInput(input, bricks, occupiedSpace);

            StepFall(bricks, occupiedSpace, true);

            var canBeDestroyed = 0;
            foreach (var brick in bricks)
            {
                var otherBricks = bricks.Except(new[] { brick }).Select(x => x.Copy()).ToList();
                var anyHaveFallen = StepFall(otherBricks, otherBricks.SelectMany(b => b.points).ToHashSet(), false) > 0;
                if (!anyHaveFallen)
                {
                    canBeDestroyed++;
                }
            }

            return canBeDestroyed;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 22);

            var bricks = new List<Brick>();
            var occupiedSpace = new HashSet<Point3d>();
            ParseInput(input, bricks, occupiedSpace);

            StepFall(bricks, occupiedSpace, true);

            var fallenBricks = 0;
            foreach (var brick in bricks)
            {
                var otherBricks = bricks.Except(new[] { brick }).Select(x => x.Copy()).ToList();
                fallenBricks += StepFall(otherBricks, otherBricks.SelectMany(b => b.points).ToHashSet(), true);
            }

            return fallenBricks;
        }

        private static void ParseInput(List<string> input, List<Brick> bricks, HashSet<Point3d> occupiedSpace)
        {
            foreach (var line in input)
            {
                var start = line.Split('~')[0].Split(',').Select(int.Parse).ToList();
                var end = line.Split('~')[1].Split(',').Select(int.Parse).ToList();

                var startPoint = new Point3d(start[0], start[1], start[2]);
                var endPoint = new Point3d(end[0], end[1], end[2]);

                var brick = new Brick(startPoint, endPoint);
                bricks.Add(brick);
                foreach (var p in brick.points)
                {
                    occupiedSpace.Add(p);
                }
            }
        }

        private static int StepFall(List<Brick> bricks, HashSet<Point3d> occupiedSpace, bool untilRest)
        {
            var fallenBricks = new HashSet<Brick>();
            int fallenInPreviousStep;

            do
            {
                fallenInPreviousStep = 0;
                foreach (var brick in bricks)
                {
                    var fallen = new Brick(brick.start, brick.end, brick.points.ToList());
                    if (!fallen.Fall())
                    {
                        continue; // can't fall further, we're on the ground
                    }

                    foreach (var p in brick.points)
                    {
                        occupiedSpace.Remove(p);
                    }

                    bool collides = false;
                    foreach (var fp in fallen.points)
                    {
                        if (occupiedSpace.Contains(fp))
                        {
                            // collision, can't fall;
                            collides = true;
                            break;
                        }
                    }

                    if (!collides) // we can fall
                    {
                        brick.Fall();
                        fallenBricks.Add(brick);
                        fallenInPreviousStep++;
                    }

                    foreach (var p in brick.points)
                    {
                        occupiedSpace.Add(p);
                    }
                }
            } while (untilRest && fallenInPreviousStep > 0);

            return fallenBricks.Count;
        }

        [DebuggerDisplay("{start} ~ {end}")]
        public class Brick
        {
            public List<Point3d> points = new();
            public Point3d start;
            public Point3d end;

            public Brick(Point3d start, Point3d end, List<Point3d> points)
            {
                this.points = points;
                this.start = start;
                this.end = end;
            }

            public Brick Copy()
            {
                return new Brick(start, end, points.ToList());
            }

            public Brick(Point3d start, Point3d end)
            {
                this.start = start;
                this.end = end;

                if (start.x == end.x && start.y == end.y)
                {
                    var small = Math.Min(start.z, end.z);
                    var big = Math.Max(start.z, end.z);

                    for (int i = small; i <= big; i++)
                    {
                        points.Add(new Point3d(start.x, start.y, i));
                    }
                }

                if (start.x == end.x && start.z == end.z)
                {
                    var small = Math.Min(start.y, end.y);
                    var big = Math.Max(start.y, end.y);

                    for (int i = small; i <= big; i++)
                    {
                        points.Add(new Point3d(start.x, i, start.z));
                    }
                }

                if (start.z == end.z && start.y == end.y)
                {
                    var small = Math.Min(start.x, end.x);
                    var big = Math.Max(start.x, end.x);

                    for (int i = small; i <= big; i++)
                    {
                        points.Add(new Point3d(i, start.y, start.z));
                    }
                }
            }

            public bool Fall()
            {
                if (points.Any(p => p.z <= 1))
                {
                    return false; //can't fall further
                }

                for (int i = 0; i < points.Count; i++)
                {
                    points[i] = new Point3d(points[i].x, points[i].y, points[i].z - 1);
                }

                start = new Point3d(start.x, start.y, start.z - 1);
                end = new Point3d(end.x, end.y, end.z - 1);

                return true;
            }
        }
    }
}
