using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day10 : IAocDay
    {
        public async Task<object> Part1()
        {            
            var input = await IO.GetInput(2023, 10);
            var loop = GetLoop(input);
            return loop.Max(x => x.distanceFromStart);
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 10);
            var grid = input.Select(x => x.ToCharArray()).ToArray();
            var loop = GetLoop(input).ToDictionary(x => x.p);
            var start = loop.Values.Single(x => x.c == 'S');
            grid[start.p.x][start.p.y] = '|';

            var insidePoints = 0;
            for (var r = 0; r < grid.Length; r++)
            {
                bool inside = false;
                char? lastCorner = null;

                for (var c = 0; c < grid[0].Length; c++)
                {
                    var ch = grid[r][c];
                    if (loop.ContainsKey(new Point(r, c)))
                    {
                        switch (ch)
                        {
                            case '-':
                                break;
                            case '|':
                                inside = !inside;
                                lastCorner = null;
                                break;
                            case 'F':
                                lastCorner = 'F';
                                break;
                            case '7':
                                if (lastCorner == 'F')
                                {
                                    lastCorner = null;
                                }
                                else if (lastCorner == 'L')
                                {
                                    inside = !inside;
                                    lastCorner = null;
                                }
                                else
                                {
                                    lastCorner = '7';
                                }
                                break;
                            case 'J':
                                if (lastCorner == 'F')
                                {
                                    inside = !inside;
                                    lastCorner = null;
                                }
                                else if (lastCorner == 'L')
                                {
                                    lastCorner = null;
                                }
                                else
                                {
                                    lastCorner = 'J';
                                }
                                break;
                            case 'L':
                                lastCorner = 'L';
                                break;
                        }
                    }
                    else if (inside)
                    {
                        insidePoints++;
                    }
                }
            }


            return insidePoints;
        }

        private List<Node> GetLoop(List<string> input)
        {
            var nodes = new Dictionary<Point, Node>();

            var grid = input.Select(x => x.ToCharArray()).ToArray();
            for (var r = 0; r < grid.Length; r++)
            {
                for (var c = 0; c < grid[0].Length; c++)
                {
                    if (grid[r][c] != '.')
                    {
                        nodes.Add(new Point(r, c), new Node
                        {
                            p = new Point(r, c), c = grid[r][c],
                            distanceFromStart = grid[r][c] == 'S' ? 0 : int.MaxValue - 1
                        });
                    }
                }
            }

            var start = nodes.Values.Single(x => x.c == 'S');
            foreach (var node in nodes.Values)
            {
                if (node == start)
                {
                    continue;
                }

                var neighbors = FindNeighbors(node).Select(p => nodes.TryGetValue(p, out var v) ? v : null)
                    .Where(x => x != null).ToList();
                foreach (var neighbor in neighbors)
                {
                    node.neighbors.Add(neighbor);
                    if (neighbor == start)
                    {
                        neighbor.neighbors.Add(node);
                    }
                }
            }

            var currents = new List<Node> { start };
            var visited = new HashSet<Point>();

            while (currents.Count > 0)
            {
                var nexts = new List<Node>();
                var notVisitedBefore = currents.Where(x => !visited.Contains(x.p));
                foreach (var current in notVisitedBefore)
                {
                    current.distanceFromStart = Math.Min(current.neighbors.Min(x => x.distanceFromStart) + 1,
                        current.distanceFromStart);
                    nexts.AddRange(current.neighbors);
                    visited.Add(current.p);
                }

                currents = nexts;
            }

            return nodes.Values.Where(x => x.distanceFromStart < int.MaxValue - 1).ToList();
        }

        private IEnumerable<Point> FindNeighbors(Node node)
        {
            switch (node.c)
            {
                case '|':
                    yield return node.p + new Point(1, 0);
                    yield return node.p + new Point(-1, 0);
                    break;
                case '-':
                    yield return node.p + new Point(0, 1);
                    yield return node.p + new Point(0, -1);
                    break;
                case 'F':
                    yield return node.p + new Point(1, 0);
                    yield return node.p + new Point(0, 1);
                    break;
                case '7':
                    yield return node.p + new Point(1, 0);
                    yield return node.p + new Point(0, -1);
                    break;
                case 'L':
                    yield return node.p + new Point(-1, 0);
                    yield return node.p + new Point(0, 1);
                    break;
                case 'J':
                    yield return node.p + new Point(-1, 0);
                    yield return node.p + new Point(0, -1);
                    break;
                default:
                    throw new Exception();
            }
        }
    }

    [DebuggerDisplay("{p.x},{p.y} {c.ToString()}")]
    public class Node
    {
        public HashSet<Node> neighbors = new();
        public int distanceFromStart;
        public Point p;
        public char c;
    }
}