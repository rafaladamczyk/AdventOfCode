using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day10 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 10);
            var map = ConstructMap(input);

            var acc = 0;
            foreach (var kvp in map)
            {
                if (kvp.Value == 0)
                {
                    var reachablePeaks = ReachablePeaks(map, kvp.Key).ToHashSet();
                    acc += reachablePeaks.Count;
                }
            }

            return acc;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 10);
            var map = ConstructMap(input);

            var acc = 0;
            foreach (var kvp in map)
            {
                if (kvp.Value == 0)
                {
                    var reachablePeaks = ReachablePeaks(map, kvp.Key).ToList();
                    var rating = reachablePeaks.GroupBy(x => x).Select(g => g.Count()).Sum();
                    acc += rating;
                }
            }

            return acc;
        }

        private static Dictionary<Point, int> ConstructMap(List<string> input)
        {
            var rows = input.Count;
            var cols = input[0].Length;
            var map = new Dictionary<Point, int>();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var symbol = input[r][c];
                    if (symbol != '.')
                    {
                        var height = int.Parse($"{symbol}");
                        map.Add(new Point(r, c), height);
                    }
                }
            }

            return map;
        }

        private static IEnumerable<Point> ReachablePeaks(Dictionary<Point, int> map, Point startingPoint)
        {
            var height = map[startingPoint];
            if (height == 9)
            {
                yield return startingPoint;
            }

            var candidatePoints = startingPoint.GetNeighbors().Where(map.ContainsKey);
            var nextPoints = candidatePoints.Where(x => map[x] == height + 1);

            foreach (var nextPoint in nextPoints)
            {
                foreach (var reachablePeak in ReachablePeaks(map, nextPoint))
                {
                    yield return reachablePeak;
                }
            }
        }
    }
}
