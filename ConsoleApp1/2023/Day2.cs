using System;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day2 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2023, 2);
            var games = input.Select(GameIdOrNull).Where(x => x != null).ToList();
            var answer = games.Sum();
            return answer;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 2);
            var games = input.Select(Power).ToList();
            var answer = games.Sum();
            return answer;
        }

        public int? GameIdOrNull(string line)
        {
            var id = int.Parse(line.Split(":")[0].Replace("Game ", ""));
            var gameLine = line.Substring(line.IndexOf(":") + 1);
            var rounds = gameLine.Split(";").Select(x => x.Trim()).ToList();

            foreach (var round in rounds)
            {
                int red = 0;
                int green = 0;
                int blue = 0;

                var colors = round.Split(",").Select(x => x.Trim()).ToList();
                foreach (var color in colors)
                {
                    var xxx = color.Split(" ");
                    var value = int.Parse(xxx[0]);
                    var name = xxx[1];

                    switch (name)
                    {
                        case "blue":
                            blue = value;
                            break;
                        case "red":
                            red = value; ;
                            break;
                        case "green":
                            green = value; ;
                            break;
                        default:
                            throw new Exception();
                    }
                }

                if (red > 12 || green > 13 || blue > 14)
                {
                    return null;
                }
            }

            return id;
        }

        public int Power(string line)
        {
            var gameLine = line.Substring(line.IndexOf(":") + 1);
            var rounds = gameLine.Split(";").Select(x => x.Trim()).ToList();

            int red = 0;
            int green = 0;
            int blue = 0;

            foreach (var round in rounds)
            {
                var colors = round.Split(",").Select(x => x.Trim()).ToList();
                foreach (var color in colors)
                {
                    var xxx = color.Split(" ");
                    var value = int.Parse(xxx[0]);
                    var name = xxx[1];

                    switch (name)
                    {
                        case "blue":
                            blue = Math.Max(blue, value);
                            break;
                        case "red":
                            red = Math.Max(red, value); ;
                            break;
                        case "green":
                            green = Math.Max(green, value); ;
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }

            return red * green * blue;
        }
    }
}
