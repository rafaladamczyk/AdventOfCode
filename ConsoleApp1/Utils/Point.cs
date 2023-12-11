using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Utils
{
    [DebuggerDisplay("{x},{y}")]
    public struct Point
    {
        public Point()
        {
            x = 0;
            y = 0;
        }

        public Point(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public long x;
        public long y;

        public static Point operator +(Point a, Point b) => new() { x = a.x + b.x, y = a.y + b.y };
        public static Point operator -(Point a, Point b) => new() { x = a.x - b.x, y = a.y - b.y };

        public override bool Equals(object obj)
        {
            if (obj is not Point p)
            {
                return false;
            }

            return p.x == x && p.y == y;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return x.GetHashCode() ^ y.GetHashCode();
            }
        }

        public IEnumerable<Point> GetNeighbors(bool includeDiagonal = false)
        {
            foreach (var dir in GetDirs(includeDiagonal))
            {
                yield return this + dir;
            }
        }

        public static Point[] GetDirs(bool includeDiagonals = false)
        {
            if (includeDiagonals)
            {
                return new Point[]
                {
                    new() { x = 1, y = 0 },
                    new() { x = 1, y = 1 },
                    new() { x = 0, y = 1 },
                    new() { x = 1, y = -1 },
                    new() { x = -1, y = 0 },
                    new() { x = -1, y = -1 },
                    new() { x = -1, y = 1 },
                    new() { x = 0, y = -1 },
                };
            }
            else
            {
                return new Point[]
                {
                    new() { x = 1, y = 0 },
                    new() { x = 0, y = 1 },
                    new() { x = -1, y = 0 },
                    new() { x = 0, y = -1 },
                };
            }
        }

        public static void PrintSparseGrid(List<Point> points)
        {
            var set = points.ToHashSet();
            var maxR = points.Max(p => p.x);
            var maxC = points.Max(p => p.y);

            for (int r = 0; r <= maxR; r++)
            {
                for (int c = 0; c <= maxC; c++)
                {
                    Console.Write(set.Contains(new Point(r, c)) ? '#' : '.');
                }

                Console.WriteLine();
            }
        }
    }

    public struct Point3d
    {
        public Point3d()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Point3d(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public int x;
        public int y;
        public int z;

        public static Point3d operator +(Point3d a, Point3d b) => new() { x = a.x + b.x, y = a.y + b.y, z = a.z + b.z };
        public static Point3d operator -(Point3d a, Point3d b) => new() { x = a.x - b.x, y = a.y - b.y, z = a.z - b.z };

        public override bool Equals(object obj)
        {
            if (obj is not Point3d p)
            {
                return false;
            }

            return p.x == x && p.y == y && p.z == z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return x ^ y ^ z;
            }
        }

        public IEnumerable<Point3d> GetNeighbors(bool includeDiagonal = false)
        {
            foreach (var dir in GetDirs(includeDiagonal))
            {
                yield return this + dir;
            }
        }

        public static Point3d[] GetDirs(bool includeDiagonals = false)
        {
            if (includeDiagonals)
            {
                return new Point3d[]
                {
                    new(-1, -1, -1),
                    new(-1, -1, 0),
                    new(-1, -1, 1),
                    new(-1, 0, -1),
                    new(-1, 0, 0),
                    new(-1, 0, 1),
                    new(-1, 1, -1),
                    new(-1, 1, 0),
                    new(-1, 1, 1),
                    new(0, -1, -1),
                    new(0, -1, 0),
                    new(0, -1, 1),
                    new(0, 0, -1),
                    new(0, 0, 1),
                    new(0, 1, -1),
                    new(0, 1, 0),
                    new(0, 1, 1),
                    new(1, -1, -1),
                    new(1, -1, 0),
                    new(1, -1, 1),
                    new(1, 0, -1),
                    new(1, 0, 0),
                    new(1, 0, 1),
                    new(1, 1, -1),
                    new(1, 1, 0),
                    new(1, 1, 1)
                };
            }
            else
            {
                return new Point3d[]
                {
                    new() { x = 1, y = 0, z = 0 },
                    new() { x = 0, y = 1, z = 0 },
                    new() { x = 0, y = 0, z = 1 },

                    new() { x = -1, y = 0, z = 0 },
                    new() { x = 0, y = -1, z = 0 },
                    new() { x = 0, y = 0, z = -1 },
                };
            }
        }
    }
}
