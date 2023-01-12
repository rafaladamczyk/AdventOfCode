using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace AdventOfCode2022
{
    public class Blueprint
    {
        // ore, clay, obsidian  
        public Dictionary<RobotType, (int, int, int)> costsPerRobotType = new Dictionary<RobotType, (int, int, int)>();
    }

    public enum RobotType
    {
        Ore,
        Clay,
        Obs,
        Geode
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct State
    {
        [FieldOffset(0)] public ulong theWholeState;
        [FieldOffset(0)] public byte oreRobotCount;
        [FieldOffset(1)] public byte clayRobotCount;
        [FieldOffset(2)] public byte obsidianRobotCount;
        [FieldOffset(3)] public byte geodeRobotCount;
        [FieldOffset(4)] public byte oreCount;
        [FieldOffset(5)] public byte clayCount;
        [FieldOffset(6)] public byte obsidianCount;
        [FieldOffset(7)] public byte geodeCount;

        public override bool Equals(object obj)
        {
            return ((State)obj).theWholeState.Equals(this.theWholeState);
        }

        public override int GetHashCode()
        {
            return this.theWholeState.GetHashCode();
        }
    }

    class Day19
    {
        private static List<Blueprint> blueprints = new List<Blueprint>();
        private static Dictionary<(int, State), int> ScoresByState = new Dictionary<(int, State), int>();

        private static List<RobotType> robotTypes = new List<RobotType>()
            { RobotType.Ore, RobotType.Clay, RobotType.Obs, RobotType.Geode };
        
        public static void Run()
        {
            using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\input-19.txt"))
            //using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\example.txt"))
            {
                using (var reader = new StreamReader(f))
                {
                    while (!reader.EndOfStream)
                    {
                        ReadInput(reader);
                    }
                }
            }

            var x = new Stopwatch();
            x.Start();
            int answer = 1;
            for (var index = 0; index < 3; index++)
            {
                var blueprint = blueprints[index];
                var score = GetBlueprintScore(blueprint, 32);
                answer *= score;

                Console.WriteLine($"Blueprint {index} evaluated to : {score} in {x.Elapsed}");
            }

            Console.WriteLine(answer);
            Console.WriteLine($"elapsed {x.Elapsed}");
        }

        private static int best = 0;
        private static Dictionary<int, int> Fibo = new Dictionary<int, int>();
        private static Dictionary<RobotType, int> maxCostsPerType = new Dictionary<RobotType, int>();

        private static int FiboSum(int n)
        {
            if (Fibo.TryGetValue(n, out var i))
            {
                return i;
            }

            var sum = 0;
            for (int aaa = 0; aaa <= n; aaa++)
            {
                sum += aaa;
            }

            Fibo[n] = sum;
            return sum;
        }

        private static int GetBlueprintScore(Blueprint blueprint, int initialTime)
        {
            var initialState = new State()
            {
                oreCount = 3,
                clayRobotCount = 0,
                geodeRobotCount = 0,
                oreRobotCount = 1,
                obsidianRobotCount = 0,
                clayCount = 0,
                geodeCount = 0,
                obsidianCount = 0
            };
            
            ScoresByState.Clear();
            best = 0;

            maxCostsPerType = new Dictionary<RobotType, int>()
            {
                { RobotType.Ore, 0 }, { RobotType.Clay, 0 },
                { RobotType.Obs, 0 }, { RobotType.Geode, 0xFF }
            };

            foreach (var cost in blueprint.costsPerRobotType)
            {
                if (maxCostsPerType[RobotType.Ore] < cost.Value.Item1)
                {
                    maxCostsPerType[RobotType.Ore] = cost.Value.Item1;
                }

                if (maxCostsPerType[RobotType.Clay] < cost.Value.Item2)
                {
                    maxCostsPerType[RobotType.Clay] = cost.Value.Item2;
                }

                if (maxCostsPerType[RobotType.Obs] < cost.Value.Item3)
                {
                    maxCostsPerType[RobotType.Obs] = cost.Value.Item3;
                }
            }

            var timer = new Stopwatch();
            timer.Start();
            var x = GetScoreRecursive(blueprint, initialTime - 3, initialState);
            Console.WriteLine($"Recursive completed in {timer.Elapsed}. Best is {x}. Seen size recursive: {ScoresByState.Count}");

            timer.Restart();
            var y = GetScoreIterative(blueprint, initialTime - 3, initialState);
            Console.WriteLine($"Iterative completed in {timer.Elapsed}. Best is {y.Item1}. Seen size iterative: {y.Item2}");

            return x;
        }

        private static (int, int) GetScoreIterative(Blueprint blueprint, int initialTime, State initialState)
        {
            var Q = new Stack<(int, State)>();
            var S = new HashSet<(int, State)>();
            Q.Push((initialTime, initialState));

            var best = 0;

            while (Q.TryPop(out var current))
            {
                if (S.Contains(current))
                    continue;

                var timeLeft = current.Item1;
                State currentState = current.Item2;

                if (timeLeft == 0)
                {
                    if (currentState.geodeCount > best)
                    {
                        best = currentState.geodeCount;
                        //Console.WriteLine($"new best found: {best}");
                    }

                    continue;
                }

                if (currentState.geodeCount + timeLeft * currentState.geodeRobotCount + FiboSum(timeLeft) < best)
                {
                    continue; // even if we build 1 georobot per turn, we can't beat current best
                }

                var nextState = currentState;
                nextState.oreCount += currentState.oreRobotCount;
                nextState.clayCount += currentState.clayRobotCount;
                nextState.obsidianCount += currentState.obsidianRobotCount;
                nextState.geodeCount += currentState.geodeRobotCount;

                S.Add(current);
                Q.Push((timeLeft - 1, nextState));

                foreach (var robotType in robotTypes)
                {
                    switch (robotType)
                    {
                        case RobotType.Ore:
                            if (currentState.oreRobotCount >= maxCostsPerType[RobotType.Ore])
                                continue;
                            break;
                        case RobotType.Clay:
                            if (currentState.clayRobotCount >= maxCostsPerType[RobotType.Clay])
                                continue;
                            break;
                        case RobotType.Obs:
                            if (currentState.obsidianRobotCount >= maxCostsPerType[RobotType.Obs])
                                continue;
                            break;
                        default:
                            break;
                    }

                    var robotCost = blueprint.costsPerRobotType[robotType];
                    if (robotCost.Item1 <= currentState.oreCount && robotCost.Item2 <= currentState.clayCount &&
                        robotCost.Item3 <= currentState.obsidianCount)
                    {
                        // robot can be built
                        var afterBuiltState = currentState;

                        afterBuiltState.oreCount += currentState.oreRobotCount;
                        afterBuiltState.clayCount += currentState.clayRobotCount;
                        afterBuiltState.obsidianCount += currentState.obsidianRobotCount;
                        afterBuiltState.geodeCount += currentState.geodeRobotCount;

                        switch (robotType)
                        {
                            case RobotType.Ore:
                                afterBuiltState.oreRobotCount++;
                                break;
                            case RobotType.Clay:
                                afterBuiltState.clayRobotCount++;
                                break;
                            case RobotType.Obs:
                                afterBuiltState.obsidianRobotCount++;
                                break;
                            case RobotType.Geode:
                                afterBuiltState.geodeRobotCount++;
                                break;
                        }

                        afterBuiltState.oreCount -= (byte)robotCost.Item1;
                        afterBuiltState.clayCount -= (byte)robotCost.Item2;
                        afterBuiltState.obsidianCount -= (byte)robotCost.Item3;

                        Q.Push((timeLeft - 1, afterBuiltState));
                    }
                }
            }
            
            return (best, S.Count);
        }


        private static int GetScoreRecursive(Blueprint blueprint, int timeLeft, State currentState)
        {
            if (ScoresByState.TryGetValue((timeLeft, currentState), out var score))
            {
                return score;
            }

            if (timeLeft == 0)
            {
                if (currentState.geodeCount > best)
                {
                    best = currentState.geodeCount;
                    //Console.WriteLine($"new best found: {best}");
                }

                return currentState.geodeCount;
            }

            if (currentState.geodeCount + timeLeft * currentState.geodeRobotCount + FiboSum(timeLeft) < best)
            {
                return 0; // even if we build 1 georobot per turn, we can't beat current best
            }

            var ans = 0;
            foreach (var robotType in robotTypes)
            {
                switch (robotType)
                {
                    case RobotType.Ore:
                        if (currentState.oreRobotCount >= maxCostsPerType[RobotType.Ore])
                            continue;
                        break;
                    case RobotType.Clay:
                        if (currentState.clayRobotCount >= maxCostsPerType[RobotType.Clay])
                            continue;
                        break;
                    case RobotType.Obs:
                        if (currentState.obsidianRobotCount >= maxCostsPerType[RobotType.Obs])
                            continue;
                        break;
                    default:
                        break;
                }

                var robotCost = blueprint.costsPerRobotType[robotType];
                if (robotCost.Item1 <= currentState.oreCount && robotCost.Item2 <= currentState.clayCount &&
                    robotCost.Item3 <= currentState.obsidianCount)
                {
                    // robot can be built
                    var afterBuiltState = currentState;

                    afterBuiltState.oreCount += currentState.oreRobotCount;
                    afterBuiltState.clayCount += currentState.clayRobotCount;
                    afterBuiltState.obsidianCount += currentState.obsidianRobotCount;
                    afterBuiltState.geodeCount += currentState.geodeRobotCount;

                    switch (robotType)
                    {
                        case RobotType.Ore:
                            afterBuiltState.oreRobotCount++;
                            break;
                        case RobotType.Clay:
                            afterBuiltState.clayRobotCount++;
                            break;
                        case RobotType.Obs:
                            afterBuiltState.obsidianRobotCount++;
                            break;
                        case RobotType.Geode:
                            afterBuiltState.geodeRobotCount++;
                            break;
                    }

                    afterBuiltState.oreCount -= (byte)robotCost.Item1;
                    afterBuiltState.clayCount -= (byte)robotCost.Item2;
                    afterBuiltState.obsidianCount -= (byte)robotCost.Item3;

                    ans = Math.Max(ans, GetScoreRecursive(blueprint, timeLeft - 1, afterBuiltState));
                }
            }

            var nextState = currentState;
            nextState.oreCount += currentState.oreRobotCount;
            nextState.clayCount += currentState.clayRobotCount;
            nextState.obsidianCount += currentState.obsidianRobotCount;
            nextState.geodeCount += currentState.geodeRobotCount;
            
            ans = Math.Max(ans, GetScoreRecursive(blueprint, timeLeft - 1, nextState));

            ScoresByState[(timeLeft, currentState)] = ans;
            return ans;
        }
        private static void ReadInput(StreamReader reader)
        {
            var bp = new Blueprint();
            var x = reader.ReadLine().Split(':')[1];
            
            var robotTypes = x.Split('.', StringSplitOptions.RemoveEmptyEntries);
            foreach (var robotTypeInput in robotTypes)
            {
                var cost = (0, 0, 0); //clay, ore, obsidian

                var zzz = robotTypeInput.Split(' ');
                for (var i = 0; i < zzz.Length-1; i++)
                {
                    var s = zzz[i];
                    int.TryParse(s.Trim(), out var thisOnesCost);
                    {
                        var type = zzz[i + 1];
                        switch (type)
                        {
                            case "ore":
                                cost.Item1 = thisOnesCost;
                                break;

                            case "clay":
                                cost.Item2 = thisOnesCost;
                                break;

                            case "obsidian":
                                cost.Item3 = thisOnesCost;
                                break;
                        }
                    }
                }


                RobotType robotType = RobotType.Clay;
                if (robotTypeInput.Contains("clay robot"))
                {
                    robotType = RobotType.Clay;
                }

                if (robotTypeInput.Contains("ore robot"))
                {
                    robotType = RobotType.Ore;
                }

                if (robotTypeInput.Contains("obsidian robot"))
                {
                    robotType = RobotType.Obs;
                }

                if (robotTypeInput.Contains("geode robot"))
                {
                    robotType = RobotType.Geode;
                }

                bp.costsPerRobotType[robotType] = cost;
            }

            blueprints.Add(bp);
        }
    }
}
