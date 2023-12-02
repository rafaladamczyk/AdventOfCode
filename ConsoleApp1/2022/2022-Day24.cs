using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2022
{
    [StructLayout(LayoutKind.Explicit)]
    public struct Blizzard
    {
        [FieldOffset(0)] public uint blizzard;
        [FieldOffset(0)] public byte x;
        [FieldOffset(1)] public byte y;
        [FieldOffset(2)] public byte dir;
    }

    public class Day24 : IAocDay
    {
        public async Task<object> Part1()
        {
            char[,] map;
            List<Blizzard> blizzards = new List<Blizzard>();
            var lines = await IO.GetInput(2022, 24);

            var rowLength = lines[0].Length;
            var colLength = lines.Count;
            map = new char[rowLength, colLength];

            for (byte y = 0; y < lines.Count; y++)
            {
                for (byte x = 0; x < rowLength; x++)
                {
                    map[x, y] = lines[y][x] == '#' ? '#' : '.';
                    if (lines[y][x] != '.' && lines[y][x] != '#')
                    {
                        byte dir = 0;
                        switch (lines[y][x])
                        {
                            case '<':
                                dir = 0;
                                break;
                            case '>':
                                dir = 2;
                                break;
                            case '^':
                                dir = 1;
                                break;
                            case 'v':
                                dir = 3;
                                break;
                            default:
                                throw new Exception();
                        }

                        blizzards.Add(new Blizzard()
                        {
                            x = x,
                            y = y,
                            dir = dir
                        });
                    }
                }
            }

            (byte x, byte y) startPos = (0, 0);
            (byte x, byte y) endingPos = (0, 0);

            bool foundStart = false;
            for (byte y = 0; y < lines.Count; y++)
            {
                for (byte x = 0; x < rowLength; x++)
                {
                    if (lines[y][x] == '.')
                    {
                        if (!foundStart)
                        {
                            foundStart = true;
                            startPos = (x, y);
                        }

                        endingPos = (x, y);
                    }
                }
            }

            string PackBlizzards(List<Blizzard> blizzards)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var bliz in blizzards)
                {
                    sb.Append(bliz.blizzard.ToString()).Append('.');
                }

                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }

            List<Blizzard> UnpackBlizzards(string bliz)
            {
                return bliz.Split('.').Select(b => new Blizzard() { blizzard = uint.Parse(b) }).ToList();
            }

            int GetDistance((byte x, byte y) start, (byte x, byte y) end)
            {
                return Math.Abs(start.x - end.x) + Math.Abs(start.y - end.y);
            }

            IReadOnlyCollection<(byte x, byte y)> GetMoves((byte x, byte y) current, (byte x, byte y) end)
            {
                var moves = new List<(int x, int y)>
                {
                    (current.x, current.y),
                    (current.x + 1, current.y),
                    (current.x - 1, current.y),
                    (current.x, current.y + 1),
                    (current.x, current.y - 1),
                };

                return moves
                    .Where(m => m.x < rowLength && m.x >= 0 && m.y < colLength && m.y >= 0 && map[m.x, m.y] != '#')
                    .OrderBy(m => GetDistance(((byte)m.x, (byte)m.y), end))
                    .Select(m => ((byte)m.x, (byte)m.y))
                    .ToList();
            }

            IEnumerable<Blizzard> SimulateBlizzards(List<Blizzard> blizzards)
            {
                foreach (var bliz in blizzards)
                {
                    // dir - 0 - left, 1 - up, 2 - right, 3 - down
                    int dx = (bliz.dir % 2 != 1 ? bliz.dir - 1 : 0);
                    int dy = (bliz.dir % 2 == 1 ? bliz.dir - 2 : 0);

                    int newx = (byte)(bliz.x + dx);
                    int newy = (byte)(bliz.y + dy);

                    if (map[newx, newy] == '#')
                    {
                        while (true)
                        {
                            newx -= dx;
                            newy -= dy;
                            if (map[newx, newy] == '#')
                            {
                                break;
                            }
                        }

                        newx += dx;
                        newy += dy;
                    }

                    yield return new Blizzard { x = (byte)newx, y = (byte)newy, dir = bliz.dir };
                }
            }

            var evaluations = 0;
            var globalKnownStates = new HashSet<(int time, int x, int y, string blizzards)>();
            var globalBest = int.MaxValue;

            void Solve((byte x, byte y) currentPos, (byte x, byte y) endPos, int time, List<Blizzard> blizzards)
            {
                if (time >= globalBest)
                {
                    return;
                }

                var bestPossibleDistanceToEnd = GetDistance(currentPos, endPos);
                if (bestPossibleDistanceToEnd == 0)
                {
                    if (globalBest > time)
                    {
                        Console.WriteLine($"new best: {time} - after {evaluations} evaluations");
                        globalBest = time;
                    }

                    return;
                }

                if (time + bestPossibleDistanceToEnd >= globalBest)
                {
                    return;
                }

                if (blizzards.Any(b => GetDistance((b.x, b.y), currentPos) == 0))
                {
                    return;
                }

                var packedBlizzards = PackBlizzards(blizzards);
                if (globalKnownStates.Contains((time, currentPos.x, currentPos.y, packedBlizzards)))
                {
                    return;
                }
                else
                {
                    globalKnownStates.Add((time, currentPos.x, currentPos.y, packedBlizzards));
                }

                //PrintMap(blizzards, currentPos);

                evaluations++;
                var candidateMoves = GetMoves(currentPos, endPos);
                var updatedBlizzards = SimulateBlizzards(blizzards).ToList();
                foreach (var move in candidateMoves)
                {
                    Solve(move, endPos, time + 1, new List<Blizzard>(updatedBlizzards));
                }
            }

            void ClearPreviousRunData()
            {
                globalBest = int.MaxValue;
                globalKnownStates.Clear();
            }

            void PrintMap(List<Blizzard> blizs, (byte x, byte y) currentPos)
            {
                StringBuilder sb = new StringBuilder();
                for (int y = 0; y < colLength; y++)
                {
                    var line = new StringBuilder();
                    for (int x = 0; x < rowLength; x++)
                    {
                        line.Append(map[x, y]);
                        var blizzards = blizs.Where(b => b.x == x && b.y == y).ToList();
                        if (blizzards.Count == 1)
                        {
                            var blizDir = '?';
                            switch (blizzards[0].dir)
                            {
                                case 0:
                                    blizDir = '<';
                                    break;
                                case 2:
                                    blizDir = '>';
                                    break;
                                case 1:
                                    blizDir = '^';
                                    break;
                                case 3:
                                    blizDir = 'v';
                                    break;
                                default:
                                    throw new Exception();
                            }

                            line[x] = blizDir;
                        }
                        else if (blizzards.Count > 1)
                        {
                            line[x] = blizzards.Count.ToString()[0];
                        }

                        if (currentPos.x == x && currentPos.y == y)
                        {
                            line[x] = 'E';
                        }
                    }

                    sb.AppendLine(line.ToString());
                }

                Console.WriteLine(sb.ToString());
            }

            ClearPreviousRunData();
            Solve(startPos, endingPos, 0, blizzards);
            var a = globalBest;
            Console.WriteLine($"There: {globalBest} minutes");

            for (int i = 0; i < globalBest; i++)
            {
                blizzards = SimulateBlizzards(blizzards).ToList();
            }

            PrintMap(blizzards, endingPos);

            ClearPreviousRunData();
            Solve(endingPos, startPos, 0, blizzards);
            var b = globalBest;
            Console.WriteLine($"Back: {globalBest} minutes");

            for (int i = 0; i < globalBest; i++)
            {
                blizzards = SimulateBlizzards(blizzards).ToList();
            }

            PrintMap(blizzards, startPos);

            ClearPreviousRunData();
            Solve(startPos, endingPos, 0, blizzards);
            var c = globalBest;
            Console.WriteLine($"There again: {globalBest} minutes");

            Console.WriteLine($"Total: {a + b + c}");

            return a + b + c;
        }

        public async Task<object> Part2()
        {
            return await Part1();
        }
    }
}
