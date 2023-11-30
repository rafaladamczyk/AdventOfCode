using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using ConsoleApp1.Utils;

namespace AoC2022
{
    public class Day18 : IAocDay
    {

        public async Task<object> Part1()
        {
            var lavaCubes = new HashSet<(int x, int y, int z)>();
            var input = await Input.GetInput(2022, 18);
            foreach (var numbers in input.Select(line => line.Split(',').Select(x => int.Parse(x.Trim())).ToArray()))
            {
                lavaCubes.Add((numbers[0], numbers[1], numbers[2]));
            }

            int minx, miny, minz;
            int maxx, maxy, maxz;
            minx = miny = minz = 9999999;
            maxx = maxy = maxz = -1;

            foreach (var c in lavaCubes)
            {
                minx = Math.Min(minx, c.x);
                miny = Math.Min(miny, c.y);
                minz = Math.Min(minz, c.z);
                maxx = Math.Max(maxx, c.x);
                maxy = Math.Max(maxy, c.y);
                maxz = Math.Max(maxz, c.z);
            }

            minx--;
            miny--;
            minz--;
            maxx++;
            maxy++;
            maxz++;

            bool IsValidAir((int x, int y, int z) p)
            {
                bool withinBounds = p.x >= minx && p.x <= maxx && p.y >= miny && p.y <= maxy && p.z >= minz && p.z <= maxz;
                return withinBounds && !lavaCubes.Contains(p);
            }

            IEnumerable<(int, int, int)> GenerateNeighbors((int x, int y, int z) p)
            {
                yield return (p.x, p.y, p.z + 1);
                yield return (p.x, p.y, p.z - 1);
                yield return (p.x, p.y + 1, p.z);
                yield return (p.x, p.y - 1, p.z);
                yield return (p.x + 1, p.y, p.z);
                yield return (p.x - 1, p.y, p.z);
            }

            var airCubes = new HashSet<(int x, int y, int z)>();
            var Q = new Queue<(int x, int y, int z)>();
            Q.Enqueue((minx, miny, minz));

            while (Q.TryDequeue(out var p))
            {
                foreach (var neighbor in GenerateNeighbors(p).Where(IsValidAir))
                {
                    if (airCubes.Contains(neighbor))
                    {
                        continue;
                    }

                    airCubes.Add(neighbor);
                    Q.Enqueue(neighbor);
                }
            }

            var surfaceArea = 0;
            foreach (var p in lavaCubes)
            {
                int exposedSides = 0;
                foreach (var air in airCubes)
                {
                    if (p.x == air.x && p.y == air.y && p.z == air.z + 1)
                    {
                        exposedSides++;
                    }
                    if (p.x == air.x && p.y == air.y && p.z == air.z - 1)
                    {
                        exposedSides++;
                    }
                    if (p.x == air.x && p.y == air.y + 1 && p.z == air.z)
                    {
                        exposedSides++;
                    }
                    if (p.x == air.x && p.y == air.y - 1 && p.z == air.z)
                    {
                        exposedSides++;
                    }
                    if (p.x == air.x + 1 && p.y == air.y && p.z == air.z)
                    {
                        exposedSides++;
                    }
                    if (p.x == air.x - 1 && p.y == air.y && p.z == air.z)
                    {
                        exposedSides++;
                    }
                }

                surfaceArea += exposedSides;
            }

            return surfaceArea;
        }

        public async Task<object> Part2()
        {
            return await Part1();
        }
    }
}
