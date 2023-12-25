using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day25 : IAocDay
    {
        public async Task<object> Part1()
        {
            var rng = new Random();
            var input = await IO.GetInput(2023, 25);
            var V = new HashSet<string>();
            var E = new List<(string, string)>();

            foreach (var line in input)
            {
                var parts = line.Split(':', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                string component = parts[0];
                parts = parts[1].Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                var neighbors = new HashSet<string>(parts);

                if (!V.Contains(component))
                {
                    V.Add(component);
                }

                foreach (var c in neighbors)
                {
                    if (!V.Contains(c))
                    {
                        V.Add(c);
                    }

                    if (!E.Contains((component, c)) && !E.Contains((c, component)))
                    {
                        E.Add((component, c));
                    }
                }
            }

            var groups = new List<HashSet<string>>();
            var count = 0;

            while (count != 3)
            {
                groups = new List<HashSet<string>>();
                foreach (var v in V)
                {
                    groups.Add(new HashSet<string> { v });
                }

                // Krager's
                while (groups.Count > 2)
                {
                    var x = rng.Next(0, E.Count);
                    var g1 = groups.First(s => s.Contains(E[x].Item1));
                    var g2 = groups.First(s => s.Contains(E[x].Item2));

                    if (g1 == g2)
                    {
                        continue;
                    }

                    groups.Remove(g2);
                    foreach (var gg in g2)
                    {
                        g1.Add(gg);
                    }
                }

                count = Count(groups);
            }

            int Count(List<HashSet<string>> g)
            {
                int cuts = 0;
                for (int i = 0; i < E.Count; i++)
                {
                    var subset1 = g.First(s => s.Contains(E[i].Item1));
                    var subset2 = g.First(s => s.Contains(E[i].Item2));
                    if (subset1 != subset2)
                    {
                        cuts++;
                    }
                }

                return cuts;
            }


            return groups[0].Count * groups[1].Count;
        }

        public async Task<object> Part2()
        {
            return 3;
        }
    }
}
