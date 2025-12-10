using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;
using static AdventOfCode.Utils.Misc;
using Microsoft.Z3;

namespace AoC2025
{
    public class Day10 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 10);
            //var input = await IO.GetExampleInput();
            var acc = 0;
            foreach (var line in input)
            {
                int desiredState = 0;
                var buttons = new List<List<int>>();
                foreach (var sub in line.Split
                         (' ',
                             StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                {
                    if (sub.StartsWith('['))
                    {
                        foreach (var (c, i) in sub.Skip(1).TakeWhile(x => x != ']').Select((c, i) => (c, i)))
                        {
                            if (c == '#')
                            {
                                desiredState |= (1 << i); //set i-th bit;
                            }
                        }
                    }

                    if (sub.StartsWith('('))
                    {
                        var affectedLights = new List<int>();
                        foreach (var item in new string(sub.Skip(1).TakeWhile(x => x != ')').ToArray()).Split(','))
                        {
                            affectedLights.Add(int.Parse(item));
                        }

                        buttons.Add(affectedLights);
                    }
                }

                var subsets = GetAllSubsets(buttons.Count);
                var min = int.MaxValue;
                foreach (var subset in subsets)
                {
                    var state = 0;
                    foreach (var index in subset)
                    {
                        state = PressButton(state, buttons[index]);
                    }

                    if (state == desiredState)
                    {
                        min = Math.Min(min, subset.Length);
                    }
                }

                acc += min;
            }

            return acc;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 10);
            //var input = await IO.GetExampleInput();
            var acc = 0;
            foreach (var line in input)
            {
                var joltages = new List<int>();
                var buttons = new List<List<int>>();
                foreach (var sub in line.Split
                         (' ',
                             StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                {
                    if (sub.StartsWith('{'))
                    {
                        foreach (var item in new string(sub.Skip(1).TakeWhile(x => x != '}').ToArray()).Split(','))
                        {
                            joltages.Add(int.Parse(item));
                        }
                    }

                    if (sub.StartsWith('('))
                    {
                        var affectedLights = new List<int>();
                        foreach (var item in new string(sub.Skip(1).TakeWhile(x => x != ')').ToArray()).Split(','))
                        {
                            affectedLights.Add(int.Parse(item));
                        }

                        buttons.Add(affectedLights);
                    }
                }

                //var best = new Dictionary<ListOfIntsKey, int?>();
                //var start = Enumerable.Range(0, joltages.Count).Select(x => 0).ToList();
                //var startKey = new ListOfIntsKey(start);
                //Solve(start, 0, joltages, buttons, best);
                //acc += best[startKey].Value;

                acc += SolveZ3(joltages, buttons);
            }

            return acc;
        }

        private int SolveZ3(List<int> joltages, List<List<int>> buttonList)
        {
            using (var ctx = new Context())
            using (var optimization = ctx.MkOptimize())
            {
                var buttons = Enumerable.Range(0, buttonList.Count).Select(x => ctx.MkIntConst($"b{x}")).ToList();
                foreach (var button in buttons)
                {
                    optimization.Add(ctx.MkGe(button, ctx.MkInt(0)));
                }

                for (int i = 0; i < joltages.Count; i++)
                {
                    var buttonsForThisJoltage = buttons.Where((expression, x) => buttonList[x].Contains(i)).ToList();
                    if (buttonsForThisJoltage.Any())
                    {
                        var result = ctx.MkAdd(buttonsForThisJoltage);
                        optimization.Add(ctx.MkEq(ctx.MkInt(joltages[i]), result));
                    }
                }

                optimization.MkMinimize(ctx.MkAdd(buttons));
                optimization.Check();

                var acc = 0;
                foreach (var b in buttons)
                {
                    var howManyTimesPressed= (IntNum)optimization.Model.Eval(b, true);
                    acc += howManyTimesPressed.Int;
                }

                return acc;
            }
        }

        public static IEnumerable<int[]> GetAllSubsets(int length)
        {
            int subsetCount = 1 << length;

            for (int mask = 0; mask < subsetCount; mask++)
            {
                var indexes = new List<int>();
                for (int i = 0; i < length; i++)
                {
                    if ((mask & (1 << i)) != 0)
                    {
                        indexes.Add(i);
                    }
                }

                yield return indexes.ToArray();
            }
        }

        private static int PressButton(int state, List<int> button)
        {
            foreach (var b in button)
            {
                state ^= (1 << b); // flip b-th bit
            }

            return state;
        }

        private List<int> ChangeJoltage(List<int> currentState, List<int> button)
        {
            var newState = currentState.ToList();
            foreach (var b in button)
            {
                newState[b] += 1;
            }

            return newState;
        }

        private void Solve(List<int> currentState, int currentDepth, List<int> desiredState, List<List<int>> buttons, Dictionary<ListOfIntsKey, int?> bestFound)
        {
            var validity = IsValid(currentState, desiredState);
            if (validity < 0)
            {
                return;
            }

            if (validity == 0)
            {
                bestFound[new ListOfIntsKey(currentState)] = 0;
                return;
            }

            var currentStateKey = new ListOfIntsKey(currentState);
            if (bestFound.TryGetValue(currentStateKey, out var best))
            {
                if (best <= currentDepth)
                {
                    return;
                }
            }

            for (var i = 0; i < buttons.Count; i++)
            {
                var newState = ChangeJoltage(currentState, buttons[i]);
                Solve(newState, currentDepth + 1, desiredState, buttons, bestFound);

                var key = new ListOfIntsKey(newState);
                bestFound.TryGetValue(key, out best);
                bestFound.TryGetValue(currentStateKey, out var currentBest);
                if (currentBest == null || (best < currentBest + 1))
                {
                    bestFound[currentStateKey] = best + 1;
                }
            }

            int IsValid(List<int> state, List<int> joltages)
            {
                bool allEqual = true;
                for (int j = 0; j < state.Count; j++)
                {
                    if (state[j] > joltages[j])
                    {
                        return -1;
                    }

                    if (state[j] != joltages[j])
                    {
                        allEqual = false;
                    }
                }

                return allEqual ? 0 : 1;
            }
        }
    }
}
