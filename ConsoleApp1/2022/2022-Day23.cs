using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2022
{
    public class Day23 : IAocDay
    { 
        private static void Print(HashSet<(int, int)> elfs)
        {
            var minX = 999999;
            var minY = 9999999;
            var maxY = -999999;
            var maxX = -9999999;

            foreach (var elf in elfs)
            {
                if (elf.Item1 > maxX)
                {
                    maxX = elf.Item1;
                }

                if (elf.Item2 > maxY)
                {
                    maxY = elf.Item2;
                }

                if (elf.Item1 < minX)
                {
                    minX = elf.Item1;
                }

                if (elf.Item2 < minY)
                {
                    minY = elf.Item2;
                }
            }

            var area = (Math.Abs(maxX - minX) + 1) * (Math.Abs(maxY - minY) + 1);
            var emptyTiles = area - elfs.Count;

            StringBuilder screen = new StringBuilder();
            for (int y = minY; y <= maxY; y++)
            {
                StringBuilder sb = new StringBuilder();
                for (int x = minX; x <= maxX; x++)
                {
                    if (elfs.Contains((x, y)))
                    {
                        sb.Append('#');
                    }
                    else
                    {
                        sb.Append('.');
                    }
                }

                screen.AppendLine(sb.ToString());
            }

            Console.WriteLine(screen.ToString());
            Console.WriteLine($"AREA: {area}   X: {minX} - {maxX}   Y: {minY} - {maxY}");
            Console.WriteLine(emptyTiles);
        }

        public async Task<object> Part1()
        {
            var input = await Input.GetInput(2022, 23);
            var rowLength = input.First().Length;

            var map = new char[rowLength, input.Count];
            var elfs = new HashSet<(int, int)>();

            for (int y = 0; y < input.Count; y++)
            {
                for (int x = 0; x < rowLength; x++)
                {
                    map[x, y] = input[y].ElementAt(x);
                    if (map[x, y] == '#')
                    {
                        elfs.Add((x, y));
                    }
                }
            }

            int roundsCounter = 0;

            bool AllDirectionsFree((int x, int y) c)
            {
                return !elfs.Contains((c.x + 1, c.y)) &&
                       !elfs.Contains((c.x + 1, c.y + 1)) &&
                       !elfs.Contains((c.x + 1, c.y - 1)) &&
                       !elfs.Contains((c.x - 1, c.y)) &&
                       !elfs.Contains((c.x - 1, c.y + 1)) &&
                       !elfs.Contains((c.x - 1, c.y - 1)) &&
                       !elfs.Contains((c.x, c.y + 1)) &&
                       !elfs.Contains((c.x, c.y - 1));
            }

            IEnumerable<(List<(int, int)> candidates, (int x, int y)? chosenMove)> GenerateCandidates((int x, int y) c)
            {
                var north = new List<(int x, int y)>();
                north.Add((c.x - 1, c.y - 1));
                north.Add((c.x, c.y - 1));
                north.Add((c.x + 1, c.y - 1));

                var south = new List<(int x, int y)>();
                south.Add((c.x - 1, c.y + 1));
                south.Add((c.x, c.y + 1));
                south.Add((c.x + 1, c.y + 1));

                var west = new List<(int x, int y)>();
                west.Add((c.x - 1, c.y - 1));
                west.Add((c.x - 1, c.y));
                west.Add((c.x - 1, c.y + 1));

                var east = new List<(int x, int y)>();
                east.Add((c.x + 1, c.y - 1));
                east.Add((c.x + 1, c.y));
                east.Add((c.x + 1, c.y + 1));

                yield return (north, (c.x, c.y - 1));
                yield return (south, (c.x, c.y + 1));
                yield return (west, (c.x - 1, c.y));
                yield return (east, (c.x + 1, c.y));
                yield return (north, (c.x, c.y - 1));
                yield return (south, (c.x, c.y + 1));
                yield return (west, (c.x - 1, c.y));
                yield return (east, (c.x + 1, c.y));

                yield return (new List<(int x, int y)>(), null);
            }

            while (true)
            {
                var chosenMoves = new Dictionary<(int x, int y), (int newX, int newY)>();
                if (elfs.All(x => AllDirectionsFree(x)))
                {
                    Print(elfs);
                    Console.WriteLine($"finished after round {roundsCounter + 1}");
                    return roundsCounter + 1;
                }

                foreach (var elf in elfs)
                {
                    if (AllDirectionsFree(elf))
                    {
                        chosenMoves[elf] = elf;
                    }
                    else
                    {
                        var candidateMoves = GenerateCandidates(elf).Skip(roundsCounter % 4);
                        var myChosenMove = candidateMoves.First(m => m.candidates.All(x => !elfs.Contains(x)));
                        if (myChosenMove.chosenMove != null)
                        {
                            chosenMoves[elf] = myChosenMove.chosenMove.Value;
                        }
                    }
                }

                var newMoves = chosenMoves.Values.ToLookup(x => x);

                foreach (var kvp in chosenMoves)
                {
                    var oldPos = kvp.Key;
                    var newPos = kvp.Value;

                    if (newMoves[newPos].Count() == 1)
                    {
                        elfs.Remove(oldPos);
                        elfs.Add(newPos);
                    }
                }

                roundsCounter++;

                //Console.Clear();
                //Console.WriteLine($"After round: {roundsCounter}");
                //Print(elfs);

                //if (roundsCounter == 10)
                //{
                //    Print(elfs);
                //    break;
                //}
            }
        }

        public async Task<object> Part2()
        {
            return await Part1();
        }
    }
}
