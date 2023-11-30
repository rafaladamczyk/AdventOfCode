using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{

    class Day22
    {
        public static Dictionary<(int, int), Node> Nodes = new Dictionary<(int, int), Node>();

        public static Dictionary<int, (int startX, int startY)> StartingPositions =
            new Dictionary<int, (int startX, int startY)>();

        private const int CubeSideLength = 50;

        public static void Run()
        {
            
            //using var f = File.OpenRead(@"C:\Users\Raf\Downloads\example.txt");
            using var f = File.OpenRead(@"C:\Users\Raf\Downloads\input-22.txt");
            using var reader = new StreamReader(f);


            ReadInput(reader);

            char temp = '?';
            var turn = temp;
            var start = Nodes.Values.Where(x => x.faceNumber == 1).OrderBy(node => node.x).ThenBy(node => node.y)
                .First();
            (int x, int y, char direction) currentPosition = (start.x, start.y, 'R');
    
            bool IsNotDir(char x)
            {
                temp = x;
                return x != 'U' && x != 'D' && x!= 'L' && x != 'R';
            }

            while (!reader.EndOfStream)
            {
                string steps = string.Empty;
                while (!reader.EndOfStream)
                {
                    if (IsNotDir((char)reader.Read()))
                    {
                        steps += temp;
                    }
                    else
                    {
                        var stepsCount = int.Parse(steps);
                        currentPosition = Node.Go(currentPosition, stepsCount);
                        
                        turn = temp;

                        currentPosition.direction = ApplyTurn(currentPosition.direction, turn);
                        steps = string.Empty;
                    }
                }

                currentPosition = Node.Go(currentPosition, int.Parse(steps));
            }


            Console.WriteLine($"I arrived at {currentPosition.x + 1},{currentPosition.y + 1}");
            var score = (1000 * (currentPosition.y + 1)) + 4 * (currentPosition.x + 1) + GetDirectionScore(currentPosition.direction);
            Console.WriteLine($"Score: {score}");
        }

        public class Node
        {
            public int x = -1;
            public int y = -1;

            public bool passable;

            public int faceNumber;
            public int xStart = 0;
            public int yStart = 0;

            public static (int, int, char) Step((int x, int y, char direction) starting)
            {
                var current = starting;
                var node = Nodes[(current.x, current.y)];

                bool IsFaceTransition(int ogFace, int x, int y)
                {
                    if (Nodes.TryGetValue((x, y), out var newNode))
                    {
                        return newNode.faceNumber != ogFace;
                    }

                    return true;
                }

                switch (starting.direction)
                {
                    case 'U':
                    {
                        current = IsFaceTransition(node.faceNumber, starting.x, starting.y - 1)
                            ? FaceTransition(current)
                            : (starting.x, starting.y - 1, starting.direction);
                        break;
                    }

                    case 'D':
                        current = IsFaceTransition(node.faceNumber, starting.x, starting.y + 1)
                            ? FaceTransition(current)
                            : (starting.x, starting.y + 1, starting.direction);
                        break;

                    case 'L':
                        current = IsFaceTransition(node.faceNumber, starting.x - 1, starting.y)
                            ? FaceTransition(current)
                            : (starting.x - 1, starting.y, starting.direction);
                        break;

                    case 'R':
                        current = IsFaceTransition(node.faceNumber, starting.x + 1, starting.y)
                            ? FaceTransition(current)
                            : (starting.x + 1, starting.y, starting.direction);
                        break;

                    default:
                        throw new Exception();
                }

                var result = Nodes[(current.x, current.y)];
                if (result.passable)
                {
                    return current;
                }
                else
                {
                    return starting;
                }
            }

            public static (int, int, char) FaceTransition((int x, int y, char direction) c)
            {
                var node = Nodes[(c.x, c.y)];

                var relativeX = node.x - node.xStart;
                var relativeY = node.y - node.yStart;

                void Check(int expectedFace, int x, int y)
                {
                    if (Nodes[(x, y)].faceNumber != expectedFace)
                    {
                        throw new Exception();
                    }
                }

                if (node.faceNumber == 1)
                {
                    switch (c.direction)
                    {
                        case 'U':
                        {
                            var newStartingPos = StartingPositions[6];
                            var newX = 0 + newStartingPos.startX;
                            var newY = relativeX + newStartingPos.startY;
                            Check(6, newX, newY);
                            return (newX, newY, 'R');
                        }

                        case 'D':
                        {
                            var newStartingPos = StartingPositions[3];
                            var newX = relativeX + newStartingPos.startX;
                            var newY = 0 + newStartingPos.startY;
                            Check(3, newX, newY);
                                return (newX, newY, 'D');
                        }

                        case 'L':
                        {
                            var newStartingPos = StartingPositions[4];
                            var newX = 0 + newStartingPos.startX;
                            var newY = CubeSideLength - 1 - relativeY + newStartingPos.startY;
                            Check(4, newX, newY);
                            return (newX, newY, 'R');
                        }

                        case 'R':
                        {
                            var newStartingPos = StartingPositions[2];
                            var newX = 0 + newStartingPos.startX;
                            var newY = relativeY + newStartingPos.startY;
                            Check(2, newX, newY);
                            return (newX, newY, 'R');
                        }

                        default:
                            throw new Exception();
                    }
                }

                if (node.faceNumber == 2)
                {
                    switch (c.direction)
                    {
                        case 'U':
                        {
                            var newStartingPos = StartingPositions[6];
                            var newX = relativeX + newStartingPos.startX;
                            var newY = CubeSideLength - 1 + newStartingPos.startY;
                            Check(6, newX, newY);
                            return (newX, newY, 'U');
                        }

                        case 'D':
                        {
                            var newStartingPos = StartingPositions[3];
                            var newX = CubeSideLength - 1 + newStartingPos.startX;
                            var newY = relativeX + newStartingPos.startY;
                            Check(3, newX, newY);
                            return (newX, newY, 'L');
                        }

                        case 'L':
                        {
                            var newStartingPos = StartingPositions[1];
                            var newX = CubeSideLength - 1 + newStartingPos.startX;
                            var newY = relativeY + newStartingPos.startY;
                            Check(1, newX, newY);
                            return (newX, newY, 'L');
                        }

                        case 'R':
                        {
                            var newStartingPos = StartingPositions[5];
                            var newX = CubeSideLength - 1 + newStartingPos.startX;
                            var newY = CubeSideLength - 1 - relativeY + newStartingPos.startY;
                            Check(5, newX, newY);
                            return (newX, newY, 'L');
                        }

                        default:
                            throw new Exception();
                    }
                }

                if (node.faceNumber == 3)
                {
                    switch (c.direction)
                    {
                        case 'U':
                        {
                            var newStartingPos = StartingPositions[1];
                            var newX = relativeX + newStartingPos.startX;
                            var newY = CubeSideLength - 1 + newStartingPos.startY;
                            Check(1, newX, newY);
                            return (newX, newY, 'U');
                        }

                        case 'D':
                        {
                            var newStartingPos = StartingPositions[5];
                            var newX = relativeX + newStartingPos.startX;
                            var newY = 0 + newStartingPos.startY;
                            Check(5, newX, newY);
                            return (newX, newY, 'D');
                        }

                        case 'L':
                        {
                            var newStartingPos = StartingPositions[4];
                            var newX = relativeY + newStartingPos.startX;
                            var newY = 0 + newStartingPos.startY;
                            Check(4, newX, newY);
                            return (newX, newY, 'D');
                        }

                        case 'R':
                        {
                            var newStartingPos = StartingPositions[2];
                            var newX = relativeY + newStartingPos.startX;
                            var newY = CubeSideLength - 1 + newStartingPos.startY;
                            Check(2, newX, newY);
                            return (newX, newY, 'U');
                        }

                        default:
                            throw new Exception();
                    }
                }

                if (node.faceNumber == 4)
                {
                    switch (c.direction)
                    {
                        case 'U':
                        {
                            var newStartingPos = StartingPositions[3];
                            var newX = 0 + newStartingPos.startX;
                            var newY = relativeX + newStartingPos.startY;
                            Check(3, newX, newY);
                            return (newX, newY, 'R');
                        }

                        case 'D':
                        {
                            var newStartingPos = StartingPositions[6];
                            var newX = relativeX + newStartingPos.startX;
                            var newY = 0 + newStartingPos.startY;
                            Check(6, newX, newY);
                            return (newX, newY, 'D');
                        }

                        case 'L':
                        {
                            var newStartingPos = StartingPositions[1];
                            var newX = 0 + newStartingPos.startX;
                            var newY = CubeSideLength - 1 - relativeY + newStartingPos.startY;
                            Check(1, newX, newY);
                            return (newX, newY, 'R');
                        }

                        case 'R':
                        {
                            var newStartingPos = StartingPositions[5];
                            var newX = 0 + newStartingPos.startX;
                            var newY = relativeY + newStartingPos.startY;
                            Check(5, newX, newY);
                            return (newX, newY, 'R');
                        }

                        default:
                            throw new Exception();
                    }
                }

                if (node.faceNumber == 5)
                {
                    switch (c.direction)
                    {
                        case 'U':
                        {
                            var newStartingPos = StartingPositions[3];
                            var newX = relativeX + newStartingPos.startX;
                            var newY = CubeSideLength - 1 + newStartingPos.startY;
                            Check(3, newX, newY);
                            return (newX, newY, 'U');
                        }

                        case 'D':
                        {
                            var newStartingPos = StartingPositions[6];
                            var newX = CubeSideLength - 1 + newStartingPos.startX;
                            var newY = relativeX + newStartingPos.startY;
                            Check(6, newX, newY);
                            return (newX, newY, 'L');
                        }

                        case 'L':
                        {
                            var newStartingPos = StartingPositions[4];
                            var newX = CubeSideLength - 1 + newStartingPos.startX;
                            var newY = relativeY + newStartingPos.startY;
                            Check(4, newX, newY);
                            return (newX, newY, 'L');
                        }

                        case 'R':
                        {
                            var newStartingPos = StartingPositions[2];
                            var newX = CubeSideLength - 1 + newStartingPos.startX;
                            var newY = CubeSideLength - 1 - relativeY + newStartingPos.startY;
                            Check(2, newX, newY);
                            return (newX, newY, 'L');
                        }

                        default:
                            throw new Exception();
                    }
                }

                if (node.faceNumber == 6)
                {
                    switch (c.direction)
                    {
                        case 'U':
                        {
                            var newStartingPos = StartingPositions[4];
                            var newX = relativeX + newStartingPos.startX;
                            var newY = CubeSideLength - 1 + newStartingPos.startY;
                            Check(4, newX, newY);
                            return (newX, newY, 'U');
                        }

                        case 'D':
                        {
                            var newStartingPos = StartingPositions[2];
                            var newX = relativeX + newStartingPos.startX;
                            var newY = 0 + newStartingPos.startY;
                            Check(2, newX, newY);
                            return (newX, newY, 'D');
                        }

                        case 'L':
                        {
                            var newStartingPos = StartingPositions[1];
                            var newX = relativeY + newStartingPos.startX;
                            var newY = 0 + newStartingPos.startY;
                            Check(1, newX, newY);
                            return (newX, newY, 'D');
                        }

                        case 'R':
                        {
                            var newStartingPos = StartingPositions[5];
                            var newX = relativeY + newStartingPos.startX;
                            var newY = CubeSideLength - 1 + newStartingPos.startY;
                            Check(5, newX, newY);
                            return (newX, newY, 'U');
                        }

                        default:
                            throw new Exception();
                    }
                }

                throw new Exception();
            }

            public static (int x, int y, char direction) Go((int x, int y, char direction) startingPosition, int steps)
            {
                (int, int, char) current = startingPosition;
                for (int i = 0; i < steps; i++)
                {
                    current = Step(current);
                }

                return current;
            }
        }

        private static int GetDirectionScore(char direction)
        {
            switch (direction)
            {
                case 'U':
                    return 3;
                case 'D':
                    return 1;
                case 'L':
                    return 2;
                case 'R':
                    return 0;
                default:
                    throw new Exception();
            }
        }

        private static char ApplyTurn(char direction, char turn)
        {
            if (turn != 'L' && turn != 'R')
            {
                throw new Exception();
            }

            switch (direction)
            {
                case 'U':
                    return turn == 'L' ? 'L' : 'R';
                case 'D':
                    return turn == 'L' ? 'R' : 'L';
                case 'L':
                    return turn == 'L' ? 'D' : 'U';
                case 'R':
                    return turn == 'L' ? 'U' : 'D';
                default:
                    throw new Exception();
            }
        }

        private static void ReadInput(StreamReader reader)
        {
            var map = new List<string>();

                while (true)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        break;
                    }

                    else
                    {
                        map.Add(line);
                    }
                }

            var maxRowLength = map.Max(l => l.Length);


            for (int i = 0; i < map.Count; i++)
            {
                var rowChars = map[i].ToCharArray();
                for (int j = 0; j < maxRowLength; j++)
                {
                    var faceNumber = -1;
                    var node = new Node
                    {
                        y = i, x = j,
                        xStart = CubeSideLength * (j / CubeSideLength),
                        yStart = CubeSideLength * (i / CubeSideLength)
                    };

                    if (j / CubeSideLength == 0)
                    {
                        if (i / CubeSideLength == 2)
                        {
                            faceNumber = 4;
                        }
                        else if (i / CubeSideLength == 3)
                        {
                            faceNumber = 6;
                        }
                    }

                    if (j / CubeSideLength == 1)
                    {
                        if (i / CubeSideLength == 0)
                        {
                            faceNumber = 1;
                        }
                        else if (i / CubeSideLength == 1)
                        {
                            faceNumber = 3;
                        }
                        else if (i / CubeSideLength == 2)
                        {
                            faceNumber = 5;
                        }
                    }

                    if (j / CubeSideLength == 2)
                    {
                        if (i / CubeSideLength == 0)
                        {
                            faceNumber = 2;
                        }
                    }
                    
                    if (j < rowChars.Length)
                    {
                        node.passable = rowChars[j] == '.';
                    }

                    node.faceNumber = faceNumber;
                    if (node.faceNumber != -1)
                    {
                        StartingPositions[faceNumber] = (node.xStart, node.yStart);
                        Nodes[(j, i)] = node;
                    }
                }
            }
        }
    }
}
