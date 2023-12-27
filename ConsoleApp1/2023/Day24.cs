using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day24 : IAocDay
    {
        public Point3df? Intersect2d((Point3df pos, Point3df v) first, (Point3df pos, Point3df v) second)
        {
            //if (Math.Sign(first.v.x) != Math.Sign(second.v.x))
            //{
            //    return null; // they will never intersect and we're not interested in past intersections
            //}

            //if (Math.Sign(first.v.y) != Math.Sign(second.v.y))
            //{
            //    return null; // they will never intersect and we're not interested in past intersections
            //}
            
            var a1 = first.v.y;
            var b1 = -first.v.x;
            var c1 = a1 * first.pos.x + b1 * first.pos.y;

            var a2 = second.v.y;
            var b2 = -second.v.x;
            var c2 = a2 * second.pos.x + b2 * second.pos.y;

            var determinant = a1 * b2 - a2 * b1;
            if (determinant == 0) //parallel lines
            {
                return null;
            }

            var x = (b2 * c1 - b1 * c2) / determinant;
            var y = (a1 * c2 - a2 * c1) / determinant;

            foreach (var stone in new[] { first, second })
            {
                var oldDistanceX = Math.Abs(stone.pos.x - x);
                var oldDistanceY = Math.Abs(stone.pos.y - y);
                var newDistanceX = Math.Abs(stone.pos.x + stone.v.x - x);
                var newDistanceY = Math.Abs(stone.pos.y + stone.v.y - y);

                if (oldDistanceX < newDistanceX) return null; // collided in the past
                if (oldDistanceY < newDistanceY) return null;
            }

            return new Point3df(x, y, 0);
        }

        public async Task<object> Part1()
        {
            var stones = new List<(Point3df pos, Point3df v)>();
            //var input = await IO.GetExampleInput();
            var input = await IO.GetInput(2023, 24);
            foreach (var line in input)
            {
                var parts = line.Split('@', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var p = parts[0].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                var v = parts[1].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

                var pos = new Point3df(p[0], p[1], p[2]);
                var vel = new Point3df(v[0], v[1], v[2]);

                stones.Add((pos, vel));
            }

            var count = 0;
            var min = 200000000000000;
            var max = 400000000000000;
            foreach (var first in stones)
            {
                foreach (var second in stones)
                {
                    if (first.Equals(second)) continue;

                    var i = Intersect2d(first, second);
                    if (i != null && i.Value.x >= min && i.Value.x <= max && i.Value.y >= min && i.Value.y <= max)
                    {
                        count++;
                    }
                }
            }

            return count / 2;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 24);
            return 3;
        }
    }
}
