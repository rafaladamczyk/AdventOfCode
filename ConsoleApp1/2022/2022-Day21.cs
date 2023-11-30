using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2022
{
    public class Monkey
    {
        public string name;
        public long yell;
        public string operation;
        public string[] arguments;
        public Monkey descendant1;
        public Monkey descendant2;

        public Monkey ascendant;
        public string reverseOperation;

    }
    
    public class Day21 : IAocDay
    {
        public static Dictionary<string, Monkey> monkeys = new Dictionary<string, Monkey>();

        public async Task<object> Part1()
        {
            var input = await Input.GetInput(2022, 21);
            foreach (var line in input.Select(x => x.Split(':')))
            {
                var monkeyName = line[0].Trim();
                if (long.TryParse(line[1].Trim(), out var yell))
                {
                    monkeys[monkeyName] = new Monkey() { name = monkeyName, yell = yell, operation = null };
                }
                else
                {
                    var parts = line[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    monkeys[monkeyName] = new Monkey()
                        { name = monkeyName, operation = monkeyName == "root" ? "=" : parts[1], arguments = new[] { parts[0], parts[2] } };
                }
            }

            foreach (var monkey in monkeys.Values)
            {
                if (monkey.arguments != null)
                {
                    monkey.descendant1 = monkeys[monkey.arguments[0]];
                    monkey.descendant2 = monkeys[monkey.arguments[1]];

                    if (monkey.descendant1.ascendant != null || monkey.descendant2.ascendant != null)
                    {
                        throw new Exception("ex");
                    }

                    monkey.descendant1.ascendant = monkey;
                    monkey.descendant2.ascendant = monkey;

                    switch (monkey.operation)
                    {
                        case "+":
                            monkey.descendant1.reverseOperation = "-";
                            monkey.descendant2.reverseOperation = "-";
                            break;

                        case "-":
                            monkey.descendant1.reverseOperation = "+";
                            monkey.descendant2.reverseOperation = "+";
                            break;

                        case "*":
                            monkey.descendant1.reverseOperation = "/";
                            monkey.descendant2.reverseOperation = "/";
                            break;

                        case "/":
                            monkey.descendant1.reverseOperation = "*";
                            monkey.descendant2.reverseOperation = "*";
                            break;

                        default:
                            break;
                    }
                }
            }

            var score = Backwards("humn");
            Console.WriteLine($"{score} <--");
            return score;
        }

        public async Task<object> Part2()
        {
            return await Part1();
        }

        
        public static long Backwards(string monkeyName)
        {
            if (monkeyName == monkeys["root"].arguments[1])
            {
                return Forward(monkeys["root"].arguments[0]);
            }

            if (monkeyName == monkeys["root"].arguments[0])
            {
                return Forward(monkeys["root"].arguments[1]);
            }

            var monke = monkeys[monkeyName];

            if (monke.operation == null && monke.name != "humn")
            {
                return monke.yell;
            }
            else
            {
                var theOtherMonke = monke.ascendant.descendant1 == monke
                    ? monke.ascendant.descendant2
                    : monke.ascendant.descendant1;

                var order = theOtherMonke == monke.ascendant.descendant1 ? 1 : 2;

                switch (monke.reverseOperation)
                {
                    case "+":
                        return order == 1 ? Forward(theOtherMonke.name) - Backwards(monke.ascendant.name) :
                            Backwards(monke.ascendant.name) + Forward(theOtherMonke.name);
                    
                    case "-":
                        return Backwards(monke.ascendant.name) - Forward(theOtherMonke.name);

                    case "*":
                        return order == 1 ? Backwards(monke.ascendant.name) / Forward(theOtherMonke.name) :  Backwards(monke.ascendant.name) * Forward(theOtherMonke.name);

                    case "/":
                        return Backwards(monke.ascendant.name) / Forward(theOtherMonke.name);

                    default:
                        throw new Exception("wrwrw");
                }
            }
        }

        public static long Forward(string monkeyName)
        {
            if (monkeyName == "humn")
            {
                Console.WriteLine("HUMAN");
            }

            var m = monkeys[monkeyName];
            if (m.operation == null)
            {
                return m.yell;
            }
            else
            {
                switch (m.operation)
                {
                    case "+":
                        return Forward(m.arguments[0]) + Forward(m.arguments[1]);

                    case "-":
                        return Forward(m.arguments[0]) - Forward(m.arguments[1]);

                    case "*":
                        return Forward(m.arguments[0]) * Forward(m.arguments[1]);

                    case "/":
                        return Forward(m.arguments[0]) / Forward(m.arguments[1]);

                    case "=":
                        return Forward(m.arguments[0]) == Forward(m.arguments[1]) ? 1 : 0;
                }
            }

            throw new Exception("wururur");
        }
    }
}
