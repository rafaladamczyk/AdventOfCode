using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day12 : IAocDay
    {
        public async Task<object> Part1()
        {
            var ans = 0UL;
            var input = await IO.GetInput(2023, 12);
            foreach (var line in ParseLines(input, false))
            {
                ans += GetValidPermutationsCountBrute(line);
            }

            return ans;
        }

        public async Task<object> Part2()
        {
            var ans = 0UL;
            var input = await IO.GetInput(2023, 12);

            var lines = ParseLines(input, true).ToArray();
            foreach (var line in lines)
            {
               ans += GetValidPermutationsCount(line, new Key { numIndex = 0, charIndex = 0, currentBlockLength = 0 }, new Dictionary<Key, ulong>());
            }

            return ans;
        }

        private static IEnumerable<Line> ParseLines(List<string> input, bool part2)
        {
            foreach (var l in input)
            {
                yield return new Line(l.Split()[0],
                    l.Split()[1].Split(",").Select(x => int.Parse(x.ToString())).ToList(), part2);
            }
        }

        public ulong GetValidPermutationsCount(Line line, Key key, Dictionary<Key, ulong> knownStates)
        {
            var count = 0UL;
            if (knownStates.TryGetValue(key, out var val))
            {
                return val;
            }

            if (key.charIndex == line.chars.Length)
            {
                if (key.numIndex == line.numbers.Count - 1 && line.numbers[key.numIndex] == key.currentBlockLength) // we arrived at the end with '#' completing current block, valid
                {
                    return 1;
                }
                if (key.currentBlockLength == 0 && key.numIndex == line.numbers.Count) //we arrived at the end with '.' as last char and no current block, valid
                {
                    return 1;
                }
             
                return 0;
            }

            // replace next character with '#'
            if (line.chars[key.charIndex] == '?' || line.chars[key.charIndex] == '#')
            {
                count += GetValidPermutationsCount(line, new Key { charIndex = key.charIndex + 1, numIndex = key.numIndex, currentBlockLength = key.currentBlockLength + 1 }, knownStates);
            }

            // replace next character with '.'
            if (line.chars[key.charIndex] == '?' || line.chars[key.charIndex] == '.')
            {
                if (key.currentBlockLength == 0)
                {
                    count += GetValidPermutationsCount(line, new Key { charIndex = key.charIndex + 1, numIndex = key.numIndex, currentBlockLength = 0 }, knownStates); // we're not in a block, so continue
                }
                else if (key.currentBlockLength > 0 && key.numIndex < line.numbers.Count && line.numbers[key.numIndex] == key.currentBlockLength) //end current block, continue
                {
                    count += GetValidPermutationsCount(line, new Key { charIndex = key.charIndex + 1, numIndex = key.numIndex + 1, currentBlockLength = 0 }, knownStates); // end current block if it matches the numbers, continue
                }
            }

            knownStates[key] = count;
            return count;
        }

        public ulong GetValidPermutationsCountBrute(Line line)
        {
            var count = 0UL;
            int questionIndex = line.chars.IndexOf('?');
            if (questionIndex == -1)
            {
                return 1;
            }

            var leftChars = line.chars.ToArray();
            leftChars[questionIndex] = '.';

            var rightChars = line.chars.ToArray();
            rightChars[questionIndex] = '#';

            var left = new Line(new string(leftChars), line.numbers, false);
            var right = new Line(new string(rightChars), line.numbers, false);

            if (left.IsValidSub())
            {
                count += GetValidPermutationsCountBrute(left);
            }

            if (right.IsValidSub())
            {
                count += GetValidPermutationsCountBrute(right);
            }

            return count;
        }


        [DebuggerDisplay("{chars}")]
        public class Line
        {
            public string chars;
            public List<int> numbers;

            public Line(string cs, List<int> nums, bool expand)
            {
                if (!expand)
                {
                    chars = cs;
                    numbers = nums;
                }
                else
                {
                    chars =
                        $"{cs}?{cs}?{cs}?{cs}?{cs}";
                    numbers =
                        $"{string.Join("", nums.Select(x => x.ToString()))}{string.Join("", nums.Select(x => x.ToString()))}{string.Join("", nums.Select(x => x.ToString()))}{string.Join("", nums.Select(x => x.ToString()))}{string.Join("", nums.Select(x => x.ToString()))}"
                            .Select(x => int.Parse(x.ToString())).ToList();
                }
            }

            public bool IsValidSub()
            {
                var firstQuestion = chars.IndexOf('?');
                if (firstQuestion == -1)
                {
                    return IsValid();
                }

                var substr = chars.Substring(0, firstQuestion);
                var groups = substr.Split('.', StringSplitOptions.RemoveEmptyEntries);

                var pounds = chars.Count(c => c == '#');
                var potentialSpaces = chars.Count(c => c == '?');
                var targetCount = numbers.Sum();
                
                if (pounds + potentialSpaces < targetCount)
                {
                    return false;
                }

                //only need to check penultimate and last group, since all previous one must have already been checked in the past.
                for (int i = Math.Max(groups.Length - 2, 0); i < groups.Length; i++) 
                {
                    if (i >= numbers.Count)
                    {
                        return false;
                    }

                    var groupSize = groups[i].Length;
                    if (groupSize > numbers[i])
                    {
                        return false;
                    }

                    if (i < groups.Length - 1 &&  groupSize < numbers[i]) // last group may be incomplete, so don't include it in this check
                    {
                        return false;
                    }
                }

                return true;
            }

            public bool IsValid()
            {
                var groupSizes = chars.Split('.', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Length).ToList();
                if (groupSizes.Count != this.numbers.Count)
                {
                    return false;
                }

                for (int i = 0; i < groupSizes.Count; i++)
                {
                    if (groupSizes[i] != numbers[i])
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        public struct Key
        {
            public int numIndex;
            public int charIndex;
            public int currentBlockLength;

            public override bool Equals(object obj)
            {
                return obj is Key key && Equals(key);
            }

            public bool Equals(Key other)
            {
                return numIndex == other.numIndex && charIndex == other.charIndex && currentBlockLength == other.currentBlockLength;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(numIndex, charIndex, currentBlockLength);
            }
        }
    }
}
