using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2022
{
    public class Line
    {
        public int startX, startY;
        public int endX, endY;
    }

    public class Sand
    {
        public int x = 500;
        public int y = 0;
        public char character = 'o';
        public bool rests = false;
    }

    public class Day14 : IAocDay
    {
        public async Task<object> Part1()
        {
            char[,] screen = new char[600, 200];
            Sand current = null;
            var sands = new List<Sand>();

            for (int x = 0; x < screen.GetLength(0); x++)
            for (int y = 0; y < screen.GetLength(1); y++)
            {
                screen[x, y] = '.';
            }

            screen[500, 0] = '+';

            var lines = new List<Line>();
            var maxY = 0;
            var input = await IO.GetInput(2022, 14);

            foreach (var inputLine in input)
            {
                var parts = inputLine.Split("->").Select(x => x.Trim()).ToList();
                for (int i = 0; i < parts.Count - 1; i++)
                {
                    var line = new Line()
                    {
                        startX = int.Parse(parts[i].Split(',')[0]),
                        startY = int.Parse(parts[i].Split(',')[1]),
                        endX = int.Parse(parts[i + 1].Split(',')[0]),
                        endY = int.Parse(parts[i + 1].Split(',')[1])
                    };

                    if (line.startY > maxY)
                    {
                        maxY = line.startY;
                    }

                    if (line.endY > maxY)
                    {
                        maxY = line.endY;
                    }

                    lines.Add(line);
                }
            }
            
            foreach (var line in lines)
            {
                DrawLine(line, screen);
            }

            for (int x = 0; x < screen.GetLength(0); x++)
            {
                screen[x, maxY + 2] = '#';
            }

            void Draw()
            {
                Console.Clear();
                Console.WriteLine(Render(screen, maxY + 2));
            }

            Draw();

            const int delay = 10;
            bool stop = false;

            char GetScreenSpace(int x, int y)
            {
                if (x >= screen.GetLength(0) || x < 0 || y >= screen.GetLength(1))
                {
                    stop = true;
                    return ' ';
                }

                return screen[x, y];
            }

            void Update()
            {
                if (current == null)
                {
                    current = new Sand();
                    if (screen[current.x, current.y] == 'o')
                    {
                        stop = true;
                        return;
                    }

                    screen[current.x, current.y] = 'o';
                }
                else
                {
                    if (GetScreenSpace(current.x, current.y + 1) == '.')
                    {
                        screen[current.x, current.y] = '.';
                        current.y += 1;
                        screen[current.x, current.y] = 'o';
                    }
                    else if (GetScreenSpace(current.x - 1, current.y + 1) == '.')
                    {
                        screen[current.x, current.y] = '.';
                        current.x -= 1;
                        current.y += 1;
                        screen[current.x, current.y] = 'o';
                    }
                    else if (GetScreenSpace(current.x + 1, current.y + 1) == '.')
                    {
                        screen[current.x, current.y] = '.';
                        current.x += 1;
                        current.y += 1;
                        screen[current.x, current.y] = 'o';
                    }
                    else if (!stop)
                    {
                        current.rests = true;
                        sands.Add(current);
                        current = null;
                    }
                }

                //Thread.Sleep(delay);
                //Draw();
            }

            while (!stop)
            {
                Update();
            }

            Draw();
            Console.WriteLine(sands.Count);
            return sands.Count;
        }

        public async Task<object> Part2()
        {
            return await Part1(); // again Part2 seems to have replaced Part 1 here.
        }

        public static void DrawLine(Line line, char[,] screen)
        {
            if (line.startX == line.endX)
            {
                for (int i = line.startY; i <= line.endY; i++)
                {
                    screen[line.startX, i] = '#';
                }
                for (int i = line.startY; i >= line.endY; i--)
                {
                    screen[line.startX, i] = '#';
                }
            }
            else if (line.startY == line.endY)
            {
                for (int i = line.startX; i <= line.endX; i++)
                {
                    screen[i, line.startY] = '#';
                }
                for (int i = line.startX; i >= line.endX; i--)
                {
                    screen[i, line.startY] = '#';
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public static string Render(char[,] spaces, int yMax)
        {
            var screen = new StringBuilder();

            for (int y = 0; y <= yMax; y++)
            {
                var line = new StringBuilder();
                for (int x = 400; x < spaces.GetLength(0); x++)
                {
                    line.Append(spaces[x, y]);
                }

                screen.Append(line.ToString()).AppendLine();
            }

            return screen.ToString();
        }
    }
}
