using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode;
using ConsoleApp1.Utils;

namespace AoC2022
{
    public class Day17 : IAocDay
    {
        public class Piece
        {
            public int x;
            public ulong y;
            public Func<(int x, ulong y), IEnumerable<(int x, ulong y)>> GetPiecePositions;
        }

        public async Task<object> Part1()
        {
            char[] windMovements = null;
            List<Piece> pieces = new List<Piece>()
            {
                new Piece()
                {
                    GetPiecePositions = og => new[]
                        { (og.x, og.y), (og.x + 1, og.y), (og.x + 2, og.y), (og.x + 3, og.y) }
                }, // ####

                new Piece()
                {
                    GetPiecePositions = og => new[]
                        { (og.x+1, og.y), (og.x, og.y+1), (og.x+1, og.y+1), (og.x + 2, og.y+1), (og.x+1, og.y+2) }
                }, // 2nd

                new Piece()
                {
                    GetPiecePositions = og => new[]
                        { (og.x, og.y), (og.x+1, og.y), (og.x+2, og.y), (og.x+2, og.y+1), (og.x+2, og.y+2) }
                }, // 3rd

                new Piece()
                {
                    GetPiecePositions = og => new[]
                        { (og.x, og.y), (og.x, og.y + 1), (og.x, og.y + 2), (og.x, og.y + 3) }
                }, // 4th

                new Piece()
                {
                    GetPiecePositions = og => new[]
                        { (og.x, og.y), (og.x+1, og.y), (og.x, og.y+1), (og.x+1, og.y+1) }
                }, // 5th
            };


            var input = await Input.GetInput(2022, 17);
            windMovements = input.Single().ToCharArray();

            var minX = 0;
            var maxX = 8;
            ulong maxY = 0;
            ulong nextPieceNumber = 0;
            int windTick = 0;
            Piece currentPiece = null;
            var solids = new HashSet<(int x, ulong y)>();
            var highests = new List<ulong>() { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = minX + 1; i < maxX; i++)
            {
                solids.Add((i, 0));
            }

            string RenderTower()
            {
                var sb = new StringBuilder();
                for (ulong y = maxY; y >= 0; y--)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        var c = '?';

                        if (y == 0)
                        {
                            c = '-';
                        }
                        else if (x == 0 || x == 8)
                        {
                            c = '|';
                        }

                        else
                        {
                            c = solids.Contains((x, y)) ? '#' : '.';
                        }

                        sb.Append(c);
                    }

                    sb.AppendLine();
                }

                return sb.ToString();
            }

            Dictionary<string, (ulong, ulong, int)> states = new Dictionary<string, (ulong, ulong, int)>();

            bool CollisionPresent()
            {
                foreach (var position in currentPiece.GetPiecePositions((currentPiece.x, currentPiece.y)))
                {
                    if (position.x == maxX)
                    {
                        return true;
                    }

                    if (position.x == minX)
                    {
                        return true;
                    }

                    if (solids.Contains(position))
                    {
                        return true;
                    }
                }

                return false;
            }

            const ulong limit = 1000000000000;
            //const ulong limit = 2022;

            void PieceRests()
            {
                foreach (var pos in currentPiece.GetPiecePositions((currentPiece.x, currentPiece.y)))
                {
                    solids.Add(pos);
                    if (pos.y > highests[pos.x])
                    {
                        highests[pos.x] = pos.y;
                    }

                    if (pos.y > maxY)
                    {
                        maxY = pos.y;
                    }

                    currentPiece = null;
                }

                ulong MaxHeightToLookAtForCycles = 20;
                var lastQrows = solids.Where(p => maxY - p.y < MaxHeightToLookAtForCycles).ToList();
                var key = string.Join(string.Empty, lastQrows.Select(p => $"{p.x}{maxY - p.y}")) + (windTick % windMovements.Length).ToString() + (nextPieceNumber % 5).ToString();

                if (states.ContainsKey(key))
                {
                    var pieceNo = nextPieceNumber - 1;
                    var windState = windTick % windMovements.Length;
                    var cycleLength = pieceNo - states[key].Item1;
                    var cycleHeight = maxY - states[key].Item2;

                    var remainingCycles = (limit - pieceNo) / cycleLength;
                    var remainderToSimulate = (limit - pieceNo) % cycleLength;

                    Console.WriteLine(
                        $"Cycle detected between heights: {states[key].Item2} and {maxY}. Between pieces {states[key].Item1} and {pieceNo} - Diffs {cycleHeight}, {cycleLength}");

                    var newHeight = maxY + remainingCycles * cycleHeight;
                    maxY = newHeight;
                    nextPieceNumber = pieceNo + remainingCycles * cycleLength + 1;
                    windTick = windState;
                    solids.Clear();
                    foreach (var p in lastQrows)
                    {
                        solids.Add((p.x, p.y + remainingCycles * cycleHeight));
                    }

                    states.Clear();
                }

                states[key] = (nextPieceNumber - 1, maxY, windTick);
            }

            while (true)
            {
                if (nextPieceNumber % 1000000 == 0)
                {
                    Console.WriteLine($"{nextPieceNumber} / {limit} - {((float)nextPieceNumber / limit) * 100:f4}%");
                }

                if (currentPiece == null)
                {

                    if (nextPieceNumber == limit)
                    {
                        //Console.WriteLine(RenderTower());
                        Console.WriteLine(maxY);
                        return maxY;
                    }

                    currentPiece = pieces[(int)(nextPieceNumber % 5)];
                    currentPiece.x = 3;
                    currentPiece.y = maxY + 4;
                    nextPieceNumber++;
                }

                if (currentPiece.y == 0)
                {
                    currentPiece.y++;
                    PieceRests();
                    continue;
                }

                var move = windMovements[windTick % windMovements.Length];
                if (move == '>')
                {
                    currentPiece.x++;
                    if (CollisionPresent())
                    {
                        currentPiece.x--;
                    }

                }
                else if (move == '<')
                {
                    currentPiece.x--;
                    if (CollisionPresent())
                    {
                        currentPiece.x++;
                    }
                }

                currentPiece.y--;
                if (CollisionPresent())
                {
                    currentPiece.y++;
                    PieceRests();
                }

                windTick++;
            }

        }

        public async Task<object> Part2()
        {
            return await Part1();
        }
    }
}
