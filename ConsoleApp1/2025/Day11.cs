using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2025
{
    public class Day11 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2025, 11);
            var allNodes = new Dictionary<string, Node>();

            foreach (var line in input)
            {
                var split = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var name = split[0];
                if (!allNodes.TryGetValue(name, out var node))
                {
                    node = new() { Name = split[0], Connections = new() };
                    allNodes[name] = node;
                }

                foreach (var c in split[1].Split(' ',
                             StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    if (!allNodes.TryGetValue(c, out var nextNode))
                    {
                        nextNode = new() { Name = c, Connections = new() };
                        allNodes[c] = nextNode;
                    }

                    node.Connections.Add(nextNode);
                }
            }

            var ans = Solve(allNodes["you"], "out");
            return ans;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2025, 11);
            var allNodes = new Dictionary<string, Node>();

            foreach (var line in input)
            {
                var split = line.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var name = split[0];
                if (!allNodes.TryGetValue(name, out var node))
                {
                    node = new() { Name = split[0], Connections = new() };
                    allNodes[name] = node;
                }

                foreach (var c in split[1].Split(' ',
                             StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                {
                    if (!allNodes.TryGetValue(c, out var nextNode))
                    {
                        nextNode = new() { Name = c, Connections = new() };
                        allNodes[c] = nextNode;
                    }

                    node.Connections.Add(nextNode);
                }
            }

            var toFft = Solve(allNodes["svr"], "fft", new() { "dac" });
            var fftDac = Solve(allNodes["fft"], "dac", new() { "srv", "out" });
            var dacOut = Solve(allNodes["dac"], "out", new() { "fft, srv" });

            var toDac = Solve(allNodes["svr"], "dac", new() { "fft" });
            var dacFft = Solve(allNodes["dac"], "fft", new() { "srv", "out" });
            var fftOut = Solve(allNodes["fft"], "out", new () { "dac, srv" });

            return toFft * fftDac * dacOut + toDac * dacFft * fftOut;
        }

        public ulong Solve(Node start, string target,
            List<string> bannedNodes = null)
        {
            var memo = new Dictionary<string, ulong>();
            return SolveInternal(0, start, target, memo, bannedNodes);
        }

        public ulong SolveInternal(int currentDepth, Node currentNode, string target, Dictionary<string, ulong> answers, List<string> bannedNodes = null)
        {
            if (currentNode.Name == target)
            {
                return 1;
            }

            if (answers.TryGetValue(currentNode.Name, out var paths))
            {
                return paths;
            }

            var ans = 0UL;
            foreach (var next in currentNode.Connections)
            {
                if (bannedNodes != null && bannedNodes.Contains(next.Name))
                    continue;

                ans += SolveInternal(currentDepth + 1, next, target, answers, bannedNodes);
            }

            answers[currentNode.Name] = ans;
            return ans;
        }

        public class Node
        {
            public string Name { get; set; }
            public List<Node> Connections { get; set; }
        }
    }
}
