using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode2022
{
    class Day20
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

        public static void Run()
        {

            Number first = null;
            Number prev = null;

            int iters = 10;
            long mul = 811589153;
            //long mul = 1;
            using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\input-20.txt"))
            //using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\example.txt"))
            {
                using (var reader = new StreamReader(f))
                {
                    while (!reader.EndOfStream)
                    {
                        var inputNumber = int.Parse(reader.ReadLine().Trim());
                        if (prev == null)
                        {
                            first = new Number { X = inputNumber * mul };
                            numbers.Add(first);
                            prev = first;
                        }
                        else
                        {
                            prev.Next = new Number { X = inputNumber * mul, Previous = prev};
                            numbers.Add(prev.Next);
                            prev = prev.Next;
                        }
                    }

                    prev.Next = first;
                    first.Previous = prev;
                }
            }

            for (int i = 0; i < iters; i++)
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

            Console.WriteLine(a.X +b.X +c.X);
        }
    }
}
