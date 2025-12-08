using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2025
{
    public class Day8 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 8);
            var points = new List<Point3d>();
            var circuits = new List<HashSet<Point3d>>();
            foreach (var line in input)
            {
                var split = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                points.Add(new Point3d(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
            }

            var conns = new List<Connection>();
            for (int j = 0; j < points.Count; j++)
            {
                for (int k = j + 1; k < points.Count; k++)
                {
                    conns.Add(new Connection
                        { a = points[j], b = points[k], distance = points[j].EuclidianDistance(points[k]) });
                }
            }

            foreach (var connection in conns.OrderBy(c => c.distance).Take(1000))
            {
                var toMerge = new List<HashSet<Point3d>>();
                foreach (var circuit in circuits)
                {
                    if (circuit.Contains(connection.a) || circuit.Contains(connection.b))
                    {
                        circuit.Add(connection.a);
                        circuit.Add(connection.b);
                        toMerge.Add(circuit);
                    }
                }

                if (toMerge.Count == 0)
                {
                    var circuit = new HashSet<Point3d> { connection.a, connection.b };
                    circuits.Add(circuit);
                }

                if (toMerge.Count > 1)
                {
                    var merged = new HashSet<Point3d>();
                    foreach (var c in toMerge)
                    {
                        circuits.Remove(c);
                        foreach (var cc in c)
                        {
                            merged.Add(cc);
                        }
                    }

                    circuits.Add(merged);
                }
            }

            var biggest = circuits.OrderByDescending(x => x.Count);
            return biggest.Take(3).Aggregate(1, (acc, x) => acc * x.Count);
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 8);
            var points = new List<Point3d>();
            var circuits = new List<HashSet<Point3d>>();
            foreach (var line in input)
            {
                var split = line.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                points.Add(new Point3d(int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
            }

            var conns = new List<Connection>();
            for (int j = 0; j < points.Count; j++)
            {
                for (int k = j + 1; k < points.Count; k++)
                {
                    conns.Add(new Connection
                        { a = points[j], b = points[k], distance = points[j].EuclidianDistance(points[k]) });
                }
            }

            foreach (var connection in conns.OrderBy(c => c.distance))
            {
                var toMerge = new List<HashSet<Point3d>>();
                foreach (var circuit in circuits)
                {
                    if (circuit.Contains(connection.a) || circuit.Contains(connection.b))
                    {
                        circuit.Add(connection.a);
                        circuit.Add(connection.b);
                        toMerge.Add(circuit);
                    }
                }

                if (toMerge.Count == 0)
                {
                    var circuit = new HashSet<Point3d> { connection.a, connection.b };
                    circuits.Add(circuit);
                }

                if (toMerge.Count > 1)
                {
                    var merged = new HashSet<Point3d>();
                    foreach (var c in toMerge)
                    {
                        circuits.Remove(c);
                        foreach (var cc in c)
                        {
                            merged.Add(cc);
                        }
                    }

                    circuits.Add(merged);
                }

                if (circuits.Count == 1 && circuits[0].Count == points.Count)
                {
                    return connection.a.x * connection.b.x;
                }
            }

            throw new Exception("Nani");
        }

        public class Connection
        {
            public Point3d a;
            public Point3d b;
            public double distance;
        }
    }
}
