using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day3 : IAocDay
{
    public class Number
    {
        public List<int> digits = new();
        public List<Point> points = new();
        public List<Point> gears = new();
        public bool considered;
    }

    public async Task<object> Part1()
    {
        var input = await IO.GetInput(2023, 3);
        var answer = 0;
        var numbers = ParseNumbers(input);

        foreach (var number in numbers)
        {
            foreach (var point in number.points)
            {
                if (!number.considered)
                {
                    foreach (var dir in Point.GetDirs(true))
                    {
                        var n = point + dir;
                        if (n.x > 0 && n.y > 0 && n.x < input.Count && n.y < input[0].Length)
                        {
                            var nc = input[n.x][n.y];
                            if (!char.IsDigit(nc) && nc != '.')
                            {
                                answer += int.Parse($"{string.Join("", number.digits)}");
                                number.considered = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        return answer;
    }

    public async Task<object> Part2()
    {
        var input = await IO.GetInput(2023, 3);
        var answer = 0;
        var numbers = ParseNumbers(input);

        foreach (var number in numbers)
        {
            foreach (var point in number.points)
            {
                if (!number.considered)
                {
                    foreach (var dir in Point.GetDirs(true))
                    {
                        var n = point + dir;
                        if (n.x > 0 && n.y > 0 && n.x < input.Count && n.y < input[0].Length)
                        {
                            var nc = input[n.x][n.y];
                            if (!char.IsDigit(nc) && nc == '*')
                            {
                                number.considered = true;
                                number.gears.Add(n);
                            }
                        }
                    }
                }
            }
        }

        var gears = numbers.SelectMany(x => x.gears).ToList();
        var blah = gears.ToLookup(x => gears.Count(y => y.Equals(x)));
        var validGears = blah[2];
        foreach (var g in validGears.Distinct())
        {
            var ns = numbers.Where(x => x.gears.Any(x => x.Equals(g))).ToList();
            answer += ns.Select(x => int.Parse(string.Join("", x.digits))).Aggregate(1, (i, j) => i * j);
        }

        return answer;
    }

    private static List<Number> ParseNumbers(List<string> input)
    {
        var result = new List<Number>();
        for (int x = 0; x < input.Count; x++)
        {
            for (int y = 0; y < input[x].Length; y++)
            {
                var number = new Number { points = new List<Point>(), digits = new List<int>() };
                while (y < input[0].Length && char.IsDigit(input[x][y]))
                {
                    var point = new Point(x, y);
                    number.digits.Add(int.Parse($"{input[x][y]}"));
                    number.points.Add(point);
                    y++;
                }

                if (number.points.Any())
                {
                    result.Add(number);
                }
            }
        }

        return result;
    }
}