using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    class Day16
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

        public static void Run()
        {
            using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\input-16.txt"))
            //using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\example.txt"))
            {
                using (var reader = new StreamReader(f))
                {
                    while (!reader.EndOfStream)
                    {
                        var parts = reader.ReadLine().Split(' ');
                        var valveName = parts[1].Trim();
                        var flowRate =  int.Parse(parts[4].Split('=')[1].Trim(';'));
                        var destinations = parts.Skip(9).Select(p => p.Trim(',').Trim()).ToList();

                        nodes.Add(new Node { name = valveName, flowRate = flowRate, destinationNames = destinations });
                    }
                }
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

            stopWatch.Restart();
            var result2 = BestFlow(0, 0UL, 26, 1);
            Console.WriteLine($"{result2} - elapsed: {stopWatch.Elapsed}");

            int BestFlow(int p, ulong valveState, int timeLeft, int guyNumber)
            {
                if (timeLeft == 0)
                {
                    return guyNumber > 0 ? BestFlow(0, valveState, 26, guyNumber - 1) : 0;
                } 
                
                if (solutions.TryGetValue((p, valveState, timeLeft, guyNumber), out var existingResult))
                {
                    return existingResult;
                }

                int bestAnswer = 0;

                // open current valve - if it makes sense to do so
                if (nodes[p].flowRate > 0 && !GetValveState(valveState, p))
                {
                    var thisFlow = (timeLeft - 1) * nodes[p].flowRate;
                    var nextStep = BestFlow(p, OpenValve(valveState, p), timeLeft - 1, guyNumber);
                    var answer = thisFlow + nextStep;
                    bestAnswer = Math.Max(bestAnswer, answer);
                }

                // travel to all neighbors
                foreach (var destIndex in nodes[p].destinations)
                {
                    var nextStep = BestFlow(destIndex, valveState, timeLeft - 1, guyNumber);
                    bestAnswer = Math.Max(bestAnswer, nextStep);
                }

                solutions[(p, valveState, timeLeft, guyNumber)] = bestAnswer;
                return bestAnswer;
            }
        }
    }
}
