using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2022
{
    public class Day20 : IAocDay 
    {
        public class Number
        {
            public long X;
            public Number Previous;
            public Number Next;

            public void Move(long by)
            {
                if (by == 0)
                {
                    return;
                }

                RemoveMyself();
                var insertInPlaceOf = GetNextNumber(by);
 
                if (by < 0)
                {
                    InsertBefore(insertInPlaceOf);
                }
                else
                {
                    InsertAfter(insertInPlaceOf);
                }
            }

            public void InsertAfter(Number insertInPlaceOf)
            {
                // insert after target
                var nextNext = insertInPlaceOf.Next;

                nextNext.Previous = this;
                this.Next = nextNext;
                this.Previous = insertInPlaceOf;
                insertInPlaceOf.Next = this;
            }

            public void InsertBefore(Number insertInPlaceOf)
            {
                // insert before target
                var prevPrev = insertInPlaceOf.Previous;

                prevPrev.Next = this;
                this.Previous = prevPrev;
                this.Next = insertInPlaceOf;
                insertInPlaceOf.Previous = this;
            }

            public void RemoveMyself()
            {
                var prev = Previous;
                var next = Next;

                prev.Next = next;
                next.Previous = prev;
            }

            public Number GetNextNumber(long distance, bool removeCurrent = true)
            {
                if (distance == 0)
                {
                    return this;
                }

                long cycleLength = removeCurrent ? numbers.Count - 1 : numbers.Count;
                long howManyToEvaluate = Math.Abs(distance % cycleLength);

                var result = this;
                if (distance > 0)
                {
                    for (int i = 0; i < howManyToEvaluate; i++)
                    {
                        result = result.Next;
                    }
                }
                else
                {
                    for (int j = 0; j < howManyToEvaluate; j++)
                    {
                        result = result.Previous;
                    }
                }

                return result;
            }
        }

        public static List<Number> numbers = new List<Number>();

        public async Task<object> Part1()
        {
            return await Run(1, 1);
        }

        public async Task<object> Part2()
        {
            return await Run(811589153, 10);
        }

        public async Task<object> Run(long mul, int mixes)
        {
            Number first = null;
            Number prev = null;
            numbers.Clear();

            var input = await Input.GetInput(2022, 20);
            foreach (var line in input)
            {
                var inputNumber = int.Parse(line.Trim());
                if (prev == null)
                {
                    first = new Number { X = inputNumber * mul };
                    numbers.Add(first);
                    prev = first;
                }
                else
                {
                    prev.Next = new Number { X = inputNumber * mul, Previous = prev };
                    numbers.Add(prev.Next);
                    prev = prev.Next;
                }
            }

            prev.Next = first;
            first.Previous = prev;

            for (int i = 0; i < mixes; i++)
            {
                foreach (var number in numbers)
                {
                    number.Move(number.X);
                }

                Console.WriteLine($" {i + 1} done");
            }

            var zero = numbers.Single(x => x.X == 0);
            var a = zero.GetNextNumber(1000, false);
            var b = zero.GetNextNumber(2000, false);
            var c = zero.GetNextNumber(3000, false);

            return (a.X + b.X + c.X);

        }
    }
}
