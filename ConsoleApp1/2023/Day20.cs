using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day20 : IAocDay
    {
        public async Task<object> Part1()
        {
            var ans = 0;
            var input = await IO.GetInput(2023, 20);
            //var input = await IO.GetExampleInput();
            var modules = ParseModules(input);

            var Q = new Queue<Pulse>();
            var lowPulses = 0;
            var highPulses = 0;

            for (int i = 0; i < 1000; i++)
            {
                Q.Enqueue(new Pulse("button", "broadcaster", false));

                while (Q.Count > 0)
                {
                    var pulse = Q.Dequeue();
                    if (pulse.high)
                    {
                        highPulses++;
                    }
                    else
                    {
                        lowPulses++;
                    }

                    var module = modules[pulse.to];
                    switch (module.type)
                    {
                        case '%':
                            if (!pulse.high)
                            {
                                module.on = !module.on;
                                foreach (var target in module.targets)
                                {
                                    Q.Enqueue(new Pulse(module.name, target, module.on));
                                }
                            }

                            break;
                        case '&':
                            module.inputs[pulse.from] = pulse.high;
                            foreach (var target in module.targets)
                            {
                                Q.Enqueue(new Pulse(module.name, target, !module.inputs.Values.All(x => x)));
                            }

                            break;
                        case 'B':
                            foreach (var target in module.targets)
                            {
                                Q.Enqueue(new Pulse(module.name, target, pulse.high));
                            }
                            break;
                        default:
                            // do nothing
                            break;
                    }

                }
            }

            return (long)highPulses * lowPulses;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 20);
            var modules = ParseModules(input);
            var interesting = new List<Module> { modules["ln"], modules["dr"], modules["zx"], modules["vn"] };
            var interestingHighs = new Dictionary<string, List<int>>();

            var Q = new Queue<Pulse>();
            var count = 0;
            
            foreach (var m in interesting)
            {
                interestingHighs.Add(m.name, new List<int>());
            }

            while (true)
            {
                if (interestingHighs.All(x => x.Value.Count >= 2))
                {
                    var cycleLengths = interestingHighs.Select(x => (ulong)x.Value.Skip(1).First() - (ulong)x.Value.First())
                        .ToArray();
                    return Misc.LCM(cycleLengths);
                }

                Q.Enqueue(new Pulse("button", "broadcaster", false));
                count++;

                while (Q.Count > 0)
                {
                    var pulse = Q.Dequeue();
                    var module = modules[pulse.to];
                    switch (module.type)
                    {
                        case '%':
                            if (!pulse.high)
                            {
                                module.on = !module.on;
                                foreach (var target in module.targets)
                                {
                                    Q.Enqueue(new Pulse(module.name, target, module.on));
                                }
                            }

                            break;
                        case '&':
                            module.inputs[pulse.from] = pulse.high;
                            var hi = !module.inputs.Values.All(x => x);
                            foreach (var target in module.targets)
                            {
                                Q.Enqueue(new Pulse(module.name, target, hi));
                            }

                            if (hi && interesting.Contains(module))
                            {
                                interestingHighs[module.name].Add(count);
                            }

                            break;
                        case 'B':
                            foreach (var target in module.targets)
                            {
                                Q.Enqueue(new Pulse(module.name, target, pulse.high));
                            }
                            break;
                        case '?':
                            if (!pulse.high)
                            {
                                return count;
                            }
                            break;
                        default:
                            throw new Exception();
                    }
                }
            }
        }

        private static Dictionary<string, Module> ParseModules(List<string> input)
        {
            var modules = new Dictionary<string, Module>();

            foreach (var line in input)
            {
                var parts = line.Split("->", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                var module = new Module();
                module.type = parts[0].First() == '%' ? '%' : parts[0].First() == '&' ? '&' : 'B';
                module.name = parts[0].Trim('%', '&');
                module.targets = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                    .ToList();
                modules.Add(module.name, module);
            }

            var unlistedModules = new List<Module>();
            foreach (var module in modules.Values)
            {
                foreach (var target in module.targets)
                {
                    if (modules.TryGetValue(target, out var m))
                    {
                        m.inputs.Add(module.name, false);
                    }
                    else
                    {
                        unlistedModules.Add(new Module() { name = target, type = '?' });
                    }
                }
            }

            foreach (var um in unlistedModules)
            {
                modules[um.name] = um;
            }

            return modules;
        }
    }

    public class Module
    {
        public string name;
        public char type;
        public List<string> targets;

        public bool on = false; // flipflop
        public Dictionary<string, bool> inputs = new(); //conjunction
    }
    
    public class Pulse
    {
        public Pulse(string from, string to, bool high)
        {
            this.from = from;
            this.to = to;
            this.high = high;
        }

        public string from;
        public string to;
        public bool high;
    }
}
