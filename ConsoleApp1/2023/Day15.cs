using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day15 : IAocDay
    {
        public async Task<object> Part1()
        {
            var ans = 0;
            var input = await IO.GetInput(2023, 15);
            //var input = await IO.GetExampleInput();

            var parts = input.Single().Split(',');
            foreach (var part in parts)
            {
                ans += ComputeHash(part);
            }

            return ans;
        }

        public async Task<object> Part2()
        {
            var ans = 0;
            var input = await IO.GetInput(2023, 15);

            var boxes = Enumerable.Range(0, 256).Select(x => new Box()).ToList();
            var parts = input.Single().Split(',');
            foreach (var part in parts)
            {
                var label = part.Split('=', '-')[0];
                var hash = ComputeHash(label);
                var box = boxes[hash];

                var separator = part.IndexOf('=');
                if (separator < 0)
                {
                    separator = part.IndexOf('-');
                }

                var op = part.Substring(separator, 1).Single();
                
                if (op == '-')
                {
                    box.lenses = box.lenses.Where(x => x.label != label).ToList();
                }
                else if (op == '=')
                {
                    var number = int.Parse(part.Split('=')[1]);

                    bool found = false;
                    for (int i = 0; i < box.lenses.Count; i++)
                    {
                        if (box.lenses[i].label == label)
                        {
                            found = true;
                            box.lenses[i] = (label, number);
                        }
                    }
                    if (!found)
                    {
                        box.lenses.Add((label, number));
                    }
                }
            }

            foreach (var t in boxes.Select((box, i) => (box, i)))
            {
                foreach (var x in t.box.lenses.Select((lens, i) => (lense: lens, i)))
                {
                    var power = (t.i + 1) * (x.i + 1) * x.lense.number;
                    ans += power;
                }
            }

            return ans;
        }

        public class Box
        {
            public List<(string label, int number)> lenses = new();
        }

        private static int ComputeHash(string part)
        {
            var value = 0;
            foreach (var c in part)
            {
                value += c;
                value *= 17;
                value %= 256;
            }

            return value;
        }
    }
}
