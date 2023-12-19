using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
                        var nextLabel = exp.Compile().Invoke(part);
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
            //var input = await IO.GetInputString(2023, 19);
            var input = await IO.GetExampleInputString();
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
                
                foreach (var ruleString in ruleStrings)
                {
                    if (!ruleString.Contains(':'))
                    {
                        node.connections.Add(new ConditionalConnection { targetNode = nodes[ruleString] });
                        continue;
                    }

                    var expression = ruleString.Split(':')[0];
                    var destLabel = ruleString.Split(':')[1];

                    var c = expression[0];
                    var op = expression[1];
                    var number = int.Parse(expression[2..]);

                    var connection = new ConditionalConnection { targetNode = nodes[destLabel] };
                    var index = connection.LetterIndex(c);

                    switch (op)
                    {
                        case '<':
                            connection.ranges[index] = connection.ranges[index].Intersect(new Range(1, (ulong)number + 1));
                            break;
                        case '>':
                            connection.ranges[index] = connection.ranges[index].Intersect(new Range((ulong)number, 4001));
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    node.connections.Add(connection);
                }
            }

            IEnumerable<Range[]> EnumerateAllPaths(Node start, Node dest, Range[] conditions)
            {
                if (start == dest)
                {
                    yield return conditions;
                }
                else
                {
                    foreach (var node in start.connections)
                    {
                        var target = node.targetNode;
                        var ranges = node.ranges.ToArray();
                        for (int r = 0; r < ranges.Length; r++)
                        {
                            ranges[r] = ranges[r].Intersect(conditions[r]);
                        }

                        foreach (var child in EnumerateAllPaths(target, dest, ranges))
                        {
                            yield return child;
                        }
                    }
                }
            }

            var rangeRequirements = EnumerateAllPaths(nodes["in"], nodes["A"],
                Enumerable.Range(1, 4).Select(x => new Range(1, 4001)).ToArray()).ToList();

            return 3;
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
            public List<Expression<Func<Part, string>>> expressions = new();
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
                ranges = Enumerable.Range(1, 4).Select(x => new Range(1, 4001)).ToArray();
            }
            public Node targetNode;
            public Range[] ranges;

            public int LetterIndex(char c)
            {
                switch (c)
                {
                    case 'x':
                        return 0;
                    case 'm':
                        return 1;
                    case 'a':
                        return 2;
                    case 's':
                        return 3;
                    default:
                        throw new Exception();
                }
            }
        }
    }
}
