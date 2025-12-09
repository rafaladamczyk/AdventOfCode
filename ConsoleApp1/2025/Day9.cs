using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2025
{
    public class Day9 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 9);
            var points = new List<Point>();
            foreach (var line in input)
            {
                var split = line.Split(',');
                var p = new Point(int.Parse(split[0]), int.Parse(split[1]));
                points.Add(p);
            }

            ulong maxArea = 0;

            for (var i = 0; i < points.Count; i++)
            {
                for (var j = i + 1; j < points.Count; j++)
                {
                    var a = points[i];
                    var b = points[j];
                    var area = (ulong)(1 + Math.Abs(a.x - b.x)) * (ulong)(1 + Math.Abs(a.y - b.y));
                    
                    if (area > maxArea)
                    {
                        maxArea = area;
                    }
                }
            }

            return maxArea;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 9);
            var redPoints = new List<Point>();
            var edges = new List<Edge>();
            foreach (var line in input)
            {
                var split = line.Split(',');
                var p = new Point(int.Parse(split[0]), int.Parse(split[1]));
                redPoints.Add(p);
            }

            for (int i = 0; i < redPoints.Count; i++)
            {
                var curr = redPoints[i];
                var next = i + 1 < redPoints.Count ? redPoints[i + 1] : redPoints[0];
                edges.Add(new Edge(curr, next));
            }

            ulong maxArea = 0;

            for (var i = 0; i < redPoints.Count; i++)
            {
                for (var j = i + 1; j < redPoints.Count; j++)
                {
                    var a = redPoints[i];
                    var b = redPoints[j];
                    if (a.x == b.x || a.y == b.y)
                    {
                        continue;
                    }

                    var rect = new[]
                    {
                        new Point(Math.Min(a.x, b.x), Math.Min(a.y, b.y)),
                        new Point(Math.Max(a.x, b.x), Math.Min(a.y, b.y)),
                        new Point(Math.Max(a.x, b.x), Math.Max(a.y, b.y)),
                        new Point(Math.Min(a.x, b.x), Math.Max(a.y, b.y))
                    };

                    var cornersAreInside = rect.All(p => IsInPolygon(p, redPoints));
                    if (!cornersAreInside)
                    {
                        continue;
                    }

                    var rectangleEdges = new[]
                    {
                        new Edge(rect[0], rect[1]),
                        new Edge(rect[1], rect[2]),
                        new Edge(rect[2], rect[3]),
                        new Edge(rect[3], rect[0])
                    };

                    var edgesAreInside = rectangleEdges.All(re => edges.All(e => !Edge.EdgesIntersect(re, e)));
                    if (edgesAreInside)
                    {
                        var area = (ulong)(1 + Math.Abs(a.x - b.x)) * (ulong)(1 + Math.Abs(a.y - b.y));
                        if (area > maxArea)
                        {
                            maxArea = area;
                        }
                    }
                }
            }

            return maxArea;
        }

        public static bool IsInPolygon(Point p, List<Point> polygon)
        {
            bool inside = false;
            for (int i = 0; i < polygon.Count; i++)
            {
                var a = polygon[i];
                var b = i + 1 < polygon.Count ? polygon[i + 1] : polygon[0];

                if (Point.AreColinear(a, b, p) && OnEdge(a, b, p))
                {
                    return true; // on edge
                }

                bool intersect = (b.y > p.y) != (a.y > p.y) &&
                                 p.x < (long)(a.x - b.x) * (p.y - b.y) / (a.y - b.y) + b.x;

                if (intersect)
                {
                    inside = !inside;
                }
            }

            return inside;

            bool OnEdge(Point a, Point b, Point p)
            {
                return p.x >= Math.Min(a.x, b.x) &&
                       p.x <= Math.Max(a.x, b.x) &&
                       p.y >= Math.Min(a.y, b.y) &&
                       p.y <= Math.Max(a.y, b.y);
            }
        }
    }
}
