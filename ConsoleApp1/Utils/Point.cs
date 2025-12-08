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

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public int x;
        public int y;

        public static Point operator +(Point a, Point b) => new() { x = a.x + b.x, y = a.y + b.y };
        public static Point operator -(Point a, Point b) => new() { x = a.x - b.x, y = a.y - b.y };
        public static Point operator *(Point a, int x) => new() { x = a.x * x, y = a.y * x };

        public Point TurnLeft()
        {
            if (x == 0)
            {
                if (y > 0)
                {
                    return new Point(-1, 0);
                }
                else
                {
                    return new Point(1, 0);
                }
            }

            if (y == 0)
            {
                if (x > 0)
                {
                    return new Point(0, 1);
                }
                else
                {
                    return new Point(0, -1);
                }
            }

            throw new Exception();
        }

        public static int ManhattanDistance(Point a, Point b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public Point TurnRight() => TurnLeft() * -1;

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

        public static char[][] GetSparseGrid(List<Point> points)
        {
            var set = points.ToHashSet();
            var minR = points.Min(p => p.x);
            var minC = points.Min(p => p.y);
            var maxR = points.Max(p => p.x) + 1;
            var maxC = points.Max(p => p.y) + 1;

            var sizeR = Math.Abs(maxR - minR);
            var sizeC = Math.Abs(maxC - minC);

            var result = new char[sizeR][];

            for (int r = 0; r < sizeR; r++)
            {
                result[r] = new char[sizeC];
                for (int c = 0; c < sizeC; c++)
                {
                    result[r][c] = set.Contains(new Point(minR+r, minC+c)) ? '#' : '.';
                }
            }

            return result;
        }

        public bool IsWithinGrid(int maxX, int maxY)
        {
            return x < maxX && y < maxY && x >= 0 && y >= 0;
        }


        public static void PrintSparseGrid(IReadOnlyCollection<Point> points)
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

        public override string ToString()
        {
            return $"'{this.x},{this.y}'";
        }

        public static bool AreColinear(Point a, Point b, Point c)
        {
            return (b.x - a.x) * (c.y - a.y) == (b.y - a.y) * (c.x - a.x);
        }
    }

    [DebuggerDisplay("{x},{y},{z}")]
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

        public double EuclidianDistance(Point3d other)
        {
            var x2 = (long)(x - other.x) * (long)(x - other.x);
            var y2 = (long)(y - other.y) * (long)(y - other.y);
            var z2 = (long) (z - other.z) * (long)(z - other.z);
            var distance = Math.Sqrt(x2 + y2 + z2);

            if (double.IsNaN(distance))
            {
                throw new Exception("Nani");
            }

            return distance;
        }

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
                return x.GetHashCode() ^ y.GetHashCode() ^ z.GetHashCode();
            }
        }

        public IEnumerable<Point3d> GetNeighbors(bool includeDiagonal = false)
        {
            foreach (var dir in GetDirs(includeDiagonal))
            {
                yield return this + dir;
            }
        }

        public override string ToString()
        {
            return $"'{this.x},{this.y}',{this.z}";
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

    [DebuggerDisplay("{x},{y},{z}")]
    public struct Point3df
    {
        public Point3df()
        {
            x = 0;
            y = 0;
            z = 0;
        }

        public Point3df(double x, double y, double z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public double x;
        public double y;
        public double z;

        public static Point3df operator +(Point3df a, Point3df b) => new() { x = a.x + b.x, y = a.y + b.y, z = a.z + b.z };
        public static Point3df operator -(Point3df a, Point3df b) => new() { x = a.x - b.x, y = a.y - b.y, z = a.z - b.z };

        public override bool Equals(object obj)
        {
            if (obj is not Point3df p)
            {
                return false;
            }

            return p.x == x && p.y == y && p.z == z;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (13 * x.GetHashCode()) ^ (11 * y.GetHashCode()) ^ z.GetHashCode();
            }
        }

        public IEnumerable<Point3df> GetNeighbors(bool includeDiagonal = false)
        {
            foreach (var dir in GetDirs(includeDiagonal))
            {
                yield return this + dir;
            }
        }

        public static Point3df[] GetDirs(bool includeDiagonals = false)
        {
            if (includeDiagonals)
            {
                return new Point3df[]
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
                return new Point3df[]
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
