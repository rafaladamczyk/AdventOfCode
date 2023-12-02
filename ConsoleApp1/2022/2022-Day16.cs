using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2022
{
    public class Day16 : IAocDay
    {
        private static List<Node> nodes = new List<Node>();

        [DebuggerDisplay("Name = {name}, Flow = {flowRate}")]
        public class Node
        {
            public string name;
            public int flowRate;
            public List<int> destinations = new List<int>();
            public List<string> destinationNames;
        }

        public async Task<object> Part1()
        {
            return await Run(1);
        }

        public async Task<object> Part2()
        {
            return await Run(2);
        }

        public async Task<int> Run(int part)
        {
            var input = await IO.GetInput(2022, 16);
            foreach (var inputLine in input)
            {
                var parts = inputLine.Split(' ');
                var valveName = parts[1].Trim();
                var flowRate = int.Parse(parts[4].Split('=')[1].Trim(';'));
                var destinations = parts.Skip(9).Select(p => p.Trim(',').Trim()).ToList();

                nodes.Add(new Node { name = valveName, flowRate = flowRate, destinationNames = destinations });

            }
            
            nodes = nodes.OrderByDescending(x => x.name == "AA").ThenByDescending(x => x.flowRate).ToList();
            foreach (var node in nodes)
            {
                foreach (var nodeDestinationName in node.destinationNames)
                {
                    var targetNode = nodes.Single(x => x.name == nodeDestinationName);
                    var index = nodes.IndexOf(targetNode);
                    node.destinations.Add(index);
                }
            }

            var valvesState = 0ul;

            ulong OpenValve(ulong currentState, int valveIndex)
            {
                return currentState | (1ul << valveIndex);
            }

            bool GetValveState(ulong state, int valveIndex)
            {
                return (state & (1ul << valveIndex )) != 0;
            }

            var solutions = new Dictionary<(int p, ulong valveState, int timeLeft, int player), int>();
            var stopWatch = new Stopwatch();
            
            stopWatch.Start();
            var result = BestFlow(0, 0UL, 30, 0);
            Console.WriteLine($"{result} - elapsed: {stopWatch.Elapsed}");
            if (part == 1)
            {
                return result;
            }

            var result2 = BestFlow(0, 0UL, 26, 1);
            Console.WriteLine($"{result2} - elapsed: {stopWatch.Elapsed}");
            return result2;

            int BestFlow(int p, ulong valveState, int timeLeft, int guy)
            {
                if (timeLeft == 0)
                {
                    return guy > 0 ? BestFlow(0, valveState, 26, guy - 1) : 0;
                } 
                
                if (solutions.TryGetValue((p, valveState, timeLeft, guy), out var existingResult))
                {
                    return existingResult;
                }

                int bestAnswer = 0;

                // open current valve - if it makes sense to do so
                if (nodes[p].flowRate > 0 && !GetValveState(valveState, p))
                {
                    var thisFlow = (timeLeft - 1) * nodes[p].flowRate;
                    var nextStep = BestFlow(p, OpenValve(valveState, p), timeLeft - 1, guy);
                    var answer = thisFlow + nextStep;
                    bestAnswer = Math.Max(bestAnswer, answer);
                }

                // travel to all neighbors
                foreach (var destIndex in nodes[p].destinations)
                {
                    var nextStep = BestFlow(destIndex, valveState, timeLeft - 1, guy);
                    bestAnswer = Math.Max(bestAnswer, nextStep);
                }

                solutions[(p, valveState, timeLeft, guy)] = bestAnswer;
                return bestAnswer;
            }
        }
    }
}
