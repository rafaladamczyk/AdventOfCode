using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;
using Microsoft.Z3;

namespace AoC2023
{
    public class Day24 : IAocDay
    {
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
            var stones = new List<(Point3df pos, Point3df v)>();
            foreach (var line in input)
            {
                var parts = line.Split('@', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var p = parts[0].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
                var v = parts[1].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

                var pos = new Point3df(p[0], p[1], p[2]);
                var vel = new Point3df(v[0], v[1], v[2]);

                stones.Add((pos, vel));
            }

            var context = new Context();
            var solver = context.MkSolver();

            var throwPosX = context.MkIntConst("posx");
            var throwPosY = context.MkIntConst("posy");
            var throwPosZ = context.MkIntConst("posz");
            var throwVelX = context.MkIntConst("vx");
            var throwVelY = context.MkIntConst("vy");
            var throwVelZ = context.MkIntConst("vz");

            for (var i = 0; i < 10; i++) // not sure, but it seems to me adding more ts to evaluate actually makes the solver go faster. it works with i as low as 3, but around 10 seems to be the sweet spot
            {
                var t = context.MkIntConst($"time{i}");
                var stone = stones[i];
                var spx = context.MkInt((long)stone.pos.x);
                var spy = context.MkInt((long)stone.pos.y);
                var spz = context.MkInt((long)stone.pos.z);
                var svx = context.MkInt((long)stone.v.x);
                var svy = context.MkInt((long)stone.v.y);
                var svz = context.MkInt((long)stone.v.z);

                var xLeft = context.MkAdd(throwPosX, context.MkMul(t, throwVelX));
                var yLeft = context.MkAdd(throwPosY, context.MkMul(t, throwVelY)); 
                var zLeft = context.MkAdd(throwPosZ, context.MkMul(t, throwVelZ)); 
                var xRight = context.MkAdd(spx, context.MkMul(t, svx)); 
                var yRight = context.MkAdd(spy, context.MkMul(t, svy)); 
                var zRight = context.MkAdd(spz, context.MkMul(t, svz)); 

                solver.Add(t >= 0);
                solver.Add(context.MkEq(xLeft, xRight));
                solver.Add(context.MkEq(yLeft, yRight));
                solver.Add(context.MkEq(zLeft, zRight));
            }

            solver.Check();
            var model = solver.Model;

            var answerx = (IntNum)model.Eval(throwPosX);
            var answery = (IntNum)model.Eval(throwPosY);
            var answerz = (IntNum)model.Eval(throwPosZ);

            return answerx.Int64 + answery.Int64 + answerz.Int64;
        }

        public Point3df? Intersect2d((Point3df pos, Point3df v) first, (Point3df pos, Point3df v) second)
        {
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
    }
}
