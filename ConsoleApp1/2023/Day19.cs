using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;
using Range = AdventOfCode.Utils.Range;

namespace AoC2023
{
    public class Day19 : IAocDay
    {
        public async Task<object> Part1()
        {
            var ans = 0;
            var input = await IO.GetInputString(2023, 19);
            var inputs = input.Split("\n\n");
            var rulesInput = inputs[0].Split();
            var partsInput = inputs[1].Split();

            var rules = new Dictionary<string, Rule>();
            var parts = new List<Part>();

            var acceptedParts = new List<Part>();

            foreach (var ruleInput in rulesInput)
            {
                var label = ruleInput.Split('{')[0];
                var ruleStrings = ruleInput.Split('{')[1].Trim('}').Split(',');
                var r = new Rule(label);

                foreach (var ruleString in ruleStrings)
                {
                    if (!ruleString.Contains(':'))
                    {
                        r.expressions.Add(x => ruleString); // no evaluations here, straight to label
                        continue;
                    }

                    var expression = ruleString.Split(':')[0];
                    var destLabel = ruleString.Split(':')[1];

                    var c = expression[0].ToString();
                    var op = expression[1];
                    var number = int.Parse(expression[2..]);

                    switch (op)
                    {
                        case '<':
                            r.expressions.Add(x => (int)typeof(Part).GetField(c).GetValue(x) < number ? destLabel : null);
                            break;
                        case '>':
                            r.expressions.Add(x => (int)typeof(Part).GetField(c).GetValue(x) > number ? destLabel : null);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }

                rules.Add(label, r);
            }

            foreach (var partInput in partsInput.Where(x => !string.IsNullOrWhiteSpace(x)))
            {
                var sections = partInput.Trim('{', '}').Split(',').Select(x => x[2..]).Select(int.Parse).ToList();
                parts.Add(new Part(sections[0], sections[1], sections[2], sections[3]));
            }

            foreach (var part in parts)
            {
                var nextRule = rules["in"];
                while (nextRule != null)
                {
                    foreach (var exp in nextRule.expressions)
                    {
                        var nextLabel = exp.Invoke(part);
                        if (nextLabel != null)
                        {
                            if (nextLabel == "A")
                            {
                                acceptedParts.Add(part);
                                nextRule = null;
                                break;
                            }

                            if (nextLabel == "R")
                            {
                                nextRule = null;
                                break;
                            }

                            nextRule = rules[nextLabel];
                            break;
                        }
                    }
                }
            }

            ans = acceptedParts.Sum(p => p.x + p.m + p.a + p.s);
            return ans;
        }

        public async Task<object> Part2()
        {
            var ans = 0;
            var input = await IO.GetInputString(2023, 19);
            var inputs = input.Split("\n\n");
            var rulesInput = inputs[0].Split();

            var nodes = new Dictionary<string, Node>() { { "A", new Node("A") }, { "R", new Node("R") } };
            foreach (var ruleInput in rulesInput)
            {
                var label = ruleInput.Split('{')[0];
                nodes.Add(label, new Node(label));
            }

            foreach (var ruleInput in rulesInput)
            {
                var label = ruleInput.Split('{')[0];
                var ruleStrings = ruleInput.Split('{')[1].Trim('}').Split(',');
                var node = nodes[label];

                var complimentaryRanges = DefaultRanges();

                foreach (var ruleString in ruleStrings)
                {
                    bool hasExpression = ruleString.Contains(':');
                    var destLabel = hasExpression ? ruleString.Split(':')[1] : ruleString;
                    var connection = new ConditionalConnection { targetNode = nodes[destLabel] };

                    foreach (var compRange in complimentaryRanges)
                    {
                        connection.ranges[compRange.Key] = connection.ranges[compRange.Key].Intersect(compRange.Value).Value;
                    }

                    if (hasExpression)
                    {
                        var expression = ruleString.Split(':')[0];
                        var c = expression[0];
                        var op = expression[1];
                        var number = int.Parse(expression[2..]);

                        switch (op)
                        {
                            case '<':
                                connection.ranges[c] = connection.ranges[c].Intersect(new Range(1, (ulong)number)).Value;
                                complimentaryRanges[c] = complimentaryRanges[c].Intersect(new Range((ulong)number, 4001)).Value;
                                break;
                            case '>':
                                connection.ranges[c] = connection.ranges[c].Intersect(new Range((ulong)number + 1, 4001)).Value;
                                complimentaryRanges[c] = complimentaryRanges[c].Intersect(new Range(1, (ulong)number + 1)).Value;
                                break;
                            default:
                                throw new NotImplementedException();
                        }
                    }

                    node.connections.Add(connection);
                }
            }

            var rangeRequirements = EnumerateAllPaths(nodes["in"], nodes["A"],
                DefaultRanges()).ToList();

            var permutations = rangeRequirements.Select(x => x['x'].Length * x['m'].Length * x['a'].Length * x['s'].Length).ToList();
            return permutations.Aggregate(0UL, (x, p) => x + p);
        }

        /// <summary>
        /// Enumerates all paths between start and dest nodes. For each such path we return a dictionary of ranges for 'x','m','a','s' that need to be satisfied.
        /// </summary>
        private static IEnumerable<Dictionary<char, Range>> EnumerateAllPaths(Node start, Node dest, Dictionary<char, Range> startingConditions)
        {
            if (start == dest)
            {
                yield return startingConditions;
            }
            else
            {
                foreach (var node in start.connections)
                {
                    var target = node.targetNode;
                    var ranges = node.ranges;
                    foreach (var kvp in ranges)
                    {
                        ranges[kvp.Key] = ranges[kvp.Key].Intersect(startingConditions[kvp.Key]).Value;
                    }

                    foreach (var child in EnumerateAllPaths(target, dest, ranges))
                    {
                        yield return child;
                    }
                }
            }
        }

        private static Dictionary<char, Range> DefaultRanges()
        {
            return new[] { 'x', 'm', 'a', 's' }.ToDictionary(x => x, x => new Range(1, 4001));
        }

        public struct Part
        {
            public Part(int x, int m, int a, int s)
            {
                this.x = x;
                this.m = m;
                this.a = a;
                this.s = s;
            }

            public int x;
            public int m;
            public int a;
            public int s;
        }

        public class Rule
        {
            public Rule(string label)
            {
                this.label = label;
            }

            public string label;
            public List<Func<Part, string>> expressions = new();
        }

        public class Node
        {
            public Node(string label)
            {
                this.label = label;
            }

            public string label;
            public List<ConditionalConnection> connections = new();
        }

        public class ConditionalConnection
        {
            public ConditionalConnection()
            {
                ranges = DefaultRanges();
            }
            public Node targetNode;
            public Dictionary<char, Range> ranges;
        }
    }
}
